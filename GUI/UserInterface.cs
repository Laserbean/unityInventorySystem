using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;

using UnityEngine.InputSystem; 


using unityInventorySystem;
using Laserbean.General;
using UnityEditor.PackageManager;

public abstract class UserInterface : MonoBehaviour
{

    public InventoryObject inventoryObject;
    public Dictionary<GameObject, InventorySlot> slotsOnInterface = new Dictionary<GameObject, InventorySlot>();
    void Start()
    {
        inventoryObject.inventory.SetSlotsAfterUpdate(OnSlotUpdate);
        inventoryObject.inventory.SetUserinterface(this);

        CreateSlots();
        AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInterface(gameObject); });
        AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnExitInterface(gameObject); });
    }

    private void OnSlotUpdate(InventorySlot _slot)
    {
        EventManager.TriggerEvent(new SlotUpdatedEvent(_slot));
        if (_slot.item.Id >= 0)
        {
            _slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().sprite = _slot.ItemObject.uiDisplay;
            _slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
            _slot.slotDisplay.GetComponentInChildren<TextMeshProUGUI>().text = _slot.amount == 1 ? "" : _slot.amount.ToString("n0");
        }
        else
        {
            _slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
            _slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
            _slot.slotDisplay.GetComponentInChildren<TextMeshProUGUI>().text = "";
        }
    }


    protected void SetEventTriggers(GameObject obj) {
        AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
        AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
        AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
        AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });

        // AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });
        AddEvent(obj, EventTriggerType.Drag, delegate (BaseEventData eventData){ OnDrag(obj, eventData); });

        AddEvent(obj, EventTriggerType.PointerClick, delegate { OnCLickedSlot(obj); });
        AddEvent(obj, EventTriggerType.Select, delegate { OnSelect(obj); });
        AddEvent(obj, EventTriggerType.Submit, delegate { OnSubmit(obj); });
    }

    //// Update is called once per frame
    //void Update()
    //{
    //    slotsOnInterface.UpdateSlotDisplay();
    //}
    public abstract void CreateSlots();

    protected void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public void OnEnter(GameObject obj) {
        MouseData.slotHoveredOver = obj;
    }

    public void OnExit(GameObject obj) {
        MouseData.slotHoveredOver = null;
    }

    public void OnEnterInterface(GameObject obj) {
        MouseData.interfaceMouseIsOver = obj.GetComponent<UserInterface>();        
    }

    public void OnExitInterface(GameObject obj) {
        MouseData.interfaceMouseIsOver = null;
    }

    public void OnDragStart(GameObject obj) {
        MouseData.tempItemBeingDragged = CreateTempItem(obj);
    }

        public GameObject CreateTempItem(GameObject obj)
        {
            GameObject tempItem = null;
            if(slotsOnInterface[obj].item.Id >= 0)
            {
                tempItem = new GameObject("drag item");
                tempItem.transform.SetParent(transform.parent);
                // tempItem.transform.SetParent(null);
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
        Destroy(MouseData.tempItemBeingDragged);
        EndDragOrSecondClick(slotsOnInterface[obj]);
    }

    void EndDragOrSecondClick(InventorySlot islot) {
        if (MouseData.interfaceMouseIsOver == null)
        {
            // islot.RemoveItem();
            Debug.Log("Tried dragging out of inventory".DebugColor(Color.red));
            return;
        }
        if (MouseData.slotHoveredOver)
        {
            InventorySlot mouseHoverSlotData = MouseData.interfaceMouseIsOver.slotsOnInterface[MouseData.slotHoveredOver];
            // InventorySlot mouseHoverSlotData = SelectedSlot.userinterface.slotsOnInterface[MouseData.slotHoveredOver];
            inventoryObject.inventory.SwapItems(islot, mouseHoverSlotData);
        }

        // Debug.Log("slot EDED");


        SlotSelection.Instance.DisenableImage(false); 
        SelectedSlot.obj = null; 
        SlotSelection.Instance.isSelecting = false; 
    }

    public void OnDrag(GameObject obj, BaseEventData eventdata = null)
    {
        Vector2 pos = Vector2.zero;
        if (eventdata != null){
            var pointervent = eventdata as PointerEventData; 
            pos = pointervent.position; 
        } else {
            #if ENABLE_INPUT_SYSTEM
                pos = Mouse.current.position.ReadValue();
            #else
                pos = Input.mousePosition;
            #endif

        }

        if (MouseData.tempItemBeingDragged != null){
            pos = Camera.main.ScreenToWorldPoint(pos); 
            MouseData.tempItemBeingDragged.GetComponent<RectTransform>().position = pos; 
        }
    }

    public void OnPointerClick(GameObject obj) {
        OnCLickedSlot(obj); 
    }

    public void OnCLickedSlot(GameObject obj) {
        // Debug.Log("CLICKED SLOT");

        if (!SlotSelection.Instance.isSelecting) {    
            // SlotSelection.Instance.inventorySlot = slotsOnInterface[obj];
            if (slotsOnInterface[obj].IsEmpty()) return;
            SelectedSlot.obj = obj; 
            SelectedSlot.slot = slotsOnInterface[obj]; 

            SelectSlot(obj); 
            EventManager.TriggerEvent(new SlotSelectedEvent(SelectedSlot.slot));



        } else {
            if (SelectedSlot.obj != null) {
                obj.GetComponent<Button>().OnDeselect(null);
                EndDragOrSecondClick(SelectedSlot.slot);                
            }

            EventManager.TriggerEvent(new SlotSelectedEvent(new InventorySlot()));

        }

    }

    void SelectSlot(GameObject obj) {
            SlotSelection.Instance.DisenableImage(true); 
            SlotSelection.Instance.GetComponent<RectTransform>().position = obj.GetComponent<RectTransform>().position; 
            SlotSelection.Instance.isSelecting = true; 
    }

    public void OnSelect(GameObject obj) {
        ButtonSelectedData.sinterface = obj.GetComponentInParent<UserInterface>(); 
        ButtonSelectedData.slotGO = obj; 
    }

    public void OnSubmit(GameObject obj) {
        if (!SlotSelection.Instance.isSelecting) {

            SelectedSlot.obj = ButtonSelectedData.slotGO; 
            SelectedSlot.slot = ButtonSelectedData.sinterface.slotsOnInterface[ButtonSelectedData.slotGO]; 

            SelectSlot(obj); 
            EventManager.TriggerEvent(new SlotSelectedEvent(SelectedSlot.slot));


        } else {
            InventorySlot curIslot = ButtonSelectedData.sinterface.slotsOnInterface[ButtonSelectedData.slotGO];
            inventoryObject.inventory.SwapItems(curIslot, SelectedSlot.slot);

            SlotSelection.Instance.DisenableImage(false); 
            SelectedSlot.obj = null; 
            SlotSelection.Instance.isSelecting = false; 

            EventManager.TriggerEvent(new SlotSelectedEvent(new InventorySlot()));

        }
    }

    public static GameObject OnCLickedSlotGO; 

    // Vector2 mousepos; 
    // public void OnPoint(InputValue value) {
    //     mousepos = value.Get<Vector2>();
    // }



}
public static class MouseData
{
    public static UserInterface interfaceMouseIsOver;
    public static GameObject tempItemBeingDragged;
    public static GameObject slotHoveredOver;
}

public static class SelectedSlot {
    public static GameObject obj;
    public static int slotnumber;
    public static InventorySlot slot; 
}

public static class ButtonSelectedData {
    public static UserInterface sinterface;
    public static GameObject slotGO;

}

public static class ExtensionMethods
{
    public static void UpdateSlotDisplay(this Dictionary<GameObject, InventorySlot> _slotsOnInterface)
    {
        foreach (KeyValuePair<GameObject, InventorySlot> _slot in _slotsOnInterface)
        {
            if (_slot.Value.item.Id >= 0)
            {
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = _slot.Value.ItemObject.uiDisplay;
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
                _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = _slot.Value.amount == 1 ? "" : _slot.Value.amount.ToString("n0");
            }
            else
            {
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
                _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
        }
    }
}

public class SlotSelectedEvent {
    public InventorySlot slot; 

    public SlotSelectedEvent(InventorySlot _slot) {
        slot = _slot; 
    }
}

public class SlotUpdatedEvent {
    public InventorySlot slot; 

    public SlotUpdatedEvent(InventorySlot _slot) {
        slot = _slot; 
    }
}