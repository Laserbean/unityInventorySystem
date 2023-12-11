// #define ENTITY_POOLER

using System.Collections;
using System.Collections.Generic;
using Laserbean.General;
using UnityEngine;

namespace unityInventorySystem.Items
{

    public class ItemDrops : MonoBehaviour
    {
#if !ENTITY_POOLER
    [SerializeField] GameObject groundItemDrop; 
#endif

        [Range(0, 30)]
        [SerializeField] int minItemDrop = 0;


        [Range(0, 30)]
        [SerializeField] int maxItemDrop = 1;

        [SerializeField] float dropradiusmin = 0f;
        
        [SerializeField] float dropradius = 1f;




        public List<ItemDrop> dropList = new();

        List<float> weights = new();


        private void Start()
        {
            SetupWeights();
        }

        private void OnValidate()
        {

            if (minItemDrop > maxItemDrop) {
                maxItemDrop = minItemDrop;
            }

        }

        void SetupWeights()
        {
            foreach (var drop in dropList) {
                weights.Add(drop.droprate);
            }

        }



        void DropItem(int itemDropIndex, Vector3 center, float mindistance, float maxdistance)
        {

#if ENTITY_POOLER
            GameObject go = EntityPooler.Instance.GetNewGroundItem();
            if (go == null) return;
            go.transform.position = center;
            go.transform.rotation = Quaternion.identity;
            go.SetActive(true);
#else
            GameObject go = Instantiate(groundItemDrop, center, Quaternion.identity); 
#endif

            var grounditem = go.GetComponent<IGroundItem>();
            grounditem.SetItem(dropList[itemDropIndex].itemObject.CreateItem(), Random.Range(dropList[itemDropIndex].min_max_amount.x, dropList[itemDropIndex].min_max_amount.y));
            
            go.transform.position += (Vector3.up * Random.Range(mindistance, maxdistance)).Rotate(Random.Range(0, 360f));
            go.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
            
            // go.transform.position += (Vector3.up * Random.Range(0f, dropradius)).Rotate(Random.Range(0, 360f));
            // go.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
        }

        int ChooseItem(float[] weights)
        {
            int itemDropIndex = Roulette.Spin(weights);
            return itemDropIndex;
        }

        [EasyButtons.Button]
        public void DropItems()
        {
            DropItemsAt(transform.position, dropradiusmin, dropradius);
        }

        public void DropItemsAt(Vector3 pos, float mindistance, float maxdistance)
        {
            float[] cur_weights = new List<float>(weights).ToArray();

            int numtodrop = Random.Range(minItemDrop, maxItemDrop + 1);

            for (int j = 0; j < numtodrop; j++) {
                int curitemtodrop = ChooseItem(cur_weights);
                if (curitemtodrop == -1) return;

                cur_weights[curitemtodrop] *= 0.5f;
                DropItem(curitemtodrop, pos, mindistance, maxdistance);
            }

        }



    }


    [System.Serializable]
    public class ItemDrop
    {
        public ItemObject itemObject;
        [Range(0f, 1f)]
        public float droprate;
        public Vector2Int min_max_amount;
    }

}

