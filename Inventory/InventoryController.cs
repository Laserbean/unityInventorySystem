using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace unityInventorySystem.Inventories
{

    public class InventoryController : MonoBehaviour
    {
        [SerializeField] InventoryDatabase inventoryDatabase; 
        [SerializeField] Inventory inventory_i;
        [SerializeField] Inventory equipment_i; 

        [SerializeField] InventoryObject inventoryObject; 
        [SerializeField] InventoryObject equipmentObject; 

        void Start()
        {
            // inventory_i = inventoryDatabase.GetInventoryObject(UnityInventoryConfig.DEF_PLAYER_INV_NAME).CopyInventory();
            // equipment_i = inventoryDatabase.GetInventoryObject(UnityInventoryConfig.DEF_PLAYER_EQUIP_NAME).CopyInventory();

            if (inventoryObject != null) inventory_i = inventoryObject.inventory;
            if (equipmentObject != null) equipment_i = equipmentObject.inventory;
        }

        // void Update()
        // {

        // }
    }
}
