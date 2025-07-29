using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UniqueId)), CanEditMultipleObjects]
public class UniqueIdEditor : Editor
{
    public override void OnInspectorGUI()
    {
        UniqueId uid = target as UniqueId;

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
