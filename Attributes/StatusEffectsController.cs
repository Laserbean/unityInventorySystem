using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using unityInventorySystem;
using unityInventorySystem.Attribute; 

using CodeMonkey.Utils; 

public class StatusEffectsController : MonoBehaviour, IStatusAffect
{

    public Dictionary<string, StatusEffectD> statusEffectsDictionary = new Dictionary<string, StatusEffectD>();

    [SerializeField]
    List<StatusEffectD> statusEffectsList = new(); 

    public void AddStatusEffect(StatusEffectObject statusEffectObj, float duration) {

        AddWorldSprite(statusEffectObj.Icon); 
        if (HasStatusEffect(statusEffectObj.Name)) {
            statusEffectsDictionary[statusEffectObj.Name].duration += duration;
            return; 
        }

        StatusEffectD statusEffect = new(dDamage, StatusEffectType.Default)
        {
            duration = duration,
            Name = statusEffectObj.Name
        };

        statusEffect.SetDoUpdate(statusEffectObj.DoUpdate); 
        statusEffect.SetDoAlso(statusEffectObj.DoIfUpdated); 


        if (statusEffectObj is AttributeStatusEffectObject obj) {
            statusEffect.type = StatusEffectType.Attribute;
            statusEffect.value = obj.value; 

            this.GetComponent<AttributesController>().AddAttributeModifier(obj.type, statusEffect); 
        }

        if (statusEffectObj is DamageOverTimeStatusEffectObject obj2) {
            statusEffect.type = StatusEffectType.DamageOverTime;
            // statusEffect.debuffType = obj2.

            statusEffect.value = obj2.damage; 
            statusEffect.rate = obj2.rate;
        }

        statusEffectsDictionary.Add(statusEffect.Name, statusEffect); 

        statusEffectsList.Add(statusEffect);
    }


    public void RemoveStatusEffect(StatusEffectObject statusEffectObj) {

        if (statusEffectsDictionary.ContainsKey(statusEffectObj.Name)) {
            if (statusEffectsDictionary[statusEffectObj.Name].type == StatusEffectType.Attribute) {
                this.GetComponent<AttributesController>().
                    RemoveAttributeModifier(statusEffectsDictionary[statusEffectObj.Name].attributeType, statusEffectsDictionary[statusEffectObj.Name]); 
            }
            statusEffectsList.Remove(statusEffectsDictionary[statusEffectObj.Name]);
            statusEffectsDictionary.Remove(statusEffectObj.Name); 

            return; 
        }

    }

    public void RemoveStatusEffect(StatusEffectD statusEffect) {
        if (statusEffect.type == StatusEffectType.Attribute) {
            this.GetComponent<AttributesController>().RemoveAttributeModifier(statusEffect.attributeType, statusEffect); 
        }

        if (statusEffectsDictionary.ContainsKey(statusEffect.Name)) {
            statusEffectsList.Remove(statusEffectsDictionary[statusEffect.Name]);
            statusEffectsDictionary.Remove(statusEffect.Name); 
        }
    }


    public void ClearStatusEffects() {
        // statusEffectsList.Clear();
    }


    public bool HasStatusEffect(string name) {
        return statusEffectsDictionary.ContainsKey(name); 
    }


    GameObject SpriteObject; 
    void AddWorldSprite(Sprite sprite) {
        SpriteObject = UtilsClass.CreateWorldSprite("statusEffect", sprite, transform.position, transform.localScale, 1, Color.green); 
        SpriteObject.transform.SetParent(this.transform); 
    }


    [SerializeReference] List<StatusEffectD> status_to_remove = new List<StatusEffectD>(); 
    private void Update() {
        status_to_remove.Clear(); 
        List<string> keylist = new (this.statusEffectsDictionary.Keys);
 
        for(int i = 0; i < keylist.Count; i++) {

            bool statusUpdated = statusEffectsDictionary[keylist[i]].DoUpdate(Time.deltaTime);

            if (statusUpdated) {
                statusEffectsDictionary[keylist[i]].AlsoDo(); 
            }


            //     //TODO add animation or something for poison and fire and stff


            if (!statusEffectsDictionary[keylist[i]].IsActive()) {
                status_to_remove.Add(statusEffectsDictionary[keylist[i]]); 
            }
        }        

        foreach(var statusthign in status_to_remove) {
            RemoveStatusEffect(statusthign); 
        }
    }

    void dDamage(int damage) {
        this.GetComponent<IDamageable>()?.Damage(damage); 
    }

    
    //DEBUG


    public StatusEffectObject statusEffectObjectDebug; 

    [EasyButtons.Button]
    public void AddDebugEffect() {
        AddStatusEffect(statusEffectObjectDebug, 5); 
    }   

    [EasyButtons.Button]
    public void RemoveDebugEffect() {
        RemoveStatusEffect(statusEffectObjectDebug); 
    }   


    // [EasyButtons.Button]
    // void Test() {
    //     StatusEffect statusEffect = new StatusEffect(DoSomething, null);
    //     statusEffect.Name = "fish"; 
    //     bool chicken = statusEffect.Update(ref statusEffect, 0f); 

    //     Debug.Log(chicken); 
    //     Debug.Log(statusEffect.Name); 

    // }


    // bool DoSomething(ref StatusEffect thing, float ffff) {
    //     Debug.Log(thing.Name); 
    //     thing.Name = "fdsad"; 

    //     return false; 
    // }


    

}



