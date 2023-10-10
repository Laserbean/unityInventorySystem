using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using unityInventorySystem; 
using unityInventorySystem.Attribute;


using Laserbean.General.GlobalTicks;

using Laserbean.SpecialData;

public class StatusEffectTController : MonoBehaviour, IStatusAffect
{

    public List<StatusEffectT> current_status_effects = new(); 

    IAttributeController attributeController; 

    private void Awake() {
        attributeController = this.GetComponent<IAttributeController>(); 

        TimeTickSystem.OnTick += delegate (object sender, TimeTickSystem.OnTickEventArgs e) {
            DoTickTurnThing();
        };
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
            AddStatusEffectStackable(statuseffect); 
        } else {
            AddStatusEffectNotStackable(statuseffect); 
        }
    }

    [EasyButtons.Button]
    public void AddStatusEffectWithValue(StatusEffectTObject statusEffectTObject, int duration, int value) { 
        var statuseffect = statusEffectTObject.GetStatusEffect(duration); 

        statuseffect.SetValue(value); 

        statuseffect.OnApply(gameObject);
        if (statusEffectTObject.IsStackable) {
            AddStatusEffectStackable(statuseffect); 
        } else {
            AddStatusEffectNotStackable(statuseffect); 
        }
    }

    void AddStatusEffectStackable(StatusEffectT statuseffect) {
        current_status_effects.Add(statuseffect); 
        if(statuseffect.attributeType != AttributeType.Nothing)
            attributeController?.AddAttributeModifier(statuseffect.attributeType, statuseffect);
    }

    void AddStatusEffectNotStackable(StatusEffectT statuseffect) {
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


    public void RemoveStatusEffect(StatusEffectT statusEffectt)
    {
        for (int i = current_status_effects.Count -1 ; i >= 0; i--) {
            if (current_status_effects[i] == statusEffectt) {
                current_status_effects[i].OnRemove(gameObject);
                if(current_status_effects[i].attributeType != AttributeType.Nothing)
                    attributeController?.RemoveAttributeModifier(current_status_effects[i].attributeType, current_status_effects[i]);

                current_status_effects.RemoveAt(i); 
                return; 
            }
        }
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

    void IStatusAffect.AddStatusEffect(StatusEffectTObject statusEffectTObject, float duration)
    {
        var tick_time = TimeTickSystem.TICK_TIME; 
        
        AddStatusEffect(statusEffectTObject, Mathf.RoundToInt(duration / tick_time));
    }


}
