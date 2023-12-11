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

using unityInventorySystem.GuiEvents; 

public abstract class UserInterface : MonoBehaviour
{

    public InventoryObject inventoryObject;
    public Dictionary<GameObject, InventorySlot> slotsOnInterface = new();


    public static Action<InventorySlot> OnSlotSelect;
    public static Action<InventorySlot> OnSlotRelease;

    void Start()
    {
        if (inventoryObject == null) return;
        SetupInventory();
    }

    private void Awake()
    {
        EventManager.AddListener<ToggleInventoryEvent>(ToggleInventoryHandler);
    }


    private void OnDestroy()
    {
        EventManager.RemoveListener<ToggleInventoryEvent>(ToggleInventoryHandler);
    }


    private void ToggleInventoryHandler(ToggleInventoryEvent @event)
    {
        if (!@event.IsInventory)
            Deselect();
    }

    public void SetInventoryObject(InventoryObject _inventoryObject)
    {
        inventoryObject = _inventoryObject;
    }

    public void SetupInventory()
    {
        inventoryObject.inventory.SetSlotsAfterUpdate(OnSlotUpdate);
        inventoryObject.inventory.SetUserinterface(this);

        CreateSlots();
        AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInterface(gameObject); });
        AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnExitInterface(gameObject); });


        inventoryObject.inventory.UpdateSlots();
    }


    private void OnSlotUpdate(InventorySlot _slot)
    {
        EventManager.TriggerEvent(new SlotUpdatedEvent(_slot));
        bool notEmpty = !_slot.IsEmpty();

        _slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().sprite = notEmpty ? _slot.ItemObject.uiDisplay : null;
        _slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().color = notEmpty ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0);
        _slot.slotDisplay.GetComponentInChildren<TextMeshProUGUI>().text = !notEmpty || _slot.amount == 1 ? "" : _slot.amount.ToString("n0");
    }

    public void DisableSlot(InventorySlot _slot)
    {
        _slot.slotDisplay.GetComponent<Button>().interactable = false;
    }

    public void EnableSlot(InventorySlot _slot)
    {
        _slot.slotDisplay.GetComponent<Button>().interactable = true;
    }


    protected void SetEventTriggers(GameObject obj)
    {
        AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
        AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
        AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
        AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });

        // AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });
        AddEvent(obj, EventTriggerType.Drag, delegate (BaseEventData eventData) { OnDrag(obj, eventData); });

        AddEvent(obj, EventTriggerType.PointerClick, delegate { OnCLickedSlot(obj); });
        AddEvent(obj, EventTriggerType.Select, delegate { OnSelect(obj); });
        AddEvent(obj, EventTriggerType.Submit, delegate { OnSubmit(obj); });
    }


    public abstract void CreateSlots();

    protected void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry {
            eventID = type
        };
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public void OnEnter(GameObject obj)
    {
        MouseData.slotHoveredOver = obj;
    }

    public void OnExit(GameObject obj)
    {
        MouseData.slotHoveredOver = null;
    }

    public void OnEnterInterface(GameObject obj)
    {
        MouseData.interfaceMouseIsOver = obj.GetComponent<UserInterface>();
    }

    public void OnExitInterface(GameObject obj)
    {
        MouseData.interfaceMouseIsOver = null;
    }

    public void OnDragStart(GameObject obj)
    {
        MouseData.tempItemBeingDragged = CreateTempItem(obj);

        if (MouseData.tempItemBeingDragged == null) return;
        OnSlotSelect.Invoke(slotsOnInterface[obj]);

        if (MouseData.tempItemCanvasObject == null) {
            MouseData.tempItemCanvasObject = new GameObject();
            var ttcanvas = MouseData.tempItemCanvasObject.AddComponent<Canvas>();
            ttcanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            ttcanvas.sortingOrder = 100;

        }
        MouseData.tempItemBeingDragged.transform.SetParent(MouseData.tempItemCanvasObject.transform);
    }

    public GameObject CreateTempItem(GameObject obj)
    {
        GameObject tempItem = null;
        if (!slotsOnInterface[obj].IsEmpty()) {

            tempItem = new GameObject("drag item");
            var rt = tempItem.AddComponent<RectTransform>();

            rt.sizeDelta = new Vector2(50, 50);
            rt.localScale = new Vector3(1, 1, 1);

            var img = tempItem.AddComponent<Image>();
            img.sprite = slotsOnInterface[obj].ItemObject.uiDisplay;
            img.raycastTarget = false;


        }
        return tempItem;
    }


    public void OnDragEnd(GameObject obj)
    {
        if (MouseData.tempItemBeingDragged == null) return;
        Destroy(MouseData.tempItemBeingDragged);
        EndDragOrSecondClick(slotsOnInterface[obj]);
    }

    void EndDragOrSecondClick(InventorySlot islot)
    {
        if (MouseData.interfaceMouseIsOver == null) {
            // Debug.Log("Tried dragging out of inventory");

            EventManager.TriggerEvent(new ItemDroppedEvent(islot.item, islot.amount)); 
            islot.RemoveItem();


            return;
        }
        if (MouseData.slotHoveredOver) {
            InventorySlot mouseHoverSlotData = MouseData.interfaceMouseIsOver.slotsOnInterface[MouseData.slotHoveredOver];

            if (!islot.IsEmpty() && mouseHoverSlotData.CanPlaceInSlot(islot.ItemObject) && MouseData.slotHoveredOver.GetInstanceID() != islot.slotDisplay.GetInstanceID())
                inventoryObject.inventory.SwapItems(islot, mouseHoverSlotData);
        }
        Deselect();
    }

    public void OnDrag(GameObject obj, BaseEventData eventdata = null)
    {
        Vector2 pos = Vector2.zero;
        if (eventdata != null) {
            var pointervent = eventdata as PointerEventData;
            pos = pointervent.position;
        } else {
#if ENABLE_INPUT_SYSTEM
            pos = Mouse.current.position.ReadValue();
#else
            pos = Input.mousePosition;
#endif
        }

        if (MouseData.tempItemBeingDragged != null) {
            MouseData.tempItemBeingDragged.GetComponent<RectTransform>().position = pos;
        }
    }


    public void OnCLickedSlot(GameObject obj)
    {
        OnSubmit(obj);
    }

    public void OnSelect(GameObject obj)
    {
        // Debug.Log("Select"); 
        ButtonSelectedData.sinterface = obj.GetComponentInParent<UserInterface>();
        ButtonSelectedData.slotGO = obj;
    }

    public void OnSubmit(GameObject obj)
    {
        if (!SelectedSlot.isSelecting) {
            if (slotsOnInterface[obj].IsEmpty()) {
                OnSlotRelease.Invoke(null);
                return; //clicked empty slot. do nothing.
            }

            SelectedSlot.obj = ButtonSelectedData.slotGO;
            SelectedSlot.slot = ButtonSelectedData.sinterface.slotsOnInterface[ButtonSelectedData.slotGO];
            SelectedSlot.isSelecting = true;
            EventManager.TriggerEvent(new SlotSelectedEvent(SelectedSlot.slot));

            OnSlotSelect.Invoke(SelectedSlot.slot);
        } else {

            if (obj.GetInstanceID() == SelectedSlot.obj.GetInstanceID()) {
                Debug.Log("Selected same slot");
            } else {
                InventorySlot curIslot = ButtonSelectedData.sinterface.slotsOnInterface[ButtonSelectedData.slotGO];
                inventoryObject.inventory.SwapItems(curIslot, SelectedSlot.slot);
            }
            obj.GetComponent<Button>().OnDeselect(null);

            Deselect();
        }
    }

    public void Deselect()
    {
        if (SelectedSlot.obj == null) return;
        SelectedSlot.obj = null;
        SelectedSlot.Deselect();

        EventManager.TriggerEvent(new SlotSelectedEvent(new InventorySlot()));

    }




}




public static class MouseData
{
    public static UserInterface interfaceMouseIsOver;
    public static GameObject tempItemBeingDragged;
    public static GameObject tempItemCanvasObject;
    public static GameObject slotHoveredOver;
}

public static class SelectedSlot
{
    public static bool isSelecting = false;
    public static GameObject obj;
    public static InventorySlot slot;


    public static void Deselect()
    {
        isSelecting = false;
        UserInterface.OnSlotRelease.Invoke(null);
    }
}

public static class ButtonSelectedData
{
    public static UserInterface sinterface;
    public static GameObject slotGO;

}


