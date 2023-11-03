using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using unityInventorySystem.Inventories;
using unityInventorySystem.Items;
namespace unityInventorySystem
{

    public static class UnityInventoryConfig
    {
        // Following folders are in the Resources folder
        public const string ItemsPath = "UnityInventory/Items/";
        public const string InventoryPath = "UnityInventory/Inventories/";

        public const string DEF_PLAYER_INV_NAME = "PlayerInventory";
        public const string DEF_PLAYER_EQUIP_NAME = "PlayerEquipment";
        public const string DEF_ITEM_DB_NAME = "ItemDB";
    }


    public static class InventoryStaticManager
    {

        public static string SavePath {
            get => GameManager.Instance.GamePath + "/";
        }


        static ItemDatabaseObject database;

        const string DatabasePath = "UnityInventory/";



        public static ItemDatabaseObject GetDatabase(string name)
        {
            return Resources.Load<ItemDatabaseObject>(DatabasePath + name);
        }

        public static InventoryObject GetInventoryObject(string name)
        {
            return Resources.Load<InventoryObject>(DatabasePath + name);
        }

    }

}


