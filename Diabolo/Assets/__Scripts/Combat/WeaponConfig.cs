using RPG.Attributes;
using UnityEngine;
using RPG.UI.Inventory;
using RPG.Stats;
using System.Collections.Generic;
using UnityEditor;
using System;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make new Weapon")]
    public class WeaponConfig : EquipableItem, IModifierProvider
    {
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] Weapon equippedPrefab = null;
        [SerializeField] int minWeaponDamage = 2;
        [SerializeField] int maxWeaponDamage = 3;
        [SerializeField] float percentageBonus = 0f;
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float weaponAttackSpeed = 100f;
        [SerializeField] bool isRightHand = true;
        [SerializeField] Projectile projectile = null;

        const string weaponName = "weapon";

        public Weapon Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            DestroyOldWeapon(rightHand, leftHand);

            Weapon weapon = null;

            if (equippedPrefab != null)
            {
                Transform handTransform = GetTransform(rightHand, leftHand);
                weapon = Instantiate(equippedPrefab, handTransform);
                weapon.name = weaponName;
            }

            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;

            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }

            else if (overrideController != null)
            {
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }

            return weapon;
        }

        private Transform GetTransform(Transform rightHand, Transform leftHand)
        {
            Transform handTransform = isRightHand ? rightHand : leftHand;
            return handTransform;
        }

        public bool HasProjectile()
        {
            return projectile != null;
        }

        public void LunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator, float calculatedDamage)
        {
            Projectile projectileInstance = Instantiate(projectile, GetTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target, instigator, calculatedDamage);
        }

        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform oldWeapon = rightHand.Find(weaponName);
            if (oldWeapon == null)
            {
                oldWeapon = leftHand.Find(weaponName);
            }

            if (oldWeapon == null) { return; }

            oldWeapon.name = "Destroying";
            Destroy(oldWeapon.gameObject);
        }

        /*
        public float GetDamage()
        {
            return weaponDamage;
        }
        */

        public int GetMinDamage()
        {
            return minWeaponDamage;
        }

        public int GetMaxDamage()
        {
            return maxWeaponDamage;
        }

        public float GetAttackSpeed()
        {
            return weaponAttackSpeed;
        }

        public float GetPercentageBonus()
        {
            return percentageBonus;
        }

        public float GetRange()
        {
            return weaponRange;
        }

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                float randomDamage = UnityEngine.Random.Range(minWeaponDamage, maxWeaponDamage + 1);
                yield return randomDamage;
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return percentageBonus;
            }
        }

        public override string GetDescription()
        {
            string result = projectile ? "Ranged Weapon" : "Melee Weapon";
            result += $"\n\n{GetRawDescription()}\n";
            result += $"\nRange {weaponRange} meters";
            result += $"\nBase Damage {minWeaponDamage} - {maxWeaponDamage} points";
            if ((int)percentageBonus != 0)
            {
                string bonus = percentageBonus > 0 ? "<color=#8888ff>bonus</color>" : "<color=#ff8888>penalty</color>";
                result += $"\n{(int)percentageBonus} percent {bonus} to attack.";
            }

            return result;
        }

#if UNITY_EDITOR

        void SetWeaponRange(float newWeaponRange)
        {
            if (FloatEquals(weaponRange, newWeaponRange)) return;
            SetUndo("Set Weapon Range");
            weaponRange = newWeaponRange;
            Dirty();
        }

        void SetWeaponAttackSpeed(float newAttackSpeed)
        {
            if (FloatEquals(weaponAttackSpeed, newAttackSpeed)) return;
            SetUndo("Set Weapon Range");
            weaponAttackSpeed = newAttackSpeed;
            Dirty();
        }

        void SetMinWeaponDamage(int newWeaponDamage)
        {
            if (minWeaponDamage == newWeaponDamage) return;
            SetUndo("Set Min Weapon Damage");
            minWeaponDamage = newWeaponDamage;
            Dirty();
        }

        void SetMaxWeaponDamage(int newWeaponDamage)
        {
            if (maxWeaponDamage == newWeaponDamage) return;
            SetUndo("Set Max Weapon Damage");
            maxWeaponDamage = newWeaponDamage;
            Dirty();
        }

        void SetPercentageBonus(float newPercentageBonus)
        {
            if (FloatEquals(percentageBonus, newPercentageBonus)) return;
            SetUndo("Set Percentage Bonus");
            percentageBonus = newPercentageBonus;
            Dirty();
        }

        void SetIsRightHanded(bool newRightHanded)
        {
            if (isRightHand == newRightHanded) return;
            SetUndo(newRightHanded ? "Set as Right Handed" : "Set as Left Handed");
            isRightHand = newRightHanded;
            Dirty();
        }

        void SetAnimatorOverride(AnimatorOverrideController newOverride)
        {
            if (newOverride == animatorOverride) return;
            SetUndo("Change AnimatorOverride");
            animatorOverride = newOverride;
            Dirty();
        }

        void SetEquippedPrefab(Weapon newWeapon)
        {
            if (newWeapon == equippedPrefab) return;
            SetUndo("Set Equipped Prefab");
            equippedPrefab = newWeapon;
            Dirty();
        }

        void SetProjectile(Projectile possibleProjectile)
        {
            if (possibleProjectile == null) return;
            if (!possibleProjectile.TryGetComponent<Projectile>(out Projectile newProjectile)) return;
            if (newProjectile == projectile) return;
            SetUndo("Set Projectile");
            projectile = newProjectile;
            Dirty();
        }

        public override bool IsLocationSelectable(Enum location)
        {
            EquipLocation candidate = (EquipLocation)location;
            return candidate == EquipLocation.Weapon;
        }

        bool drawInventoryItem = true;
        public override void DrawCustomInspector()
        {
            base.DrawCustomInspector();

            foldoutStyle = new GUIStyle(EditorStyles.foldout);
            foldoutStyle.fontStyle = FontStyle.Bold;
            drawInventoryItem = EditorGUILayout.Foldout(drawInventoryItem, "Weapon Config", foldoutStyle);
            if (!drawInventoryItem) return;
            SetEquippedPrefab((Weapon)EditorGUILayout.ObjectField("Equipped Prefab", equippedPrefab, typeof(System.Object), false));
            SetMinWeaponDamage(EditorGUILayout.IntSlider("Weapon Damage Min", minWeaponDamage, 0, 100));
            SetMaxWeaponDamage(EditorGUILayout.IntSlider("Weapon Damage Max", maxWeaponDamage, 0, 100));
            SetWeaponRange(EditorGUILayout.Slider("Weapon Range", weaponRange, 1, 40));
            SetWeaponAttackSpeed(EditorGUILayout.Slider("Weapon Attack Speed", weaponAttackSpeed, 10, 300));
            SetPercentageBonus(EditorGUILayout.IntSlider("Percentage Bonus", (int)percentageBonus, -10, 100));
            SetIsRightHanded(EditorGUILayout.Toggle("Is Right Handed", isRightHand));
            SetAnimatorOverride((AnimatorOverrideController)EditorGUILayout.ObjectField("Animator Override", animatorOverride, typeof(AnimatorOverrideController), false));
            SetProjectile((Projectile)EditorGUILayout.ObjectField("Projectile", projectile, typeof(Projectile), false));
        }

#endif
    }
}

