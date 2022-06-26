using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float health = 100f;
        [SerializeField] float curretHealth;

        private void Awake()
        {
            curretHealth = health;
        }

        public void TakeDamage(float damage)
        {
            curretHealth = Mathf.Max(curretHealth - damage, 0);
        }
    }
}
