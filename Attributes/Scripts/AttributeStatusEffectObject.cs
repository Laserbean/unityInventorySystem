using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using unityInventorySystem.Attribute; 

[CreateAssetMenu(fileName = "AttributeSE", menuName = "Status Effects/Attribute", order = 2)]
public class AttributeStatusEffectObject : StatusEffectObject
{
    [SerializeField] AttributeType _type; 
    [SerializeField] int _value; 

    public AttributeType type {
        get {return _type;}
    }

    public int value {
        get {return _value;}
    }


}
