using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Dialogue
{
    public class PlayerConversant : MonoBehaviour
    {
        DialogueSO currentDialogue;
        DialogueNode currentNode = null;

        bool isChoosing = false;

        public event Action OnConversationUpdated;

        public void StartDialogue(DialogueSO newDialogue)
        {
            currentDialogue = newDialogue;
            currentNode = currentDialogue.GetRootNode();
            TriggerEnterAction();
            OnConversationUpdated?.Invoke();
        }

        public void Quit()
        {
            currentDialogue = null;
            TriggerExitAction();
            currentNode = null;
            isChoosing = false;
            OnConversationUpdated?.Invoke();
        }

        public bool IsActive()
        {
            return currentDialogue != null;
        }

        public bool IsChoosing()
        {
            return isChoosing;
        }

        public string GetText()
        {
            if (currentNode == null) return "";

            return currentNode.GetText();
        }

        public void SelectChoices(DialogueNode chosenNode)
        {
            currentNode = chosenNode;
            TriggerEnterAction();
            isChoosing = false;
            Next();
        }

        public IEnumerable<DialogueNode> GetChoices()
        {
            return currentDialogue.GetPlayerChildren(currentNode);
        }

        public void Next()
        {
            int numPlayerResponses = currentDialogue.GetPlayerChildren(currentNode).Count();
            if (numPlayerResponses > 0)
            {
                isChoosing = true;
                TriggerExitAction();
                OnConversationUpdated?.Invoke();
                return;
            }

            DialogueNode[] children = currentDialogue.GetAIChildren(currentNode).ToArray();
            int randomIndex = UnityEngine.Random.Range(0, children.Count());
            TriggerExitAction();
            currentNode = children[randomIndex];
            TriggerEnterAction();
            OnConversationUpdated?.Invoke();
        }

        public bool HasNext()
        {
            return currentDialogue.GetAllChildren(currentNode).Count() > 0;
        }

        private void TriggerEnterAction()
        {
            if (currentNode != null && currentNode.GetOnEnterAction() != "")
            {

            }
        }

        private void TriggerExitAction()
        {
            if (currentNode != null && currentNode.GetOnExitAction() != "")
            {

            }
        }
    }
}
