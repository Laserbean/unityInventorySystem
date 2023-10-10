using System.Collections;
using System.Collections.Generic;
using Laserbean.General.GlobalTicks;
using UnityEngine;

public class TriggerStainer : MonoBehaviour
{
    public StainType stainType;
    public int value;


    bool canStain = true; 
    private void Start() {
        TimeTickSystem.OnTick += delegate (object sender, TimeTickSystem.OnTickEventArgs e) {
            canStain = true; 
        };
    }


    private void OnTriggerStay2D(Collider2D other) {
        // if (!canStain) return; 


        var istainable = other.gameObject.GetComponent<IStainable>(); 

        istainable?.SetStain(stainType, value); 

        canStain = false;
    }
}
