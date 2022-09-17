using RPG.Core;
using RPG.UI.Inventory;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    [CreateAssetMenu(fileName = "Quest_", menuName = "Quest/New Quest")]
    public class QuestSO : ScriptableObject
    {
        [SerializeField] List<Objective> objectives = new List<Objective>();
        [SerializeField] List<Reward> rewards = new List<Reward>();

        [System.Serializable]
        public class Reward
        {
            [Min(1)]
            public int number;
            public InventoryItem item;
        }

        [System.Serializable]
        public class Objective
        {
            public string references;
            public string description;
            public bool usesCondition = false;
            public Condition completionCondition;
        }

        public string GetTitle()
        {
            return name;
        }

        public int GetObjectivesCount()
        {
            return objectives.Count;
        }

        public IEnumerable<Objective> GetObjectives()
        {
            return objectives;
        }

        public bool HasObjective(string objectiveRef)
        {
            foreach (Objective objective in objectives)
            {
                if (objective.references == objectiveRef)
                {
                    return true;
                }
            }

            return false;
        }

        public IEnumerable<Reward> GetRewards()
        {
            return rewards;
        }

        public static QuestSO GetByName(string questName)
        {
            foreach (QuestSO quest in Resources.LoadAll<QuestSO>(""))
            {
                if (quest.name == questName) return quest;
            }

            return null;
        }
    }
}
