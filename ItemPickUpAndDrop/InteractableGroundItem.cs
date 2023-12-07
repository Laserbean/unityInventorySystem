using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace unityInventorySystem.Items
{
    public class InteractableGroundItem : GroundItem, IInteractable
    {
        void IInteractable.Highlight()
        {
            this.transform.localScale = new Vector3 (1.5f, 1.5f, 1); 

        }

        void IInteractable.UnHighlight()
        {
            this.transform.localScale = new Vector3 (1, 1, 1); 
        }

        void IInteractable.Interact(GameObject gameObject)
        {
            if (!CanPickUp) return; 
            gameObject.GetComponent<IPickUp>()?.PickUpItem(this); 
        }
    }
}
