using RPG.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI.Inventory
{
    [CreateAssetMenu(menuName = ("Equipment/Inventory/EQ Item"))]
    public class StatsEquipableItem : EquipableItem, IModifierProvider
    {
        [SerializeField] Modifier[] additiveModifier;
        [SerializeField] Modifier[] percentageModifier;

        [System.Serializable]
        struct Modifier
        {
            public Stat stat;
            public float value;
        }

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            foreach (var modifier in additiveModifier)
            {
                if (modifier.stat == stat)
                {
                    yield return modifier.value;
                }
            }
        }

        public IEnumerable<string> GetAdditiveModifiers()
        {
            foreach (var modifier in additiveModifier)
            {
                yield return $"{modifier.stat}: +{modifier.value}";
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            foreach (var modifier in percentageModifier)
            {
                if (modifier.stat == stat)
                {
                    yield return modifier.value;
                }
            }
        }

        public IEnumerable<string> GetPercentageModifiers()
        {
            foreach (var modifier in percentageModifier)
            {
                yield return $"{modifier.stat}: +{modifier.value}%";
            }
        }

        public string AdditiveModifier()
        {
            for (int i = 0; i < additiveModifier.Length; i++)
            {
                return $"Increase {additiveModifier[i].stat} : {additiveModifier[i].value} \n";
            }

            return null;
        }

        public string PercentageModifier()
        {
            for (int i = 0; i < percentageModifier.Length; i++)
            {
                return $"Increase {percentageModifier[i].stat} : {percentageModifier[i].value}% \n";
            }

            return null;
        }
    }
}
