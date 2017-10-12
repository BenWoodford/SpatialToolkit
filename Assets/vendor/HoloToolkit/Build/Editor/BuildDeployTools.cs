//
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
//

using System;
using System.IO;
using System.Xml;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Xml.Linq;
using Microsoft.Win32;

namespace HoloToolkit.Unity
{
    /// <summary>
    /// Contains utility functions for building for the device
    /// </summary>
    public class BuildDeployTools
    {
        // Consts
        public static readonly string DefaultMSBuildVersion = "15.0";

        // Functions
        public static bool BuildSLN(string buildDirectory, bool showConfDlg = true)
        {
            // Use BuildSLNUtilities to create the SLN
            bool buildSuccess = false;

            var buildInfo = new BuildSLNUtilities.BuildInfo()
            {
                // These properties should all match what the Standalone.proj file specifies
                OutputDirectory = buildDirectory,
                Scenes = EditorBuildSettings.scenes.Where(scene => scene.enabled).Select(scene => scene.path),
                BuildTarget = BuildTarget.WSAPlayer,
                WSASdk = WSASDK.UWP,
                WSAUWPBuildType = WSAUWPBuildType.D3D,

                // Configure a post build action that will compile the generated solution
                PostBuildAction = (innerBuildInfo, buildError) =>
                {
                    if (!string.IsNullOrEmpty(buildError))
                    {
                        EditorUtility.DisplayDialog(PlayerSettings.productName + " WindowsStoreApp Build Failed!", buildError, "OK");
                    }
                    else
                    {
                        if (showConfDlg)
                        {
                            EditorUtility.DisplayDialog(PlayerSettings.productName, "Build Complete", "OK");
                        }
                        buildSuccess = true;
                    }
                }
            };

            BuildSLNUtilities.RaiseOverrideBuildDefaults(ref buildInfo);

            BuildSLNUtilities.PerformBuild(buildInfo);

            return buildSuccess;
        }

