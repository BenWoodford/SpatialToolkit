using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STPlacementConstraint {

    public SpatialUnderstandingDllObjectPlacement.ObjectPlacementConstraint.ObjectPlacementConstraintType Type;

#if UNITY_EDITOR
    [HideInInspector]
    public bool foldedOut = false;
#endif

    /// <summary>
    /// Position for Near/AwayFromPoint constraints
    /// </summary>
    public Vector3 Position;

    /// <summary>
    /// Minimum Distance from constraint target
    /// </summary>
    public float MinDistance = 0f;

    /// <summary>
    /// Maximum Distance from constraint target
    /// </summary>
    public float MaxDistance = 10f;

    /// <summary>
    /// Minimum wall height for NearWall queries
    /// </summary>
    public float MinWallHeight = 0f;

    /// <summary>
    /// Should the constraint take virtual walls into consideration?
    /// </summary>
    public bool IncludeVirtual = false;

    public STPlacementConstraint()
    {

    }

    public SpatialUnderstandingDllObjectPlacement.ObjectPlacementConstraint ToNativeConstraint()
    {
        switch (Type)
        {
            case SpatialUnderstandingDllObjectPlacement.ObjectPlacementConstraint.ObjectPlacementConstraintType.Constraint_NearPoint:
                return SpatialUnderstandingDllObjectPlacement.ObjectPlacementConstraint.Create_NearPoint(Position, MinDistance, MaxDistance);
            case SpatialUnderstandingDllObjectPlacement.ObjectPlacementConstraint.ObjectPlacementConstraintType.Constraint_NearWall:
                return SpatialUnderstandingDllObjectPlacement.ObjectPlacementConstraint.Create_NearWall(MinDistance, MaxDistance, MinWallHeight, IncludeVirtual);
            case SpatialUnderstandingDllObjectPlacement.ObjectPlacementConstraint.ObjectPlacementConstraintType.Constraint_NearCenter:
                return SpatialUnderstandingDllObjectPlacement.ObjectPlacementConstraint.Create_NearCenter(MinDistance, MaxDistance);
            case SpatialUnderstandingDllObjectPlacement.ObjectPlacementConstraint.ObjectPlacementConstraintType.Constraint_AwayFromOtherObjects:
                return SpatialUnderstandingDllObjectPlacement.ObjectPlacementConstraint.Create_AwayFromOtherObjects();
            case SpatialUnderstandingDllObjectPlacement.ObjectPlacementConstraint.ObjectPlacementConstraintType.Constraint_AwayFromPoint:
                return SpatialUnderstandingDllObjectPlacement.ObjectPlacementConstraint.Create_AwayFromPoint(Position);

            default:
            case SpatialUnderstandingDllObjectPlacement.ObjectPlacementConstraint.ObjectPlacementConstraintType.Constraint_AwayFromWalls:
                return SpatialUnderstandingDllObjectPlacement.ObjectPlacementConstraint.Create_AwayFromWalls();
        }
    }
}
