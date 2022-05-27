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

}
}
