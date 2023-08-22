using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using unityInventorySystem;


[CreateAssetMenu(fileName = "InventoryDB", menuName = "unity Inventory System/Inventory Database")]
public class InventoryDatabase : ScriptableObject
{

    public List<InventoryObject> inventoryObjects = new(); 

    [System.NonSerialized]
    Dictionary<string, InventoryObject> invDict = new(); 

    private void OnEnable() {
        OnValidate(); 
    }

    private void OnValidate() {
        invDict.Clear(); 
        foreach(var inv in inventoryObjects) {
            invDict.Add(inv.inventory.Name, inv); 
        }
    }

    public InventoryObject GetInventoryObject(string _name) {
        return invDict[_name]; 
    }
}
