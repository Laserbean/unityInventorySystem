using System.Collections;
using System.Collections.Generic;
using Laserbean.SpecialData;
using Unity.Plastic.Newtonsoft.Json.Converters;
using UnityEngine;

public class ElementController : MonoBehaviour
{
    
    [SerializeField] CustomDictionary<ElementType, int> statusDict = new ();


    [SerializeField] SerializedKeyValuePair<int, string> testkvp; 

    // [EasyButtons.Button] 
    void TestAddToStatusDict() {
        statusDict[ElementType.Bleed]= 3;
    }

    // [EasyButtons.Button] 
    void TestEditStatusDict() {
        statusDict[ElementType.Wet] -= 1; 
    }


}


