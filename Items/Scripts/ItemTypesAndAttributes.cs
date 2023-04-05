using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// This thing can be changed i guess. 
namespace unityInventorySystem {

public enum ItemType 
{
    Consumable,
    Helmet,
    Weapon,
    Shield,
    Boots,
    Chest,
    Attachment, 
    SmallAmmo,
    LargeAmmo,
    ShotgunAmmo, 
    ArrowAmmo, 
    Default
}

public enum Attributes
{
    Agility,
    Defence,
    Stamina,
    Strength,
}

public enum EquipmentTag 
{
    Head, lefthand, righthand, body, feet, equipment

}

}