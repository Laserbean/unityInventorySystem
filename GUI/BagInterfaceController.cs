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
            }
            userInterface?.SetInventoryObject(fish);
            userInterface?.SetupInventory();
        }

        public void OnBagDequip(InventorySlot _slot) {


        }

    }
}