using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using System.Linq;

using System.IO;

#if UNITY_EDITOR
using UnityEditorInternal;
#endif

using Laserbean.General;
namespace unityInventorySystem {


[CreateAssetMenu(fileName = "ConfigObject", menuName = "unity Inventory System/Config", order = 5)]
public class ConfigObject : ScriptableObject {

    public List<string> tags = new List<string>();


}

// public static class InventoryDBs {
//     private static InventoryDatabaseObject _inventoryDB;
//     private static ItemDatabaseObject _itemDB;

//     public static InventoryDatabaseObject inventoryDB {
//         get {
//             if (_inventoryDB == null) {
//                 #if UNITY_EDITOR
//                     _inventoryDB = AssetDatabase.LoadAssetAtPath<InventoryDatabaseObject>("Assets/Resources/LB_Inventory/InventoryDB.asset");
//                 #else
//                     _inventoryDB = Resources.Load<InventoryDatabaseObject>("InventoryDB");
//                 #endif
//             }
//             return _inventoryDB; 

//         }
//     }

//     public static ItemDatabaseObject itemDB {
//         get {
//             if (_itemDB == null) {
//                 #if UNITY_EDITOR
//                     _itemDB = AssetDatabase.LoadAssetAtPath<ItemDatabaseObject>("Assets/Resources/LB_Inventory/ItemDB.asset");
//                 #else
//                     _itemDB = Resources.Load<ItemDatabaseObject>("ItemDB");
//                 #endif
//             }
//             return _itemDB; 

//         }
//     }
// }



#if UNITY_EDITOR
public static class ConfigStatic {

    private static string folderPath = ""; 

    private static ConfigObject configObject; 


    private static void UpdatePath () {
        ConfigObject fakeConfig = ScriptableObject.CreateInstance<ConfigObject>();
        folderPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(fakeConfig)));
    }

    public static ConfigObject GetConfigObject() {
        // string[] tags = { "Head", "Body", "Hand", "Feet" };

        // TagList tagList = Resources.Load<TagList>("TagList");
        if (configObject != null) {
            return configObject; 
        }

        if (folderPath == "") UpdatePath();

        if (configObject == null) {
            configObject = AssetDatabase.LoadAssetAtPath<ConfigObject>(folderPath + "/ConfigObject.asset");
            if (configObject != null && configObject.tags.Count == 0) {
                // configObject.tags = tags.ToList(); 
            }
        }

        if (configObject == null) {
            ScriptableObjectUtility.CreateAsset<ConfigObject>("ConfigObject", folderPath); 
            configObject = AssetDatabase.LoadAssetAtPath<ConfigObject>(folderPath + "/ConfigObject.asset");
            // configObject.tags = tags.ToList(); 

             EditorUtility.DisplayDialog("Created Config asset",
                "Make sure you set the custom tags.", "Okay");
        }

        return configObject; 

    }
}
#endif

}
