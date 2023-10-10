using System.Collections;
using System.Collections.Generic;
using Laserbean.General.GlobalTicks;
using Laserbean.SpecialData;
using UnityEngine;
using UnityEngine.Search;
using unityInventorySystem;
using unityInventorySystem.Attribute;

public class StainController : MonoBehaviour, IStainable
{
    
    // [SearchContext("p: Stain")]
    public List<StatusEffectTObject> statusEffectObjects = new(); 

    
    public CustomDictionary<StainType, StatusEffectT> all_stains = new(); 


    private void Start() {
        UpdateDict();
        TimeTickSystem.OnTick += delegate (object sender, TimeTickSystem.OnTickEventArgs e) {
            TickTurn();
        };
    }

    IAttributeController attributeController; 

    private void Awake() {
        attributeController = this.GetComponent<IAttributeController>(); 
    }



    [EasyButtons.Button]
    void UpdateDict() {
        all_stains.Clear(); 
        foreach(var statusObject in statusEffectObjects) {
            var curstatusfx = statusObject.GetStatusEffect(0); 
            all_stains.Add(curstatusfx.stainType, curstatusfx);
        }
    }

    public int GetStainValue(StainType stainType) {
        if (all_stains.ContainsKey(stainType)) {
            return all_stains[stainType].Value;
        }
        return -1; 
    }


    public void SetStain(StainType stainType, int value) {
        if (all_stains.ContainsKey(stainType)) {
            int val_start = all_stains[stainType].Value;

            if (val_start == 0 && value != 0) 
                all_stains[stainType].OnApply(gameObject); 
            
            all_stains[stainType].SetValue(value);

            if (val_start != 0 && value == 0) 
                all_stains[stainType].OnRemove(gameObject); 
        }        
    }


    public void ModifyStain(StainType stainType, int value) {
        if (value == 0) return;
        if (all_stains.ContainsKey(stainType)) {
            if (all_stains[stainType].Value == 0) 
                all_stains[stainType].OnApply(gameObject); 
            
            all_stains[stainType].ModifyValue(value);

            if (all_stains[stainType].Value == 0) 
                all_stains[stainType].OnRemove(gameObject); 
        }        
    }

    public void RemoveStain(StainType stainType) {
        if (all_stains.ContainsKey(stainType)){
            all_stains[stainType].OnRemove(gameObject); 
            all_stains[stainType].SetValue(0); 
        }
    }


    [EasyButtons.Button]
    public void TickTurn() {
        foreach(StatusEffectT statusEffectT in all_stains.Values) {
            statusEffectT.OnTurn(gameObject); 
        }
    }




    // public void RemoveStatusEffect(StatusEffectT statusEffectt)
    // {
    //     for (int i = current_status_effects.Count -1 ; i >= 0; i--) {
    //         if (current_status_effects[i] == statusEffectt) {
    //             current_status_effects[i].OnRemove(gameObject);
    //             if(current_status_effects[i].attributeType != AttributeType.Nothing)
    //                 attributeController?.RemoveAttributeModifier(current_status_effects[i].attributeType, current_status_effects[i]);

    //             current_status_effects.RemoveAt(i); 
    //             return; 
    //         }
    //     }
    // }

}


// [System.Serializable]
// public class Stain 
// {
//     public int value = 0; 
//     public Vector2Int min_max_values = Vector2Int.zero; 

//     public void OnTick() {

//     }

// }


public interface IStainable {
    public int GetStainValue(StainType stainType);
    public void SetStain(StainType stainType, int value); 
    public void ModifyStain(StainType staintype, int value);
    public void RemoveStain(StainType staintype);
}