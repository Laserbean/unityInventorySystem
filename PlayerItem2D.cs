using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using unityInventorySystem;

using UnityEngine.Events; 


[System.Serializable]
public class EquipmentStuff {
    public RestrictedList tags; 
    public SpriteRenderer spriteRenderer; 
    public EquipmentEvents events; 
}

[System.Serializable]
public class EquipmentEvents {
    public MyItemObjectEvent onAdd, onRemove; 
}

[System.Serializable]
public class MyItemObjectEvent : UnityEvent<ItemObject> {}



// public class AttributeAddEvent : SingleItemEvent {
//     public AttributeAddEvent(ItemBuff _buff) : base(_buff) {}
// }

// public class AttributeRemoveEvent : SingleItemEvent {
//     public AttributeRemoveEvent(ItemBuff _buff) : base(_buff) {}
// }


public class PlayerItem2D : MonoBehaviour, IAttributeModified
{
    // Start is called before the first frame update


    private AttributesController attributesController;

    public InventoryObject inventory; 
    public InventoryObject equipment;

    public List<EquipmentStuff> equipmentStuff = new List<EquipmentStuff>();




    public void AddItemToInventory(unityInventorySystem.GroundItem gitem) {
        if (gitem) {
            Item _item = new Item(gitem.item);
            // Debug.Log(_item.Id);
            inventory.AddItem(_item, gitem.ammount);
        }

    }

    // public Attribute[] attributes;
    
    public void OnRemoveItem(InventorySlot _slot)
    {
        if (_slot.ItemObject == null)
            return;
        switch (_slot.parent.inventory.type)
        {
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:
                // print(string.Concat("Removed ", _slot.ItemObject, " on ", _slot.parent.inventory.type, ", Allowed Items: ", string.Join(", ", _slot.AllowedItems)));

                for (int i = 0; i < _slot.item.buffs.Length; i++) {
                    // for (int j = 0; j < attributes.Length; j++) {
                    //     if (attributes[j].type == _slot.item.buffs[i].attribute)
                    //         attributes[j].value.RemoveModifier(_slot.item.buffs[i]);
                    // }
                    // removeAttribute.Invoke(_slot.item.buffs[i].attributes); 

                    attributesController.RemoveAttributeModifier(_slot.item.buffs[i].attribute, _slot.item.buffs[i]);
                }

                


                if (_slot.ItemObject.characterDisplay2D != null) {
                    foreach(EquipmentStuff cur in equipmentStuff) {
                        if (_slot.tags.list.Overlap(cur.tags.list).Count > 0) {

                            // TODO: Remove the sprite renderers or something
                            if (cur.spriteRenderer != null) {
                                cur.spriteRenderer.sprite = null; 
                            }
                            cur.events.onRemove.Invoke(_slot.ItemObject);

                        }
                    }
                }


                // if (_slot.ItemObject.GetType() == typeof(WeaponObject)) {
                //     this.gameObject.GetComponent<PlayerWeapon>().SetDefaultWeapon(); 
                // }

                break;
            case InterfaceType.Chest:
                break;
            default:
                break;
        }
    }



    public void OnAddItem(InventorySlot _slot)
    {
        if (_slot.ItemObject == null)
            return;
        switch (_slot.parent.inventory.type)
        {
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:
                // print($"Placed {_slot.ItemObject}  on {_slot.parent.inventory.type}, Allowed Items: {string.Join(", ", _slot.AllowedItems)}");

                for (int i = 0; i < _slot.item.buffs.Length; i++)
                {
                    // for (int j = 0; j < attributes.Length; j++)
                    // {
                    //     if (attributes[j].type == _slot.item.buffs[i].attribute)
                    //         attributes[j].value.AddModifier(_slot.item.buffs[i]);
                    // }
                    attributesController.AddAttributeModifier(_slot.item.buffs[i].attribute, _slot.item.buffs[i]);

                }

                if (_slot.ItemObject.characterDisplay2D != null) {
                    foreach(EquipmentStuff cur in equipmentStuff) {
                        if (_slot.tags.list.Overlap(cur.tags.list).Count > 0) {

                            if (cur.spriteRenderer != null) {
                                cur.spriteRenderer.sprite = _slot.ItemObject.characterDisplay2D; 
                            }
                            cur.events.onAdd.Invoke(_slot.ItemObject);

                        }
                    }

                }
                break;
            case InterfaceType.Chest:
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

        // boneCombiner = new BoneCombiner(gameObject);

        // for (int i = 0; i < attributes.Length; i++)
        // {
        //     attributes[i].SetParent(this.gameObject);
        // }

        for (int i = 0; i < equipment.GetSlots.Length; i++)
        {
            equipment.GetSlots[i].OnBeforeUpdate += OnRemoveItem;
            equipment.GetSlots[i].OnAfterUpdate += OnAddItem;
        }
    }


    private void OnApplicationQuit() {
        inventory.Clear();
        equipment.Clear();
        // inventory.Container.Clear(); 
        // inventory.Container.Items = new InventorySlot[28];

    }
}


