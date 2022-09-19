using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.UI.Inventory;
using RPG.Attributes;

namespace RPG.Consumable
{
    [CreateAssetMenu(menuName = ("Equipment/Action Item/Potions/Heal Potion"))]
    public class HealPotion : ActionItem
    {
        [SerializeField] float amountToHeal = 50f;
        [SerializeField, Min(1)] int perioid = 5;
        [SerializeField] float timeBetweenHeal = 1f;
        [SerializeField] GameObject particleEffect;

        public override bool Use(GameObject user)
        {
            if (!user.TryGetComponent(out Health health)) return false;
            if (health.IsDead() || health.GetPercentage() >= 100.0f) return false;
                
            float heal = amountToHeal / perioid;

            if (health != null)
            {
                health.StartHealCoroutine(heal, perioid, timeBetweenHeal);

                if (particleEffect != null)
                {
                    GameObject particle = Instantiate(particleEffect, user.transform);
                    Destroy(particle, 2f);
                }
            }

            return true;
        }
    }
}
