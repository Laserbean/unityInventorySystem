using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

using unityInventorySystem.Items;


namespace unityInventorySystem.Inventories
{
    public delegate void SlotUpdated(InventorySlot _slot);

    [System.Serializable]
    public class InventorySlot
    {
        public ItemType[] AllowedItems = new ItemType[0];
        [field: System.NonSerialized] public UserInterface Parent {get; private set;}
        [System.NonSerialized] public GameObject slotDisplay;
        [System.NonSerialized] public SlotUpdated OnAfterUpdate;
        [System.NonSerialized] public SlotUpdated OnBeforeUpdate;

        public EquipmentTag tag;

        public Item item = new ();
        public int amount;

        public void SetParent(UserInterface parnt) {
            Parent = parnt; 

            UserInterface.OnSlotSelect += OnSlotSelect;
            UserInterface.OnSlotRelease += OnSlotRelease;
        }


        public ItemObject ItemObject {
            get {
                if (item.Id >= 0)
                    return Parent.inventoryObject.inventory.Database.GetItemObject(item.Name);

                return null;
            }
        }

        public InventorySlot(InventorySlot other)
        {
            this.AllowedItems = other.AllowedItems;
            this.tag = other.tag;
            this.item = other.item;
            this.amount = other.amount;
        }

        public InventorySlot()
        {
            UpdateSlot(new Item(), 0);
        }

        public InventorySlot(Item _item, int _amount)
        {
            UpdateSlot(_item, _amount);
        }

        public void UpdateSlot(Item _item, int _amount)
        {
            OnBeforeUpdate?.Invoke(this);
            item = _item;
            amount = _amount;
            OnAfterUpdate?.Invoke(this);
        }

        void OnSlotSelect(InventorySlot other) {
            if (CanPlaceInSlot(other.ItemObject))
                Parent.EnableSlot(this);
            else 
                Parent.DisableSlot(this);
        }

        void OnSlotRelease(InventorySlot other) {
            Parent.EnableSlot(this);
        }



        public bool IsEmpty()
        {
            // if (Slots[i].item.Id <= -1)
            return item.Id <= -1;
            // return item.Name == (new Item()).Name;
        }


        public void RemoveItem()
        {
            UpdateSlot(new Item(), 0);
        }

        public void AddAmount(int value)
        {
            UpdateSlot(item, amount += value);
        }

        public void RemoveAmount(int value)
        {
            UpdateSlot(item, amount -= value);
            if (amount <= 0) {
                RemoveItem();
            }
        }

        public bool CanPlaceInSlot(ItemObject _itemObject)
        {
            if (AllowedItems.Length <= 0 || _itemObject == null || _itemObject.item.Id < 0)
                return true;
            for (int i = 0; i < AllowedItems.Length; i++) {
                if (_itemObject.type == AllowedItems[i])
                    return true;
            }
            return false;
        }
    }


}
