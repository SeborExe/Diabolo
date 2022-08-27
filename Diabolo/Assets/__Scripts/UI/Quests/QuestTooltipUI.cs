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

        public void Setup(QuestSO quest)
        {
            //QuestSO quest = status.GetQuest();
            title.text = quest.GetTitle();

            foreach (Transform child in objectivesContainer)
            {
                Destroy(child.gameObject);
            }

            foreach (string objective in quest.GetObjectives())
            {
                GameObject prefab = objectiveIncompletePrefab;

                //if (status.IsObjectiveComplete(objective.references))
                //{
                //    prefab = objectivePrefab;
                //}

                GameObject objectiveInstance = Instantiate(prefab, objectivesContainer);
                TMP_Text objectiveText = objectiveInstance.GetComponentInChildren<TMP_Text>();
                objectiveText.text = objective;
            }

            //rewardText.text = GetRewardText(quest);
        }
    }
}
