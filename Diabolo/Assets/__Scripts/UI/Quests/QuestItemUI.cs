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

        
        QuestStatus status;

        public void Setup(QuestStatus status)
        {
            this.status = status;
            title.text = status.GetQuest().GetTitle();
            progress.text = $"{status.GetCompletedCount()}/{status.GetQuest().GetObjectivesCount()}";

            if (status.IsComplete())
            {
                title.color = Color.gray;
                progress.color = Color.gray;
            }
        }

        public QuestStatus GetQuestStatus()
        {
            return status;
        }
    }
}
