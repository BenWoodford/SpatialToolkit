using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class STShapeComponent
{
    public string ComponentName;
    public List<STShapeConstraint> Constraints;

    public List<HoloToolkit.Unity.SpatialUnderstandingDllShapes.ShapeComponentConstraint> GetConstraints()
    {
        List<HoloToolkit.Unity.SpatialUnderstandingDllShapes.ShapeComponentConstraint> ret = new List<HoloToolkit.Unity.SpatialUnderstandingDllShapes.ShapeComponentConstraint>();

        foreach (STShapeConstraint c in Constraints)
        {
            ret.Add(c.ToNativeConstraint());
        }

        return ret;
    }
}