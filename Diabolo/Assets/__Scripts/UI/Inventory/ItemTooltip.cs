using TMPro;
using UnityEngine;
using RPG.Combat;

namespace RPG.UI.Inventory
{
    public class ItemTooltip : MonoBehaviour
    {
        [SerializeField] TMP_Text titleText = null;
        [SerializeField] TMP_Text bodyText = null;

        public void Setup(InventoryItem item)
        {
            titleText.text = item.GetDisplayName();
            bodyText.text = item.GetDescription();

            if (item is StatsEquipableItem)
            {
                StatsEquipableItem itemEQ = (StatsEquipableItem)item;

                bodyText.text += '\n';
                bodyText.text += '\n';
                bodyText.text += itemEQ.AdditiveModifier();
                bodyText.text += '\n';
                bodyText.text += itemEQ.PercentageModifier();
            }

            if (item is WeaponConfig)
            {
                WeaponConfig weapon = (WeaponConfig)item;

                bodyText.text += '\n';
                bodyText.text += '\n';
                bodyText.text += $"Damage: {weapon.GetDamage()}";
                bodyText.text += '\n';
                bodyText.text += $"Percent Bonus: {weapon.GetPercentageBonus()}";
            }
        }
    }
}
