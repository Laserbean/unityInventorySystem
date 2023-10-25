using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;


using Laserbean.General;
using Laserbean.SpecialData;

namespace unityInventorySystem.Attribute {



public class AttributesController : MonoBehaviour, IAttributeUsage, IAttributeController
{
    // [SerializeField]
    // CustomDictionary<AttributeType, Attribute> newAttributeDict = new(); 

    Dictionary<AttributeType, int> AttributeDict = new (); 

    void Start()
    {
        UpdateDict();
    }

    void UpdateDict() {
        AttributeDict.Clear(); 
        for (int i = 0; i < attributes.Length; i++)
        {
            AttributeDict.Add(attributes[i].type, i); 

            OnAttributeChange?.Invoke(attributes[i]); 
            // Debug.Log(attributes[i].type + ": " + attributes[i].value.BaseValue); 
        }
    }

    #if UNITY_EDITOR 
    private void OnValidate() {
        UpdateDict();
    }
    #endif

    public Attribute[] attributes;

    public void AddAttributeModifier(AttributeType type, IModifier value) {    
        if (AttributeDict.ContainsKey(type)) {
            attributes[AttributeDict[type]]?.AddModifier(value); 
            OnAttributeChangeInternal(attributes[AttributeDict[type]]); 
        } 
    }

    public void RemoveAttributeModifier(AttributeType type, IModifier value) {
        if (AttributeDict.ContainsKey(type)) {
            attributes[AttributeDict[type]]?.RemoveModifier(value); 
            OnAttributeChangeInternal(attributes[AttributeDict[type]]); 
        } 
    }

    void OnAttributeChangeInternal(Attribute attribute) {
        OnAttributeChange?.Invoke(attribute); 

        List<IAttributeModified> interfaceList = gameObject.GetInterfacesInChildren<IAttributeModified>().ToList();
        foreach(var inter in interfaceList) {
            inter?.AttributeModified(attribute); 
        }
    }

    public int GetAttributeValue(AttributeType typ) {
        if (AttributeDict.ContainsKey(typ)) 
            return attributes[AttributeDict[typ]].ModifiedValue; 
        return 0; 
    }

    public int GetAttributeBaseValue(AttributeType typ) {        
        if (AttributeDict.ContainsKey(typ)) 
            return attributes[AttributeDict[typ]].BaseValue; 
        return 0; 
    }

    public Attribute GetAttribute(AttributeType typ) {
        if (AttributeDict.ContainsKey(typ)) 
            return attributes[AttributeDict[typ]]; 
        return null; 
    }

    public AttributeChange OnAttributeChange;

    AttributeChange IAttributeUsage.OnAttributeChange { 
        get => OnAttributeChange; 
        set => OnAttributeChange = value; 
    }
}



[System.Serializable]
public class Attribute : ModifiableInt
{
    public AttributeType type;

    public Attribute() {
    }

}


public interface IAttributeModified {
    public void AttributeModified(Attribute attribute); 
}



public enum AttributeType
{
    Nothing,
    Agility,
    Defense,
    Armour,
    Stamina,
    Strength,
    Health,
    Luck,
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
        return Mathf.RoundToInt((5 * Mathf.Sqrt(val)) + 0);
    }

    public static float CalculateDefensePercentage(int val) {
        return ((5 * Mathf.Sqrt(val)) + 0)/100f;
    }

}

}