using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Quests;

namespace RPG.UI.Quests
{
    public class QuestListUI : MonoBehaviour
    {
        [SerializeField] QuestSO[] tempQuests;
        [SerializeField] QuestItemUI questPrefab;

        private void Start()
        {
            foreach (Transform child in transform)
            {
                Destroy(child);
            }

            foreach (QuestSO quest in tempQuests)
            {
                QuestItemUI questInstance = Instantiate<QuestItemUI>(questPrefab, transform);
                questInstance.Setup(quest);
            }
        }
    }
}
