using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace unityInventorySystem.Inventories
{

    public class InventoryController : MonoBehaviour
    {
        [SerializeField] InventoryDatabase inventoryDatabase; 
        [SerializeField] public Inventory inventory_i;
        [SerializeField] public Inventory equipment_i; 

        [SerializeField] InventoryObject inventoryObject; 
        [SerializeField] InventoryObject equipmentObject; 

        void Awake()
        {
            if (inventoryObject != null) inventory_i = inventoryObject.inventory;
            if (equipmentObject != null) equipment_i = equipmentObject.inventory;
        }


        // void Update()
        // {

        // }
    }
}
