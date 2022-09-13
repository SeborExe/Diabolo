using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    [DisallowMultipleComponent]
    public class QuestCompletion : MonoBehaviour
    {
        [SerializeField] QuestSO quest;
        [SerializeField] string objective;

        public void CompleteObjective()
        {
            QuestList questList = GameManager.Instance.GetPlayer().GetComponent<QuestList>();
            questList.CompleteObjective(quest, objective);
        }

        public void CompleteObjective(QuestSO questToComplete, string objectiveToComplete)
        {
            QuestList questList = GameManager.Instance.GetPlayer().GetComponent<QuestList>();
            questList.CompleteObjective(questToComplete, objectiveToComplete);
        }
    }
}
