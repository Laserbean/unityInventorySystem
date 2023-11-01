using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Laserbean.SpecialData;

namespace unityInventorySystem.Items
{
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
            var folderpath = UnityInventoryConfig.ItemsPath;
            // var folderpath = "dfs";
            var list1 = Resources.LoadAll<ItemObject>(folderpath).ToList(); 

            int ind = 0; 

            foreach(var asset in list1) {
                if (asset is null) continue;
                asset.item.Id = ind; 
                ItemDict.Add(asset.item.Name, asset);

                ind += 1; 
            }
            // Debug.Log(folderpath); 
        }


        public void OnEnable()
        {
            UpdateDictionary();
        }

        public ItemObject GetItemObject(string name) {
            return ItemDict[name]; 
        }
    }



}
