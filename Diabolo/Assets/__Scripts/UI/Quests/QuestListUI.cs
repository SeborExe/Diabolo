using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Quests;

namespace RPG.UI.Quests
{
    public class QuestListUI : MonoBehaviour
    {
        [SerializeField] QuestItemUI questPrefab;
        /*
        QuestList questList;

        private void Start()
        {
            questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
            questList.OnUpdate += Redraw;
            Redraw();
        }

        private void Redraw()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            foreach (QuestStatus status in questList.GetStatuses())
            {
                QuestItemUI questInstance = Instantiate<QuestItemUI>(questPrefab, transform);
                questInstance.Setup(status);
            }
        }
        */
    }
}
