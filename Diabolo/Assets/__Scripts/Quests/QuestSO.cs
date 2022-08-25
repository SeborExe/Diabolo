using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    [CreateAssetMenu(fileName = "Quest_", menuName = "Quest/New Quest")]
    public class QuestSO : ScriptableObject
    {
        [SerializeField] List<string> objectives = new List<string>();

        public string GetTitle()
        {
            return name;
        }

        public int GetObjectivesCount()
        {
            return objectives.Count;
        }

        public IEnumerable<string> GetObjectives()
        {
            return objectives;
        }

        public bool HasObjective(string objective)
        {
            return objectives.Contains(objective);
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
