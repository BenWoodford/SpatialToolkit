using HoloToolkit.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class STShape
{


    public string ShapeName;
    public List<STShapeComponent> Components;

    public List<STShapeConstraint> Constraints;

#if UNITY_EDITOR
    [HideInInspector]
    public bool foldedOut = false;
#endif

    public void Register()
    {
        List<SpatialUnderstandingDllShapes.ShapeComponent> compList = new List<SpatialUnderstandingDllShapes.ShapeComponent>();
        List<SpatialUnderstandingDllShapes.ShapeConstraint> conList = new List<SpatialUnderstandingDllShapes.ShapeConstraint>();

        foreach(STShapeComponent c in Components)
        {
            compList.Add(new SpatialUnderstandingDllShapes.ShapeComponent(c.GetConstraints()));
        }

        foreach(STShapeConstraint c in Constraints)
        {
            conList.Add(c.ToNativeConstraint());
        }

        IntPtr componentsPtr = SpatialUnderstanding.Instance.UnderstandingDLL.PinObject(compList.ToArray());
        IntPtr constraintsPtr = SpatialUnderstanding.Instance.UnderstandingDLL.PinObject(conList.ToArray());

        if(SpatialUnderstandingDllShapes.AddShape(ShapeName, compList.Count, componentsPtr, conList.Count, constraintsPtr) == 0)
        {
            Debug.LogError("Failed to create custom shape");
        } else
        {
            Debug.Log("Added Shape " + ShapeName + " with " + Components.Count + " components and " + Constraints.Count + " constraints");
        }
    }

    public int GetComponentIndex(STShapeComponent component)
    {
        return Components.IndexOf(component);
    }

    public STShapeComponent GetComponent(string componentName)
    {
        return Components.Single(c => c.ComponentName == componentName);
    }
}
