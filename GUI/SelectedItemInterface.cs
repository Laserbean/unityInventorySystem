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
        EventManager.AddListener<SlotUpdatedEvent>(OnSlotUpdated);

    }

    void OnDisable() {
        EventManager.RemoveListener<SlotSelectedEvent>(OnSlotSelected);
        EventManager.RemoveListener<SlotUpdatedEvent>(OnSlotUpdated);

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
            //TODO show stats for food i guess.

        } else {
            description.text = ""; 

            usebutton.gameObject.SetActive(false); 

        }
    }

    void OnSlotUpdated(SlotUpdatedEvent evnt) {
        if (slot == evnt.slot) {
            if (evnt.slot.amount <= 0) {
                usebutton.gameObject.SetActive(false); 
            }
        }

    }

    public void OnButton() {
        // inventory.RemoveItem(slot.item);
        if (slot.ItemObject.IsConsumable()) {
            slot.RemoveAmount(1);  
            EventManager.TriggerEvent(new ConsumeItemEvent(slot.ItemObject.GetConsumable()));
        }
 

    }

}
