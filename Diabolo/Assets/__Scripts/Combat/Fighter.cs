using RPG.Movement;
using RPG.Core;
using UnityEngine;
using RPG.Saving;
using RPG.Attributes;
using RPG.Stats;
using System.Collections.Generic;
using RPG.Utils;
using RPG.UI.Inventory;
using System;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        Mover mover;
        Health target;
        Animator animator;
        ActionScheduler actionScheduler;
        BaseStats baseStats;
        WeaponConfig currentWeaponConfig;
        Equipment equipment;
        LazyValue <Weapon> currentWeapon;

        float attackSpeed;
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] WeaponConfig defaultWeapon = null;
        [SerializeField] float autoAttackRange = 2.5f;

        float timeSinceLastAttack = Mathf.Infinity;

        [SerializeField] private string lastAttack = string.Empty;
        private const string stopAttack = "stopAttack";
        private const string firstAttack = "attack";
        private const string secondAttack = "attack2";

        private void Awake()
        {
            mover = GetComponent<Mover>();
            actionScheduler = GetComponent<ActionScheduler>();
            animator = GetComponent<Animator>();
            baseStats = GetComponent<BaseStats>();
            equipment = GetComponent<Equipment>();

            currentWeaponConfig = defaultWeapon;
            currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);

            if (equipment != null)
            {
                equipment.equipmentUpdated += UpdateWeapon;
            }
        }

        private void Start()
        {
            currentWeapon.ForceInit();
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;
            if (target.IsDead())
            {
                target = FindNewTargetInRange();

                if (target == null) return;
            }

            MoveToAttack();
        }

        private Weapon SetupDefaultWeapon()
        {
            return AttachWeapon(defaultWeapon);
        }

        private void MoveToAttack()
        {
            if (!GetIsInRange(target.transform))
            {
                mover.MoveTo(target.transform.position, 1f);
            }
            else
            {
                mover.Cancel();
                transform.LookAt(target.transform);

                //TODO: Przebudowaæ szybkoœæ ataku
                attackSpeed = baseStats.GetStat(Stat.AttackSpeed) + (currentWeaponConfig.GetAttackSpeed() / 100f);
                //Debug.Log(attackSpeed);

                if (timeSinceLastAttack > 2.5f / attackSpeed)
                {
                    //Trigger Hit event
                    TriggerAttack();
                    timeSinceLastAttack = 0;
                }
            }
        }

        private void TriggerAttack()
        {
            if (lastAttack == string.Empty || lastAttack == secondAttack)
            {
                animator.ResetTrigger(stopAttack);
                animator.SetTrigger(firstAttack);
                lastAttack = firstAttack;
            }

            else if (lastAttack == firstAttack)
            {
                animator.ResetTrigger(stopAttack);
                animator.SetTrigger(secondAttack);
                lastAttack = secondAttack;
            }
        }

        public void Cancel()
        {
            StopAttack();
            target = null;
            mover.Cancel();
        }

        private void StopAttack()
        {
            animator.ResetTrigger(lastAttack);
            animator.SetTrigger(stopAttack);
            lastAttack = string.Empty;
        }

        //Animation Event
        public void Hit()
        {
            if (target == null) { return; }

            float damage = baseStats.GetStat(Stat.Damage);
            BaseStats targetBaseStats = target.GetComponent<BaseStats>();

            if (targetBaseStats != null)
            {
                float defence = targetBaseStats.GetStat(Stat.Defence);
                damage /= 1 + defence / damage;
            }

            if (currentWeapon.value != null)
            {
                currentWeapon.value.OnHit();
            }

            if (currentWeaponConfig.HasProjectile())
            {
                currentWeaponConfig.LunchProjectile(rightHandTransform, leftHandTransform, target, gameObject, damage);
            }
            else
            {
                target.TakeDamage(gameObject, damage);
            }
        }

        private Health FindNewTargetInRange()
        {
            Health best = null;
            float bestDistance = Mathf.Infinity;

            foreach (Health canditate in FindAllTargetsInRange())
            {
                float candidateDinstance = Vector3.Distance(transform.position, canditate.transform.position);
                if (candidateDinstance < bestDistance)
                {
                    best = canditate;
                    bestDistance = candidateDinstance;
                } 
            }

            return best;
        }

        private IEnumerable<Health> FindAllTargetsInRange()
        {
            RaycastHit[] raycastHits = Physics.SphereCastAll(transform.position, autoAttackRange, Vector3.up);

            foreach (RaycastHit hit in raycastHits)
            {
                Health health = hit.transform.GetComponent<Health>();

                if (health == null) continue;
                if (health.IsDead()) continue;
                if (health.gameObject == gameObject) continue;

                yield return health;
            }
        }

        public void Shoot()
        {
            Hit();
        }

        private bool GetIsInRange(Transform targetTransform)
        {
            return Vector3.Distance(transform.position, targetTransform.position) < currentWeaponConfig.GetRange();
        }

        public void Attack(GameObject combatTarget)
        {
            actionScheduler.StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) return false;
            if (!mover.CanMoveTo(combatTarget.transform.position) && !GetIsInRange(combatTarget.transform)) return false;

            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }

        public void EquipWeapon(WeaponConfig weapon)
        {
            currentWeaponConfig = weapon;
            currentWeapon.value = AttachWeapon(weapon);
        }

        private void UpdateWeapon()
        {
            WeaponConfig weapon = equipment.GetItemInSlot(EquipLocation.Weapon) as WeaponConfig;

            if (weapon == null)
            {
                EquipWeapon(defaultWeapon);
            }
            else
            {
                EquipWeapon(weapon);
            }
        }

        private Weapon AttachWeapon(WeaponConfig weapon)
        {
            return weapon.Spawn(rightHandTransform, leftHandTransform, animator);
        }

        public Health GetTarget()
        {
            return target;
        }

        public Transform GetHandTransform(bool isRightHand)
        {
            if (isRightHand)
            {
                return rightHandTransform;
            }
            else
            {
                return leftHandTransform;
            }
        }
    }
}
