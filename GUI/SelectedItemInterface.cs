using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using unityInventorySystem; 

public class SelectedItemInterface : MonoBehaviour
{
    private void Start() {
        itemName.text = "";
        description.text = ""; 
        usebutton.gameObject.SetActive(false); 
        
    }
    void OnEnable()
    {
        EventManager.AddListener<SlotSelectedEvent>(OnSlotSelected);       
    }

    void OnDisable() {
        EventManager.RemoveListener<SlotSelectedEvent>(OnSlotSelected);
    }

    [SerializeField] TMPro.TextMeshProUGUI itemName;
    [SerializeField] TMPro.TextMeshProUGUI description;
    [SerializeField] ItemDatabaseObject itemDatabase; 
    [SerializeField] Button usebutton; 


    InventorySlot slot; 

    void OnSlotSelected(SlotSelectedEvent slotSelectedEvent) {
        slot = slotSelectedEvent.slot; 

        itemName.text = slot.item.Name;
        if (slot.item.Name != "") {
            description.text = itemDatabase.ItemObjects[slot.item.Id].description;

            if (itemDatabase.ItemObjects[slot.item.Id].type == ItemType.Consumable) {
                usebutton.gameObject.SetActive(true); 
            }


        } else {
            description.text = ""; 

            usebutton.gameObject.SetActive(false); 

        }
    }

    public void OnButton() {
        // inventory.RemoveItem(slot.item);
        slot.RemoveAmount(1);  

    }

}
