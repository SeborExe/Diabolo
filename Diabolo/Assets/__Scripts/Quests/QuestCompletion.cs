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
            QuestList questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
            questList.CompleteObjective(quest, objective);
        }
    }
}
