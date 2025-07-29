using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SaveStation)), CanEditMultipleObjects]
public class SaveStationEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SaveStation uid = target as SaveStation;

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
