using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "StatusEffectSettings", menuName = "Status Effects T/Settings", order = 0)]
public class StatusEffectElementTypeSetter : ScriptableObject {
    
    [Header("StainType")]
    [CustomEnum("StainType", "Assets/Scripts/unityInventorySystem/Attributes/")]
    public CustomEnumValueList stringEnumConverter; 

}