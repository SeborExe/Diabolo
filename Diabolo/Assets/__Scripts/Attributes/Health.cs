using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using System;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        Animator animator;
        ActionScheduler actionScheduler;

        [SerializeField] float curretHealth;
        float maxHealth;

        bool isDead;

        public bool IsDead() => isDead;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            actionScheduler = GetComponent<ActionScheduler>();
            maxHealth = GetComponent<BaseStats>().GetHealth();

            curretHealth = maxHealth;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            curretHealth = Mathf.Max(curretHealth - damage, 0);
            if (curretHealth == 0)
            {
                Die();
                AwardExperience(instigator);
            }
        }

        public float GetPercentage()
        {
            return (curretHealth / maxHealth) * 100;
        }

        private void Die()
        {
            if (isDead) return;

            isDead = true;
            animator.SetTrigger("die");
            actionScheduler.CancelCurrentAction();
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) { return; }

            experience.GainExperience(GetComponent<BaseStats>().GetExperienceReward());
        }

        public object CaptureState()
        {
            return curretHealth;
        }

        public void RestoreState(object state)
        {
            curretHealth = (float)state;
            Die();
        }
    }
}
