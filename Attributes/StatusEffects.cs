
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using unityInventorySystem.Attribute; 


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace unityInventorySystem {


public interface IStatusDoable {
    public bool DoUpdate(StatusEffectD statusEffectD, float dtime);
    public void DoIfUpdated(int value);
}

public delegate bool StatusEffectDoUpdate(StatusEffectD statusEffectD, float dtime); 
public delegate void StatusEffectDoIfUpdated(int value); 

public delegate void StatusEffectDoDamage(int damage); 

[System.Serializable]
public class StatusEffectD : IModifier
{
    public int value; //damage or attribut amount

    public string Name = "";

    public StatusEffectType type; 
    public AttributeType attributeType; 
    public DebuffType debuffType; 

    public float duration = 0f; 
    public float elapsed_time = 0f; 

    public float rate = 1f; 
    public float t_last_call = 0f; 


    public StatusEffectDoUpdate doUpdate; 
    public StatusEffectDoIfUpdated alsoDo; 
    public StatusEffectDoDamage doDamage; 

    public bool DoUpdate(float dtime) {

        elapsed_time += dtime; 
        duration -= dtime; 
        
        if (doUpdate != null) {
            return doUpdate.Invoke(this, dtime); 
        }

        return false; 
    }
    
    public void AlsoDo() {
        alsoDo?.Invoke(value);  
    }


    public bool IsActive() {
        return (duration > 0f); 
    }

    public StatusEffectD(StatusEffectDoDamage _doDamage, StatusEffectType _type) {
        if (_doDamage != null) {
            doDamage = _doDamage; 
        }

        type = _type; 
    }

    public void SetDoAlso(StatusEffectDoIfUpdated _alsoDO) {
        if (_alsoDO != null) {
            alsoDo = _alsoDO; 
        }

    }

    public void SetDoUpdate(StatusEffectDoUpdate _doUpdate) {
        if (_doUpdate != null) {
            doUpdate = _doUpdate; 
        }
    }


    void IModifier.AddValue(ref int baseValue)
    {
        baseValue += value; 
    }
}


// [System.Serializable]
// public class StatusEffect {
//     public string Name = ""; 
//     public float duration; 

//     public StatusEffectDoUpdate DoUpdate; 


//     public void SetUpdateMethod(StatusEffectDoUpdate doSomething) {       
//         if (doSomething != null) {
//             DoUpdate = doSomething; 
//         }
//     }


//     public StatusEffect(float dur) {
//         duration = dur;
//     }

//     public StatusEffect(StatusEffect ste) {
//         duration = ste.duration; 
//     }

//     public StatusEffect(StatusEffectObject obj, float _duration) {

//         Name = obj.Name; 
//         duration = _duration;
//     }
// }


// [System.Serializable]
// public class DamageOverTimeStatusEffect : StatusEffect{
//     DebuffType type; 
//     public int damage; 
//     public int reduction; 
//     public float rate; 
//     public float t_last_call = 0f; 

//     public bool isPercentage = false; 

//     public DamageOverTimeStatusEffect(DebuffType _type, float dur, int _damage, int _reduction, float _rate, bool _ispercentage = false): base(dur) {
//         type = _type; 
//         damage = _damage;
//         rate = _rate; 
//         reduction = _reduction; 

//         isPercentage = _ispercentage; 
//     }

//     public DamageOverTimeStatusEffect(DamageOverTimeStatusEffect dotse):base(dotse.duration) {
//         type = dotse.type; 
//         damage = dotse.damage; 
//         reduction = dotse.reduction; 
//         rate = dotse.rate; 
//         t_last_call = dotse.t_last_call; 
//         isPercentage = dotse.isPercentage; 
//     }

//     public DamageOverTimeStatusEffect(DamageOverTimeStatusEffectObject obj, float _duration) : base(obj, _duration){
//         damage = obj.damage;
//         reduction = obj.reduction; 
//         rate = obj.rate; 
//     }

//     public void ReduceDamage() {
//         if (isPercentage) {
//             damage = Mathf.RoundToInt(((float)damage * reduction)/100f); 
//         } else {
//             damage -= reduction; 
//         }
//     }

//     public bool UpdateTimeAndCheckDamage(float deltaTime) {
//         t_last_call += deltaTime; 
//         if (t_last_call > rate) {
//             t_last_call = 0; 
//             return true; 
//         }
//         return false; 
//     }

    
// }


// [System.Serializable]
// public class AttributeStatusEffect : StatusEffect, IModifier {
//     public AttributeType type; 
//     public int value; 

    
//     public AttributeStatusEffect(AttributeType _type, float dur, int val): base(dur) {
//         value = val; 
//         type = _type; 
//     }

//     public AttributeStatusEffect(AttributeStatusEffect ase) : base(ase.duration) {
//         type = ase.type;
//         value = ase.value; 
//     }

//     public AttributeStatusEffect(AttributeStatusEffectObject obj, float _duration) : base(obj, _duration){
//         type = obj.type;
//         value = obj.value; 
//     }

//     public void AddValue(ref int baseValue)
//     {
//         baseValue += value;
//     }


// }

}


[System.Serializable]
public enum DebuffType {
    None, 
    Poison, 
    Fire, 
    Blindness
}


public interface IStatusAffect {
    public void AddStatusEffect(StatusEffectObject statusEffect, float duration);
}