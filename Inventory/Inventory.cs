using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

using unityInventorySystem.Items;
using System.Runtime.CompilerServices;
using Laserbean.General;

[assembly: InternalsVisibleTo("TestsUnityInventorySystem")]

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
                    // _database = InventoryStaticManager.GetDatabase(DatabaseName);
                    // _database.Initialize();
                    _database = InventorySystemManager.Instance.ItemDatabase; 
                }
                return _database;
            }
        }

        public int EmptySlotCount {
            get {
                int counter = 0;
                for (int i = 0; i < Slots.Length; i++) {
                    // if (Slots[i].item.Id <= -1)
                    if (Slots[i].IsEmpty) {
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


        public bool TryAddItem(Item _item, int _amount)
        {
            var itemObj = Database.GetItemObject(_item.Name);

            if (itemObj == null || itemObj.stackable) {
                return TryAddStackableItem(_item, _amount);
            } else {
                return TryAddNonStackableItem(_item, _amount);
            }

        }


        bool TryAddStackableItem(Item _item, int _amount)
        {
            var slot = GetNonFullItemSlot(_item);

            if (slot != null && !slot.IsFull) {
                int spaceToAdd = Math.Min(slot.RemainingSpace, _amount);
                slot.AddAmount(spaceToAdd);

                if (spaceToAdd < _amount) {
                    return TryAddStackableItem(_item, _amount - spaceToAdd);
                }

                return true;
            } else {
                var newSlot = GetEmptySlot();
                if (newSlot == null) return false;
                newSlot.UpdateSlot(_item, 0);

                int spaceToAdd = Math.Min(newSlot.RemainingSpace, _amount);
                newSlot.AddAmount(spaceToAdd);

                if (spaceToAdd < _amount) {
                    return TryAddStackableItem(_item, _amount - spaceToAdd);
                }

                return true;
            }
        }


        bool TryAddNonStackableItem(Item _item, int _amount)
        {
            return AddToNewSlot(_item, _amount) != null;
        }


        internal bool TryAddToExistingSlot(Item _item, int _amount)
        {
            if (EmptySlotCount <= 0) return false;

            InventorySlot slot = GetItemSlot(_item);
            if (slot == null)
                return false;
            slot.AddAmount(_amount);
            return true;
        }



        public bool RemoveItem(Item _item)
        {
            if (EmptySlotCount <= 0) return false;

            InventorySlot slot = GetItemSlot(_item);
            if (!Database.GetItemObject(_item.Name).stackable || slot == null) {
                return true;
            }
            slot.RemoveAmount(1);
            return true;
        }


        internal InventorySlot GetNonFullItemSlot(Item _item)
        {
            for (int i = 0; i < Slots.Length; i++) {
                if (Slots[i].item.Id == _item.Id && !Slots[i].IsFull) {
                    return Slots[i];
                }
            }
            return null;
        }


        internal InventorySlot GetItemSlot(Item _item)
        {
            for (int i = 0; i < Slots.Length; i++) {
                if (Slots[i].item.Id == _item.Id) {
                    return Slots[i];
                }
            }
            return null;
        }

        internal List<InventorySlot> GetItemSlots(Item _item)
        {
            List<InventorySlot> slots = new();
            for (int i = 0; i < Slots.Length; i++) {
                if (Slots[i].item.Id == _item.Id) {
                    slots.Add(Slots[i]);
                }
            }
            return slots;
        }

        internal InventorySlot GetEmptySlot()
        {
            for (int i = 0; i < Slots.Length; i++) {
                if (Slots[i].IsEmpty) return Slots[i];
            }
            return null;
        }

        internal InventorySlot AddToNewSlot(Item _item, int _amount)
        {
            for (int i = 0; i < Slots.Length; i++) {
                if (Slots[i].IsEmpty) {
                    Slots[i].UpdateSlot(_item, _amount);
                    return Slots[i];
                }
            }
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


        internal InventorySlot FindFirstSlotWithType(ItemType itype)
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


        internal List<InventorySlot> FindSlotsWithType(ItemType itype)
        {
            List<InventorySlot> list = new();

            for (int i = 0; i < Slots.Length; i++) {
                if (Slots[i].ItemObject.type == itype) {
                    list.Add(Slots[i]);
                }
            }
            return list;
        }

        public static void SwapItems(InventorySlot item1, InventorySlot item2)
        {
            if (item2.CanPlaceInSlot(item1.ItemObject) && item1.CanPlaceInSlot(item2.ItemObject)) {
                InventorySlot temp = new(item2.item, item2.amount);

                item2.UpdateSlot(item1.item, item1.amount);
                item1.UpdateSlot(temp.item, temp.amount);
            }
        }

        public static void SplitItems(InventorySlot initial_stack_slot, InventorySlot other_slot)
        {
            int amount_transfer = Mathf.RoundToInt(initial_stack_slot.amount / 2);

            if (amount_transfer <= 0) {
                SwapItems(initial_stack_slot, other_slot);
            }

            if (other_slot.CanPlaceInSlot(initial_stack_slot.ItemObject) && (other_slot.IsEmpty || (other_slot.RemainingSpace <= amount_transfer && other_slot.item.Name == initial_stack_slot.item.Name))) {

                initial_stack_slot.UpdateSlot(initial_stack_slot.item, initial_stack_slot.amount - amount_transfer);
                other_slot.UpdateSlot(initial_stack_slot.item, amount_transfer);
            }
        }




    }



}