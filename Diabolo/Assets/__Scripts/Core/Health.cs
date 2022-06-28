using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    
namespace RPG.Core
{
    public class Health : MonoBehaviour
    {
        Animator animator;
        ActionScheduler actionScheduler;

        [SerializeField] float maxHealth = 100f;
        [SerializeField] float curretHealth;

        bool isDead;

        public bool IsDead() => isDead;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            actionScheduler = GetComponent<ActionScheduler>();

            curretHealth = maxHealth;
        }

        public void TakeDamage(float damage)
        {
            curretHealth = Mathf.Max(curretHealth - damage, 0);

            if (curretHealth == 0)
            {
                if (isDead) return;

                isDead = true;
                animator.SetTrigger("die");
                actionScheduler.CancelCurrentAction();
            }
        }
    }
}
