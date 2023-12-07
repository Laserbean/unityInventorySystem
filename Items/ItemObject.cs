using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        [HideInInspector]
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

        public void AddData(ItemComponent itemComponent)
        {
            if (_item.Components.Contains(itemComponent)) return;
            itemComponent.SetParentItem(_item);
            _item.Components.Add(itemComponent);
        }

    }







}