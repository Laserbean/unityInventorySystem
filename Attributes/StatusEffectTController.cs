using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using unityInventorySystem; 
using unityInventorySystem.Attribute;


using Laserbean.SpecialData;

public class StatusEffectTController : MonoBehaviour
{

    public List<StatusEffectT> current_status_effects = new(); 

    IAttributeController attributeController; 

    private void Awake() {
        attributeController = this.GetComponent<IAttributeController>(); 
    }

    #if UNITY_EDITOR
    private void OnValidate() {
        attributeController ??= this.GetComponent<IAttributeController>(); 
    }
    #endif
    
    [EasyButtons.Button]
    public void AddStatusEffect(StatusEffectTObject statusEffectTObject, int duration) { 
        var statuseffect = statusEffectTObject.GetStatusEffect(duration); 

        statuseffect.OnApply(gameObject);


        if (statusEffectTObject.IsStackable) {
            current_status_effects.Add(statuseffect); 
            if(statuseffect.attributeType != AttributeType.Nothing)
                attributeController?.AddAttributeModifier(statuseffect.attributeType, statuseffect);
        } else {
            bool found = false; 
            foreach(var statusfx in current_status_effects) {
                if (statusfx.Name == statuseffect.Name) {
                    statusfx.Stack(statuseffect);
                    found = true; 
                    break; 
                }
            }
            if (!found) {
                if(statuseffect.attributeType != AttributeType.Nothing)
                    attributeController?.AddAttributeModifier(statuseffect.attributeType, statuseffect);
                current_status_effects.Add(statuseffect); 
            }
        }




        // AddStatusEffect(statuseffect);

    }

    [EasyButtons.Button]
    public void RemoveStatusEffect(StatusEffectTObject statusEffectTObject) {
        bool isStackable = statusEffectTObject.IsStackable; 
        
       for (int i = current_status_effects.Count -1 ; i >= 0; i--) {
            if (current_status_effects[i].Name == statusEffectTObject.Name) {
                current_status_effects[i].OnRemove(gameObject);
                if(current_status_effects[i].attributeType != AttributeType.Nothing)
                    attributeController?.RemoveAttributeModifier(current_status_effects[i].attributeType, current_status_effects[i]);

                current_status_effects.RemoveAt(i); 
            }
        }

        // var curstatuseffect = current_status_effects[statusEffectTObject.Name];
        // curstatuseffect.OnRemove(gameObject); 
        // current_status_effects.Remove(statusEffectTObject.Name);

        // if(statusEffectTObject.attributeType != AttributeType.Nothing)
        //     attributeController?.RemoveAttributeModifier(statusEffectTObject.attributeType, curstatuseffect);
    }


    public void AddStatusEffect(StatusEffectT statuseffect) {

    }

    [EasyButtons.Button]

    public void DoTickTurnThing() {
        List<StatusEffectT> statusfxtoremove = new(); 
        foreach(var curstfx in current_status_effects) {
            curstfx.OnTurn(gameObject); 
            if (!curstfx.IsActive)
                statusfxtoremove.Add(curstfx); 
        }
        foreach(var thing in statusfxtoremove) {
            thing.OnRemove(gameObject); 
            attributeController?.RemoveAttributeModifier(thing.attributeType, thing);
            current_status_effects.Remove(thing); 
        }
    }


}
