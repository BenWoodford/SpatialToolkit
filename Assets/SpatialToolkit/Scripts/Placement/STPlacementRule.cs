using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;

public class STPlacementRule {
    public SpatialUnderstandingDllObjectPlacement.ObjectPlacementRule.ObjectPlacementRuleType Type;

#if UNITY_EDITOR
    [HideInInspector]
    public bool foldedOut = false;
#endif

    /// <summary>
    /// Only used for AwayFromPosition rule
    /// </summary>
    public Vector3 Position;

    /// <summary>
    /// Minimum distance from position/walls/other objects
    /// </summary>
    public float MinDistance = 0f;

    /// <summary>
    /// Minimum wall height for AwayFromWalls query
    /// </summary>
    public float MinWallHeight = 0f;

    public STPlacementRule()
    {

    }

    public SpatialUnderstandingDllObjectPlacement.ObjectPlacementRule ToNativeRule()
    {
        switch (Type)
        {
            case SpatialUnderstandingDllObjectPlacement.ObjectPlacementRule.ObjectPlacementRuleType.Rule_AwayFromPosition:
                return SpatialUnderstandingDllObjectPlacement.ObjectPlacementRule.Create_AwayFromPosition(Position, MinDistance);
            case SpatialUnderstandingDllObjectPlacement.ObjectPlacementRule.ObjectPlacementRuleType.Rule_AwayFromWalls:
                return SpatialUnderstandingDllObjectPlacement.ObjectPlacementRule.Create_AwayFromWalls(MinDistance, MinWallHeight);

            default:
            case SpatialUnderstandingDllObjectPlacement.ObjectPlacementRule.ObjectPlacementRuleType.Rule_AwayFromOtherObjects:
                return SpatialUnderstandingDllObjectPlacement.ObjectPlacementRule.Create_AwayFromOtherObjects(MinDistance);

        }
    }
}
