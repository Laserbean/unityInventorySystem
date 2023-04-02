using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using unityInventorySystem; 

public class SlotSelection : Singleton<SlotSelection>
{
    // Start is called before the first frame update

    public bool isSelecting = false; 

    public Image image; 

    public int slotNumber;
    // public InventorySlot inventorySlot; 

    // public GameObject obj; 

    void Start()
    {
        image = this.GetComponent<Image>();
        // inventorySlot = null; 
    }


}


