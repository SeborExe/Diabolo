using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestGiver : MonoBehaviour
    {
        [SerializeField] QuestSO quest;

        public void GiveQuest()
        {
            QuestList questList = GameManager.Instance.GetPlayer().GetComponent<QuestList>();
            questList.AddQuest(quest);
        }
    }
}
