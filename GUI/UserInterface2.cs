

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
using Unity.VisualScripting.YamlDotNet.Serialization;


namespace unityInventorySystem.TemporaryBackup
{


    public abstract class UserInterface2 : MonoBehaviour
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


        private void ToggleInventoryHandler(ToggleInventoryEvent _event)
        {
            if (!_event.IsInventory)
                Deselect();
        }

        #endregion


        public void SetupInventory()
        {
            inventoryObject.inventory.SetSlotsAfterUpdate(OnSlotUpdate);
            // inventoryObject.inventory.SetUserinterface(this);

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
            void modifiedAction(BaseEventData arg0)
            {
                action.Invoke(obj, arg0); // Invoke the original action with the additional parameter
            }

            eventTrigger.callback.AddListener(modifiedAction);
            trigger.triggers.Add(eventTrigger);
        }

        #region SlotStuff


        private void OnSlotUpdate(InventorySlot _slot)
        {
            EventManager.TriggerEvent(new SlotUpdatedEvent(_slot));
            bool notEmpty = !_slot.IsEmpty;

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
        #endregion

        protected void SetEventTriggers(GameObject obj)
        {
            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnPointerEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnPointerExit(obj); });

            AddEvent(obj, EventTriggerType.BeginDrag, OnDragStart);
            AddEvent(obj, EventTriggerType.EndDrag, OnDragEnd);

            AddEvent(obj, EventTriggerType.Drag, OnDrag);

            AddEvent(obj, EventTriggerType.PointerClick, OnClickedSlot);
            AddEvent(obj, EventTriggerType.Select, delegate { OnSelect(obj); });
            AddEvent(obj, EventTriggerType.Submit, delegate { OnSubmit(obj); });
        }


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
            if (interfaceState != UIState.Nothing) return;
            interfaceState = UIState.Dragging;

            bool isRight = eventdata != null && (eventdata as PointerEventData).button == PointerEventData.InputButton.Right;
            if (slotsOnInterface[obj] == null || slotsOnInterface[obj].IsEmpty) return;


            if (!isRight) {
                Inventory.SwapItems(HandStackData.Holding, slotsOnInterface[obj]);
            } else {
                Inventory.SplitItems(slotsOnInterface[obj], HandStackData.Holding);
            }

            MouseData.CreateTempItem(HandStackData.Holding);
            OnSlotSelect.Invoke(HandStackData.Holding);

        }


        public void OnDrag(GameObject obj, BaseEventData eventdata = null)
        {
            if (interfaceState != UIState.Dragging) return;

            // Vector2 pos;
            // if (eventdata != null) {
            //     var pointervent = eventdata as PointerEventData;
            //     pos = pointervent.position;
            // } else {
            //     pos = Mouse.current.position.ReadValue();
            // }
            // MouseData.SetTempItemPosition(pos);
        }


        public void OnDragEnd(GameObject obj, BaseEventData eventdata = null)
        {
            if (interfaceState != UIState.Dragging) return;
            interfaceState = UIState.Nothing;

            MouseData.DestroyTempItem();

            OnPutDown(obj);
        }

        void OnPutDown(GameObject obj)
        {
            var curinterface = MouseData.interfaceMouseIsOver;

            if (MouseData.interfaceMouseIsOver == null) {
                DropItem(HandStackData.Release());
                return;
            }
            if (MouseData.slotHoveredOver == null) {
                Inventory.SwapItems(HandStackData.Holding, slotsOnInterface[obj]);
                HandStackData.Release();
                return;
            }

            var curslot = curinterface.slotsOnInterface[MouseData.slotHoveredOver];

            if (curslot.IsEmpty) {
                Inventory.SwapItems(HandStackData.Holding, curslot);
            } else if (HandStackData.Holding.SameItem(curslot)) {
                curslot.AddAmount(HandStackData.Holding.amount);
            } else {
                Inventory.SwapItems(slotsOnInterface[obj], curslot);
                Inventory.SwapItems(HandStackData.Holding, curslot);
            }
            HandStackData.Release();
            MouseData.DestroyTempItem();


        }

        void DropItem(InventorySlot islot)
        {
            EventManager.TriggerEvent(new ItemDroppedEvent(islot.item, islot.amount));
        }

        private void Update()
        {
            if (MouseData.tempItemBeingDragged == null) return;
            Vector2 pos = Mouse.current.position.ReadValue();
            MouseData.SetTempItemPosition(pos);
        }

        public void OnClickedSlot(GameObject obj, BaseEventData eventdata = null)
        {
            bool isRight = eventdata != null && (eventdata as PointerEventData).button == PointerEventData.InputButton.Right;

            switch (interfaceState) {
                case UIState.Nothing:
                    if (slotsOnInterface[obj] == null || slotsOnInterface[obj].IsEmpty) return;


                    if (!isRight) {
                        Inventory.SwapItems(HandStackData.Holding, slotsOnInterface[obj]);
                    } else {
                        Inventory.SplitItems(slotsOnInterface[obj], HandStackData.Holding);
                    }
                    MouseData.CreateTempItem(HandStackData.Holding);

                    interfaceState = UIState.Selecting;
                    break;

                case UIState.Selecting:
                    if (slotsOnInterface[obj] == null) {
                        Deselect();
                        return;
                    }
                    OnPutDown(obj);
                    Deselect();

                    break;
            }


        }

        public void OnSelect(GameObject obj)
        {

        }

        public void OnSubmit(GameObject obj)
        {
            switch (interfaceState) {
                case UIState.Nothing:
                    if (slotsOnInterface[obj] == null) return;

                    Inventory.SwapItems(HandStackData.Holding, slotsOnInterface[obj]);
                    interfaceState = UIState.Selecting;
                    MouseData.CreateTempItem(HandStackData.Holding);

                    break;

                case UIState.Selecting:
                    if (slotsOnInterface[obj] == null) {
                        Deselect();
                    }
                    OnPutDown(obj);
                    break;
            }
        }

        public void Deselect()
        {
            if (interfaceState != UIState.Nothing) {
                EventManager.TriggerEvent(new SlotSelectedEvent(new InventorySlot()));
            }
            interfaceState = UIState.Nothing;

            // if (SelectedSlot.obj == null) return;
            // SelectedSlot.obj = null;
            // SelectedSlot.Deselect();


        }




    }



    public enum UIState
    {
        Nothing,
        Dragging,
        Selecting
    }

    public static class HandStackData
    {
        public static InventorySlot Holding = new();

        public static void Hold(InventorySlot slot)
        {
            Holding = slot;
        }

        public static InventorySlot Release()
        {
            var temp = Holding;
            Holding.RemoveItem();
            return temp;
        }
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





}