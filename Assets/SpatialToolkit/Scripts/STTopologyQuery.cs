using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using System;

public class STTopologyQuery
{
    public SpatialUnderstandingDllTopology.TopologyResult[] Results;
    

    public readonly STTopologyQueryType Type;

    bool _hasRun = false;

    public bool HasRun { get { return _hasRun; } }

    const int _queryMaxResultCount = 512;

    /// <summary>
    /// Used by LargePositionsOnWalls, PositionsOnFloor and PositionsOnWalls query types
    /// </summary>
    public float MinHeight { get; set; }

    /// <summary>
    /// Used by LargePositionsOnWalls, LargestPositionsOnFloor and PositionsOnWalls query types
    /// </summary>
    public float MinWidth { get; set; }

    /// <summary>
    /// Used by LargePositionsOnWall and PositionsOnWalls query types
    /// </summary>
    public float Wall_MinHeightAboveFloor { get; set; }

    /// <summary>
    /// Used by LargePositionsOnWall, PositionsOnWalls and PositionsSittable query types
    /// </summary>
    public float MinFacingClearance { get; set; }

    /// <summary>
    /// Used only by PositionsSittable query type
    /// </summary>
    public float Sittable_MaxHeight { get; set; }

    /// <summary>
    /// Used only by PositionsOnFloor query type
    /// </summary>
    public float Floor_MinLength { get; set; }


    public STTopologyQuery(STTopologyQueryType type)
    {
        Type = type;
    }

    public int RunQuery()
    {
        Results = new SpatialUnderstandingDllTopology.TopologyResult[_queryMaxResultCount];
        SpatialUnderstandingDllTopology.TopologyResult SingleResult = new SpatialUnderstandingDllTopology.TopologyResult();


        IntPtr resultsTopologyPtr = SpatialUnderstanding.Instance.UnderstandingDLL.PinObject(Results);
        IntPtr singleResultPtr = SpatialUnderstanding.Instance.UnderstandingDLL.PinObject(SingleResult);

        int resultCount = 0;

        switch (Type)
        {
            case STTopologyQueryType.LargePositionsOnWalls:
                resultCount = SpatialUnderstandingDllTopology.QueryTopology_FindLargePositionsOnWalls(MinHeight, MinWidth, Wall_MinHeightAboveFloor, MinFacingClearance, Results.Length, resultsTopologyPtr);
                break;
            case STTopologyQueryType.LargestPositionsOnFloor:
                resultCount = SpatialUnderstandingDllTopology.QueryTopology_FindLargestPositionsOnFloor(Results.Length, resultsTopologyPtr);
                break;
            case STTopologyQueryType.LargestWall:
                resultCount = SpatialUnderstandingDllTopology.QueryTopology_FindLargestWall(singleResultPtr);
                Results = new SpatialUnderstandingDllTopology.TopologyResult[] { SingleResult };
                break;
            case STTopologyQueryType.PositionsOnFloor:
                resultCount = SpatialUnderstandingDllTopology.QueryTopology_FindPositionsOnFloor(Floor_MinLength, MinWidth, Results.Length, resultsTopologyPtr);
                break;
            case STTopologyQueryType.PositionsOnWalls:
                resultCount = SpatialUnderstandingDllTopology.QueryTopology_FindPositionsOnWalls(MinHeight, MinWidth, Wall_MinHeightAboveFloor, MinFacingClearance, Results.Length, resultsTopologyPtr);
                break;
            case STTopologyQueryType.PositionsSittable:
                resultCount = SpatialUnderstandingDllTopology.QueryTopology_FindPositionsSittable(MinHeight, Sittable_MaxHeight, MinFacingClearance, Results.Length, resultsTopologyPtr);
                break;
        }

        List<SpatialUnderstandingDllTopology.TopologyResult> resultTemp = new List<SpatialUnderstandingDllTopology.TopologyResult>();

        for (int i = 0; i < resultCount; i++)
        {
            resultTemp.Add(Results[i]);
        }

        Results = resultTemp.ToArray();

        _hasRun = true;

        return resultCount;
    }
}
