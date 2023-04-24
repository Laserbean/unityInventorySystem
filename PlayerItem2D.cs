using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using unityInventorySystem;

using UnityEngine.Events; 


[System.Serializable]
public class EquipmentStuff {
    public ItemType type;
    public EquipmentTag tag; 
    public SpriteRenderer spriteRenderer; 
    public EquipmentEvents events; 
}

[System.Serializable]
public class EquipmentEvents {
    public MyItemObjectEvent onAdd, onRemove; 
}

[System.Serializable]
public class MyItemObjectEvent : UnityEvent<ItemObject> {}

// [System.Serializable]
// public class EquipmentStuffs {
//     public List<EquipmentStuff> EquipmentStuff = new List<EquipmentStuff>(); 

//     public SpriteRenderer GetSprite(ItemType type) {
//         foreach(EquipmentStuff cur in EquipmentStuff) {
//             if (type == cur.type) {
//                 return cur.spriteRenderer; 
//             }
//         }
//         return null; 
//     }
// }


public class PlayerItem2D : MonoBehaviour
{
    // Start is called before the first frame update

    public InventoryObject inventory; 
    public InventoryObject equipment;

    public List<EquipmentStuff> equipmentStuff = new List<EquipmentStuff>();


    // public void OnTriggerEnter2D(Collider2D other)
    // {
    //     var gitem = other.GetComponent<unityInventorySystem.GroundItem>();
    //     if (gitem) {
    //         Item _item = new Item(gitem.item);
    //         Debug.Log(_item.Id);
    //         inventory.AddItem(_item, gitem.ammount);
    //         Destroy(other.gameObject);
    //     }
    // }

    public void AddItemToInventory(unityInventorySystem.GroundItem gitem) {
        if (gitem) {
            Item _item = new Item(gitem.item);
            // Debug.Log(_item.Id);
            inventory.AddItem(_item, gitem.ammount);
        }

    }

    public Attribute[] attributes;
    
    public void OnRemoveItem(InventorySlot _slot)
    {
        if (_slot.ItemObject == null)
            return;
        switch (_slot.parent.inventory.type)
        {
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:
                print(string.Concat("Removed ", _slot.ItemObject, " on ", _slot.parent.inventory.type,
                    ", Allowed Items: ", string.Join(", ", _slot.AllowedItems)));

                for (int i = 0; i < _slot.item.buffs.Length; i++) {
                    for (int j = 0; j < attributes.Length; j++) {
                        if (attributes[j].type == _slot.item.buffs[i].attribute)
                            attributes[j].value.RemoveModifier(_slot.item.buffs[i]);
                    }
                }


                if (_slot.ItemObject.characterDisplay2D != null) {
                    if (_slot.ItemObject.characterDisplay2D != null) {
                        foreach(EquipmentStuff cur in equipmentStuff) {
                            if (_slot.tag == cur.tag) {
                                for (int i = 0; i < _slot.AllowedItems.Length; i++) {
                                    if (_slot.AllowedItems[i] == cur.type) {
                                        // TODO: Remove the sprite renderers or something
                                        if (cur.spriteRenderer != null) {
                                            cur.spriteRenderer.sprite = null; 
                                        }
                                        cur.events.onRemove.Invoke(_slot.ItemObject);

                                        break; 
                                        }
                                }
                            }
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
                print($"Placed {_slot.ItemObject}  on {_slot.parent.inventory.type}, Allowed Items: {string.Join(", ", _slot.AllowedItems)}");

                for (int i = 0; i < _slot.item.buffs.Length; i++)
                {
                    for (int j = 0; j < attributes.Length; j++)
                    {
                        if (attributes[j].type == _slot.item.buffs[i].attribute)
                            attributes[j].value.AddModifier(_slot.item.buffs[i]);
                    }
                }

                if (_slot.ItemObject.characterDisplay2D != null) {
                    foreach(EquipmentStuff cur in equipmentStuff) {
                        if (_slot.tag == cur.tag) {
                        for (int i = 0; i < _slot.AllowedItems.Length; i++) {
                            if (_slot.AllowedItems[i] == cur.type) {

                                if (cur.spriteRenderer != null) {
                                    cur.spriteRenderer.sprite = _slot.ItemObject.characterDisplay2D; 
                                }
                                cur.events.onAdd.Invoke(_slot.ItemObject);
                                break; 

                            }

                        }
                        
                        // if (_slot.AllowedItems[0] == cur.type && _slot.tag == cur.tag) {

                        }
                    }

                }

                // OnItemEquiped.Invoke(_slot.ItemObject); 

                // if (_slot.ItemObject.GetType() == typeof(WeaponObject)) {
                //     this.gameObject.GetComponent<PlayerWeapon>().SetWeapon(_slot.WeaponObject.weapon); 
                // }
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
        currentItemPos = new Vector3(0.55f,2.15f,0);

        // boneCombiner = new BoneCombiner(gameObject);

        for (int i = 0; i < attributes.Length; i++)
        {
            attributes[i].SetParent(this);
        }

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




[System.Serializable]
public class Attribute
{
    [System.NonSerialized] public PlayerItem2D parent;
    public Attributes type;
    public ModifiableInt value;

    public void SetParent(PlayerItem2D _parent)
    {
        parent = _parent;
        value = new ModifiableInt(AttributeModified);
    }

    public void AttributeModified()
    {
        parent.AttributeModified(this);
    }
}