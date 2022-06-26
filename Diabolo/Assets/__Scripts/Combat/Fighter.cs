using RPG.Movement;
using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        Mover mover;
        Transform target;
        Animator animator;
        ActionScheduler actionScheduler;

        [SerializeField] float weaponRange = 2f;
        [SerializeField] float timeBetweenAttacks = 1.3f;

        float timeSinceLastAttack = 0f;

        private void Awake()
        {
            mover = GetComponent<Mover>();
            actionScheduler = GetComponent<ActionScheduler>();
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;
            MoveToAttack();
        }

        private void MoveToAttack()
        {
            if (!GetIsInRange())
            {
                mover.MoveTo(target.position);
            }
            else
            {
                mover.Cancel();

                if (timeSinceLastAttack > timeBetweenAttacks)
                {
                    animator.SetTrigger("attack");
                    timeSinceLastAttack = 0;
                }
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

        //Animation Event
        public void Hit()
        {

        }
    }
}
