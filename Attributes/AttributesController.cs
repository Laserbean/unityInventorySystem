using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;


using Laserbean.General;


using unityInventorySystem; 
public class AttributesController : MonoBehaviour, IAttributeUsage
{

    Dictionary<AttributeType, int> AttributeDict = new Dictionary<AttributeType, int>(); 

    void Start()
    {
        for (int i = 0; i < attributes.Length; i++)
        {
            attributes[i].SetParent(this.gameObject);
            AttributeDict.Add(attributes[i].type, i); 
        }
    }

    public Attribute[] attributes;


    // void AddAttribute1(AttributeType attType, IModifier buff) {
    //     for (int j = 0; j < attributes.Length; j++) {
    //         if (attributes[j].type == attType)
    //             attributes[j].value?.AddModifier(buff);
    //     }
    // }

    // void RemoveAttribute1(AttributeType attType, IModifier buff) {
    //     for (int j = 0; j < attributes.Length; j++) {
    //         if (attributes[j].type == attType)
    //             attributes[j].value?.RemoveModifier(buff);
    //     }
    // }

    public void AddAttributeModifier(AttributeType type, IModifier value) {    
        if (AttributeDict.ContainsKey(type)) {
            attributes[AttributeDict[type]].value?.AddModifier(value); 
        } 
    }

    public void RemoveAttributeModifier(AttributeType type, IModifier value) {
        if (AttributeDict.ContainsKey(type)) {
            attributes[AttributeDict[type]].value?.RemoveModifier(value); 
        } 
    }

    public int GetAttributeValue(AttributeType typ) {
        if (AttributeDict.ContainsKey(typ)) {
            return attributes[AttributeDict[typ]].value.ModifiedValue; 
        } 
        return 0; 
    }

}



[System.Serializable]
public class Attribute
{
    [System.NonSerialized] public GameObject parent;
    public AttributeType type;
    public ModifiableInt value;

    public void SetParent(GameObject _parent)
    {
        parent = _parent;
        value = new ModifiableInt(AttributeModified);
    }

    public void AttributeModified()
    {
        if (parent == null) {
            return; 
        }
        List<IAttributeModified> interfaceList = parent.GetInterfaces<IAttributeModified>().ToList();

        // var inter = parent.GetComponent<IAttributeModified>();

        foreach(var inter in interfaceList) {
            inter?.AttributeModified(this); 
        }
    }
}


public interface IAttributeModified {
    public void AttributeModified(Attribute attribute); 
}



public enum AttributeType
{
    Agility,
    Defense,
    Stamina,
    Strength,
}

public static class AttributeCalculations {


    public static float CalculateAgility(int val) {

        // Currently returns the extra percentage. Eg. returns 0.6 for a 60% speed buff
        return (float) val/50;

    }

    public static int CalculateStrength(int val) {
        return val; 
    }

    public static int CalculateDefense(int val) {
        return val; 
    }

}