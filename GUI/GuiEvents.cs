
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;

using UnityEngine.InputSystem;


using unityInventorySystem.Inventories;

namespace unityInventorySystem.GuiEvents
{

    public class SlotSelectedEvent
    {
        public InventorySlot slot;

        public SlotSelectedEvent(InventorySlot _slot)
        {
            slot = _slot;
        }
    }


    public class SlotUpdatedEvent
    {
        public InventorySlot slot;

        public SlotUpdatedEvent(InventorySlot _slot)
        {
            slot = _slot;
        }
    }

    public class ToggleInventoryEvent
    {
        public bool IsInventory { get; private set; }

        public ToggleInventoryEvent(bool fish)
        {
            IsInventory = fish;
        }
    }

    public class ItemDroppedEvent 
    {
        public Item item;

        public ItemDroppedEvent(Item item)
        {
            this.item = item;
        }
    }

}
