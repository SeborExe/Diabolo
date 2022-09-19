using RPG.Attributes;
using RPG.UI.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Consumabl
{
    [CreateAssetMenu(menuName = ("Equipment/Action Item/Potions/Mana Potion"))]
    public class ManaPotion : ActionItem
    {
        [SerializeField] float amountToRestore = 50f;
        [SerializeField, Min(1)] int perioid = 5;
        [SerializeField] float timeBetweenHeal = 1f;
        [SerializeField] GameObject particleEffect;

        public override bool Use(GameObject user)
        {
            float manaToRestore = amountToRestore / perioid;

            Mana mana = user.GetComponent<Mana>();
            if (mana != null)
            {
                mana.StartRestoreManaCoroutine(manaToRestore, perioid, timeBetweenHeal);

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
