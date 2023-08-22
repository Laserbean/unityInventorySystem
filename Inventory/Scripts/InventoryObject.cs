using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;
using System.Runtime.Serialization;
using System;

namespace unityInventorySystem {

public enum InterfaceType
{
    Inventory,
    Equipment,
    Chest
}

[CreateAssetMenu(fileName = "New Inventory", menuName = "unity Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public string savePath;
    // public ItemDatabaseObject database;
    public InterfaceType type;
    public Inventory inventory;
    public InventorySlot[] GetSlots { get { return inventory.Slots; } }



    [ContextMenu("Save")]
    public void Save(string path = "")
    {
        //string saveData = JsonUtility.ToJson(this, true);
        //BinaryFormatter bf = new BinaryFormatter();
        //FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
        //bf.Serialize(file, saveData);
        //file.Close();

        IFormatter formatter = new BinaryFormatter();
        Stream stream;
        if (path == "") {
            stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
        } else {
            stream = new FileStream(path, FileMode.Create, FileAccess.Write);
        }

        formatter.Serialize(stream, inventory);
        stream.Close();
    }


    [ContextMenu("Load")]
    public void Load(string path = "")
    {
        // if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        if (File.Exists(path))
        {
            //BinaryFormatter bf = new BinaryFormatter();
            //FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
            //JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
            //file.Close();

            IFormatter formatter = new BinaryFormatter();
            Stream stream;

            if (path == "") {
                stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
            } else {
                stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            }

            
            Inventory newContainer = (Inventory)formatter.Deserialize(stream);
            for (int i = 0; i < GetSlots.Length; i++)
            {
                GetSlots[i].UpdateSlot(newContainer.Slots[i].item, newContainer.Slots[i].amount);
            }
            stream.Close();
        }
    }

    

    [ContextMenu("Clear")]
    public void Clear()
    {
        inventory.Clear();
    }
}



[System.Serializable]
public class Inventory
{
    [SerializeField]
    string DatabaseName = "ItemDB"; 

    [SerializeField]
    public string Name = "Inventory"; 

    [NonSerialized]
    ItemDatabaseObject _database;

    public InventorySlot[] Slots = new InventorySlot[28];
    
    public Inventory Copy() {
            Inventory inventory = new()
            {
                DatabaseName = DatabaseName,

                Slots = new InventorySlot[Slots.Length]
            };

            for (int i = 0; i < Slots.Length; i++) {
            inventory.Slots[i] = new InventorySlot(Slots[i]);
        }

        return inventory; 
    }
    
    public void SetUserinterface(UserInterface userInterface) {
        for (int i = 0; i < Slots.Length; i++)
        {
            Slots[i].parent = userInterface;
        }
    }

    public void SetSlotsAfterUpdate(SlotUpdated OnSlotUpdate) {
        for (int i = 0; i < Slots.Length; i++)
        {
            Slots[i].OnAfterUpdate += OnSlotUpdate;
        }
    }

    public void SetSlotsBeforeUpdate(SlotUpdated OnSlotUpdate) {
        for (int i = 0; i < Slots.Length; i++)
        {
            Slots[i].OnBeforeUpdate += OnSlotUpdate;
        }
    }
    

    public void ClearAllEvents() {
        for (int i = 0; i < Slots.Length; i++)
        {
            Slots[i].OnBeforeUpdate = null;
            Slots[i].OnAfterUpdate  = null;
        }
    }


    public void Clear()
    {
        for (int i = 0; i < Slots.Length; i++)
        {
            Slots[i].RemoveItem();
        }
    }

    public ItemDatabaseObject database {
        get {
            if (_database == null) _database = InventoryStaticManager.GetDatabase(DatabaseName);
            return _database; 

        }

    }

    
    public int EmptySlotCount
    {
        get
        {
            int counter = 0;
            for (int i = 0; i < Slots.Length; i++)
            {
                if (Slots[i].item.Id <= -1)
                {
                    counter++;
                }
            }
            return counter;
        }
    }

    
    public bool AddItem(Item _item, int _amount)
    {
        if (EmptySlotCount <= 0)
            return false;
        InventorySlot slot = FindItemSlonOnInventory(_item);
        if(!database.ItemObjects[_item.Id].stackable || slot == null)
        {
            SetEmptySlot(_item, _amount);
            return true;
        }
        slot.AddAmount(_amount);
        return true;
    }

    public bool RemoveItem(Item _item)
    {
        if (EmptySlotCount <= 0)
            return false;
        InventorySlot slot = FindItemSlonOnInventory(_item);
        if(!database.ItemObjects[_item.Id].stackable || slot == null)
        {
            slot = new InventorySlot();
            return true;
        }
        slot.RemoveAmount(1); 
        return true;
    }

    ///<summary>
    /// Looks for the slot in the inventory that already contains the item. 
    ///</summary>
    public InventorySlot FindItemSlonOnInventory(Item _item)
    {
        for (int i = 0; i < Slots.Length; i++)
        {
            if(Slots[i].item.Id == _item.Id)
            {
                return Slots[i];
            }
        }
        return null;
    }

    public InventorySlot SetEmptySlot(Item _item, int _amount)
    {
        for (int i = 0; i < Slots.Length; i++)
        {
            if (Slots[i].item.Id <= -1)
            {
                Slots[i].UpdateSlot(_item, _amount);
                return Slots[i];
            }
        }
        //set up functionality for full inventory
        return null;
    }

    public bool AddEquipment(Item _item, EquipmentTag eq_tag, ItemType type) {
        if (EmptySlotCount <= 0)
            return false;
        InventorySlot slot = FindFirstSlotWithType(type);
        
        if (slot == null) {
            return false; 
        }
        slot.UpdateSlot(_item, 1); 

        return true; 
    }
    

    public InventorySlot FindFirstSlotWithType(ItemType itype) { //make sure type is unique, example, bullets. 
        for (int i = 0; i < Slots.Length; i++)
        {
            try {
            if(Slots[i].ItemObject != null && Slots[i].ItemObject.type == itype)
            {
                return Slots[i];
            }
            } catch {
                Debug.LogError("idk what's the rerroror"); 
            }
        }
        return null;
    }


    public List<InventorySlot> FindSlotsWithType(ItemType itype) {
        List<InventorySlot> list = new List<InventorySlot>(); 

        for (int i = 0; i < Slots.Length; i++)
        {
            if(Slots[i].ItemObject.type == itype)
            {
                list.Add(Slots[i]); 
            }
        }
        return list;
    }

    public void SwapItems(InventorySlot item1, InventorySlot item2)
    {
        if(item2.CanPlaceInSlot(item1.ItemObject) && item1.CanPlaceInSlot(item2.ItemObject))
        {
            InventorySlot temp = new InventorySlot( item2.item, item2.amount);
            item2.UpdateSlot(item1.item, item1.amount);
            item1.UpdateSlot(temp.item, temp.amount);
        }
    }
    
    

}



public delegate void SlotUpdated(InventorySlot _slot);

[System.Serializable]
public class InventorySlot
{
    public ItemType[] AllowedItems = new ItemType[0];
    [System.NonSerialized]
    public UserInterface parent;
    [System.NonSerialized]
    public GameObject slotDisplay;
    [System.NonSerialized]
    public SlotUpdated OnAfterUpdate;
    [System.NonSerialized]
    public SlotUpdated OnBeforeUpdate;


        public EquipmentTag tag;




    public Item item = new Item();
    public int amount;


    public ItemObject ItemObject {
        get
        {
            if(item.Id >= 0)
            {
                return parent.inventoryObject.inventory.database.ItemObjects[item.Id];
            }
            return null;
        }
    }

    public InventorySlot(InventorySlot other) {
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
        if (OnBeforeUpdate != null)
            OnBeforeUpdate.Invoke(this);
        item = _item;
        amount = _amount;
        if (OnAfterUpdate != null)
            OnAfterUpdate.Invoke(this);
    }

    public bool IsEmpty() {
        return item.Name == (new Item()).Name;
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
        for (int i = 0; i < AllowedItems.Length; i++)
        {
            if (_itemObject.type == AllowedItems[i])
                return true;
        }
        return false;
    }
}

// public class ConsumeItemEvent : SingleItemEvent
// {
//     public ConsumeItemEvent(Consumable value) : base(value) {}
// }


// public class ConsumeItemEvent {
//     public Consumable item; 

//     public ConsumeItemEvent(Consumable _item) {
//         item = _item; 
//     }

// }



}