using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using unityInventorySystem; 

public class StaticInterface : UserInterface
{
    public GameObject[] slots;

    public override void CreateSlots()
    {
        slotsOnInterface = new Dictionary<GameObject, InventorySlot>();
        for (int i = 0; i < inventory.GetSlots.Length; i++)
        {
            var obj = slots[i];

            SetEventTriggers(obj); 

            inventory.GetSlots[i].slotDisplay = obj;

            // inventory.GetSlots[i].SetSlotNumber(i); 
            slotsOnInterface.Add(obj, inventory.GetSlots[i]);

                
        }
    }

    // public override void SelectSlot(GameObject obj)
    // {
    //     throw new System.NotImplementedException();
    // }
}
