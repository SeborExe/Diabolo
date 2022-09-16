using TMPro;
using UnityEngine;
using RPG.Combat;
using RPG.Core;
using UnityEngine.UI;

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
                Equipment equipment = GameManager.Instance.GetPlayer().GetComponent<Equipment>();

                if (!itemEQ.CanEquip(equipment))
                {
                    GetComponent<Image>().color = Color.red;
                }

                bodyText.text += '\n';
                bodyText.text += '\n';

                foreach (string additiveModifier in itemEQ.GetAdditiveModifiers())
                {
                    bodyText.text += additiveModifier;
                    bodyText.text += '\n';
                }

                foreach(string percentageModifier in itemEQ.GetPercentageModifiers())
                {
                    bodyText.text += '\n';
                    bodyText.text += percentageModifier;
                }

                if (itemEQ.GetConditions().and.Length != 0)
                {
                    bodyText.text += '\n';
                    bodyText.text += '\n';
                }

                foreach (var disjunction in itemEQ.GetConditions().and)
                {
                    foreach (var predicate in disjunction.or)
                    {
                        bodyText.text += $"{predicate.predicate}: ";

                        if (predicate.predicate == EPredicate.MinimumTrait)
                        {
                            bodyText.text += $"{predicate.parameters[0]} : {predicate.parameters[1]}";
                            bodyText.text += '\n';
                        }
                        else
                        {
                            foreach (var parameter in predicate.parameters)
                            {
                                bodyText.text += parameter;
                                bodyText.text += '\n';
                            }
                        }
                    }
                }
            }

            if (item is WeaponConfig)
            {
                WeaponConfig weapon = (WeaponConfig)item;
                Equipment equipment = GameManager.Instance.GetPlayer().GetComponent<Equipment>();

                if (!weapon.CanEquip(equipment))
                {
                    GetComponent<Image>().color = Color.red;
                }

                bodyText.text += '\n';
                bodyText.text += '\n';
                bodyText.text += $"Damage: {weapon.GetDamage()}";
                bodyText.text += '\n';
                bodyText.text += $"Attack Speed: {weapon.GetAttackSpeed()}%";
                bodyText.text += '\n';
                bodyText.text += $"Percent Bonus: {weapon.GetPercentageBonus()}";

                if (weapon.GetConditions().and.Length != 0)
                {
                    bodyText.text += '\n';
                    bodyText.text += '\n';
                }

                foreach (var disjunction in weapon.GetConditions().and)
                {
                    foreach (var predicate in disjunction.or)
                    {
                        bodyText.text += $"{predicate.predicate}: ";

                        if (predicate.predicate == EPredicate.MinimumTrait)
                        {
                            bodyText.text += $"{predicate.parameters[0]} : {predicate.parameters[1]}";
                            bodyText.text += '\n';
                        }
                        else
                        {
                            foreach (var parameter in predicate.parameters)
                            {
                                bodyText.text += parameter;
                                bodyText.text += '\n';
                            }
                        }
                    }
                }
            }
        }
    }
}
