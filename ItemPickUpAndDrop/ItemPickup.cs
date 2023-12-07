


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;


using UnityEngine.Events;

using unityInventorySystem.Inventories;

namespace unityInventorySystem.Items
{
    public class ItemPickup : MonoBehaviour, IPickUp
    {

        [SerializeField] bool pickupOnTrigger = false;

        [SerializeField] InventoryObject inventory;
        public void OnTriggerStay2D(Collider2D other)
        {
            if (!pickupOnTrigger) return;

            var gitem = other.GetComponent<IGroundItem>();
            if (gitem == null) return;
            if (!gitem.CanPickUp) return;
            AddItem(gitem);
        }

        void IPickUp.PickUpItem(IGroundItem groundItem)
        {
            AddItem(groundItem);
        }

        void AddItem(IGroundItem groundItem)
        {
            bool success = inventory.inventory.TryAddItem(groundItem.GetItem(), groundItem.Amount);
            if (!success) return;
            groundItem.DestroyItem();
        }
    }

    public interface IPickUp
    {
        public void PickUpItem(IGroundItem groundItem);
    }

    public interface IGroundItem
    {
        public Item GetItem();
        public void DestroyItem();
        public void SetItem(Item item, int amount = 1);
        public int Amount { get; }

        public bool CanPickUp { get; }
    }
}