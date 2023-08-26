using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq; 

using unityInventorySystem;
using unityInventorySystem.Attribute; 


using UnityEngine.Events; 


[System.Serializable]
public class EquipmentStuff {
    public ItemType type;
    public EquipmentTag tag; 
    public EquipmentEvents events; 

    public bool RequirementsNotMet(InventorySlot _slot) {
        return _slot.tag != tag;
        // || !_slot.AllowedItems.ToList().Contains(type);
    }
}

[System.Serializable]
public class EquipmentEvents {
    public MyInventorySlotEvent onAdd, onRemove; 
}


[System.Serializable]
public class MyInventorySlotEvent : UnityEvent<InventorySlot> {}



public class PlayerItem2D : MonoBehaviour, IAttributeModified
{

    [SerializeField] InventoryObject inventoryObject; 
    [SerializeField] InventoryObject equipmentObject;
    [SerializeField] List<EquipmentStuff> equipmentStuff = new();

    [SerializeField] EquipmentEvents OnBagEqDequip;


    AttributesController attributesController;


    // public Attribute[] attributes;
    
    public void OnRemoveItem(InventorySlot _slot)
    {
        if (_slot.ItemObject == null)
            return;
        switch (_slot.parent.inventoryObject.type)
        {
            case InterfaceType.Equipment:
                // print(string.Concat("Removed ", _slot.ItemObject, " on ", _slot.parent.inventoryObject.type, ", Allowed Items: ", string.Join(", ", _slot.AllowedItems)));

                for (int i = 0; i < _slot.item.buffs.Length; i++) {
                    attributesController.RemoveAttributeModifier(_slot.item.buffs[i].attribute, _slot.item.buffs[i]);
                }

                // if (_slot.ItemObject.characterDisplay2D == null) break;
                foreach(EquipmentStuff cur in equipmentStuff) {
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
        switch (_slot.parent.inventoryObject.type)
        {
            case InterfaceType.Equipment:
                // print($"Placed {_slot.ItemObject}  on {_slot.parent.inventoryObject.type}, Allowed Items: {string.Join(", ", _slot.AllowedItems)}");

                for (int i = 0; i < _slot.item.buffs.Length; i++) {
                    attributesController.AddAttributeModifier(_slot.item.buffs[i].attribute, _slot.item.buffs[i]);
                }

                // if (_slot.ItemObject.characterDisplay2D == null) break;
                foreach(EquipmentStuff cur in equipmentStuff) {
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
        Debug.Log(string.Concat(attribute.type, " was updated! Value is now ", attribute.value.ModifiedValue));
    }

    Vector3 currentItemPos;


    void Start()
    {
        attributesController = this.GetComponent<AttributesController>(); 
        currentItemPos = new Vector3(0.55f,2.15f,0);

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

    private void OnDisable() {
        equipmentObject.inventory.ClearAllEvents();
    }

    private void OnApplicationQuit() {
        inventoryObject.Clear();
        equipmentObject.Clear();
        // inventoryObject.Container.Clear(); 
        // inventoryObject.Container.Items = new InventorySlot[28];

    }
}


