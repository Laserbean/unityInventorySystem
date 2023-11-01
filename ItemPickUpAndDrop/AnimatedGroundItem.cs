using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace unityInventorySystem.Items
{
    public class AnimatedGroundItem : GroundItem
    {

        float time = 0;
        [SerializeField] float animation_speed = 1f;
        [Range(0f, 1f)]
        [SerializeField] float sin_ratio = 0.7f;
        [SerializeField] bool isAnimated = true;

        private void Update()
        {
            if (isAnimated) {
                time += Time.deltaTime;
                this.transform.localScale = Vector3.one + (Vector3.one * Mathf.Sin(time * animation_speed) * sin_ratio);
            }
        }


    }
}