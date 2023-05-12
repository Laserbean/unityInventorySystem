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
    Placeable, 
    Default
}

public enum AttributeType
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



/*

Player properties? ATtributes???
Dexterity should increase aim accuracy of weapons i guess. 


*/