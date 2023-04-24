using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace unityInventorySystem {

[CreateAssetMenu(fileName = "New Item", menuName = "unity Inventory System/Items/Weapon")]
public class WeaponObject : ItemObject
{
    public Weapon weapon; 

    public WeaponObject() {
        type = ItemType.Weapon;
    }


    protected override void OnValidatee()
    {
        base.OnValidatee(); // call the base implementation to ensure the ItemObject OnValidate is also called
        // add any additional validation specific to the weapon here
        weapon.sprite = this.characterDisplay2D; 

    }

    public override Weapon GetWeapon() {
        return weapon; 
    }

    public override bool IsWeapon() {
        return true; 
    }
}
}
