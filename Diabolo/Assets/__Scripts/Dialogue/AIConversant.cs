using RPG.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Attributes;
using RPG.Stats;

namespace RPG.Dialogue
{
    public class AIConversant : MonoBehaviour, IRaycastable
    {
        [SerializeField] DialogueSO dialogue = null;
        [SerializeField] float distanceToTalk = 2f;

        public CursorType GetCursorType()
        {
            return CursorType.Dialogue;
        }
        
        public bool HandleRaycast(PlayerController callingController)
        {
            if (dialogue == null) return false;

            Health health = GetComponent<Health>();
            if (health && health.IsDead()) return false;

            if (Input.GetMouseButtonDown(0))
            {
                callingController.GetComponent<Mover>().MoveTo(transform.position + Vector3.one, 1f);

                if (Vector3.Distance(transform.position, callingController.transform.position) < distanceToTalk)
                {
                    callingController.GetComponent<PlayerConversant>().StartDialogue(this ,dialogue);
                }
            }

            return true;
        }

        public string GetNPCName()
        {
            return GetComponent<BaseStats>().GetCharacterName();
        }
    }
}
