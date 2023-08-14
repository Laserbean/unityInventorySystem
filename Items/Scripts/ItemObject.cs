using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR 
using UnityEditor; 
#endif

namespace unityInventorySystem {

[CreateAssetMenu(fileName = "New Item", menuName = "unity Inventory System/Items/item")]
public class ItemObject : ScriptableObject
{

    public Sprite uiDisplay;
    public GameObject characterDisplay;
    public Sprite characterDisplay2D; 

    //TODO add gameobject for it to be placed on the ground


    public bool stackable;
    public int stackSize; //idk how to use this now
    
    public ItemType type;
    [TextArea(15, 20)]
    public string description;
    public Item item = new Item();
    
    // public List<string> boneNames = new List<string>();

    public Item CreateItem()
    {
        Item newItem = new Item(this);
        return newItem;
    }

    void OnValidate() {
        OnValidatee(); 
    }

    protected virtual void OnValidatee(){
        item.Name = this.name; 
    }


}

[System.Serializable]
public class Item
{
    public string Name;
    public int Id = -1;
    public ItemBuff[] buffs;
    
    public Item()
    {
        Name = "";
        Id = -1;
    }

    public Item(ItemObject item)
    {
        Name = item.name;
        Id = item.item.Id;
        buffs = new ItemBuff[item.item.buffs.Length];
        for (int i = 0; i < buffs.Length; i++)
        {
            buffs[i] = new ItemBuff(item.item.buffs[i].min, item.item.buffs[i].max)
            {
                attribute = item.item.buffs[i].attribute
            };
        }
    }

    // public string MetaData = ""; 
    public SpecialDict specialDict = new SpecialDict(); 
}

[System.Serializable]
public class ItemBuff : IModifier
{
    public AttributeType attribute;
    public int value;
    public int min;
    public int max;
    public ItemBuff(int _min, int _max)
    {
        min = _min;
        max = _max;
        GenerateValue();
    }

    public void AddValue(ref int baseValue)
    {
        baseValue += value;
    }

    public void GenerateValue()
    {
        value = UnityEngine.Random.Range(min, max);
    }
}
}