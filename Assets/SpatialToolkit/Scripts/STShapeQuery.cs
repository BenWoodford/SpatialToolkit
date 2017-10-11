using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using System;

public class STShapeQuery {
    public SpatialUnderstandingDllShapes.ShapeResult[] Results;

    public readonly STShapeQueryType Type;

    bool _hasRun = false;

    public string ShapeName;

    public bool HasRun { get { return _hasRun; } }

    const int _queryMaxResultCount = 512;

    float _minRadius = 0f;

    public STShapeQuery(STShapeQueryType type, string shapeName, float minRadiusForShapePositions = 0)
    {
        ShapeName = shapeName;
        Type = type;
        _minRadius = minRadiusForShapePositions;
    }

    public int RunQuery()
    {
        Results = new SpatialUnderstandingDllShapes.ShapeResult[_queryMaxResultCount];

        IntPtr resultsShapePtr = SpatialUnderstanding.Instance.UnderstandingDLL.PinObject(Results);

        int shapeCount = 0;

        if (String.IsNullOrEmpty(ShapeName))
        {
            Debug.LogError("Empty shape name, cannot run query.");
            return 0;
        }

        switch (Type)
        {
            case STShapeQueryType.ShapeBounds:
                shapeCount = SpatialUnderstandingDllShapes.QueryShape_FindShapeHalfDims(ShapeName, Results.Length, resultsShapePtr);
                break;

            case STShapeQueryType.PositionsOnShape:
                shapeCount = SpatialUnderstandingDllShapes.QueryShape_FindPositionsOnShape(ShapeName, _minRadius, Results.Length, resultsShapePtr);
                break;
        }

        List<SpatialUnderstandingDllShapes.ShapeResult> resultTemp = new List<SpatialUnderstandingDllShapes.ShapeResult>();

        for(int i = 0; i < shapeCount; i++)
        {
            resultTemp.Add(Results[i]);
        }

        Results = resultTemp.ToArray();

        _hasRun = true;

        return shapeCount;
    }
}
