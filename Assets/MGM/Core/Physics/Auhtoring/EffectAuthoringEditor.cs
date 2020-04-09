using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Wayn.Mgm.Effects;

[CustomEditor(typeof(EffectReferenceBufferAuthoring), true)]
public class EffectAuthoringEditor : Editor
{

    private List<Type> AvailableTypes = new List<Type>();
    private List<string> AvailableTypeNames = new List<string>();

    private List<EffectAuthoring> Effects;

    private SerializedObject _serializedObject;
    private SerializedProperty effectList;
    private int selectedIndex = 0;

    protected virtual void OnEnable()
    {
        // Initialize list of available Effect Type
        AvailableTypes.Clear();
        AvailableTypes.AddRange(AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => p.IsValueType && typeof(IEffect).IsAssignableFrom(p)));

        // Initialize Effect types name list for popup selection.
        AvailableTypeNames.Clear();
        AvailableTypeNames.AddRange(AvailableTypes.Select(s => s.Name).ToArray());

        _serializedObject = serializedObject;

        Effects = ((EffectReferenceBufferAuthoring)target).Effects ;

    }

    public override void OnInspectorGUI()
    {
        // Display the name of the authoring component script
        GUI.enabled = false;
        EditorGUILayout.PropertyField(_serializedObject.FindProperty("m_Script"),true);
        GUI.enabled = true;

        // Enable update registration.
        _serializedObject.Update();

        // Get the list of effects
         effectList = _serializedObject.FindProperty("Effects");

        EditorGUILayout.BeginHorizontal();


        selectedIndex = EditorGUILayout.Popup(selectedIndex, AvailableTypeNames.ToArray());
        if (GUILayout.Button("+", new GUILayoutOption[] { GUILayout.Width(20) }))
        {
            effectList.InsertArrayElementAtIndex(0);
            _serializedObject.ApplyModifiedProperties();
            Effects[0].Effect = Activator.CreateInstance(AvailableTypes[selectedIndex]) as IEffect;
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.LabelField("Effects");


        // For each effect
        var enumerator = effectList.GetEnumerator();
        int index = 0;
        while (enumerator.MoveNext()) {
            // Draw the effect
            DrawEffectAuthoring(enumerator.Current as SerializedProperty,index);
            index++;
        }
       
        // Apply modifications
        _serializedObject.ApplyModifiedProperties();
   }

    private void DrawEffectAuthoring(SerializedProperty effectAuhtoring, int index)
    {
        SerializedProperty sp = effectAuhtoring.FindPropertyRelative("Effect");

        EditorGUILayout.BeginHorizontal();
        
        EditorGUILayout.LabelField(sp.managedReferenceFullTypename.Split('.').Reverse().ToArray()[0]);
        if (GUILayout.Button("-", new GUILayoutOption[] { GUILayout.Width(20) }))
        {
            effectList.DeleteArrayElementAtIndex(index);
            return;
        }

        EditorGUILayout.EndHorizontal();
        var enumerator = sp.GetEnumerator();
        if (enumerator == null) return;
        while (enumerator.MoveNext())
        {
            SerializedProperty current = enumerator.Current as SerializedProperty;
            EditorGUILayout.PropertyField(current);
        }


    }
}
