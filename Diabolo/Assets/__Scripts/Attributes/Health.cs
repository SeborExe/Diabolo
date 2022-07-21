using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;

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

        public void TakeDamage(float damage)
        {
            curretHealth = Mathf.Max(curretHealth - damage, 0);
            Die();
        }

        private void Die()
        {
            if (curretHealth == 0)
            {
                if (isDead) return;

                isDead = true;
                animator.SetTrigger("die");
                actionScheduler.CancelCurrentAction();
            }
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
