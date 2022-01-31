using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BoolVariable))]
public class BoolVariableInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        BoolVariable variable = (BoolVariable)target;

        GUILayout.BeginHorizontal("box");
        GUILayout.Label($"Runtime Value: {variable.RuntimeValue}");
        if (GUILayout.Button("True"))
        {
            variable.SetValue(true);
        }
        if (GUILayout.Button("False"))
        {
            variable.SetValue(false);
        }
        GUILayout.EndHorizontal();
    }
}
