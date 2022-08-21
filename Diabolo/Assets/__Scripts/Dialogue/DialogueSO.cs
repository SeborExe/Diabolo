using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Dialogue
{
    [CreateAssetMenu(fileName = "Dialogue_", menuName = "Dialogue/Create Dialogue")]
    public class DialogueSO : ScriptableObject
    {
        [SerializeField] List<DialogueNode> nodes = new List<DialogueNode>();

        public IEnumerable<DialogueNode> GetAllNodes()
        {
            return nodes;
        }

        public DialogueNode GetRootNode()
        {
            return nodes[0];
        }

#if UNITY_EDITOR

        private void Awake()
        {
            if (nodes.Count == 0)
            {
                nodes.Add(new DialogueNode());
            }
        }
    }

#endif
}
