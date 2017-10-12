using UnityEngine;
using HoloToolkit.Unity;
using System;

[System.Serializable]
public class STShapeConstraint
{
    public SpatialUnderstandingDllShapes.ShapeConstraintType Type;

    public float FloatValue1;

    public STShapeComponent ComponentA;
    public STShapeComponent ComponentB;

#if UNITY_EDITOR
    [HideInInspector]
    public bool foldedOut = false;
#endif

    [NonSerialized]
    STShapeComponent _parent;

    public STShapeConstraint(STShapeComponent parent)
    {
        _parent = parent;
    }

    public SpatialUnderstandingDllShapes.ShapeConstraint ToNativeConstraint()
    {
        switch (Type)
        {
            case SpatialUnderstandingDllShapes.ShapeConstraintType.AwayFromWalls:
                return SpatialUnderstandingDllShapes.ShapeConstraint.Create_AwayFromWalls();
            case SpatialUnderstandingDllShapes.ShapeConstraintType.RectanglesParallel:
                return SpatialUnderstandingDllShapes.ShapeConstraint.Create_RectanglesParallel(GetComponentIndex(ComponentA), GetComponentIndex(ComponentB));
            case SpatialUnderstandingDllShapes.ShapeConstraintType.RectanglesPerpendicular:
                return SpatialUnderstandingDllShapes.ShapeConstraint.Create_RectanglesPerpendicular(GetComponentIndex(ComponentA), GetComponentIndex(ComponentB));
            case SpatialUnderstandingDllShapes.ShapeConstraintType.RectanglesAligned:
                return SpatialUnderstandingDllShapes.ShapeConstraint.Create_RectanglesAligned(GetComponentIndex(ComponentA), GetComponentIndex(ComponentB), FloatValue1);
            case SpatialUnderstandingDllShapes.ShapeConstraintType.RectanglesSameLength:
                return SpatialUnderstandingDllShapes.ShapeConstraint.Create_RectanglesSameLength(GetComponentIndex(ComponentA), GetComponentIndex(ComponentB), FloatValue1);
            case SpatialUnderstandingDllShapes.ShapeConstraintType.AtFrontOf:
                return SpatialUnderstandingDllShapes.ShapeConstraint.Create_AtFrontOf(GetComponentIndex(ComponentA), GetComponentIndex(ComponentB));
            case SpatialUnderstandingDllShapes.ShapeConstraintType.AtBackOf:
                return SpatialUnderstandingDllShapes.ShapeConstraint.Create_AtBackOf(GetComponentIndex(ComponentA), GetComponentIndex(ComponentB));
            case SpatialUnderstandingDllShapes.ShapeConstraintType.AtLeftOf:
                return SpatialUnderstandingDllShapes.ShapeConstraint.Create_AtLeftOf(GetComponentIndex(ComponentA), GetComponentIndex(ComponentB));
            case SpatialUnderstandingDllShapes.ShapeConstraintType.AtRightOf:
                return SpatialUnderstandingDllShapes.ShapeConstraint.Create_AtRightOf(GetComponentIndex(ComponentA), GetComponentIndex(ComponentB));

            default:
            case SpatialUnderstandingDllShapes.ShapeConstraintType.NoOtherSurface:
                return SpatialUnderstandingDllShapes.ShapeConstraint.Create_NoOtherSurface();
        }
    }

    int GetComponentIndex(STShapeComponent component)
    {
        return _parent.GetComponentIndex(component);
    }
}