using System.Collections;
using System.Collections.Generic;
using Laserbean.SpecialData;
using Unity.Plastic.Newtonsoft.Json.Converters;
using UnityEngine;
using unityInventorySystem;

public class ElementController : MonoBehaviour
{
    
    [SerializeField] CustomDictionary<StainType, int> statusDict = new ();



    // [EasyButtons.Button] 
    void TestAddToStatusDict() {
        statusDict[StainType.Slime]= 3;
    }

    // [EasyButtons.Button] 
    void TestEditStatusDict() {
        statusDict[StainType.Wet] -= 1; 
    }

    public List<StatusEffectT> statusEffectTs = new(); 



}


