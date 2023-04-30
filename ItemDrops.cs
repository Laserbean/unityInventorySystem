using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace unityInventorySystem {



public class ItemDrops : MonoBehaviour
{
    [SerializeField] GameObject groundItemDrop; 
    
    public List<ItemDrop> dropList = new List<ItemDrop>(); 

    public void DropItems() {
        for (int j = 0; j < dropList.Count; j++) {
            if (UnityEngine.Random.Range(0f, 1f) < dropList[j].droprate) {

                GameObject go = Instantiate(groundItemDrop, this.transform.position, this.transform.rotation); 
                // GameObject go = Instantiate(AllDatabases.Instance.entityDB.groundItemDropPrefab, this.transform.position, this.transform.rotation); 


                go.GetComponent<GroundItem>().item = dropList[j].item; 

            }
        }
    }



}


[System.Serializable]
public class ItemDrop {
    public ItemObject item;
    [Range(0f, 1f)]
    public float droprate; 
}

}

