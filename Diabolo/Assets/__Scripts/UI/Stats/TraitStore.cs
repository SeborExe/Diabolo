using RPG.Stats;
using RPG.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.UI.Stats
{
    public class TraitStore : MonoBehaviour, IModifierProvider, ISaveable, IPredicateEvaluator
    {
        [SerializeField] TraitBonus[] bonusConfig;

        Dictionary<Trait, int> assignedPoints = new Dictionary<Trait, int>();
        Dictionary<Trait, int> stagedPoints = new Dictionary<Trait, int>();

        Dictionary<Stat, Dictionary<Trait, float>> additiveBonusCash;
        Dictionary<Stat, Dictionary<Trait, float>> percentageBonusCash;

        [System.Serializable]
        class TraitBonus
        {
            public Trait trait;
            public Bonuses[] bonuses;
        }

        [System.Serializable]
        class Bonuses
        {
            public Stat stat;
            public float additiveBonusPerPoint = 0;
            public float percentageBonusPerPoint = 0;
        }

        private void Awake()
        {
            additiveBonusCash = new Dictionary<Stat, Dictionary<Trait, float>>();
            percentageBonusCash = new Dictionary<Stat, Dictionary<Trait, float>>();

            foreach (TraitBonus traitBonus in bonusConfig)
            {
                foreach (Bonuses bonus in traitBonus.bonuses)
                {
                    if (!additiveBonusCash.ContainsKey(bonus.stat))
                    {
                        additiveBonusCash[bonus.stat] = new Dictionary<Trait, float>();
                    }

                    if (!percentageBonusCash.ContainsKey(bonus.stat))
                    {
                        percentageBonusCash[bonus.stat] = new Dictionary<Trait, float>();
                    }

                    additiveBonusCash[bonus.stat][traitBonus.trait] = bonus.additiveBonusPerPoint;
                    percentageBonusCash[bonus.stat][traitBonus.trait] = bonus.percentageBonusPerPoint;
                }
            }
        }

        public int GetProposedPoints(Trait trait)
        {
            return GetPoints(trait) + GetStagedPoints(trait);
        }

        public int GetPoints(Trait trait)
        {
            return assignedPoints.ContainsKey(trait) ? assignedPoints[trait] : 0;
        }

        public int GetStagedPoints(Trait trait)
        {
            return stagedPoints.ContainsKey(trait) ? stagedPoints[trait] : 0;
        }

        public void AssignPoint(Trait trait, int points)
        {
            if (!CanAssignPoints(trait, points)) return;

            stagedPoints[trait] = GetStagedPoints(trait) + points;
        }

        public bool CanAssignPoints(Trait trait, int points)
        {
            if (GetStagedPoints(trait) + points < 0) return false;
            if (GetUnassignedPoints() < points) return false;

            return true;
        }

        public int GetUnassignedPoints()
        {
            return GetAssignablePoints() - GetTotalProposedPoints();
        }

        public int GetTotalProposedPoints()
        {
            int total = 0;
            foreach (int points in assignedPoints.Values)
            {
                total += points;
            }

            foreach (int points in stagedPoints.Values)
            {
                total += points;
            }

            return total;
        }

        public void Commit()
        {
            foreach (Trait trait in stagedPoints.Keys)
            {
                assignedPoints[trait] = GetProposedPoints(trait);
            }

            stagedPoints.Clear();
        }

        public int GetAssignablePoints()
        {
            return (int)GetComponent<BaseStats>().GetStat(Stat.TotalTraitPoints);
        }

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if (!additiveBonusCash.ContainsKey(stat)) yield break;

            foreach (Trait trait in additiveBonusCash[stat].Keys)
            {
                float bonus = additiveBonusCash[stat][trait];
                yield return bonus * GetPoints(trait);
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (!percentageBonusCash.ContainsKey(stat)) yield break;

            foreach (Trait trait in percentageBonusCash[stat].Keys)
            {
                float bonus = percentageBonusCash[stat][trait];
                yield return bonus * GetPoints(trait);
            }
        }

        public object CaptureState()
        {
            return assignedPoints;
        }

        public void RestoreState(object state)
        {
            assignedPoints = new Dictionary<Trait, int>((IDictionary<Trait, int>)state);
        }

        public bool? Evaluate(EPredicate predicate, string[] parameters)
        {
            if (predicate == EPredicate.MinimumTrait)
            {
                if (Enum.TryParse<Trait>(parameters[0], out Trait trait))
                {
                    return GetPoints(trait) >= Int32.Parse(parameters[1]);
                }
            }
            return null;
        }
    }
}
