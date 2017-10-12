using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using System;

public class STPlacementQuery
{
    public SpatialUnderstandingDllObjectPlacement.ObjectPlacementResult Result;

    public readonly STPlacementType Type;

    static bool solverIsInit = false;

    bool _hasRun = false;

    public string QueryName { get; set; }

    public bool HasRun { get { return _hasRun; } }

    public List<STPlacementRule> Rules { get; set; }
    public List<STPlacementConstraint> Constraints { get; set; }

    /// <summary>
    /// Minimum height for wall placement only
    /// </summary>
    public float MinHeight { get; set; }

    /// <summary>
    /// Maximum height for wall placement only
    /// </summary>
    public float MaxHeight { get; set; }

    /// <summary>
    /// Target shape for placement on a shape
    /// </summary>
    public STShape TargetShape { get; set; }

    /// <summary>
    /// Target component for placement on a shape's component
    /// </summary>
    public STShapeComponent TargetShapeComponent { get; set; }

    public STPlacementQuery(string queryName, STPlacementType type, List<STPlacementRule> rules = null, List<STPlacementConstraint> constraints = null)
    {
        QueryName = queryName;
        Type = type;

        if (rules != null)
            Rules = rules;
        else
            Rules = new List<STPlacementRule>();

        if (constraints != null)
            Constraints = constraints;
        else
            Constraints = new List<STPlacementConstraint>();
    }

    /// <summary>
    /// Place given object
    /// </summary>
    /// <param name="placementObject">The object to place</param>
    /// <param name="requiredSpace">The bounds of the object to place</param>
    /// <param name="requiredSurfaceSpace">The bounds of the bottom of the shape to place, only used for edge and floor/ceiling placement</param>
    /// <returns></returns>
    public STPlacementResult Place(GameObject placementObject, Vector3 requiredSpace, Vector3 requiredSurfaceSpace = default(Vector3))
    {
        if (!solverIsInit)
        {
            solverIsInit = SpatialUnderstandingDllObjectPlacement.Solver_Init() == 1;
        }

        Result = new SpatialUnderstandingDllObjectPlacement.ObjectPlacementResult();

        List<SpatialUnderstandingDllObjectPlacement.ObjectPlacementRule> nativeRules = new List<SpatialUnderstandingDllObjectPlacement.ObjectPlacementRule>();
        List<SpatialUnderstandingDllObjectPlacement.ObjectPlacementConstraint> nativeConstraints = new List<SpatialUnderstandingDllObjectPlacement.ObjectPlacementConstraint>();

        foreach(STPlacementRule rule in Rules)
        {
            nativeRules.Add(rule.ToNativeRule());
        }

        foreach(STPlacementConstraint con in Constraints)
        {
            nativeConstraints.Add(con.ToNativeConstraint());
        }

        IntPtr rulePtr = SpatialUnderstanding.Instance.UnderstandingDLL.PinObject(nativeRules.ToArray());
        IntPtr constraintPtr = SpatialUnderstanding.Instance.UnderstandingDLL.PinObject(nativeConstraints.ToArray());
        IntPtr definitionPtr = IntPtr.Zero;

        switch (Type)
        {
            case STPlacementType.Floor:
                definitionPtr = SpatialUnderstanding.Instance.UnderstandingDLL.PinObject(SpatialUnderstandingDllObjectPlacement.ObjectPlacementDefinition.Create_OnFloor(requiredSpace));
                break;

            case STPlacementType.Wall:
                definitionPtr = SpatialUnderstanding.Instance.UnderstandingDLL.PinObject(SpatialUnderstandingDllObjectPlacement.ObjectPlacementDefinition.Create_OnWall(requiredSpace, MinHeight, MaxHeight));
                break;

            case STPlacementType.Ceiling:
                definitionPtr = SpatialUnderstanding.Instance.UnderstandingDLL.PinObject(SpatialUnderstandingDllObjectPlacement.ObjectPlacementDefinition.Create_OnCeiling(requiredSpace));
                break;

            case STPlacementType.Shape:
                definitionPtr = SpatialUnderstanding.Instance.UnderstandingDLL.PinObject(SpatialUnderstandingDllObjectPlacement.ObjectPlacementDefinition.Create_OnShape(requiredSpace, TargetShape.ShapeName, TargetShape.GetComponentIndex(TargetShapeComponent)));
                break;

            case STPlacementType.Edge:
                definitionPtr = SpatialUnderstanding.Instance.UnderstandingDLL.PinObject(SpatialUnderstandingDllObjectPlacement.ObjectPlacementDefinition.Create_OnEdge(requiredSpace, requiredSurfaceSpace));
                break;

            case STPlacementType.FloorAndCeiling:
                definitionPtr = SpatialUnderstanding.Instance.UnderstandingDLL.PinObject(SpatialUnderstandingDllObjectPlacement.ObjectPlacementDefinition.Create_OnFloorAndCeiling(requiredSpace, requiredSurfaceSpace));
                break;

            case STPlacementType.RandomInAir:
                definitionPtr = SpatialUnderstanding.Instance.UnderstandingDLL.PinObject(SpatialUnderstandingDllObjectPlacement.ObjectPlacementDefinition.Create_RandomInAir(requiredSpace));
                break;

            case STPlacementType.MidAir:
                definitionPtr = SpatialUnderstanding.Instance.UnderstandingDLL.PinObject(SpatialUnderstandingDllObjectPlacement.ObjectPlacementDefinition.Create_InMidAir(requiredSpace));
                break;

            case STPlacementType.UnderPlatformEdge:
                definitionPtr = SpatialUnderstanding.Instance.UnderstandingDLL.PinObject(SpatialUnderstandingDllObjectPlacement.ObjectPlacementDefinition.Create_UnderPlatformEdge(requiredSpace));
                break;

            default:
                return null;
        }

        if(SpatialUnderstandingDllObjectPlacement.Solver_PlaceObject(QueryName + "_" + placementObject.name, definitionPtr, Rules.Count, rulePtr, Constraints.Count, constraintPtr, SpatialUnderstanding.Instance.UnderstandingDLL.GetStaticObjectPlacementResultPtr()) > 0)
        {
            _hasRun = true;
            Debug.Log("Placing object for query " + QueryName);

            Result = SpatialUnderstanding.Instance.UnderstandingDLL.GetStaticObjectPlacementResult();

            GameObject resultantObject = GameObject.Instantiate(placementObject, Result.Position, Quaternion.LookRotation(Result.Forward, Result.Up));
            if (resultantObject.GetComponent<STPlacedObject>() == null) {
                STPlacedObject placedObject = resultantObject.AddComponent<STPlacedObject>();
                placedObject.PlacedObjectName = QueryName + "_" + placementObject.name;
            }
            // TODO: Add a component to the object to remove it from Spatial Understanding if removed in Unity.
            return new STPlacementResult(resultantObject, Result);
        } else
        {
            Debug.Log("Failed to place object for query " + QueryName);
            _hasRun = true;
            return null;
        }
    }
}
