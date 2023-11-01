using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using unityInventorySystem; 

public class SelectedItemInterface : MonoBehaviour
{    
    [SerializeField] TMPro.TextMeshProUGUI itemNameTMP;
    [SerializeField] TMPro.TextMeshProUGUI descriptionTMP;

    [SerializeField] string databaseName; 
    [SerializeField] ItemDatabaseObject itemDatabase; 
    [SerializeField] Button usebutton; 

    private void Start() {
        itemNameTMP.text = "";
        descriptionTMP.text = ""; 
        usebutton.gameObject.SetActive(false); 

        if (string.IsNullOrEmpty(databaseName))
            itemDatabase = InventoryStaticManager.GetDatabase(databaseName);
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


    InventorySlot slot; 

    void OnSlotSelected(SlotSelectedEvent slotSelectedEvent) {
        slot = slotSelectedEvent.slot; 

        itemNameTMP.text = slot.item.Name;
        if (slot.item.Name != "") {

            string description = itemDatabase.GetItemObject(slot.item.Name).description;

            foreach(var itembuff in slot.item.buffs) {
                description += "\n" + itembuff.attribute + " " + itembuff.value;  
            }

            if (itemDatabase.GetItemObject(slot.item.Name).type == ItemType.Consumable) {
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
        // // inventoryObject.RemoveItem(slot.item);
        // if (slot.ItemObject is ConsumableObject) {
        //     slot.RemoveAmount(1);  
        //     EventManager.TriggerEvent(new ConsumeItemEvent(((ConsumableObject)slot.ItemObject).consumable));
        // } FIXME
 

    }

}
