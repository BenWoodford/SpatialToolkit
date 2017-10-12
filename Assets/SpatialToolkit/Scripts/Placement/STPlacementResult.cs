using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
public class STPlacementResult {
    public SpatialUnderstandingDllObjectPlacement.ObjectPlacementResult ObjectPlacementResult { get; private set; }
    public GameObject PlacedObject { get; private set; }

    public STPlacementResult(GameObject newObject, SpatialUnderstandingDllObjectPlacement.ObjectPlacementResult result)
    {
        PlacedObject = newObject;
        ObjectPlacementResult = result;
    }
}
