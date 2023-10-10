using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using unityInventorySystem;

[CreateAssetMenu(fileName = "StatusEffectValue", menuName = "Status Effects T/Value", order = 3)]
public class ValueStatusEffect : StatusEffectTObject
{
    
    public override StatusEffectT GetStatusEffect(int duration)
    {
        StatusEffectValue statusfx = new(statuseffect, Name, duration);
        return statusfx;
    }

}

public class StatusEffectValue : StatusEffectT
{
    public StatusEffectValue(string nname, int duration) : base(nname, duration)
    {
    }

    public StatusEffectValue(StatusEffectT statusfx, string nname, int duration) : base(statusfx, nname, duration)
    {
    }

    public override void Stack(StatusEffectT statuseffect)
    {
        Value += statuseffect.Value; 
    }

    public override void OnValueMin(GameObject gameObject){
        if (Value == 0) {
            turns_remaining = 0; 
        }
    }
}