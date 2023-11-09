using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace unityInventorySystem.Items.Components
{

    [System.Serializable]
    public abstract class ItemComponent
    {
        [HideInInspector]
        public string Name; 

        protected Item ParentItem; 

        public ItemComponent() {
        }

        public ItemComponent(ItemComponent itemComponent) {
            ParentItem = itemComponent.ParentItem; 
            Name = itemComponent.Name; 
        }

        public abstract ItemComponent Copy(); 

        public abstract void OnEquip(GameObject character, EquipmentTag tag);
        public abstract void OnUnequip(GameObject character, EquipmentTag tag);

        public virtual void OnUse(GameObject character) {

        }

        public override string ToString()
        {
            return base.ToString();
        }

        public void SetParentItem(Item _item) {
            ParentItem = _item; 
        }

    }


    [System.Serializable]
    public class TestItemComponent : ItemComponent
    {
        public int Fish; 

        public TestItemComponent() {
            Fish = -1;
        }


        public TestItemComponent(int f) {
            Fish = f; 
        }

        public override ItemComponent Copy()
        {
            return new TestItemComponent(Fish);
        }

        public override void OnEquip(GameObject character, EquipmentTag tag)
        {
            // Do nothing
        }

        public override void OnUnequip(GameObject character, EquipmentTag tag)
        {
            // 
            // Do nothing.... but more
            // 
        }
    }


}

