using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Health : MonoBehaviour
    {
        Animator animator;

        [SerializeField] float maxHealth = 100f;
        [SerializeField] float curretHealth;

        bool isDead;

        public bool IsDead() => isDead;

        private void Awake()
        {
            animator = GetComponent<Animator>();

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
            }
        }
    }
}
