using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using Laserbean.SpecialData;


using unityInventorySystem.Items.Component; 

namespace unityInventorySystem
{
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

        public Item(Item item)
        {
            Name = item.Name;
            Id = item.Id;
            buffs = new ItemBuff[item.buffs.Length];

            for (int i = 0; i < buffs.Length; i++) {
                buffs[i] = new ItemBuff(item.buffs[i].min, item.buffs[i].max) {
                    attribute = item.buffs[i].attribute
                };
            }
            foreach (var kvp in item.specialDict) {
                specialDict.Add(kvp.Key, kvp.Value);
            }
        }

        // public string MetaData = ""; 
        public SpecialDict specialDict = new SpecialDict();
    }



}

