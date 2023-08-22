using System.Collections;
using System.Collections.Generic;
using Laserbean.General;
using UnityEngine;

namespace unityInventorySystem
{

    public class BagInterfaceController : MonoBehaviour
    {
        [SerializeField] DynamicInterface userInterface; 


        string current_inventory_bag_name = ""; 

        public void OnBagEquip(InventorySlot _slot) {
            Debug.Log("bag equipped");

            var fish = InventoryStaticManager.GetInventoryObject(_slot.item.specialDict["Bag"].String); 

            current_inventory_bag_name = _slot.item.specialDict["BagID"].String; 
            if (string.IsNullOrEmpty(current_inventory_bag_name)) {
                _slot.item.specialDict["BagID"] = new SpecialData{String = RandomStatic.GenerateRandomString(10)};  
                current_inventory_bag_name = _slot.item.specialDict["BagID"].String; 
            } else {
                fish.inventory = SaveAnything.LoadJson<Inventory>(InventoryStaticManager.save_path, current_inventory_bag_name); 
            }

            userInterface?.SetInventoryObject(fish);
            userInterface?.SetupInventory();
        }

        public void OnBagDequip(InventorySlot _slot) {
            // userInterface.inventoryObject?.Save(InventoryStaticManager.save_path);
            if (userInterface.inventoryObject == null) return; 

            
            SaveAnything.SaveJson<Inventory>(userInterface.inventoryObject.inventory, InventoryStaticManager.save_path, current_inventory_bag_name); 
            userInterface.inventoryObject.Clear(); 
        
            userInterface.RemoveSlots(); 

        }

    }
}