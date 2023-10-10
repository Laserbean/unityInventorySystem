using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using unityInventorySystem.Attribute; 
using unityInventorySystem;
using Laserbean.General;
using Laserbean.General.GlobalTicks;
using UnityEngine.Search;

[CreateAssetMenu(fileName = "StatusEffect", menuName = "Status Effects T/Default", order = 1)]
public class StatusEffectTObject : ScriptableObject {

    [SpritePreview (0, 64)] 
    // [SearchContext("", SearchViewFlags.GridView)]
    [SerializeField] Sprite _icon; 

    [SerializeField] 
    string _Name = "";


    [TextArea(15, 20)]

    [SerializeField] string _Description = ""; 

    [SerializeField] protected StatusEffectT statuseffect; 

    // public StatusEffectT StatusEffect { get => _statuseffect;}


    public Sprite Icon {
        get => _icon;
    }

    public string Name {
        get =>  _Name;
    }


    public string Description {
        get =>  _Description;
    }

    [SerializeField] bool isStackable; 

    public bool IsStackable {
        get => isStackable; 
    }


    public virtual StatusEffectT GetStatusEffect(int duration) {
        StatusEffectT statusfx = new(statuseffect, Name, duration);
        
        return statusfx;
    }


    [EasyButtons.Button]
    protected void SetRate(float period, float tick_time = 0f) {
        if (tick_time == 0f) {
            tick_time = TimeTickSystem.TICK_TIME; 
        }

        int ticks = Mathf.RoundToInt(period/tick_time); 
        statuseffect.SetRate(ticks); 
    }
}

