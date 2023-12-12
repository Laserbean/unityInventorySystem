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

    #region EventHandlers

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

    #endregion

    // public void SetInventoryObject(InventoryObject _inventoryObject)
    // {
    //     inventoryObject = _inventoryObject;
    // }

    public void SetupInventory()
    {
        inventoryObject.inventory.SetSlotsAfterUpdate(OnSlotUpdate);
        inventoryObject.inventory.SetUserinterface(this);

        CreateSlots();
        AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInterface(gameObject); });
        AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnExitInterface(gameObject); });


        inventoryObject.inventory.UpdateSlots();
    }

    protected void AddEvent(GameObject obj, EventTriggerType type, UnityAction<GameObject, BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry {
            eventID = type
        };

        // Adjust the UnityAction to include the additional parameter
        UnityAction<BaseEventData> modifiedAction = (arg0) => {
            action.Invoke(obj, arg0); // Invoke the original action with the additional parameter
        };

        eventTrigger.callback.AddListener(modifiedAction);
        trigger.triggers.Add(eventTrigger);
    }


    private void OnSlotUpdate(InventorySlot _slot)
    {
        EventManager.TriggerEvent(new SlotUpdatedEvent(_slot));
        bool notEmpty = !_slot.IsEmpty;

        _slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().sprite = notEmpty ? _slot.ItemObject.uiDisplay : null;
        _slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().color = notEmpty ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0);
        _slot.slotDisplay.GetComponentInChildren<TextMeshProUGUI>().text = !notEmpty || _slot.amount == 1 ? "" : _slot.amount.ToString("n0");
    }

    #region SlotDisenableing
    public void DisableSlot(InventorySlot _slot)
    {
        _slot.slotDisplay.GetComponent<Button>().interactable = false;
    }

    public void EnableSlot(InventorySlot _slot)
    {
        _slot.slotDisplay.GetComponent<Button>().interactable = true;
    }
    #endregion

    protected void SetEventTriggers(GameObject obj)
    {
        AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnPointerEnter(obj); });
        AddEvent(obj, EventTriggerType.PointerExit, delegate { OnPointerExit(obj); });

        AddEvent(obj, EventTriggerType.BeginDrag, OnDragStart);
        AddEvent(obj, EventTriggerType.EndDrag,  OnDragEnd);

        AddEvent(obj, EventTriggerType.Drag, OnDrag);

        AddEvent(obj, EventTriggerType.PointerClick, delegate { OnCLickedSlot(obj); });
        AddEvent(obj, EventTriggerType.Select, delegate { OnSelect(obj); });
        AddEvent(obj, EventTriggerType.Submit, delegate { OnSubmit(obj); });


    }

    // private void OnPointerClickTest(GameObject go, BaseEventData arg0)
    // {
    //     // if ((arg0 as PointerEventData).button == PointerEventData.InputButton.Right) {
    //     //     Debug.Log("Right Mouse Button Clicked on: " + name);
    //     // }

    // }

    public abstract void CreateSlots();


    UIState interfaceState = UIState.Nothing;

    public void OnEnterInterface(GameObject obj)
    {
        MouseData.interfaceMouseIsOver = obj.GetComponent<UserInterface>();
    }

    public void OnExitInterface(GameObject obj)
    {
        MouseData.interfaceMouseIsOver = null;
    }

    public void OnPointerEnter(GameObject obj)
    {
        MouseData.slotHoveredOver = obj;
    }

    public void OnPointerExit(GameObject obj)
    {
        MouseData.slotHoveredOver = null;
    }

    public void OnDragStart(GameObject obj, BaseEventData eventdata = null)
    {
        var slot = slotsOnInterface[obj];
        if (interfaceState != UIState.Nothing || slot.IsEmpty)
            return;

        bool isRight = eventdata != null && (eventdata as PointerEventData).button == PointerEventData.InputButton.Right;


        interfaceState = UIState.Dragging;

        MouseData.CreateTempItem(slot);
        OnSlotSelect.Invoke(slot);
    }


    public void OnDragEnd(GameObject obj, BaseEventData eventdata = null)
    {

        interfaceState = UIState.Nothing;

        MouseData.DestroyTempItem();

        var islot = slotsOnInterface[obj];

        if (MouseData.interfaceMouseIsOver == null) {
            DropItem(islot);
            return;
        }

        bool isRight = eventdata != null && (eventdata as PointerEventData).button == PointerEventData.InputButton.Right;

        if (MouseData.slotHoveredOver) {
            InventorySlot mouseHoverSlotData = MouseData.interfaceMouseIsOver.slotsOnInterface[MouseData.slotHoveredOver];

            if (!islot.IsEmpty && mouseHoverSlotData.IsEmpty && isRight) {
                inventoryObject.inventory.SplitItems(islot, mouseHoverSlotData); 
            }
            if (!islot.IsEmpty && mouseHoverSlotData.CanPlaceInSlot(islot.ItemObject) && MouseData.slotHoveredOver.GetInstanceID() != islot.slotDisplay.GetInstanceID())
                inventoryObject.inventory.SwapItems(islot, mouseHoverSlotData);
        }
        Deselect();
    }

    void DropItem(InventorySlot islot)
    {
        interfaceState = UIState.Nothing;

        EventManager.TriggerEvent(new ItemDroppedEvent(islot.item, islot.amount));
        islot.RemoveItem();

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

        MouseData.SetTempItemPosition(pos);
    }


    public void OnCLickedSlot(GameObject obj)
    {
        ButtonSelectedData.sinterface = obj.GetComponentInParent<UserInterface>();
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
        interfaceState = UIState.Nothing;

        if (!SelectedSlot.isSelecting) {
            if (slotsOnInterface[obj].IsEmpty) {
                OnSlotRelease.Invoke(null);
                return;
            }

            SelectedSlot.obj = ButtonSelectedData.slotGO;
            SelectedSlot.slot = ButtonSelectedData.sinterface.slotsOnInterface[ButtonSelectedData.slotGO];
            SelectedSlot.isSelecting = true;
            EventManager.TriggerEvent(new SlotSelectedEvent(SelectedSlot.slot));

            OnSlotSelect.Invoke(SelectedSlot.slot);
        } else {
            //TODO: Inventory slot check for same item

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



public enum UIState
{
    Nothing,
    Dragging,
    Holding,
    Selecting
}


public static class MouseData
{
    public static UserInterface interfaceMouseIsOver;
    public static GameObject tempItemBeingDragged;
    public static GameObject tempItemCanvasObject;
    public static GameObject slotHoveredOver;

    public static void SetupCanvas()
    {
        if (tempItemCanvasObject == null) {
            tempItemCanvasObject = new GameObject();
            var ttcanvas = tempItemCanvasObject.AddComponent<Canvas>();
            ttcanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            ttcanvas.sortingOrder = 100;
        }
    }

    // public static void CreateTempItem(Sprite uiDisplay)
    public static void CreateTempItem(InventorySlot slot)
    {
        if (slot.IsEmpty) tempItemBeingDragged = null;

        tempItemBeingDragged = new("drag item");
        var rt = tempItemBeingDragged.AddComponent<RectTransform>();

        rt.sizeDelta = new Vector2(50, 50);
        rt.localScale = new Vector3(1, 1, 1);

        var img = tempItemBeingDragged.AddComponent<Image>();
        img.sprite = slot.ItemObject.uiDisplay;
        img.raycastTarget = false;

        if (tempItemCanvasObject == null)
            SetupCanvas();

        tempItemBeingDragged.transform.SetParent(tempItemCanvasObject.transform);
    }

    public static void DestroyTempItem()
    {
        if (tempItemBeingDragged == null) return;
        UnityEngine.Object.Destroy(tempItemBeingDragged);
    }

    internal static void SetTempItemPosition(Vector2 pos)
    {
        if (tempItemBeingDragged == null) return;
        tempItemBeingDragged.GetComponent<RectTransform>().position = pos;
    }
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


// // https://discussions.unity.com/t/how-would-i-detect-a-right-click-with-an-event-trigger/128238
// public class MyRightClickClass : MonoBehaviour, IPointerClickHandler
// {

//     public void OnPointerClick(PointerEventData eventData)
//     {
//         if (eventData.button == PointerEventData.InputButton.Right) {
//             Debug.Log("Right Mouse Button Clicked on: " + name);
//         }
//     }

// }