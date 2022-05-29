using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using unityInventorySystem;


[System.Serializable]
public class EquipmentSprite {
    public ItemType type;
    public SpriteRenderer spriteRenderer; 
}

// [System.Serializable]
// public class EquipmentSprites {
//     public List<EquipmentSprite> equipmentSprite = new List<EquipmentSprite>(); 

//     public SpriteRenderer GetSprite(ItemType type) {
//         foreach(EquipmentSprite cur in equipmentSprite) {
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

    public List<EquipmentSprite> equipmentSprite = new List<EquipmentSprite>(); 


    public void OnTriggerEnter2D(Collider2D other)
    {
        var item = other.GetComponent<unityInventorySystem.GroundItem>();
        if (item) {
            Item _item = new Item(item.item);
            Debug.Log(_item.Id);
            inventory.AddItem(_item, 1);
            Destroy(other.gameObject);
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
                        foreach(EquipmentSprite cur in equipmentSprite) {
                            if (_slot.AllowedItems[0] == cur.type) {
                                cur.spriteRenderer.sprite = null; 
                                break; 
                            }
                        }

                    }
                }

                if (_slot.ItemObject.GetType() == typeof(WeaponObject)) {
                    this.gameObject.GetComponent<PlayerWeapon>().SetDefaultWeapon(); 
                }

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
                print(
                    $"Placed {_slot.ItemObject}  on {_slot.parent.inventory.type}, Allowed Items: {string.Join(", ", _slot.AllowedItems)}");

                for (int i = 0; i < _slot.item.buffs.Length; i++)
                {
                    for (int j = 0; j < attributes.Length; j++)
                    {
                        if (attributes[j].type == _slot.item.buffs[i].attribute)
                            attributes[j].value.AddModifier(_slot.item.buffs[i]);
                    }
                }

                if (_slot.ItemObject.characterDisplay2D != null) {
                    foreach(EquipmentSprite cur in equipmentSprite) {
                        if (_slot.AllowedItems[0] == cur.type) {
                            cur.spriteRenderer.sprite = _slot.ItemObject.characterDisplay2D; 
                            break; 
                        }
                    }

                }

                if (_slot.ItemObject.GetType() == typeof(WeaponObject)) {
                    this.gameObject.GetComponent<PlayerWeapon>().SetWeapon(_slot.WeaponObject.weapon); 
                }
                break;
            case InterfaceType.Chest:
                break;
            default:
                break;
        }

    }

                        // switch (_slot.AllowedItems[0])
                    // {
                        // case ItemType.Shield:
                            // switch (_slot.ItemObject.type

                    // }


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

    // Update is called once per frame
    // void Update()
    // {
    //     if (Input.GetMouseButtonDown(0)) {
    //         Vector3 playerpos = Camera.main.WorldToScreenPoint(this.transform.position);
    //         GameObject bullet = ObjectPooler.SharedInstance.GetPooledObject(1); 
    //         bullet.SetActive(true); 

    //         bullet.GetComponent<Bullet>().Init(Input.mousePosition - playerpos);
    //         bullet.transform.position = this.transform.position + currentItemPos.Rotate(this.transform.rotation.eulerAngles.z); 
    //         bullet.transform.rotation = this.transform.rotation; 
    //         // bullet.transform.localPosition = currentItemPos; 

            
    //     }
    // }

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