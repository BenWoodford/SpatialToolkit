using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System;
using System.Linq;

[CustomEditor(typeof(SpatialToolkit))]
public class SpatialToolkitEditor : Editor {
    Dictionary<STShape, bool> foldouts;

    private void OnEnable()
    {
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SpatialToolkit tk = (SpatialToolkit)target;

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Shapes", EditorStyles.boldLabel);

        if (GUILayout.Button("+", GUILayout.ExpandWidth(false)))
        {
            tk.Shapes.Add(new STShape() { ShapeName = "New Shape" });
        }

        EditorGUILayout.EndHorizontal();

        for (int i = 0; i < tk.Shapes.Count; i++)
        {
            if (tk.Shapes.Count - 1 < i)
                break;

            STShape shape = tk.Shapes[i];

            shape.foldedOut = EditorGUILayout.Foldout(shape.foldedOut, shape.ShapeName);

            if (shape.foldedOut)
            {
                EditorGUILayout.BeginHorizontal();

                shape.ShapeName = EditorGUILayout.TextField("Shape Name", shape.ShapeName, GUILayout.ExpandWidth(true));

                if (GUILayout.Button("-", GUILayout.ExpandWidth(false)))
                {
                    tk.Shapes.RemoveAt(i);
                }

                EditorGUILayout.EndHorizontal();


                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Components", EditorStyles.boldLabel);

                if (GUILayout.Button("+", GUILayout.ExpandWidth(false)))
                {
                    if (shape.Components == null)
                        shape.Components = new List<STShapeComponent>();
                    shape.Components.Add(new STShapeComponent(shape) { ComponentName = "New Component" });
                }

                EditorGUILayout.EndHorizontal();

                if (shape.Components != null)
                {
                    for (int j = 0; j < shape.Components.Count; j++)
                    {
                        if (shape.Components.Count - 1 < j)
                            break;

                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                        STShapeComponent component = shape.Components[j];

                        component.ComponentName = EditorGUILayout.TextField(component.ComponentName, GUILayout.ExpandWidth(true));

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Constraints", EditorStyles.boldLabel);

                        if (GUILayout.Button("+", GUILayout.ExpandWidth(false)))
                        {
                            if (component.Constraints == null)
                                component.Constraints = new List<STShapeComponentConstraint>();
                            component.Constraints.Add(new STShapeComponentConstraint() { Type = HoloToolkit.Unity.SpatialUnderstandingDllShapes.ShapeComponentConstraintType.IsRectangle });
                        }

                        EditorGUILayout.EndHorizontal();

                        if (component.Constraints != null)
                        {

                            for (int p = 0; p < component.Constraints.Count; p++)
                            {
                                if (component.Constraints.Count - 1 < p)
                                    break;

                                STShapeComponentConstraint constraint = component.Constraints[p];

                                EditorGUILayout.BeginVertical(new GUIStyle() { margin = new RectOffset(10, 10, 10, 10) });

                                EditorGUILayout.BeginHorizontal();

                                constraint.Type = (HoloToolkit.Unity.SpatialUnderstandingDllShapes.ShapeComponentConstraintType)EditorGUILayout.EnumPopup(constraint.Type, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(false));

                                if (GUILayout.Button("-", GUILayout.ExpandWidth(false)))
                                {
                                    component.Constraints.RemoveAt(p);
                                }

                                EditorGUILayout.EndHorizontal();

                                HandleConstraintGUI(constraint);

                                EditorGUILayout.EndVertical();
                            }
                        }

                        EditorGUILayout.EndVertical();
                    }
                }
            }
        }
    }

    void HandleConstraintGUI(STShapeComponentConstraint constraint)
    {
        EditorGUILayout.BeginVertical(new GUIStyle() { margin = new RectOffset(10, 10, 10, 10) });
        constraint.foldedOut = EditorGUILayout.Foldout(constraint.foldedOut, "Settings");

        EditorGUILayout.BeginVertical();

        if (constraint.foldedOut)
        {

            switch (constraint.Type)
            {
                case HoloToolkit.Unity.SpatialUnderstandingDllShapes.ShapeComponentConstraintType.CircleRadius_Between:
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Radius", GUILayout.ExpandWidth(false));

                    constraint.FloatValue1 = EditorGUILayout.FloatField(constraint.FloatValue1, GUILayout.ExpandWidth(false), GUILayout.Width(50));
                    EditorGUILayout.MinMaxSlider(ref constraint.FloatValue1, ref constraint.FloatValue2, 0f, 100f, GUILayout.ExpandWidth(true));
                    constraint.FloatValue2 = EditorGUILayout.FloatField(constraint.FloatValue2, GUILayout.ExpandWidth(false), GUILayout.Width(50));
                    EditorGUILayout.EndHorizontal();
                    break;
            }
        }

        EditorGUILayout.EndVertical();

        EditorGUILayout.EndVertical();
    }
}
