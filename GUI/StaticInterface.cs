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
        slotsOnInterface.Clear();
        for (int i = 0; i < inventoryObject.GetSlots.Length; i++) {
            var obj = slots[i];

            SetEventTriggers(obj);

            inventoryObject.GetSlots[i].slotDisplay = obj;

            // inventoryObject.GetSlots[i].SetSlotNumber(i); 
            slotsOnInterface.Add(obj, inventoryObject.GetSlots[i]);


        }
    }

    // public override void SelectSlot(GameObject obj)
    // {
    //     throw new System.NotImplementedException();
    // }
}
