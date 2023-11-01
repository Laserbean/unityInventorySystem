using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using unityInventorySystem.Attribute;

namespace unityInventorySystem.Items.Component
{
    public class BuffItemComponent 
    {

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

