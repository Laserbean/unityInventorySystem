using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// This thing can be changed i guess. 
namespace unityInventorySystem {

public enum ItemType 
{
    Food,
    Helmet,
    Weapon,
    Shield, //should be only for inventory equipping
    Boots,
    Chest,
    Attachment, 
    Ammo,
    Default
}

public enum Attributes
{
    Agility,
    Intellect,
    Stamina,
    Strength
}

}