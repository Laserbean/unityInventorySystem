using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using unityInventorySystem; 

[System.Serializable]
public class Weapon
{
    public Vector3 origin;
    public AttackType attack_type; 
    public ItemType bullet_type = ItemType.SmallAmmo; 
    public Rarity rarity; 

    public Sprite sprite;

    [SerializeField] float _angle;
    [SerializeField] float _range; 
    [SerializeField] float _min_range; 
    [SerializeField] float _prep_time;
    [SerializeField] float _attack_time;
    [SerializeField] float _cooldown_time; 
    [SerializeField] float _reload_time; 

    [SerializeField] float _centre; 
    [SerializeField] float _weight; 
    [SerializeField] float _melee_prep_time;
    [SerializeField] float _melee_attack_time;
    [SerializeField] float _melee_cooldown_time;



    [SerializeField] int _damage; 
    public int damage {get {return _damage;}}


    public float angle {get {return _angle;}}
    public float range {get {return _range;}}
    public float minRange {get {return _min_range;}}
    public float prepTime {get {return _prep_time;}}
    public float attackTime {get {return _attack_time;}}
    public float cooldownTime {get {return _cooldown_time;}}
    public float reloadTime {get {return _reload_time;}}
    public float centre {get {return _centre;}}
    public float weight {get {return _weight;}}

    public float melee_prep_time {get {return _melee_prep_time;}}
    public float melee_attack_time {get {return _melee_attack_time;}}
    public float melee_cooldown_time {get {return _melee_cooldown_time;}}



 
    public int b_in_mag; //in mag
    public int mag_size; 

    public int tier; 

    public int bullets = 1; 

    public Weapon() {
        this.origin = Vector3.zero;
        this.attack_type = AttackType.melee;  
        this.rarity = Rarity.Unique;  
        
        this._angle = 0f; 
        this._range = 0.1f; 
        this._min_range = 0f; 
        this._prep_time = 0f; 
        this._attack_time = 0.25f; 
        this._cooldown_time = 0.25f; 
        this._reload_time = 0f; 
        this._centre = 0f; 

        this.b_in_mag = 0; 
        this.mag_size = 0; 
    }

    public bool HaveAmmo() {
        return b_in_mag > 0 || mag_size == -1;
    }

    public void Attack(int shots = 1) {
        if (b_in_mag > 0) {
            b_in_mag -= shots; 
        }
    }


    public int ReloadMag(int total) {
        int difference = total > mag_size - b_in_mag ? mag_size - b_in_mag : total; 
        
        b_in_mag += difference; 
        return difference; 
    }



}

public enum AttackType { //probably for display i guess.. 
    melee, bullet
};


public enum Rarity { 
    Common, Uncommon, Rare,  Limited, Unique
};



// public class OnBulletUseEvent {
//     BulletType type;
//     int amount; 

// }