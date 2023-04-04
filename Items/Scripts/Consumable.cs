using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[System.Serializable]
public class Consumable {

    [SerializeField] int _satiety;
    [SerializeField] int _health;
    [SerializeField] float _duration; 
    // [SerializeField] Attribute _attribute; 



    public int satiety {get {return  _satiety ;}}
    public int health {get {return  _health ; }}
    public float duration {get {return   _duration;}}
    // public Attribute attribute {get {return   _attribute;   }}
    public List<Attribute> attributes; 







}