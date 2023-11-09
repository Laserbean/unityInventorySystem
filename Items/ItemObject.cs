using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR 
using UnityEditor;
#endif

using Laserbean.SpecialData;

using unityInventorySystem.Attribute;
// using unityInventorySystem.Items;




using System.Linq;
using UnityEditor.Callbacks;
using unityInventorySystem.Items.Components;

namespace unityInventorySystem.Items
{

    [CreateAssetMenu(fileName = "New Item", menuName = "unity Inventory System/Items/item")]
    public class ItemObject : ScriptableObject
    {

        public Sprite uiDisplay;
        public GameObject characterDisplay;
        public Sprite characterDisplay2D;

        //TODO add gameobject for it to be placed on the ground


        public bool stackable;
        public int stackSize; //idk how to use this now

        public ItemType type;
        [TextArea(15, 20)]
        public string description;

        [SerializeField]
        protected Item _item = new();

        public Item item { get => _item; }


        public Item CreateItem()
        {
            Item newItem = new(_item);
            return newItem;
        }

        void OnValidate()
        {
            OnValidatee();
        }

        protected virtual void OnValidatee()
        {
            _item.Name = this.name;
        }

        public void AddData(ItemComponent itemComponent) {
            if (_item.Components.Contains(itemComponent)) return; 
            itemComponent.SetParentItem(_item);
            _item.Components.Add(itemComponent);
        }

    }


    #if UNITY_EDITOR 

    [CustomEditor(typeof(ItemObject))]
    public class ItemObjectEditor : Editor
    {
        private static List<Type> dataCompTypes = new ();

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

    #endif






}