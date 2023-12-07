using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using unityInventorySystem.Items.Components;

using UnityEditor;
using UnityEditor.Callbacks;


namespace unityInventorySystem.Items
{

    [CustomEditor(typeof(ItemObject))]
    public class ItemObjectEditor : Editor
    {
        private static List<Type> dataCompTypes = new();

        private ItemObject this_so;

        private bool showAddComponentButtons;

        private void OnEnable()
        {
            this_so = target as ItemObject;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            showAddComponentButtons = EditorGUILayout.Foldout(showAddComponentButtons, "Add Components");

            if (showAddComponentButtons) {
                foreach (var dataCompType in dataCompTypes) {
                    if (GUILayout.Button(dataCompType.Name)) {
                        if (Activator.CreateInstance(dataCompType) is not ItemComponent comp)
                            return;

                        this_so.AddData(comp);
                        EditorUtility.SetDirty(this_so);
                    }
                }
            }
        }

        [DidReloadScripts]
        private static void OnRecompile()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var types = assemblies.SelectMany(assembly => assembly.GetTypes());
            var filteredTypes = types.Where(
                type => type.IsSubclassOf(typeof(ItemComponent)) && !type.ContainsGenericParameters && type.IsClass
            );
            dataCompTypes = filteredTypes.ToList();
        }

    }

}
