using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using Object = UnityEngine.Object;

using System;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using Laserbean.General;

namespace unityInventorySystem {

public static class SetupFolderContextMenu 
{
    const string folderpath = "Assets/Resources/UnityInventory";

    [UsedImplicitly]
    [MenuItem("Assets/unityInventory/Initialize Scriptable Objects", false, 0)]
    public static void InitializeDatabase()
    {

        // Object[] selectedAssets = Selection.GetFiltered(typeof(Object), SelectionMode.Assets);
        // if (selectedAssets.Length > 0)
        // {
        //     string selectedFolder = AssetDatabase.GetAssetPath(selectedAssets[0]);
        //     Debug.Log("Selected Folder: " + selectedFolder);
        // } else
        // {
        //     Debug.LogWarning("No assets selected. Right-click on a folder to use this menu.");
        // }


        if (!Directory.Exists(folderpath))
            Debug.Log("Created: " + folderpath);
            Directory.CreateDirectory(folderpath);
        if (!Directory.Exists(folderpath + "/Items"))
            Debug.Log("Created: " + folderpath + "/Items");
            Directory.CreateDirectory(folderpath + "/Items");

        

        if (!SaveAnything.FileExists(folderpath, InventoryStaticManager.DEF_ITEM_DB_NAME, "asset")) {
            var itemDB = ScriptableObject.CreateInstance<ItemDatabaseObject>();
            AssetDatabase.CreateAsset(itemDB, folderpath + "/ItemDB.asset");
        }

        if (!SaveAnything.FileExists(folderpath, InventoryStaticManager.DEF_PLAYER_EQUIP_NAME, "asset")) {
            var itemDB1 = ScriptableObject.CreateInstance<InventoryObject>();
            AssetDatabase.CreateAsset(itemDB1, folderpath + "/PlayerEquipment.asset");
        }

        if (!SaveAnything.FileExists(folderpath, InventoryStaticManager.DEF_PLAYER_INV_NAME, "asset")) {
            var itemDB2 = ScriptableObject.CreateInstance<InventoryObject>();
            AssetDatabase.CreateAsset(itemDB2, folderpath + "/PlayerInventory.asset");
        }



        
    }

}

}
