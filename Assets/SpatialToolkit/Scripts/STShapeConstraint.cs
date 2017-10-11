using UnityEngine;
using HoloToolkit.Unity;

[System.Serializable]
public class STShapeConstraint
{
    public SpatialUnderstandingDllShapes.ShapeComponentConstraintType Type;

    public float FloatValue1;
    public float FloatValue2;

    public string StringValue1;
    public string StringValue2;

    public int IntValue1;
    public int IntValue2;

#if UNITY_EDITOR
    [HideInInspector]
    public bool foldedOut = false;
#endif

    public SpatialUnderstandingDllShapes.ShapeComponentConstraint ToNativeConstraint()
    {
        switch (Type)
        {
            case SpatialUnderstandingDllShapes.ShapeComponentConstraintType.SurfaceNotPartOfShape:
                return SpatialUnderstandingDllShapes.ShapeComponentConstraint.Create_SurfaceNotPartOfShape(StringValue1);

            case SpatialUnderstandingDllShapes.ShapeComponentConstraintType.SurfaceHeight_Min:
                return SpatialUnderstandingDllShapes.ShapeComponentConstraint.Create_SurfaceHeight_Min(FloatValue1);

            case SpatialUnderstandingDllShapes.ShapeComponentConstraintType.SurfaceHeight_Max:
                return SpatialUnderstandingDllShapes.ShapeComponentConstraint.Create_SurfaceHeight_Max(FloatValue1);

            case SpatialUnderstandingDllShapes.ShapeComponentConstraintType.SurfaceHeight_Between:
                return SpatialUnderstandingDllShapes.ShapeComponentConstraint.Create_SurfaceHeight_Between(FloatValue1, FloatValue2);

            case SpatialUnderstandingDllShapes.ShapeComponentConstraintType.SurfaceHeight_Is:
                return SpatialUnderstandingDllShapes.ShapeComponentConstraint.Create_SurfaceHeight_Is(FloatValue1);

            case SpatialUnderstandingDllShapes.ShapeComponentConstraintType.SurfaceCount_Min:
                return SpatialUnderstandingDllShapes.ShapeComponentConstraint.Create_SurfaceCount_Min(IntValue1);

            case SpatialUnderstandingDllShapes.ShapeComponentConstraintType.SurfaceCount_Max:
                return SpatialUnderstandingDllShapes.ShapeComponentConstraint.Create_SurfaceCount_Max(IntValue1);

            case SpatialUnderstandingDllShapes.ShapeComponentConstraintType.SurfaceCount_Between:
                return SpatialUnderstandingDllShapes.ShapeComponentConstraint.Create_SurfaceCount_Between(IntValue1, IntValue2);

            case SpatialUnderstandingDllShapes.ShapeComponentConstraintType.SurfaceCount_Is:
                return SpatialUnderstandingDllShapes.ShapeComponentConstraint.Create_SurfaceCount_Is(IntValue1);

            case SpatialUnderstandingDllShapes.ShapeComponentConstraintType.SurfaceArea_Min:
                return SpatialUnderstandingDllShapes.ShapeComponentConstraint.Create_SurfaceArea_Min(FloatValue1);

            case SpatialUnderstandingDllShapes.ShapeComponentConstraintType.SurfaceArea_Max:
                return SpatialUnderstandingDllShapes.ShapeComponentConstraint.Create_SurfaceArea_Max(FloatValue1);

            case SpatialUnderstandingDllShapes.ShapeComponentConstraintType.SurfaceArea_Between:
                return SpatialUnderstandingDllShapes.ShapeComponentConstraint.Create_SurfaceArea_Between(FloatValue1, FloatValue2);

            case SpatialUnderstandingDllShapes.ShapeComponentConstraintType.SurfaceArea_Is:
                return SpatialUnderstandingDllShapes.ShapeComponentConstraint.Create_SurfaceArea_Is(FloatValue1);

            case SpatialUnderstandingDllShapes.ShapeComponentConstraintType.IsRectangle:
                return SpatialUnderstandingDllShapes.ShapeComponentConstraint.Create_IsRectangle(FloatValue1);

            case SpatialUnderstandingDllShapes.ShapeComponentConstraintType.RectangleSize_Min:
                return SpatialUnderstandingDllShapes.ShapeComponentConstraint.Create_RectangleSize_Min(FloatValue1, FloatValue2);

            case SpatialUnderstandingDllShapes.ShapeComponentConstraintType.RectangleSize_Max:
                return SpatialUnderstandingDllShapes.ShapeComponentConstraint.Create_RectangleSize_Max(FloatValue1, FloatValue2);

            case SpatialUnderstandingDllShapes.ShapeComponentConstraintType.RectangleSize_Between:
                //return SpatialUnderstandingDllShapes.ShapeComponentConstraint.Create_RectangleSize_Between(FloatValue1, FloatValue2, FloatValue3, FloatValue4);
                throw new System.Exception("Please use RectangleLength and RectangleWidth Between instead of RectangleSize_Between.");

            case SpatialUnderstandingDllShapes.ShapeComponentConstraintType.RectangleSize_Is:
                return SpatialUnderstandingDllShapes.ShapeComponentConstraint.Create_RectangleSize_Is(FloatValue1, FloatValue2);

            case SpatialUnderstandingDllShapes.ShapeComponentConstraintType.RectangleLength_Min:
                return SpatialUnderstandingDllShapes.ShapeComponentConstraint.Create_RectangleLength_Min(FloatValue1);

            case SpatialUnderstandingDllShapes.ShapeComponentConstraintType.RectangleLength_Max:
                return SpatialUnderstandingDllShapes.ShapeComponentConstraint.Create_RectangleLength_Max(FloatValue1);

            case SpatialUnderstandingDllShapes.ShapeComponentConstraintType.RectangleLength_Between:
                return SpatialUnderstandingDllShapes.ShapeComponentConstraint.Create_RectangleLength_Between(FloatValue1, FloatValue2);

            case SpatialUnderstandingDllShapes.ShapeComponentConstraintType.RectangleLength_Is:
                return SpatialUnderstandingDllShapes.ShapeComponentConstraint.Create_RectangleLength_Is(FloatValue1);

            case SpatialUnderstandingDllShapes.ShapeComponentConstraintType.RectangleWidth_Min:
                return SpatialUnderstandingDllShapes.ShapeComponentConstraint.Create_RectangleWidth_Min(FloatValue1);

            case SpatialUnderstandingDllShapes.ShapeComponentConstraintType.RectangleWidth_Max:
                return SpatialUnderstandingDllShapes.ShapeComponentConstraint.Create_RectangleWidth_Max(FloatValue1);

            case SpatialUnderstandingDllShapes.ShapeComponentConstraintType.RectangleWidth_Between:
                return SpatialUnderstandingDllShapes.ShapeComponentConstraint.Create_RectangleWidth_Between(FloatValue1, FloatValue2);

            case SpatialUnderstandingDllShapes.ShapeComponentConstraintType.RectangleWidth_Is:
                return SpatialUnderstandingDllShapes.ShapeComponentConstraint.Create_RectangleWidth_Is(FloatValue1);

            case SpatialUnderstandingDllShapes.ShapeComponentConstraintType.IsSquare:
                return SpatialUnderstandingDllShapes.ShapeComponentConstraint.Create_IsSquare(FloatValue1);

            case SpatialUnderstandingDllShapes.ShapeComponentConstraintType.SquareSize_Min:
                return SpatialUnderstandingDllShapes.ShapeComponentConstraint.Create_SquareSize_Min(FloatValue1);

            case SpatialUnderstandingDllShapes.ShapeComponentConstraintType.SquareSize_Max:
                return SpatialUnderstandingDllShapes.ShapeComponentConstraint.Create_SquareSize_Max(FloatValue1);

            case SpatialUnderstandingDllShapes.ShapeComponentConstraintType.SquareSize_Between:
                return SpatialUnderstandingDllShapes.ShapeComponentConstraint.Create_SquareSize_Between(FloatValue1, FloatValue2);

            case SpatialUnderstandingDllShapes.ShapeComponentConstraintType.SquareSize_Is:
                return SpatialUnderstandingDllShapes.ShapeComponentConstraint.Create_SquareSize_Is(FloatValue1);

            case SpatialUnderstandingDllShapes.ShapeComponentConstraintType.IsCircle:
                return SpatialUnderstandingDllShapes.ShapeComponentConstraint.Create_IsCircle(FloatValue1);

            case SpatialUnderstandingDllShapes.ShapeComponentConstraintType.CircleRadius_Min:
                return SpatialUnderstandingDllShapes.ShapeComponentConstraint.Create_CircleRadius_Min(FloatValue1);

            case SpatialUnderstandingDllShapes.ShapeComponentConstraintType.CircleRadius_Max:
                return SpatialUnderstandingDllShapes.ShapeComponentConstraint.Create_CircleRadius_Max(FloatValue1);

            case SpatialUnderstandingDllShapes.ShapeComponentConstraintType.CircleRadius_Between:
                return SpatialUnderstandingDllShapes.ShapeComponentConstraint.Create_CircleRadius_Between(FloatValue1, FloatValue2);

            case SpatialUnderstandingDllShapes.ShapeComponentConstraintType.CircleRadius_Is:
                return SpatialUnderstandingDllShapes.ShapeComponentConstraint.Create_CircleRadius_Is(FloatValue1);

            default:
                return SpatialUnderstandingDllShapes.ShapeComponentConstraint.Create_IsRectangle();
        }
    }
}