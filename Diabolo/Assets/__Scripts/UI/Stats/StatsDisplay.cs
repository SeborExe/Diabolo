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

        [SerializeField] TMP_Text damageText;


        //Zmieniæ na UpdateUI i aktualizowaæ za ka¿dym razem gdy jest uruchamiana
        private void OnEnable()
        {
            baseStats = GameManager.Instance.GetPlayer().GetComponent<BaseStats>();
            Fighter fighter = GameManager.Instance.GetPlayer().GetComponent<Fighter>();

            float weaponMinDamage = fighter.GetWeaponConfig().GetMinDamage();
            float weaponMaxDamage = fighter.GetWeaponConfig().GetMaxDamage();
            float totalMinDamage = baseStats.GetStat(Stat.Damage) + weaponMinDamage;
            float totalMaxDamage = baseStats.GetStat(Stat.Damage) + weaponMaxDamage;

            damageText.text = $"{totalMinDamage} - {totalMaxDamage}";
        }
    }
}
