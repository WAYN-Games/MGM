using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Wayn.Mgm.Event.Registry.Editor
{
    /// <summary>
    /// Base class for registry event authoring component.
    /// Extending this class allow to define an authoring component that handle a collection of registered event instance to use in your systems.
    /// </summary>
    /// <typeparam name="EVENT_INTERFACE">The interface that implement all event.</typeparam>
    public abstract class RegistryReferenceBufferAuthoringEditor<EVENT_INTERFACE> : UnityEditor.Editor
        where EVENT_INTERFACE : IRegistryEvent
    {
        /// <summary>
        /// List of all the aivalable events matching the <code>ELEMENT</code> type.
        /// </summary>
        private List<Type> AvailableTypes = new List<Type>();
        /// <summary>
        /// List of all the aivalable events name matching the <code>ELEMENT</code> type.
        /// </summary>
        private List<string> AvailableTypeNames = new List<string>();

        /// <summary>
        /// The object on which to store the events.
        /// </summary>
        private SerializedObject _serializedObject;

        /// <summary>
        /// The editor serialzed list of events.
        /// </summary>
        private SerializedProperty entryList;

        /// <summary>
        /// The index of the currently selected event type in hte list of event.
        /// </summary>
        private int selectedIndex = 0;

        /// <summary>
        /// Retrieve and cache the list of all event types matching the <code>EVENT_INTERFACE</code>.
        /// </summary>
        protected virtual void OnEnable()
        {
            // Initialize list of available Entry Type
            AvailableTypes.Clear();
            AvailableTypes.AddRange(AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => p.IsValueType && typeof(EVENT_INTERFACE).IsAssignableFrom(p)));

            // Initialize Entry types name list for popup selection.
            AvailableTypeNames.Clear();
            AvailableTypeNames.AddRange(AvailableTypes.Select(s => s.Name).ToArray());
            _serializedObject = serializedObject;

        }

        /// <summary>
        /// Draws :
        /// - the authoring script reference so that the user can click on it to edit it
        /// - a drop down list of all events type mathincg the <code>EVENT_INTERFACE</code> and propose to add a instance of the selected one to the list of events
        /// - each event instance and the properties with the possibilitiy to remove the instance from the list
        /// </summary>
        public override void OnInspectorGUI()
        {
            // Display the authoring script reference so that hte user can click on it to edit it.
            GUI.enabled = false;
            EditorGUILayout.PropertyField(_serializedObject.FindProperty("m_Script"), true);
            GUI.enabled = true;


            // Enable update registration.
            _serializedObject.Update();

            // Get the list of entries
            entryList = _serializedObject.FindProperty("Entries");

            // Provdide a drop down list of all matching event type and propose to add a instance of the selected one to the list of events.
            EditorGUILayout.BeginHorizontal();
            selectedIndex = EditorGUILayout.Popup(selectedIndex, AvailableTypeNames.ToArray());
            if (GUILayout.Button("+", new GUILayoutOption[] { GUILayout.Width(20) }))
            {
                entryList.InsertArrayElementAtIndex(0);
                _serializedObject.ApplyModifiedProperties();

                entryList.GetArrayElementAtIndex(0).FindPropertyRelative("Entry").managedReferenceValue = (EVENT_INTERFACE)Activator.CreateInstance(AvailableTypes[selectedIndex]);

                _serializedObject.ApplyModifiedProperties();
                Debug.Log(_serializedObject.FindProperty("Entries.Array.data[0].Entry").managedReferenceFullTypename);
            }
            EditorGUILayout.EndHorizontal();

            // Skip the rest of hte rendering if there is no events instance to display.
            if (entryList.arraySize == 0) return;


            // For each event instance
            IEnumerator enumerator = entryList.GetEnumerator();
            int index = 0;
            List<int> itemToRemove = new List<int>();
            while (enumerator.MoveNext())
            {
                // Draw the entry
                DrawEntryAuthoring(enumerator.Current as SerializedProperty, index, itemToRemove);
                index++;
            }

            // Remove the desired elements form the list of events
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

            // Display the type name of the event instance and allow for it's removal form the list.
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(sp.managedReferenceFullTypename.Split('.').Reverse().ToArray()[0]);
            if (GUILayout.Button("-", new GUILayoutOption[] { GUILayout.Width(20) }))
            {
                itemToRemove.Add(index);
            }
            EditorGUILayout.EndHorizontal();

            // Display all the properties of the event in edit mode.
            IEnumerator enumerator = sp.GetEnumerator();
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