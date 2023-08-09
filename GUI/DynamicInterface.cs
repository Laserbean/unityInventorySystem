using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


using UnityEngine.UI;

using unityInventorySystem; 

public class DynamicInterface : UserInterface
{
    public GameObject inventoryPrefab;
    public int X_START;
    public int Y_START;
    public int X_SPACE_BETWEEN_ITEM;
    public int NUMBER_OF_COLUMN;
    public int Y_SPACE_BETWEEN_ITEMS;

    public override void CreateSlots()
    {
        slotsOnInterface = new Dictionary<GameObject, InventorySlot>();
        for (int i = 0; i < inventoryObject.GetSlots.Length; i++)
        {
            var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);

            SetEventTriggers(obj); 

            inventoryObject.GetSlots[i].slotDisplay = obj;

            slotsOnInterface.Add(obj, inventoryObject.GetSlots[i]);
        }


    }
    private Vector3 GetPosition(int i)
    {
        return new Vector3(X_START + (X_SPACE_BETWEEN_ITEM * (i % NUMBER_OF_COLUMN)), Y_START + (-Y_SPACE_BETWEEN_ITEMS * (i / NUMBER_OF_COLUMN)), 0f);
    }

    // public override void SelectSlot(GameObject obj)
    // {
  

    //     // GameObject tempItem = null;
    //     // if(slotsOnInterface[obj].item.Id >= 0)
    //     // {
    //     //     tempItem = new GameObject();
    //     //     tempItem.transform.position = Vector3.zero; 
    //     //     var rt = tempItem.AddComponent<RectTransform>();
    //     //     rt.position = obj.GetComponent<RectTransform>().position; 
    //     //     rt.sizeDelta = new Vector2(50, 50);


    //     //     tempItem.transform.SetParent(transform);

    //     //     var img = tempItem.AddComponent<Image>();
    //     //     // img.sprite = slotsOnInterface[obj].ItemObject.uiDisplay;
    //     //     img.sprite = selectedSlot; 


    //     //     img.raycastTarget = false;
    //     // }
    //     // // throw new System.NotImplementedException();
    // }

    public Sprite selectedSlot;
}
