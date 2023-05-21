using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using unityInventorySystem; 

public class SelectedItemInterface : MonoBehaviour
{
    private void Start() {
        itemNameTMP.text = "";
        descriptionTMP.text = ""; 
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

    [SerializeField] TMPro.TextMeshProUGUI itemNameTMP;
    [SerializeField] TMPro.TextMeshProUGUI descriptionTMP;
    [SerializeField] ItemDatabaseObject itemDatabase; 
    [SerializeField] Button usebutton; 


    InventorySlot slot; 

    void OnSlotSelected(SlotSelectedEvent slotSelectedEvent) {
        slot = slotSelectedEvent.slot; 

        itemNameTMP.text = slot.item.Name;
        if (slot.item.Name != "") {

            string description = itemDatabase.ItemObjects[slot.item.Id].description;

            foreach(var itembuff in slot.item.buffs) {
                description += "\n" + itembuff.attribute + " " + itembuff.value;  
            }

            if (itemDatabase.ItemObjects[slot.item.Id].type == ItemType.Consumable) {
                usebutton.gameObject.SetActive(true); 
            }


            descriptionTMP.text = description; 
            //TODO show stats for stuff.

        } else {
            descriptionTMP.text = ""; 

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
