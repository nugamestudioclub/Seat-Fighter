using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerConfig))]
public class PlayerConfigEditor : Editor
{
    private SerializedProperty aiProperty;

    private void OnEnable()
    {
        //foreach(var field in typeof(PlayerConfig).GetFields())
        //{
        //    Debug.Log(field.Name);
        //}
        aiProperty = serializedObject.FindProperty("<AI>k__BackingField");
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        serializedObject.Update();
        EditorGUILayout.PropertyField(aiProperty);
        serializedObject.ApplyModifiedProperties();
    }
}
