using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

using unityInventorySystem.Items;
using unityInventorySystem.Items.Components;
using unityInventorySystem.Attribute;
using unityInventorySystem.Inventories;


using UnityEngine.Events;


[System.Serializable]
public class EquipmentStuff
{
    public ItemType type;
    public EquipmentTag tag;
    public EquipmentEvents events;

    public bool RequirementsNotMet(InventorySlot _slot)
    {
        return _slot.tag != tag;
        // || !_slot.AllowedItems.ToList().Contains(type);
    }
}

[System.Serializable]
public class EquipmentEvents
{
    public MyInventorySlotEvent onAdd, onRemove;
}


[System.Serializable]
public class MyInventorySlotEvent : UnityEvent<InventorySlot> { }



public class PlayerItem2D : MonoBehaviour, IAttributeModified
{

    [SerializeField] InventoryObject inventoryObject;
    [SerializeField] InventoryObject equipmentObject;
    [SerializeField] List<EquipmentStuff> equipmentStuff = new();


    public void OnRemoveItem(InventorySlot _slot)
    {
        if (_slot.ItemObject == null)
            return;
        switch (_slot.Parent.inventoryObject.type) {
            case InterfaceType.Equipment:
                foreach (var comp in _slot.item.Components)
                    comp.OnUnequip(transform.parent.gameObject);

                foreach (EquipmentStuff cur in equipmentStuff) {
                    if (cur.RequirementsNotMet(_slot))
                        continue;
                    cur.events.onRemove.Invoke(_slot);
                }

                break;
            default:
                break;
        }
    }

    public void OnAddItem(InventorySlot _slot)
    {
        if (_slot.ItemObject == null)
            return;
        switch (_slot.Parent.inventoryObject.type) {
            case InterfaceType.Equipment:
                foreach (var comp in _slot.item.Components)
                    comp.OnEquip(transform.parent.gameObject);

                foreach (EquipmentStuff cur in equipmentStuff) {
                    if (cur.RequirementsNotMet(_slot))
                        continue;
                    cur.events.onAdd.Invoke(_slot);
                }

                break;
            default:
                break;
        }

    }



    public void AttributeModified(Attribute attribute)
    {
        Debug.Log(string.Concat(attribute.type, " was updated! Value is now ", attribute.ModifiedValue));
    }



    void Start()
    {

        #region 3dstuff
        // boneCombiner = new BoneCombiner(gameObject);

        // for (int i = 0; i < attributes.Length; i++)
        // {
        //     attributes[i].SetParent(this.gameObject);
        // }
        #endregion

        equipmentObject.inventory.SetSlotsBeforeUpdate(OnRemoveItem);
        equipmentObject.inventory.SetSlotsAfterUpdate(OnAddItem);
    }

    private void OnDisable()
    {
        equipmentObject.inventory.ClearAllEvents();
    }

    private void OnApplicationQuit()
    {
        inventoryObject.Clear();
        equipmentObject.Clear();
        // inventoryObject.Container.Clear(); 
        // inventoryObject.Container.Items = new InventorySlot[28];

    }
}


