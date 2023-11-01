﻿
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using Laserbean.SpecialData;
using System;

namespace unityInventorySystem {
public class GroundItem : MonoBehaviour, IGroundItem

{
    public ItemObject itemObject;

    Item this_item; 

    int _amount = 1; 

    int IGroundItem.Amount => _amount;


    public string ItemDatabaseName = InventoryStaticManager.DEF_ITEM_DB_NAME;


    protected void OnEnable()
    {
        if (itemObject != null) {
            gameObject.GetComponent<SpriteRenderer>().sprite = itemObject.uiDisplay; 
            this_item = itemObject.CreateItem(); 
        }
    }

    private void OnValidate() {
        #if UNITY_EDITOR
        OnEnable(); 
        #endif
    }

    public void SetItemObject(ItemObject itemo) {
        gameObject.GetComponent<SpriteRenderer>().sprite = itemo.uiDisplay; 
        itemObject = itemo; 
        this_item = itemo.CreateItem(); 
    }

    Item IGroundItem.GetItem()
    {
        return this_item; 
    }

    void IGroundItem.DestroyItem()
    {
        // Destroy(this.gameObject); 
        GetComponent<IDestroyOrDisable>().DestroyOrDisable(); 
    }

    void IGroundItem.SetItem(Item _item, int amm)
    {
        _amount = amm; 
        this_item = _item; 

        itemObject =InventoryStaticManager.GetDatabase(ItemDatabaseName).GetItemObject(_item.Name); 

        this.gameObject.GetComponent<SpriteRenderer>().sprite = itemObject.characterDisplay2D;        
    }


    }
}