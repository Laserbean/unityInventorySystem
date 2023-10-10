

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
    [field: SerializeField]
    public string Name {get; private set;} 
    [field: SerializeField]
    public bool DoImediate {get; private set;} 


    [field: SerializeField]
    public int Value {get; protected set;} 
    [field: SerializeField]
    public int ValueOnApply {get; private set;} 
    [field: SerializeField]
    public int ValueOnRemove {get; private set;} 
    // public int ValueDurationModifier {get => _valueDurationModifier;} 

    [field: SerializeField, Tooltip("[onturn, min, max]")]
    public Vector3Int ValueDurationModifier {get; private set;} 

    [field: SerializeField]
    public int Rate {get; private set;}  

    public AttributeType attributeType; 
    public int attribute_value; 


    public StainType stainType; 

    [SerializeField]
    protected int turns_remaining = 0; 
    protected int total_turns = 0; 
    public int TotalTurns { get => total_turns; }
    public int TurnsRemaining { get => turns_remaining; }
    public bool IsActive { get => turns_remaining > 0 || turns_remaining == -1; }


    string _id; 

    public void SetRate(int rate) {
        Rate = rate; 
    }

    public void SetValue(int val) {
        Value= val; 
    }
    
    public void ModifyValue(int val) {
        Value += val; 
    }


    void IModifier.AddValue(ref int baseValue)
    {
        baseValue += attribute_value; 
    }

    public StatusEffectT (string nname, int duration) {
        turns_remaining = duration; 
        Name = nname;

        _id = RandomStatic.GenerateRandomString(5); 
    }

    public StatusEffectT(StatusEffectT statusfx, string nname, int duration) {
        _id = RandomStatic.GenerateRandomString(5); 

        DoImediate = statusfx.DoImediate;
        Value = statusfx.Value;
        ValueOnApply = statusfx.ValueOnApply;
        ValueOnRemove = statusfx.ValueOnRemove;
        ValueDurationModifier = statusfx.ValueDurationModifier;

        attribute_value = statusfx.attribute_value; 

        Rate = statusfx.Rate;

        attributeType = statusfx.attributeType; 
        stainType = statusfx.stainType; 

        turns_remaining = duration; 
        Name = nname;
    }

        public StatusEffectT(int turns_remaining)
        {
            this.turns_remaining = turns_remaining;
        }

        public virtual void Stack(StatusEffectT statuseffect) {
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

        Value += ValueDurationModifier.x; 

        if (ValueDurationModifier.x != 0)
            Value = Mathf.Clamp(Value, ValueDurationModifier.y, ValueDurationModifier.z);
        // _value += Mathf.RoundToInt((TotalTurns-1) * ValueDurationModifier/Rate); 

        if (Value == (ValueDurationModifier.y)) OnValueMin(gameobject); 
        if (Value == (ValueDurationModifier.z)) OnValueMax(gameobject); 

        if (Rate == 0) return; 
        if (total_turns % Rate == 0)
            OnTurnInternal(gameobject); 
    }

    public virtual void OnValueMin(GameObject gameobject) {
    }

    public virtual void OnValueMax(GameObject gameobject) {
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
    void RemoveStatusEffect(StatusEffectTObject statusEffectTObject); 
    void RemoveStatusEffect(StatusEffectT statusEffectt); 
}


}

