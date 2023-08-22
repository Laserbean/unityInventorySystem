using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace unityInventorySystem {
[CreateAssetMenu(fileName = "New Item Database", menuName = "unity Inventory System/Items/Database")]
public class ItemDatabaseObject : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField] string SavePath = "D:/unity_projects";
    public ItemObject[] ItemObjects;

    [System.NonSerialized]
    public static Dictionary<string, int> name_index_dict = new Dictionary<string, int>(); 


    private void OnEnable() {
        if(!string.IsNullOrEmpty(SavePath)) InventoryStaticManager.SetSavePath(SavePath); 
        UpdateID();

        name_index_dict.Clear(); 
        for (int i = 0; i < ItemObjects.Length; i++) {
            name_index_dict.Add(ItemObjects[i].item.Name, i); 
        }  
        
    }

    [ContextMenu("Update ID's")]
    public void UpdateID()
    {
        for (int i = 0; i < ItemObjects.Length; i++)
        {
            if (ItemObjects[i] == null) {
                Debug.LogWarning("Warning: ItemObject is null"); 
                continue;
            }
            if (ItemObjects[i].item.Id != i)
                ItemObjects[i].item.Id = i;
        }
    }
    public void OnAfterDeserialize()
    {
        UpdateID();

    }

    public void OnBeforeSerialize()
    {
        if(!string.IsNullOrEmpty(SavePath)) InventoryStaticManager.SetSavePath(SavePath); 

    }


    public ItemObject GetItemObject(string name) {
        return ItemObjects[name_index_dict[name]]; 
    }
}

public static class InventoryStaticManager
{
    static string _save_path; 

    public static string save_path {
        get => _save_path; 
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

    public static void SetSavePath(string path) {
        _save_path = path; 
    }
}


}
