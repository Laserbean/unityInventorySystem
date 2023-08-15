


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq; 


using UnityEngine.Events; 

namespace unityInventorySystem {
public class ItemPickup : MonoBehaviour, IPickUp
{

    [SerializeField] bool pickupOnTrigger = false; 

    [SerializeField] InventoryObject inventory; 
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (!pickupOnTrigger) return; 
        
        var gitem = other.GetComponent<IGroundItem>();
        if (gitem != null) {
            // Debug.Log(_item.ID);
            inventory.inventory.AddItem(gitem.GetItem(), gitem.amount);
            gitem.DestroyItem();
        }
    }

    void IPickUp.PickUpItem(IGroundItem groundItem)
    {            
        inventory.inventory.AddItem(groundItem.GetItem(), groundItem.amount);
        groundItem.DestroyItem();
    }
}

public interface IPickUp {
    public void PickUpItem(IGroundItem groundItem); 
}

public interface IGroundItem {
    public Item GetItem(); 
    public void DestroyItem(); 
    public void SetItem(Item item, int amount = 1); 
    public int amount {get;}
}
}