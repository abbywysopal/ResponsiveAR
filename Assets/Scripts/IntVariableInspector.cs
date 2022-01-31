using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(IntVariable))]
public class IntVariableInspector : Editor
{
    int currentValue;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        IntVariable variable = (IntVariable)target;

        GUILayout.BeginHorizontal("box");
        GUILayout.Label($"Runtime Value: {variable.RuntimeValue}");

        currentValue = variable.RuntimeValue;
        int newValue = EditorGUILayout.IntField("Set Value:", currentValue);
        if (newValue != currentValue)
        {
            variable.SetValue(newValue);
        }
        GUILayout.EndHorizontal();
    }
}
