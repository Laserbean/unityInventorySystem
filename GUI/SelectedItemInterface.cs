using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using unityInventorySystem;

using unityInventorySystem.Inventories;
using unityInventorySystem.Items;

using unityInventorySystem.GuiEvents;

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

    void Awake()
    {
        EventManager.AddListener<SlotSelectedEvent>(OnSlotSelected);
        EventManager.AddListener<SlotUpdatedEvent>(OnSlotUpdated);
    }

    void OnDestroy()
    {
        EventManager.RemoveListener<SlotSelectedEvent>(OnSlotSelected);
        EventManager.RemoveListener<SlotUpdatedEvent>(OnSlotUpdated);
    }


    InventorySlot slot;

    void OnSlotSelected(SlotSelectedEvent slotSelectedEvent)
    {
        slot = slotSelectedEvent.slot;

        itemNameTMP.text = slot.item.Name;
        if (!slot.IsEmpty()) {
            string description = itemDatabase.GetItemObject(slot.item.Name).description;

            foreach (var itemcomponents in slot.item.Components) {
                description += itemcomponents.ToString();
                description += "\n";
            }


            if (usebutton != null && itemDatabase.GetItemObject(slot.item.Name).type == ItemType.Consumable) {
                usebutton.gameObject.SetActive(true);
            }

            descriptionTMP.text = description;
            //TODO show stats for stuff.

        } else {
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
        // inventoryObject.RemoveItem(slot.item);
        // if (slot.item.GetItemComponent<BuffItemComponent>() is BuffItemComponent buffcomp) {
        //     slot.RemoveAmount(1);  
        // } 
        // // EventManager.TriggerEvent(new ConsumeItemEvent(((ConsumableObject)slot.ItemObject).consumable));

        slot.item.UseComponents(GameManager.Instance.player);

    }

}
