using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace unityInventorySystem {
public class GroundItem : MonoBehaviour, ISerializationCallbackReceiver
{
    public ItemObject item;

    public int ammount = 1; 

    public void OnAfterDeserialize()
    {
    }

    public void OnBeforeSerialize()
    {
#if UNITY_EDITOR
        // GetComponentInChildren<SpriteRenderer>().sprite = item.uiDisplay;
        // EditorUtility.SetDirty(GetComponentInChildren<SpriteRenderer>());
        if (item != null && isAnimated) {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = item.uiDisplay; 
        }
                

#endif
    }

    float time = 0; 
    [SerializeField] float animation_speed = 1f; 

    [Range(0f, 1f)]
    [SerializeField] float sin_ratio = 0.7f; 


    [SerializeField] bool isAnimated = true; 
    private void Update() {
        if (isAnimated) {
            time += Time.deltaTime; 
            this.transform.localScale =  Vector3.one+ (Vector3.one * Mathf.Sin(time *animation_speed) * sin_ratio); 
        }
    }

    public void SetItem(ItemObject item) {
        this.gameObject.GetComponent<SpriteRenderer>().sprite = item.uiDisplay; 

        this.item = item; 
    }
}
}