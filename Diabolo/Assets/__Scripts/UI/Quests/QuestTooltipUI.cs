using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Quests;
using TMPro;

namespace RPG.UI.Quests
{
    public class QuestTooltipUI : MonoBehaviour
    {
        [SerializeField] TMP_Text title;
        [SerializeField] Transform objectivesContainer;
        [SerializeField] GameObject objectivePrefab;
        [SerializeField] GameObject objectiveIncompletePrefab;

        public void Setup(QuestStatus status)
        {
            QuestSO quest = status.GetQuest();
            title.text = quest.GetTitle();

            foreach (Transform child in objectivesContainer)
            {
                Destroy(child);
            }

            foreach (string objective in quest.GetObjectives())
            {
                GameObject prefab = status.IsObjectiveComplete(objective) ? objectivePrefab : objectiveIncompletePrefab;
                GameObject objectiveInstance = Instantiate(prefab, objectivesContainer);
                TMP_Text objectiveText = objectiveInstance.GetComponentInChildren<TMP_Text>();
                objectiveText.text = objective; 
            }
        }
    }
}
