using HoloToolkit.Unity;
using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class STShape
{


    public string ShapeName;
    public List<STShapeComponent> Components;

#if UNITY_EDITOR
    [HideInInspector]
    public bool foldedOut = false;
#endif

    public void Register()
    {
        List<SpatialUnderstandingDllShapes.ShapeComponent> compList = new List<SpatialUnderstandingDllShapes.ShapeComponent>();

        foreach(STShapeComponent c in Components)
        {
            compList.Add(new SpatialUnderstandingDllShapes.ShapeComponent(c.GetConstraints()));
        }

        IntPtr componentsPtr = SpatialUnderstanding.Instance.UnderstandingDLL.PinObject(compList);

        if(SpatialUnderstandingDllShapes.AddShape(ShapeName, compList.Count, componentsPtr, 0, IntPtr.Zero) == 0)
        {
            Debug.LogError("Failed to create custom shape");
        }
    }
}
