using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using RPG.Quests;

namespace RPG.UI.Quests
{
    public class QuestTooltipUI : MonoBehaviour
    {
        [SerializeField] TMP_Text title;
        [SerializeField] Transform objectivesContainer;
        [SerializeField] GameObject objectivePrefab;
        [SerializeField] GameObject objectiveIncompletePrefab;
        [SerializeField] TMP_Text rewardText;

        public void Setup(QuestStatus status)
        {
            QuestSO quest = status.GetQuest();
            title.text = quest.GetTitle();

            foreach (Transform child in objectivesContainer)
            {
                Destroy(child.gameObject);
            }

            foreach (QuestSO.Objective objective in quest.GetObjectives())
            {
                GameObject prefab = objectiveIncompletePrefab;

                if (status.IsObjectiveComplete(objective.references))
                {
                    prefab = objectivePrefab;
                }

                GameObject objectiveInstance = Instantiate(prefab, objectivesContainer);
                TMP_Text objectiveText = objectiveInstance.GetComponentInChildren<TMP_Text>();
                objectiveText.text = objective.description;
            }

            rewardText.text = GetRewardText(quest);
        }

        private string GetRewardText(QuestSO quest)
        {
            string rewardText = "";
            foreach (QuestSO.Reward reward in quest.GetRewards())
            {
                if (rewardText != "")
                {
                    rewardText += ", ";
                }

                if (reward.number > 1)
                {
                    rewardText += $"{reward.number}x";
                }
                rewardText += reward.item.GetDisplayName();
            }

            if (rewardText == "")
            {
                rewardText = "No reward";
            }

            rewardText += ".";
            return rewardText;
        }
    }
}
