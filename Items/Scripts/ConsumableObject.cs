using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace unityInventorySystem {

[CreateAssetMenu(fileName = "New Item", menuName = "unity Inventory System/Items/Consumable")]
public class ConsumableObject : ItemObject
{
    public Consumable consumable; 

    public ConsumableObject() {
        type = ItemType.Consumable;
    }

    // private void OnValidate() {
    //     weapon.sprite = this.characterDisplay2D; 
    // }

    public override Consumable GetConsumable() {
        return consumable; 
    }

    public override bool IsConsumable() {
        return true; 
    }
}
}


