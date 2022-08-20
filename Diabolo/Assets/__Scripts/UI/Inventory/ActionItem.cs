using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI.Inventory
{
    [CreateAssetMenu(menuName = ("ScriptableObjects/Action Item"))]
    public class ActionItem : InventoryItem
    {
        [Tooltip("Does an instance of this item get consumed every time it's used.")]
        [SerializeField] bool consumable = false;

        public virtual void Use(GameObject user)
        {
            Debug.Log("Using action: " + this);
        }

        public bool isConsumable()
        {
            return consumable;
        }
    }
}
