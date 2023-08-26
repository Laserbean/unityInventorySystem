using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using unityInventorySystem; 

using unityInventorySystem.Attribute; 


[CreateAssetMenu(fileName = "IncreasingSE", menuName = "Status Effects/Increasing", order = 3)]
public class IncreasingStatusEffectObject : StatusEffectObject
{
    [SerializeField] int   _damage; 
    [SerializeField] int   _change; 
    [SerializeField] float _rate; 

    [SerializeField] ChangeMode damageType; 

    public int Damage {
        get{return _damage;}
    }

    public int Change {
        get{return _change;}
    }

    public float Rate {
        get{return _rate;}
    }

    public override bool DoUpdate(StatusEffectD statuseffect, float dtime)
    {
        statuseffect.t_last_call += dtime; 
        if (statuseffect.t_last_call > statuseffect.rate) {
            statuseffect.t_last_call = 0; 
            
            int vllue = 1;
            if (damageType == ChangeMode.Constant) {
                statuseffect.value += _change; 
            } else if (damageType == ChangeMode.Scaled) {
                vllue = Mathf.RoundToInt(((float) _change)/10f * statuseffect.elapsed_time);  
            }


            return true; 
        }
        return false; 
    }


    enum ChangeMode {
        Constant, 
        Scaled
    }

}


