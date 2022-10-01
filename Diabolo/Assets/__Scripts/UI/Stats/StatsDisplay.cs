using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using RPG.Stats;
using RPG.Combat;

namespace RPG.UI.Stats
{
    public class StatsDisplay : MonoBehaviour
    {
        BaseStats baseStats;
        Fighter fighter;

        [SerializeField] TMP_Text damageText;
        [SerializeField] TMP_Text healthText;
        [SerializeField] TMP_Text manaText;
        [SerializeField] TMP_Text healthRegenText;
        [SerializeField] TMP_Text manaRegenText;
        [SerializeField] TMP_Text defenceText;
        [SerializeField] TMP_Text spellDamageText;
        [SerializeField] TMP_Text attackSpeedText;
        [SerializeField] TMP_Text movementSpeedText;
        [SerializeField] TMP_Text chanceToBlockText;


        private void Start()
        {
            baseStats = GameManager.Instance.GetPlayer().GetComponent<BaseStats>();
            fighter = GameManager.Instance.GetPlayer().GetComponent<Fighter>();
        }

        private void OnEnable()
        {
            if (baseStats != null && fighter != null)
            {
                DisplayAttack();
                DisplayHealth();
                DisplayMana();
                DisplayHealthRegeneration();
                DisplayManaRegeneration();
                DisplayDefence();
                DisplaySpellDamage();
                DisplayAttackSpeed();
                DisplayMovementSpeed();
                DisplayChanceToBlock();
            }
        }

        private void DisplayChanceToBlock()
        {
            chanceToBlockText.text = $"{baseStats.GetStat(Stat.ChanceToBlock)}%";
        }

        private void DisplayMovementSpeed()
        {
            movementSpeedText.text = $"{baseStats.GetStat(Stat.MovementSpeed)}";
        }

        private void DisplayAttackSpeed()
        {
            attackSpeedText.text = $"{baseStats.GetStat(Stat.AttackSpeed) * 100}%";
        }

        private void DisplaySpellDamage()
        {
            spellDamageText.text = $"{baseStats.GetStat(Stat.SpellDamage)}";
        }

        private void DisplayDefence()
        {
            defenceText.text = $"{baseStats.GetStat(Stat.Defence)}";
        }

        private void DisplayManaRegeneration()
        {
            manaRegenText.text = $"{baseStats.GetStat(Stat.ManaRegeneration)}";
        }

        private void DisplayHealthRegeneration()
        {
            healthRegenText.text = $"{baseStats.GetStat(Stat.HealthRegeneration)}";
        }

        private void DisplayMana()
        {
            manaText.text = $"{baseStats.GetStat(Stat.Mana)}";
        }

        private void DisplayHealth()
        {
            healthText.text = $"{baseStats.GetStat(Stat.Health)}";
        }

        private void DisplayAttack()
        {
            float weaponMinDamage = fighter.GetWeaponConfig().GetMinDamage();
            float weaponMaxDamage = fighter.GetWeaponConfig().GetMaxDamage();
            float totalMinDamage = baseStats.GetStat(Stat.Damage) + weaponMinDamage;
            float totalMaxDamage = baseStats.GetStat(Stat.Damage) + weaponMaxDamage;

            damageText.text = $"{Mathf.RoundToInt(totalMinDamage)} - {Mathf.RoundToInt(totalMaxDamage)}";
        }
    }
}
