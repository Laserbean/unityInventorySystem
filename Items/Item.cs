using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using Laserbean.SpecialData;


using unityInventorySystem.Items.Components;
using System;

namespace unityInventorySystem
{
    [System.Serializable]
    public class Item
    {
        public string Name;
        public int Id = -1;

        [field: SerializeReference] public List<ItemComponent> Components = new();

        public Item()
        {
            Name = "";
            Id = -1;
        }

        public Item(Item item)
        {
            Name = item.Name;
            Id = item.Id;

            foreach (var comp in item.Components) {
                Components.Add(comp.Copy());
                comp.SetParentItem(this); 
            }

        }

        public void UseComponents(GameObject character)
        {
            foreach (var comp in Components) {
                comp.OnUse(character);
            }

        }

        [System.NonSerialized] Dictionary<Type, int> comp_index_dict = new();
        public ItemComponent GetItemComponent<TIComp>() where TIComp : ItemComponent
        {
            if (comp_index_dict.Keys.Count != Components.Count) UpdateDict();
            if (comp_index_dict.ContainsKey(typeof(TIComp))) {
                return Components[comp_index_dict[typeof(TIComp)]];
            }
            return null;
        }

        void UpdateDict()
        {
            comp_index_dict.Clear();


            for (int i = 0; i < Components.Count; i++) {
                comp_index_dict.Add(Components[i].GetType(), i);
            }

        }
    }



}

