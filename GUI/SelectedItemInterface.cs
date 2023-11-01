using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using unityInventorySystem;

using unityInventorySystem.Inventories;
using unityInventorySystem.Items;
using unityInventorySystem.Items.Components;

public class SelectedItemInterface : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI itemNameTMP;
    [SerializeField] TMPro.TextMeshProUGUI descriptionTMP;

    [SerializeField] string databaseName;
    [SerializeField] ItemDatabaseObject itemDatabase;
    [SerializeField] Button usebutton;

    private void Start()
    {
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

    void OnDisable()
    {
        EventManager.RemoveListener<SlotSelectedEvent>(OnSlotSelected);
        EventManager.RemoveListener<SlotUpdatedEvent>(OnSlotUpdated);
    }


    InventorySlot slot;

    void OnSlotSelected(SlotSelectedEvent slotSelectedEvent)
    {
        slot = slotSelectedEvent.slot;

        itemNameTMP.text = slot.item.Name;
        if (slot.item.Name != "") {
            string description = itemDatabase.GetItemObject(slot.item.Name).description;


            if (slot.item.GetItemComponent<BuffItemComponent>() is BuffItemComponent buffcomp) {
                foreach (var itembuff in buffcomp.buffs) {
                    description += "\n" + itembuff.attribute + " " + itembuff.value;
                }
            }

            if (itemDatabase.GetItemObject(slot.item.Name).type == ItemType.Consumable) {
                usebutton.gameObject.SetActive(true);
            }

            descriptionTMP.text = description;
            //TODO show stats for stuff.

        }
        else {
            descriptionTMP.text = "";
            if (usebutton != null) 
                usebutton?.gameObject.SetActive(false);

        }
    }

    void OnSlotUpdated(SlotUpdatedEvent evnt)
    {
        if (slot == evnt.slot && evnt.slot.amount <= 0 && usebutton != null) {
            usebutton.gameObject.SetActive(false);
        }

    }

    public void OnButton()
    {
        // // inventoryObject.RemoveItem(slot.item);
        // if (slot.ItemObject is ConsumableObject) {
        //     slot.RemoveAmount(1);  
        //     EventManager.TriggerEvent(new ConsumeItemEvent(((ConsumableObject)slot.ItemObject).consumable));
        // } FIXME


    }

}
