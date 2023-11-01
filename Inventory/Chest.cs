using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Laserbean.SpecialData;
using Laserbean.General;
using unityInventorySystem.Inventories;

namespace unityInventorySystem.Bags
{

    public class Chest : MonoBehaviour
    {

        string inventory_id = "";

        Inventory inventory;
        void Start()
        {

        }

        void Update()
        {

        }

        string current_inventory_bag_name = "";

        // public void OnBagEquip(InventorySlot _slot) {
        //     gameObject.SetActive(true); 
        //     var fish = InventoryStaticManager.GetInventoryObject(_slot.item.specialDict["Bag"].String); 

        //     current_inventory_bag_name = _slot.item.specialDict["BagID"].String; 
        //     if (string.IsNullOrEmpty(current_inventory_bag_name)) {
        //         _slot.item.specialDict["BagID"] = new SpecialData{String = RandomStatic.GenerateRandomString(10)};  
        //         current_inventory_bag_name = _slot.item.specialDict["BagID"].String; 
        //     } else {
        //         Debug.Log("Bag Loaded from to: " + InventoryStaticManager.SavePath + "/ " + current_inventory_bag_name);

        //         fish.inventory = SaveAnything.LoadJson<Inventory>(InventoryStaticManager.SavePath, current_inventory_bag_name); 

        //     }

        //     // userInterface?.SetInventoryObject(fish);
        //     // userInterface?.SetupInventory();
        // }
    }


}
