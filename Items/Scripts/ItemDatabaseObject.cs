using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Laserbean.SpecialData;

namespace unityInventorySystem {
    [CreateAssetMenu(fileName = "New Item Database", menuName = "unity Inventory System/Items/Database")]
    public class ItemDatabaseObject : ScriptableObject//, ISerializationCallbackReceiver
    {
        [SerializeField] CustomDictionary<string, ItemObject> ItemDict = new(); 

        public void Initialize() {
            UpdateDictionary();            
        }


        [EasyButtons.Button]
        public void UpdateDictionary() {
            ItemDict.Clear(); 
            var folderpath = UnityInventoryConfig.ITEMS_PATH;
            // var folderpath = "dfs";
            var list1 = Resources.LoadAll<ItemObject>(folderpath).ToList(); 

            foreach(var asset in list1) {
                if (asset is null) continue;

                var curstatusobject = asset as ItemObject;
                ItemDict.Add(curstatusobject.item.Name, curstatusobject);
            }
            Debug.Log(folderpath); 
        }


        public void OnEnable()
        {
            UpdateDictionary();
        }

        public ItemObject GetItemObject(string name) {
            return ItemDict[name]; 
        }
    }

public static class InventoryStaticManager
{

    public static string SavePath {
        get => GameManager.Instance.GamePath + "/"; 
    }


    static ItemDatabaseObject database; 

    const string DatabasePath = "UnityInventory/";

    public const string DEF_PLAYER_INV_NAME = "PlayerInventory";
    public const string DEF_PLAYER_EQUIP_NAME = "PlayerEquipment";
    public const string DEF_ITEM_DB_NAME = "ItemDB";

    public static ItemDatabaseObject GetDatabase(string name) {
        return Resources.Load<ItemDatabaseObject>(DatabasePath + name);
    }

    public static InventoryObject GetInventoryObject(string name) {
        return Resources.Load<InventoryObject>(DatabasePath + name);
    }

}


}
