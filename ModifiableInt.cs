﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ModifiedEvent();
[System.Serializable]
public class ModifiableInt
{
    [SerializeField]
    private int baseValue;
    public int BaseValue { get { return baseValue; } set { baseValue = value; UpdateModifiedValue(); } }

    [field: SerializeField]
    public int ModifiedValue {get; private set;}

    [System.NonSerialized]
    public List<IModifier> modifiers = new ();

    public event ModifiedEvent ValueModified;
    public ModifiableInt(ModifiedEvent method = null) {
        ModifiedValue = BaseValue;
        if (method != null) ValueModified += method;
    }

    public void RegsiterModEvent(ModifiedEvent method) {
        ValueModified += method;
    }
    
    public void UnregsiterModEvent(ModifiedEvent method) {
        ValueModified -= method;
    }

    public void UpdateModifiedValue() {
        var valueToAdd = 0;
        for (int i = 0; i < modifiers.Count; i++) {
            modifiers[i].AddValue(ref valueToAdd);
        }
        ModifiedValue = baseValue + valueToAdd;
        ValueModified?.Invoke();
    }

    public void AddModifier(IModifier _modifier) {
        modifiers.Add(_modifier);
        UpdateModifiedValue();
    }

    public void RemoveModifier(IModifier _modifier) {
        modifiers.Remove(_modifier);
        UpdateModifiedValue();
    }

}
