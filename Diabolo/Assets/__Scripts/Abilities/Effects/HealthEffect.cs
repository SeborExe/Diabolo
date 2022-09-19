using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Abilities;
using System;
using RPG.Attributes;
using RPG.Stats;

namespace RPG.Abilities.Effects
{
    [CreateAssetMenu(fileName = "HealthEffect_", menuName = "Abilities/Effects/Health Effect")]
    public class HealthEffect : EffectStrategy
    {
        [Tooltip("Schould be MINUS when spell deal a damage, POSITIVE when heal")]
        [SerializeField] float healthChange;

        public override void StartEffect(AbilityData data, Action finished)
        {
            foreach (GameObject target in data.GetTargets())
            {
                Health health = target.GetComponent<Health>();

                float spellDamage = data.GetUser().GetComponent<BaseStats>().GetStat(Stat.SpellDamage);

                if (health != null)
                {
                    if (healthChange < 0)
                    {
                        float damage = -healthChange + spellDamage;
                        health.TakeDamage(data.GetUser(), damage);
                    }
                    else
                    {
                        float heal = healthChange + spellDamage;
                        health.Heal(heal);
                    }
                }
            }

            finished();
        }
    }
}
