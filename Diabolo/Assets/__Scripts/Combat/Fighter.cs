using RPG.Movement;
using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        Mover mover;
        Health target;
        Animator animator;
        ActionScheduler actionScheduler;

        [SerializeField] float weaponRange = 2f;
        [SerializeField] float timeBetweenAttacks = 1.3f;
        [SerializeField] float weaponDamage = 10f;

        [SerializeField] GameObject weaponPrefab = null;
        [SerializeField] Transform handTransform = null;

        float timeSinceLastAttack = Mathf.Infinity;

        private void Awake()
        {
            mover = GetComponent<Mover>();
            actionScheduler = GetComponent<ActionScheduler>();
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            SpawnWeapon();
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;
            if (target.IsDead()) return;

            MoveToAttack();
        }

        private void MoveToAttack()
        {
            if (!GetIsInRange())
            {
                mover.MoveTo(target.transform.position, 1f);
            }
            else
            {
                mover.Cancel();
                transform.LookAt(target.transform);

                if (timeSinceLastAttack > timeBetweenAttacks)
                {
                    //Trigger Hit event
                    TriggerAttack();
                    timeSinceLastAttack = 0;
                }
            }
        }

        private void TriggerAttack()
        {
            animator.ResetTrigger("stopAttack");
            animator.SetTrigger("attack");
        }

        //Animation Event
        public void Hit()
        {
            if (target == null) { return; }

            target.TakeDamage(weaponDamage);
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < weaponRange;
        }

        public void Attack(GameObject combatTarget)
        {
            actionScheduler.StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) return false;

            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }

        public void Cancel()
        {
            StopAttack();
            target = null;
            mover.Cancel();
        }

        private void StopAttack()
        {
            animator.ResetTrigger("attack");
            animator.SetTrigger("stopAttack");
        }

        private void SpawnWeapon()
        {
            Instantiate(weaponPrefab, handTransform);
        }
    }
}