        public static string CalcMSBuildPath(string msBuildVersion)
        {
            if (msBuildVersion.Equals("14.0"))
            {
                using (RegistryKey key =
                    Registry.LocalMachine.OpenSubKey(
                        string.Format(@"Software\Microsoft\MSBuild\ToolsVersions\{0}", msBuildVersion)))
                {
                    if (key != null)
                    {
                        var msBuildBinFolder = (string)key.GetValue("MSBuildToolsPath");
                        return Path.Combine(msBuildBinFolder, "msbuild.exe");
                    }
                }
            }

            // For MSBuild 15+ we should to use vswhere to give us the correct instance
            string output = @"/C vswhere -version " + msBuildVersion + " -products * -requires Microsoft.Component.MSBuild -property installationPath";

            // get the right program files path based on whether the pc is x86 or x64
            string programFiles = @"C:\Program Files\";
            if (Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE", EnvironmentVariableTarget.Machine) == "AMD64")
            {
                programFiles = @"C:\Program Files (x86)\";
            }

            var vswherePInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = "cmd.exe",
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = false,
                Arguments = output,
                WorkingDirectory = programFiles + @"Microsoft Visual Studio\Installer"
            };

            using (var vswhereP = new System.Diagnostics.Process())
            {
                vswhereP.StartInfo = vswherePInfo;
                vswhereP.Start();
                output = vswhereP.StandardOutput.ReadToEnd();
                vswhereP.WaitForExit();
            }

            string[] paths = output.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            if (paths.Length > 0)
            {
                // if there are multiple 2017 installs,
                // prefer enterprise, then pro, then community
                string bestPath = paths.OrderBy(p => p.ToLower().Contains("enterprise"))
                                        .ThenBy(p => p.ToLower().Contains("professional"))
                                        .ThenBy(p => p.ToLower().Contains("community")).First();
                if (File.Exists(bestPath + @"\MSBuild\" + msBuildVersion + @"\Bin\MSBuild.exe"))
                {
                    return bestPath + @"\MSBuild\" + msBuildVersion + @"\Bin\MSBuild.exe";
                }
            }

            Debug.LogError("Unable to find a valid path to Visual Studio Instance!");
            return string.Empty;
        }

        public static bool BuildAppxFromSolution(string productName, string msBuildVersion, bool forceRebuildAppx, string buildConfig, string buildDirectory, bool incrementVersion)
        {
            // Get and validate the msBuild path...
            string vs = CalcMSBuildPath(msBuildVersion);
            if (!File.Exists(vs))
            {
                Debug.LogError("MSBuild.exe is missing or invalid (path=" + vs + "). Note that the default version is " + DefaultMSBuildVersion);
                return false;
            }

            // Get the path to the NuGet tool
            string unity = Path.GetDirectoryName(EditorApplication.applicationPath);
            string nugetPath = Path.Combine(unity, @"Data\PlaybackEngines\MetroSupport\Tools\NuGet.exe");
            string storePath = Path.GetFullPath(Path.Combine(Path.Combine(Application.dataPath, ".."), buildDirectory));
            string solutionProjectPath = Path.GetFullPath(Path.Combine(storePath, productName + @".sln"));

            // Before building, need to run a nuget restore to generate a json.lock file. Failing to do
            // this breaks the build in VS RTM
            var nugetPInfo = new System.Diagnostics.ProcessStartInfo();
            nugetPInfo.FileName = nugetPath;
            nugetPInfo.WorkingDirectory = buildDirectory;
            nugetPInfo.UseShellExecute = false;
            nugetPInfo.Arguments = @"restore " + PlayerSettings.productName + "/project.json";
            using (var nugetP = new System.Diagnostics.Process())
            {
                Debug.Log(nugetPath + " " + nugetPInfo.Arguments);
                nugetP.StartInfo = nugetPInfo;
                nugetP.Start();
                nugetP.WaitForExit();
            }

            // Ensure that the generated .appx version increments by modifying
            // Package.appxmanifest
            if (incrementVersion)
            {
                IncrementPackageVersion();
            }

            // Now do the actual build
            var pinfo = new System.Diagnostics.ProcessStartInfo();
            pinfo.FileName = vs;
            pinfo.UseShellExecute = false;
            string buildType = forceRebuildAppx ? "Rebuild" : "Build";
            pinfo.Arguments = string.Format("\"{0}\" /t:{2} /p:Configuration={1} /p:Platform=x86", solutionProjectPath, buildConfig, buildType);
            var p = new System.Diagnostics.Process();

            Debug.Log(vs + " " + pinfo.Arguments);
            p.StartInfo = pinfo;
            p.Start();

            p.WaitForExit();
            if (p.ExitCode == 0)
            {
                Debug.Log("APPX build succeeded!");
            }
            else
            {
                Debug.LogError("MSBuild error (code = " + p.ExitCode + ")");
            }

            if (p.ExitCode != 0)
            {
                EditorUtility.DisplayDialog(PlayerSettings.productName + " build Failed!", "Failed to build appx from solution. Error code: " + p.ExitCode, "OK");
                return false;
            }
            else
            {
                // Build succeeded. Allow user to install build on remote PC
                BuildDeployWindow.OpenWindow();
                return true;
            }
        }

        private static void IncrementPackageVersion()
        {
            // Find the manifest, assume the one we want is the first one
            string[] manifests = Directory.GetFiles(BuildDeployPrefs.AbsoluteBuildDirectory, "Package.appxmanifest", SearchOption.AllDirectories);
            if (manifests.Length == 0)
            {
                Debug.LogError("Unable to find Package.appxmanifest file for build (in path - " + BuildDeployPrefs.AbsoluteBuildDirectory + ")");
                return;
            }
            string manifest = manifests[0];

            XElement rootNode = XElement.Load(manifest);
            XNamespace ns = rootNode.GetDefaultNamespace();
            var identityNode = rootNode.Element(ns + "Identity");
            if (identityNode == null)
            {
                Debug.LogError("Package.appxmanifest for build (in path - " + BuildDeployPrefs.AbsoluteBuildDirectory + ") is missing an <Identity /> node");
                return;
            }

            // We use XName.Get instead of string -> XName implicit conversion because
            // when we pass in the string "Version", the program doesn't find the attribute.
            // Best guess as to why this happens is that implicit string conversion doesn't set the namespace to empty
            var versionAttr = identityNode.Attribute(XName.Get("Version"));
            if (versionAttr == null)
            {
                Debug.LogError("Package.appxmanifest for build (in path - " + BuildDeployPrefs.AbsoluteBuildDirectory + ") is missing a version attribute in the <Identity /> node.");
                return;
            }

            // Assume package version always has a '.'.
            // According to https://msdn.microsoft.com/en-us/library/windows/apps/br211441.aspx
            // Package versions are always of the form Major.Minor.Build.Revision
            var version = new Version(versionAttr.Value);
            var newVersion = new Version(version.Major, version.Minor, version.Build, version.Revision + 1);

            versionAttr.Value = newVersion.ToString();
            rootNode.Save(manifest);
        }
    }
}
