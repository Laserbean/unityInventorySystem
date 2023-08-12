


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq; 

using unityInventorySystem;

using UnityEngine.Events; 

public class ItemPickup : MonoBehaviour {


    [SerializeField] InventoryObject inventory; 
    public void OnTriggerEnter2D(Collider2D other)
    {
        var gitem = other.GetComponent<GroundItem>();
        if (gitem) {
            Item _item = new Item(gitem.item);
            // Debug.Log(_item.ID);
            inventory.AddItem(_item, gitem.ammount);
            Destroy(other.gameObject);
        }
    }
    
}