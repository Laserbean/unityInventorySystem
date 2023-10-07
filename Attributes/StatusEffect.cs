

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using unityInventorySystem.Attribute; 


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace unityInventorySystem {

public delegate void StatusEffectDelegate(StatusEffectT statusEffectT, GameObject gameObject);

[System.Serializable]
public class StatusEffectT : IModifier
{
    [SerializeField] bool _doImediate = false; 
    [SerializeField] int _value = 0;
    [SerializeField] int _rate = 1; 


    public bool DoImediate {get => _doImediate;} 
    public int Value {get => _value;} 
    public int Rate {get => _rate;}  

    public void SetRate(int rate) {
        _rate = rate; 
    }
        
    int turns_remaining = 0; 
    int total_turns = 0; 

    public int TotalTurns {
        get => total_turns; 
    }


    public int TurnsRemaining {
        get => turns_remaining; 
    }
    

    StatusEffectDelegate _OnApply; 
    StatusEffectDelegate _OnRemove; 
    StatusEffectDelegate _OnTurn; 

    public bool IsActive {
        get => turns_remaining > 0 || turns_remaining == -1; 
    }


    void IModifier.AddValue(ref int baseValue)
    {
        baseValue += _value; 
    }

    public StatusEffectT (int duration) {
        turns_remaining = duration; 
    }

    public void Stack(StatusEffectT statuseffect) {
        turns_remaining += statuseffect.TurnsRemaining;
    }

    public void SetDelegates(StatusEffectDelegate onapply, StatusEffectDelegate onremove, StatusEffectDelegate onturn) {
        _OnApply = onapply; 
        _OnRemove = onremove;
        _OnTurn = onturn; 
    }

    public enum CallCondition {
        OnApply,
        OnRemove,
        OnTurn
    }

    public void AddDelegate(CallCondition condition, StatusEffectDelegate methodtocall) {
        switch(condition) {
            case CallCondition.OnApply:
                _OnApply += methodtocall; 
            break;
            case CallCondition.OnRemove:
                _OnRemove += methodtocall; 
            break;
            case CallCondition.OnTurn:
                _OnTurn += methodtocall; 
            break;
        }
    }

    public void RemoveDelegate(CallCondition condition, StatusEffectDelegate methodtocall) {
        switch(condition) {
            case CallCondition.OnApply:
                _OnApply -= methodtocall; 
            break;
            case CallCondition.OnRemove:
                _OnRemove -= methodtocall; 
            break;
            case CallCondition.OnTurn:
                _OnTurn -= methodtocall; 
            break;
        }
    }


    public void OnApply(GameObject gameobject) {
        _OnApply?.Invoke(this, gameobject); 
    }

    public void OnRemove(GameObject gameobject) {
        _OnRemove?.Invoke(this, gameobject); 
    }

    public void OnTurn(GameObject gameobject) {
        total_turns++;
        turns_remaining--; 

        if (_rate == 0) return; 
        if (total_turns % _rate == 0)
            _OnTurn?.Invoke(this, gameobject); 
    }

}


public interface IStatusAffect {
    void AddStatusEffect(StatusEffectTObject statusEffectTObject, float duration); 
}


}

