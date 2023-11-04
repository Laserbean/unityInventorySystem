using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using unityInventorySystem.Attribute;

using unityInventorySystem.Items; 

namespace unityInventorySystem.Items.Components
{
    [System.Serializable]
    public class BuffItemComponent : ItemComponent
    {
        public List<ItemBuff> buffs = new();

        public BuffItemComponent()
        {
            Name = "BuffItemComponent"; 
        }

        public BuffItemComponent(BuffItemComponent itemcomp)
        {
            // buffs = new ItemBuff[itemcomp.buffs.Count];
            for (int i = 0; i < itemcomp.buffs.Count; i++) {
                var newbuff = new ItemBuff(itemcomp.buffs[i].min, itemcomp.buffs[i].max) {
                    attribute = itemcomp.buffs[i].attribute
                };
                buffs.Add(newbuff);
            }
        }

        public override ItemComponent Copy()
        {
            return new BuffItemComponent(this);
        }

        public override void OnEquip(GameObject character, EquipmentTag tag)
        {
            var attributesController = character.GetComponent<AttributesController>();
            if (attributesController == null) return; 
            for (int i = 0; i < buffs.Count; i++) 
                attributesController.AddAttributeModifier(buffs[i].attribute, buffs[i]);            
        }

        public override void OnUnequip(GameObject character, EquipmentTag tag)
        {
            var attributesController = character.GetComponent<AttributesController>();
            if (attributesController == null) return; 
            for (int i = 0; i < buffs.Count; i++) 
                attributesController.RemoveAttributeModifier(buffs[i].attribute, buffs[i]);          
        }
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

