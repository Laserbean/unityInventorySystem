using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using unityInventorySystem.Items;

namespace unityInventorySystem
{
    public class InventorySystemManager : Singleton<InventorySystemManager>
    {
        [SerializeField] ItemDatabaseObject itemDatabase;

        [SerializeField] string DefaultDatabaseName = "ItemDB";
        public ItemDatabaseObject ItemDatabase {
            get {
                if (itemDatabase == null) {
                    itemDatabase = InventoryStaticManager.GetDatabase(DefaultDatabaseName);
                }
                return itemDatabase;
            }
        }
    }
}
