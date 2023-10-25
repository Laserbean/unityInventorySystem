using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using unityInventorySystem;

using Laserbean.General.GlobalTicks;
using Laserbean.General.OtherInterfaces;

[CreateAssetMenu(fileName = "StatusEffectDOT", menuName = "Status Effects T/DamageOverTime", order = 2)]
public class DamageOverTimeStatusEffect : StatusEffectTObject
{

    public override StatusEffectT GetStatusEffect(int duration)
    {
        StatusEffectDOT statusfx = new(statuseffect, Name, duration);
        return statusfx;
    }

}

public class StatusEffectDOT : StatusEffectT
{
    public StatusEffectDOT(string nname, int duration) : base(nname, duration)
    {
    }

    public StatusEffectDOT(StatusEffectT statusfx, string nname, int duration) : base(statusfx, nname, duration)
    {
    }

    public override void OnApply(GameObject gameobject)
    {
        gameobject.GetComponent<IDamageableInt>()?.Damage(ValueOnApply); 
    }

    public override void OnRemove(GameObject gameobject)
    {
        gameobject.GetComponent<IDamageableInt>()?.Damage(ValueOnRemove); 
    }

    public override void OnTurnInternal(GameObject gameobject)
    {
        gameobject.GetComponent<IDamageableInt>()?.Damage(Value); 
    }


}
