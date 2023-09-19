using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using unityInventorySystem; 
using unityInventorySystem.Attribute; 

[CreateAssetMenu(fileName = "DamageOverTimeSE", menuName = "Status Effects/DamageOverTime", order = 1)]
public class DamageOverTimeStatusEffectObject : StatusEffectObject
{
    [SerializeField] int   _damage; 
    [SerializeField] int   _reduction; 
    [SerializeField] float _rate; 

    [SerializeField] DamageOverTimeType damageType; 

    public int damage {
        get{return _damage;}
    }

    public int reduction {
        get{return _reduction;}
    }

    public float rate {
        get{return _rate;}
    }

    public override bool DoUpdate(StatusEffectD statuseffect, float dtime)
    {
        statuseffect.t_last_call += dtime; 
        if (statuseffect.t_last_call > statuseffect.rate) {
            statuseffect.t_last_call = 0; 
            
            int curdamage = 1;
            if (damageType == DamageOverTimeType.Constant) {
                curdamage = statuseffect.value; 
            } else if (damageType == DamageOverTimeType.Scaled) {
                curdamage = Mathf.RoundToInt(((float) statuseffect.value)/10f * statuseffect.duration + 1f);  
            }

            statuseffect.doDamage.Invoke(curdamage); 
            return true; 
        }
        return false; 
    }


    enum DamageOverTimeType {
        Constant, 
        Scaled
    }

}


