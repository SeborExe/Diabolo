using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.UI.Inventory;
using RPG.Attributes;

namespace RPG.Consumable
{
    [CreateAssetMenu(menuName = ("ScriptableObjects/Action Item/Heal Potion"))]
    public class HealPotion : ActionItem
    {
        [SerializeField] float amountToHeal = 50f;

        public override void Use(GameObject user)
        {
            Health health = user.GetComponent<Health>();
            if (health != null)
            {
                health.Heal(amountToHeal);
            }
        }
    }
}
