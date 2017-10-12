using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class SpatialToolkit : MonoBehaviour, IInputClickHandler {

    [HideInInspector]
    public List<STShape> Shapes;

    public TextMesh InfoText;
    public TextMesh InfoSubText;
    public float minimumHorizontalScanArea = 25f;
    public float minimumWallScanArea = 10f;

    public SpatialToolkit Instance;

    public UnityEvent OnScanComplete;

    bool ScanMinimumMet
    {
        get
        {
            SpatialUnderstandingDll.Imports.PlayspaceStats stats = GetScanStats();
            return stats.HorizSurfaceArea >= minimumHorizontalScanArea && stats.WallSurfaceArea > minimumWallScanArea;
        }
    }

	// Use this for initialization
	void Start () {
        Instance = this;
        Shapes[0].Components[0].Constraints.Add(new STShapeComponentConstraint() { Type = SpatialUnderstandingDllShapes.ShapeComponentConstraintType.SurfaceArea_Min, FloatValue1 = 0.035f });
        Shapes[0].Components[0].Constraints.Add(new STShapeComponentConstraint() { Type = SpatialUnderstandingDllShapes.ShapeComponentConstraintType.SurfaceCount_Min, IntValue1 = 1 });

        InputManager.Instance.PushFallbackInputHandler(gameObject);
        RegisterShapes();
        ProcessScanState();
    }

    public STShape GetShape(string shapeName)
    {
        return Shapes.Single(s => s.ShapeName == shapeName);
    }

    private void Instance_ScanStateChanged()
    {
        ProcessScanState();
    }

    void ProcessScanState()
    {
        SpatialUnderstandingDll.Imports.PlayspaceStats stats;

        switch (SpatialUnderstanding.Instance.ScanState)
        {
            case SpatialUnderstanding.ScanStates.None:
                UpdateText("No scan state");
                break;
            case SpatialUnderstanding.ScanStates.ReadyToScan:
                UpdateText("Scanner Ready");
                break;
            case SpatialUnderstanding.ScanStates.Scanning:
                UpdateMidScanState();
                break;
            case SpatialUnderstanding.ScanStates.Finishing:
                stats = GetScanStats();
                UpdateText("Finishing Scan...", System.String.Format("Total Surface Area: {0:0.##}, Horizontal: {1:0.##}, Wall: {2:0.##}", stats.TotalSurfaceArea, stats.HorizSurfaceArea, stats.WallSurfaceArea));
                break;
            case SpatialUnderstanding.ScanStates.Done:
                stats = GetScanStats();
                UpdateText("Scan Complete!", System.String.Format("Total Surface Area: {0:0.##}, Horizontal: {1:0.##}, Wall: {2:0.##}", stats.TotalSurfaceArea, stats.HorizSurfaceArea, stats.WallSurfaceArea));

                if (OnScanComplete != null)
                    OnScanComplete.Invoke();
                break;
        }
    }

    void UpdateMidScanState()
    {
        SpatialUnderstandingDll.Imports.PlayspaceStats stats = GetScanStats();
        UpdateText(ScanMinimumMet ? "Air Tap to Finish Scan" : "Scanning...", System.String.Format("Total Surface Area: {0:0.##}, Horizontal: {1:0.##}, Wall: {2:0.##}", stats.TotalSurfaceArea, stats.HorizSurfaceArea, stats.WallSurfaceArea));
    }

    SpatialUnderstandingDll.Imports.PlayspaceStats GetScanStats()
    {
        System.IntPtr statsPtr = SpatialUnderstanding.Instance.UnderstandingDLL.GetStaticPlayspaceStatsPtr();

        if(SpatialUnderstandingDll.Imports.QueryPlayspaceStats(statsPtr) != 0)
        {
            SpatialUnderstandingDll.Imports.PlayspaceStats stats = SpatialUnderstanding.Instance.UnderstandingDLL.GetStaticPlayspaceStats();
            return stats;
        } else
        {
            return null;
        }

    }

    void UpdateText(string text, string detail = "")
    {
        if (InfoText != null)
        {
            InfoText.text = text;
        }

        if (InfoSubText != null) {
            InfoSubText.text = detail;
        }
    }

    void RegisterShapes()
    {
        foreach (STShape s in Shapes)
        {
            s.Register();
        }

        SpatialUnderstandingDllShapes.ActivateShapeAnalysis();
    }

    public void AddShape(STShape shape)
    {
        Shapes.Add(shape);
        shape.Register();
    }

    public void RefreshShapes()
    {
        HoloToolkit.Unity.SpatialUnderstandingDllShapes.RemoveAllShapes();
        RegisterShapes();
    }

    public void StartScanning()
    {
        SpatialUnderstanding.Instance.ScanStateChanged += Instance_ScanStateChanged;
        SpatialUnderstanding.Instance.RequestBeginScanning();
    }
	
	// Update is called once per frame
	void Update () {
		if(SpatialUnderstanding.Instance.ScanState == SpatialUnderstanding.ScanStates.Scanning)
        {
            UpdateMidScanState();
        }
	}

    public void OnInputClicked(InputClickedEventData eventData)
    {
        if(SpatialUnderstanding.Instance.ScanState == SpatialUnderstanding.ScanStates.Scanning && ScanMinimumMet)
        {
            SpatialUnderstanding.Instance.RequestFinishScan();
        }
    }
}
