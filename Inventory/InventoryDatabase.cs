using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using unityInventorySystem;

using Laserbean.SpecialData;
using System.Linq; 


[CreateAssetMenu(fileName = "InventoryDB", menuName = "unity Inventory System/Inventory Database")]
public class InventoryDatabase : ScriptableObject
{

    [SerializeField] CustomDictionary<string, InventoryObject> InventoryDict = new(); 
    

    private void OnEnable() {
        UpdateDictionary();
    }


    [EasyButtons.Button]
    public void UpdateDictionary() {
        InventoryDict.Clear(); 
        var folderpath = UnityInventoryConfig.InventoryPath;
        // var folderpath = "dfs";
        var list1 = Resources.LoadAll<InventoryObject>(folderpath).ToList(); 

        foreach(var asset in list1) {
            if (asset is null) continue;
            InventoryDict.Add(asset.inventory.Name, asset);
        }
        Debug.Log(folderpath); 
    }


    public InventoryObject GetInventoryObject(string _name) {
        return InventoryDict[_name]; 
    }
}
