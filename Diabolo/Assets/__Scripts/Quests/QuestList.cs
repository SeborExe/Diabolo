using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using RPG.Core;
using RPG.UI.Inventory;

namespace RPG.Quests
{
    public class QuestList : MonoBehaviour, ISaveable, IPredicateEvaluator
    {
        List<QuestStatus> statuses = new List<QuestStatus>();

        public event Action OnUpdate;

        public void AddQuest(QuestSO quest)
        {
            if (HasQuest(quest)) return;

            QuestStatus newStatus = new QuestStatus(quest);
            statuses.Add(newStatus);
            OnUpdate?.Invoke();
        }

        public void CompleteObjective(QuestSO quest, string objective)
        {
            QuestStatus status = GetQuestStatus(quest);
            status.CompleteObjective(objective);

            if (status.IsComplete())
            {
                GiveReward(quest);
            }

            OnUpdate?.Invoke();
        }

        public IEnumerable<QuestStatus> GetStatuses()
        {
            return statuses;
        }

        public bool HasQuest(QuestSO quest)
        {
            return GetQuestStatus(quest) != null;
        }

        private QuestStatus GetQuestStatus(QuestSO quest)
        {
            foreach (QuestStatus status in statuses)
            {
                if (status.GetQuest() == quest) return status;
            }

            return null;
        }

        private void GiveReward(QuestSO quest)
        {
            foreach (QuestSO.Reward reward in quest.GetRewards())
            {
                if (!reward.item.IsStackable())
                {
                    int given = 0;

                    for (int i = 0; i < reward.number; i++)
                    {
                        bool isGiven = GetComponent<Inventory>().AddToFirstEmptySlot(reward.item, 1);
                        if (!isGiven) break;
                        given++;
                    }

                    if (given == reward.number) continue;

                    for (int i = given; i < reward.number; i++)
                    {
                        GetComponent<ItemDropper>().DropItem(reward.item, 1);
                    }
                }
                else
                {
                    bool isGiven = GetComponent<Inventory>().AddToFirstEmptySlot(reward.item, reward.number);
                    if (!isGiven)
                    {
                        for (int i = 0; i < reward.number; i++)
                        {
                            GetComponent<ItemDropper>().DropItem(reward.item, reward.number);
                        }
                    }
                }
            }
        }

        public bool? Evaluate(string predicate, string[] parameters)
        {
            switch (predicate)
            {
                case "HasQuest":
                    return HasQuest(QuestSO.GetByName(parameters[0]));

                case "CompletedQuest":
                    return GetQuestStatus(QuestSO.GetByName(parameters[0])).IsComplete();

                case "CompleteObjective":
                    return GetQuestStatus(QuestSO.GetByName(parameters[0])).IsObjectiveComplete(parameters[1]);
            }

            return null;
        }

        public object CaptureState()
        {
            List<object> state = new List<object>();
            foreach (QuestStatus status in statuses)
            {
                state.Add(status.CaptureState());
            }

            return state;
        }

        public void RestoreState(object state)
        {
            List<object> stateList = state as List<object>;
            if (stateList == null) return;

            statuses.Clear();
            foreach (object objectState in stateList)
            {
                statuses.Add(new QuestStatus(objectState));
            }
        }
    }
}
