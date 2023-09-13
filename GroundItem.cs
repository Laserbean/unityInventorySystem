// #define ENTITYDATA

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using Laserbean.SpecialData; 
namespace unityInventorySystem {
public class GroundItem : MonoBehaviour, IGroundItem

#if ENTITYDATA
    , IEntityData
#endif

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
        Destroy(this.gameObject); 
    }

    void IGroundItem.SetItem(Item _item, int amm)
    {
        _amount = amm; 
        this_item = _item; 

        itemObject =InventoryStaticManager.GetDatabase(ItemDatabaseName).GetItemObject(_item.Name); 

        this.gameObject.GetComponent<SpriteRenderer>().sprite = itemObject.characterDisplay2D;        
    }

#if ENTITYDATA

    public void SetEntityData(EntityData entityData)
    {
        
        SpecialData spdata = entityData.specialDict[item_key]; 
        this_item.Name = spdata.String;
        this_item.Id = spdata.Int;

        SpecialData spdata2 = entityData.specialDict[amount_key]; 

        _amount = spdata2.Int; 
    }

    public EntityData GetEntityData()
    {
        EntityData entityData = new (gameObject);

        entityData.specialDict.Add(item_key, new SpecialData{String = this_item.Name, Int = this_item.Id});
        entityData.specialDict.Add(amount_key, new SpecialData{Int = _amount});

        return entityData; 
    }

    const string item_key = "item_key";
    const string amount_key = "amount_key";

#endif
    }
}