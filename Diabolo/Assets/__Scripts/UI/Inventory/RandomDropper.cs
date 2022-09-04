using RPG.Attributes;
using RPG.Control;
using RPG.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.UI.Inventory
{
    public class RandomDropper : ItemDropper
    {
        [SerializeField] float scatterDistance = 1f;
        [SerializeField] DropLibrary dropLibrary;

        Health health;
        BaseStats baseStats;

        const int Attempts = 80;

        private void Awake()
        {
            health = GetComponent<Health>();
            baseStats = GetComponent<BaseStats>();
        }

        private void OnEnable()
        {
            health.OnDie += RandomDrop;
        }

        private void OnDisable()
        {
            health.OnDie -= RandomDrop;
        }

        protected override Vector3 GetDropLocation()
        {
            for (int i = 0; i < Attempts; i++)
            {
                Vector3 randomPoint = transform.position + Random.insideUnitSphere * scatterDistance;

                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomPoint, out hit, 0.1f, NavMesh.AllAreas))
                {
                    return hit.position;
                }
            }

            return transform.position;
        }

        public void RandomDrop()
        {
            if (health.GetComponent<PlayerController>() != null) return;

            var drops = dropLibrary.GetRandomDrops(baseStats.GetLevel());

            foreach (var drop in drops)
            {
                DropItem(drop.item, drop.number);
            }
        }
    }
}
