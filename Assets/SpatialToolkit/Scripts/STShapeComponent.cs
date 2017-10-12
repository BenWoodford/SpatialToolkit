using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class STShapeComponent
{
    public string ComponentName;
    public List<STShapeComponentConstraint> Constraints;

    STShape _parent;

    public STShapeComponent(STShape parent)
    {
        _parent = parent;
    }

    public List<HoloToolkit.Unity.SpatialUnderstandingDllShapes.ShapeComponentConstraint> GetConstraints()
    {
        List<HoloToolkit.Unity.SpatialUnderstandingDllShapes.ShapeComponentConstraint> ret = new List<HoloToolkit.Unity.SpatialUnderstandingDllShapes.ShapeComponentConstraint>();

        foreach (STShapeComponentConstraint c in Constraints)
        {
            ret.Add(c.ToNativeConstraint());
        }

        return ret;
    }

    public int GetComponentIndex(STShapeComponent component)
    {
        return _parent.GetComponentIndex(component);
    }
}