using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace unityInventorySystem.Items.Components
{

    [System.Serializable]
    public abstract class ItemComponent
    {
        protected string Name; 
        
        public abstract ItemComponent Copy(); 
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
    }


}

