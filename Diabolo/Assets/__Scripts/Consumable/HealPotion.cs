using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.UI.Inventory;
using RPG.Attributes;

namespace RPG.Consumable
{
    [CreateAssetMenu(menuName = ("Equipment/Action Item/Heal Potion"))]
    public class HealPotion : ActionItem
    {
        [SerializeField] float amountToHeal = 50f;
        [SerializeField, Min(1)] int perioid = 5;
        [SerializeField] float timeBetweenHeal = 1f;
        [SerializeField] GameObject particleEffect;

        public override void Use(GameObject user)
        {
            float heal = amountToHeal / perioid;

            Health health = user.GetComponent<Health>();
            if (health != null)
            {
                health.StartHealCoroutine(heal, perioid, timeBetweenHeal);

                if (particleEffect != null)
                {
                    GameObject particle = Instantiate(particleEffect, user.transform);
                    Destroy(particle, 2f);
                }
            }
        }


    }
}
