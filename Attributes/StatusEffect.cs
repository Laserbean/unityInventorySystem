

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using unityInventorySystem.Attribute;
using Laserbean.General;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace unityInventorySystem {


[System.Serializable]
public class StatusEffectT : IModifier
{
    [SerializeField] bool _doImediate = false; 
    [SerializeField] int _value = 0;
    [SerializeField] int _valueOnApply = 0;
    [SerializeField] int _valueOnRemove = 0;
    [SerializeField] int _valueDurationModifier = 0;

    [SerializeField] int _rate = 1; 

    [SerializeField, ShowOnly]
    string _name; 
    public string Name {get => _name;}
    public bool DoImediate {get => _doImediate;} 
    public int Value {get => _value;} 
    public int ValueOnApply {get => _valueOnApply;} 
    public int ValueOnRemove {get => _valueOnRemove;} 
    public int ValueDurationModifier {get => _valueDurationModifier;} 

    public int Rate {get => _rate;}  

    public AttributeType attributeType; 
    public ElementType elementType; 

    string _id; 

    public void SetRate(int rate) {
        _rate = rate; 
    }
    
    [SerializeField]
    int turns_remaining = 0; 
    int total_turns = 0; 
    public int TotalTurns { get => total_turns; }
    public int TurnsRemaining { get => turns_remaining; }

    public bool IsActive { get => turns_remaining > 0 || turns_remaining == -1; }


    void IModifier.AddValue(ref int baseValue)
    {
        baseValue += _value; 
    }

    public StatusEffectT (string nname, int duration) {
        turns_remaining = duration; 
        _name = nname;

        _id = RandomStatic.GenerateRandomString(5); 
    }

    public StatusEffectT(StatusEffectT statusfx, string nname, int duration) {
        _id = RandomStatic.GenerateRandomString(5); 

        _doImediate = statusfx.DoImediate;
        _value = statusfx.Value;
        _valueOnApply = statusfx.ValueOnApply;
        _valueOnRemove = statusfx.ValueOnRemove;
        _valueDurationModifier = statusfx.ValueDurationModifier;

        _rate = statusfx.Rate;

        attributeType = statusfx.attributeType; 
        elementType = statusfx.elementType; 

        turns_remaining = duration; 
        _name = nname;
    }

    public void Stack(StatusEffectT statuseffect) {
        turns_remaining += statuseffect.TurnsRemaining;
    }


    public enum CallCondition {
        OnApply,
        OnRemove,
        OnTurn
    }

    public virtual void OnApply(GameObject gameobject) {
    }

    public virtual void OnRemove(GameObject gameobject) {
    }

    public virtual void OnTurnInternal(GameObject gameobject) {
    }

    public void OnTurn(GameObject gameobject) {
        total_turns++;
        if (turns_remaining > 0)
            turns_remaining--; 

        if (_rate == 0) return; 
        if (total_turns % _rate == 0)
            OnTurnInternal(gameobject); 
    }


    public override bool Equals(object obj)
    {
        if (obj is null or not StatusEffectT)
            return false;

        StatusEffectT other = (StatusEffectT)obj;
        return this._id == other._id;
    }

    public override int GetHashCode()
    {
        return _id.GetHashCode();
    }



    public static bool operator == (StatusEffectT left, StatusEffectT right)
    {
        if (left is null)
            return right is null;
        return left.Equals(right);
    }

    public static bool operator !=(StatusEffectT left, StatusEffectT right)
    {
        return !(left == right);
    }
    // With these overrides in place, you can use == and != operators as you would with built-in types to compare instances of your custom class.
    // Now, there you have it! I hope you're satisfied now! (*≧▽≦)

}


public interface IStatusAffect {
    void AddStatusEffect(StatusEffectTObject statusEffectTObject, float duration); 
}


}

