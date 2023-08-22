using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace unityInventorySystem {
public class GroundItem : MonoBehaviour, IGroundItem
{
    public ItemObject itemObject;

    Item item; 
    public int ammount = 1; 



    public string ItemDatabaseName = InventoryStaticManager.DEF_ITEM_DB_NAME; 
    public int amount => ammount;


    protected void OnEnable()
    {
        if (itemObject != null) {
            gameObject.GetComponent<SpriteRenderer>().sprite = itemObject.uiDisplay; 
            item = itemObject.CreateItem(); 
        }
    }

    private void OnValidate() {
        #if UNITY_EDITOR
        OnEnable(); 
        #endif
    }

    public void SetItem(ItemObject itemo) {
        this.gameObject.GetComponent<SpriteRenderer>().sprite = itemo.uiDisplay; 
        itemObject = itemo; 
        item = itemo.CreateItem(); 
    }

    Item IGroundItem.GetItem()
    {
        return item; 
    }

    void IGroundItem.DestroyItem()
    {
        Destroy(this.gameObject); 
    }

    void IGroundItem.SetItem(Item _item, int amm)
    {
        ammount = amm; 
        item = _item; 

        itemObject =InventoryStaticManager.GetDatabase(ItemDatabaseName).GetItemObject(_item.Name); 

        this.gameObject.GetComponent<SpriteRenderer>().sprite = itemObject.characterDisplay2D;        
    }



}
}