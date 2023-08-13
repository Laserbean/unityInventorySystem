using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace unityInventorySystem {
[CreateAssetMenu(fileName = "New Item Database", menuName = "unity Inventory System/Items/Database")]
public class ItemDatabaseObject : ScriptableObject, ISerializationCallbackReceiver
{
    public ItemObject[] ItemObjects;

    [ContextMenu("Update ID's")]
    public void UpdateID()
    {
        for (int i = 0; i < ItemObjects.Length; i++)
        {
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
    }

    // [System.NonSerialized]
    // public static Dictionary<string, int> name_index_dict; 

    // public ItemObject GetItemObject(string name) {
    //     if (name_index_dict == null) {
    //         name_index_dict = new Dictionary<string, int>(); 
    //         for (int i = 0; i < ItemObjects.Length; i++)
    //         {
    //             name_index_dict.Add(ItemObjects[i].data.Name, i); 
    //         }  
    //     }
    //     return ItemObjects[name_index_dict[name]]; 
    // }
}

public static class ItemClassManager
{
    public static string save_path = "D:/unity_projects"; 
    static ItemDatabaseObject database; 

    const string DatabasePath = "UnityInventory/";

    public static ItemDatabaseObject GetDatabase(string name) {
        return Resources.Load<ItemDatabaseObject>(DatabasePath + name);
    }

}


}
