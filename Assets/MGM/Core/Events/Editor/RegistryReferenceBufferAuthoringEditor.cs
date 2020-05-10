using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Wayn.Mgm.Events.Registry
{

    public abstract class RegistryReferenceBufferAuthoringEditor<ELEMENT, AUTHORING> : Editor 
        where ELEMENT : IRegistryElement
        where AUTHORING : RegisteryReferenceAuthoring<ELEMENT>, new()
    {

        private List<Type> AvailableTypes = new List<Type>();
        private List<string> AvailableTypeNames = new List<string>();

        private SerializedObject _serializedObject;
        private SerializedProperty entryList;
        private int selectedIndex = 0;

        protected virtual void OnEnable()
        {
            // Initialize list of available Entry Type
            AvailableTypes.Clear();
            AvailableTypes.AddRange(AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => p.IsValueType && typeof(ELEMENT).IsAssignableFrom(p)));

            // Initialize Entry types name list for popup selection.
            AvailableTypeNames.Clear();
            AvailableTypeNames.AddRange(AvailableTypes.Select(s => s.Name).ToArray());
            _serializedObject = serializedObject;

        }

        public override void OnInspectorGUI()
        {
            GUI.enabled = false;
            EditorGUILayout.PropertyField(_serializedObject.FindProperty("m_Script"), true);
            GUI.enabled = true;


            // Enable update registration.
            _serializedObject.Update();




            // Get the list of entries
            entryList = _serializedObject.FindProperty("Entries");
            EditorGUILayout.BeginHorizontal();


            selectedIndex = EditorGUILayout.Popup(selectedIndex, AvailableTypeNames.ToArray());
            if (GUILayout.Button("+", new GUILayoutOption[] { GUILayout.Width(20) }))
            {
                entryList.InsertArrayElementAtIndex(0);
                _serializedObject.ApplyModifiedProperties();

                entryList.GetArrayElementAtIndex(0).FindPropertyRelative("Entry").managedReferenceValue = (ELEMENT)Activator.CreateInstance(AvailableTypes[selectedIndex]);

                _serializedObject.ApplyModifiedProperties();
                Debug.Log(_serializedObject.FindProperty("Entries.Array.data[0].Entry").managedReferenceFullTypename);
            }
            EditorGUILayout.EndHorizontal();

            if (entryList.arraySize == 0) return;


            // For each entry
            var enumerator = entryList.GetEnumerator();
            int index = 0;
            List<int> itemToRemove = new List<int>();
            while (enumerator.MoveNext())
            {
                // Draw the entry
                DrawEntryAuthoring(enumerator.Current as SerializedProperty, index, itemToRemove);
                index++;
            }

            foreach (int item in itemToRemove)
            {
                entryList.DeleteArrayElementAtIndex(item);
            }

            // Apply modifications
            _serializedObject.ApplyModifiedProperties();
        }

        private void DrawEntryAuthoring(SerializedProperty entryAuhtoring, int index, List<int> itemToRemove)
        {

            SerializedProperty sp = entryAuhtoring.FindPropertyRelative("Entry");

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(sp.managedReferenceFullTypename.Split('.').Reverse().ToArray()[0]);

            if (GUILayout.Button("-", new GUILayoutOption[] { GUILayout.Width(20) }))
            {
                itemToRemove.Add(index);
            }

            EditorGUILayout.EndHorizontal();
            var enumerator = sp.GetEnumerator();
            while (enumerator.MoveNext())
            {
                SerializedProperty current = enumerator.Current as SerializedProperty;
                current.serializedObject.Update();
                EditorGUILayout.PropertyField(current);
                current.serializedObject.ApplyModifiedProperties();
            }


        }

    }
}