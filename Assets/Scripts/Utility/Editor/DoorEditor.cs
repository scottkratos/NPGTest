using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Door)), CanEditMultipleObjects]
public class DoorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Door uid = target as Door;

        DrawDefaultInspector();

        EditorGUILayout.Space();

        GUIStyle style = new GUIStyle();
        style.fontSize = 14;
        style.fontStyle = FontStyle.Bold;
        EditorGUILayout.LabelField("Generate Unique ID", style);

        if (GUILayout.Button("Generate UID"))
        {
            Undo.RecordObject(uid, "Generate UID");
            uid.GenerateUIDEditor();
            EditorUtility.SetDirty(uid);
        }
        EditorGUILayout.Space();
    }
}
