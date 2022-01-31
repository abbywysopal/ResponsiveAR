using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BaseEntity), true)]
public class BaseEntityInspector : Editor
{
    public override void OnInspectorGUI()
    {
        GUIStyle customStyle = new GUIStyle(EditorStyles.largeLabel);
        customStyle.alignment = TextAnchor.MiddleCenter;

        // base.OnInspectorGUI();
        EditorGUILayout.LabelField($"ENTITY", customStyle);
        DrawDefaultInspector();
    }
}
