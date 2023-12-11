using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

using unityInventorySystem.Items;

namespace unityInventorySystem.Inventories
{
    [System.Serializable]
    public class Inventory
    {
        [SerializeField] string DatabaseName = "ItemDB";
        [SerializeField] public string Name = "Inventory";

        [NonSerialized] ItemDatabaseObject _database;

        public InventorySlot[] Slots = new InventorySlot[28];

        public Inventory(int size = 28)
        {
            Slots = new InventorySlot[size];

            for (int i = 0; i < Slots.Length; i++) {
                Slots[i] = new InventorySlot();
            }
        }


        public Inventory Copy()
        {
            Inventory inventory = new() {
                DatabaseName = DatabaseName,
                Slots = new InventorySlot[Slots.Length]
            };

            for (int i = 0; i < Slots.Length; i++) {
                inventory.Slots[i] = new InventorySlot(Slots[i]);
            }

            return inventory;
        }

        #region EventSetup

        public void SetUserinterface(UserInterface userInterface)
        {
            for (int i = 0; i < Slots.Length; i++) {
                Slots[i].SetParent(userInterface);
            }
        }

        public void UpdateSlots()
        {
            for (int i = 0; i < Slots.Length; i++) {
                Slots[i].OnAfterUpdate?.Invoke(Slots[i]);
            }

        }

        public void SetSlotsAfterUpdate(SlotUpdated OnSlotUpdate)
        {
            for (int i = 0; i < Slots.Length; i++) {
                Slots[i].OnAfterUpdate += OnSlotUpdate;
            }
        }

        public void SetSlotsBeforeUpdate(SlotUpdated OnBeforeUpdate)
        {
            for (int i = 0; i < Slots.Length; i++) {
                Slots[i].OnBeforeUpdate += OnBeforeUpdate;
            }
        }

        public void ClearAllEvents()
        {
            for (int i = 0; i < Slots.Length; i++) {
                Slots[i].OnBeforeUpdate = null;
                Slots[i].OnAfterUpdate = null;
            }
        }

        #endregion

        public ItemDatabaseObject Database {
            get {
                if (_database == null) {
                    _database = InventoryStaticManager.GetDatabase(DatabaseName);
                    _database.Initialize();
                }
                return _database;
            }
        }

        public int EmptySlotCount {
            get {
                int counter = 0;
                for (int i = 0; i < Slots.Length; i++) {
                    // if (Slots[i].item.Id <= -1)
                    if (Slots[i].IsEmpty()) {
                        counter++;
                    }
                }
                return counter;
            }
        }

        public void Clear()
        {
            for (int i = 0; i < Slots.Length; i++) {
                Slots[i].RemoveItem();
            }
        }

        public bool TryAddToExistingSlot(Item _item, int _amount)
        {
            if (EmptySlotCount <= 0) return false;

            InventorySlot slot = GetItemSlot(_item);
            if (slot == null)
                return false;
            slot.AddAmount(_amount);
            return true;
        }


        public bool TryAddItem(Item _item, int _amount)
        {
            if (EmptySlotCount <= 0) return false;

            InventorySlot slot = GetItemSlot(_item);
            if (slot == null) //!database.ItemObjects[_item.Id].stackable ||
            {
                AddToNewSlot(_item, _amount);
                return true;
            }

            slot.AddAmount(_amount);
            return true;
        }

        public bool RemoveItem(Item _item)
        {
            if (EmptySlotCount <= 0) return false;

            InventorySlot slot = GetItemSlot(_item);
            if (!Database.GetItemObject(_item.Name).stackable || slot == null) {
                // slot = new InventorySlot();
                return true;
            }
            slot.RemoveAmount(1);
            return true;
        }

        ///<summary>
        /// Looks for the slot in the inventory that already contains the item. 
        ///</summary>
        public InventorySlot GetItemSlot(Item _item)
        {
            for (int i = 0; i < Slots.Length; i++) {
                if (Slots[i].item.Id == _item.Id) {
                    return Slots[i];
                }
            }
            return null;
        }

        public InventorySlot AddToNewSlot(Item _item, int _amount)
        {
            for (int i = 0; i < Slots.Length; i++) {
                if (Slots[i].IsEmpty()) {
                    Slots[i].UpdateSlot(_item, _amount);
                    return Slots[i];
                }
            }
            // TODO: set up functionality for full inventory
            return null;
        }

        public bool AddEquipment(Item _item, EquipmentTag eq_tag, ItemType type)
        {
            if (EmptySlotCount <= 0)
                return false;
            InventorySlot slot = FindFirstSlotWithType(type);

            if (slot == null) {
                return false;
            }
            slot.UpdateSlot(_item, 1);

            return true;
        }


        public InventorySlot FindFirstSlotWithType(ItemType itype)
        { //make sure type is unique, example, bullets. 
            for (int i = 0; i < Slots.Length; i++) {
                try {
                    if (Slots[i].ItemObject != null && Slots[i].ItemObject.type == itype) {
                        return Slots[i];
                    }
                }
                catch {
                    Debug.LogError("idk what's the rerroror");
                }
            }
            return null;
        }


        public List<InventorySlot> FindSlotsWithType(ItemType itype)
        {
            List<InventorySlot> list = new();

            for (int i = 0; i < Slots.Length; i++) {
                if (Slots[i].ItemObject.type == itype) {
                    list.Add(Slots[i]);
                }
            }
            return list;
        }

        public void SwapItems(InventorySlot item1, InventorySlot item2)
        {
            if (item2.CanPlaceInSlot(item1.ItemObject) && item1.CanPlaceInSlot(item2.ItemObject)) {
                InventorySlot temp = new(item2.item, item2.amount);
                // item2.parent = item1.parent; 
                // item1.parent = temp.parent; 
                item2.UpdateSlot(item1.item, item1.amount);
                item1.UpdateSlot(temp.item, temp.amount);
            }
        }



    }



}