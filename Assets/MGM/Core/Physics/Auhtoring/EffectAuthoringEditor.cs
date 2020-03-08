using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EffectReferenceBufferAuthoring), true)]
public class EffectAuthoringEditor : Editor
{

    private SerializedObject _serializedObject;

    protected virtual void OnEnable()
    {
        _serializedObject = serializedObject;
        Debug.Log("Create");
    }

    public override void OnInspectorGUI()
    {
        GUI.enabled = false;
        EditorGUILayout.PropertyField(_serializedObject.FindProperty("m_Script"),true);
        GUI.enabled = true;
        _serializedObject.Update();
        SerializedProperty effectList = _serializedObject.FindProperty("Effects");

        EditorGUILayout.PropertyField(effectList);
        _serializedObject.ApplyModifiedProperties();

        Debug.Log("Draw");
    }
}
