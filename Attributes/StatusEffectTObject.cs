using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using unityInventorySystem.Attribute; 
using unityInventorySystem;
using Laserbean.General;
using Laserbean.General.GlobalTicks;

[CreateAssetMenu(fileName = "StatusEffect", menuName = "Status Effects T/Default", order = 1)]
public class StatusEffectTObject : ScriptableObject {

    [SpritePreview (0, 64)] 
    [SerializeField] Sprite _icon; 

    [SerializeField] 
    string _Name = "";


    [TextArea(15, 20)]

    [SerializeField] string _Description = ""; 

    public ElementType elementType; 
    public AttributeType attributeType; 


    [SerializeField] StatusEffectT _statuseffect; 

    // public StatusEffectT StatusEffect { get => _statuseffect;}


    public Sprite Icon {
        get => _icon;
    }

    public string Name {
        get =>  _Name;
    }


    // [SerializeField] 
    // string _Type = "";
    // public string Type {
    //     get =>  _Type;
    // }

    public string Description {
        get =>  _Description;
    }

    public virtual void OnApply(StatusEffectT statusEffect, GameObject gameobject)
    {
        Debug.Log(_Name + " Applied.".DebugColor(Color.cyan));
    }

    public virtual void OnRemove(StatusEffectT statusEffect, GameObject gameobject)
    {
        Debug.Log(_Name + " Removed.".DebugColor(Color.cyan));
    }

    public virtual void OnTurn(StatusEffectT statusEffect, GameObject gameobject)
    {
        Debug.Log(_Name + " OnTurn.".DebugColor(Color.cyan));
    }



    public StatusEffectT GetStatusEffect(int duration) {
        StatusEffectT statuseffect = new(duration);
        
        statuseffect.SetDelegates(OnApply, OnRemove, OnTurn);

        return statuseffect;
    }


    [EasyButtons.Button]
    void SetRate(float period, float tick_time = 0f) {
        if (tick_time == 0f) {
            tick_time = TimeTickSystem.TICK_TIME; 
        }

        int ticks = Mathf.RoundToInt(period/tick_time); 
        _statuseffect.SetRate(ticks); 
    }
}

