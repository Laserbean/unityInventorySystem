using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using unityInventorySystem; 
using unityInventorySystem.Attribute;


using Laserbean.SpecialData;

public class StatusEffectTController : MonoBehaviour
{

    public CustomDictionary<string, StatusEffectT> statusEffectTDict = new(); 

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
        statusEffectTDict.Add(statusEffectTObject.Name, statuseffect); 

        // AddStatusEffect(statuseffect);
        if(statusEffectTObject.attributeType != AttributeType.Nothing)
            attributeController?.AddAttributeModifier(statusEffectTObject.attributeType, statuseffect);
    }

    [EasyButtons.Button]
    public void RemoveStatusEffect(StatusEffectTObject statusEffectTObject) {
        var statuseffect = statusEffectTObject.GetStatusEffect(0); 

        statuseffect.OnRemove(gameObject); 
        statusEffectTDict.Remove(statusEffectTObject.Name);

        if(statusEffectTObject.attributeType != AttributeType.Nothing)
            attributeController?.RemoveAttributeModifier(statusEffectTObject.attributeType, statuseffect);
    }


    public void AddStatusEffect(StatusEffectT statuseffect) {

    }

    [EasyButtons.Button]

    public void DoTickTurnThing() {
        List<string> keystoremove = new(); 
        foreach(var kvp in statusEffectTDict) {
            kvp.Value.OnTurn(gameObject); 
            if (!kvp.Value.IsActive)
                keystoremove.Add(kvp.Key);
        }
        foreach(var key in keystoremove) {
            statusEffectTDict[key].OnRemove(gameObject); 
            statusEffectTDict.Remove(key); 
        }
    }


}
