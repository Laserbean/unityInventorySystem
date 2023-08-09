using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using unityInventorySystem; 
[CreateAssetMenu(fileName = "StatusEffect", menuName = "Status Effects/Default", order = 0)]
public class StatusEffectObject : ScriptableObject, IStatusDoable
{
    [SerializeField] string _Name = "";
    [SerializeField] string _Description = ""; 
    [SerializeField] Sprite _icon; 
    // [SerializeField] float  _duration; 

    public string Name {
        get {return _Name;}
    }

    public string description {
        get {return _Description;}
    }

    public Sprite icon {
        get {return _icon;}
    }

    public virtual bool DoUpdate(StatusEffectD statusEffectD, float dtime)
    {
        // throw new System.NotImplementedException();
        return false; 
    }

    public virtual void DoIfUpdated(int value)
    {
        // throw new System.NotImplementedException();
    }

    // public float duration {
    //     get {return _duration;}
    // }

}



public enum StatusEffectType {
    Default, Attribute, RepeatOverTime, DamageOverTime, Buildup
}

/*

How can I implement a buildup of poison or something? 

Point base? 
[ ][ ][ ][ ][ ]
[X][X][ ][ ][ ]

Percent base?
30%

Confusion
Charm?

Fire (DamageOverTime)
Frozen (Maybe temperature stat?)
Slow (attribute)

Fear (Maybe later)

Adrenaline ? (Attribute) (maybe boost stats when near death)? Unlockable skill? 

Wet (Attribute)


*/