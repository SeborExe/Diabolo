using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Quests;
using TMPro;

namespace RPG.UI.Quests
{
    public class QuestItemUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text title;
        [SerializeField] private TMP_Text progress;

        QuestSO quest;

        public void Setup(QuestSO quest)
        {
            this.quest = quest;
            title.text = quest.GetTitle();
            progress.text = $"0/{quest.GetObjectivesCount()}";
        }

        public QuestSO GetQuest()
        {
            return quest;
        }
    }
}
