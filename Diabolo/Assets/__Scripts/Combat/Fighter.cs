using RPG.Movement;
using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        Transform target;
        Mover mover;
        ActionScheduler actionScheduler;

        [SerializeField] float weaponRange = 2f;

        private void Awake()
        {
            mover = GetComponent<Mover>();
            actionScheduler = GetComponent<ActionScheduler>();
        }

        private void Update()
        {
            if (target == null) return;

            if (!GetIsInRange())
            {
                mover.MoveTo(target.position);
            }
            else
            {
                mover.Cancel();
            }
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.position) < weaponRange;
        }

        public void Attack(CombatTarget combatTarget)
        {
            actionScheduler.StartAction(this);
            target = combatTarget.transform;
        }

        public void Cancel()
        {
            target = null;
        }
    }
}
