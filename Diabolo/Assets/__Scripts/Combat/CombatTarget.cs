using RPG.Attributes;
using UnityEngine;
using RPG.Control;
using UnityEngine.EventSystems;

namespace RPG.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        [SerializeField] InfoAboveHead infoAboveHead;

        public CursorType GetCursorType()
        {
            return CursorType.Combat;
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if (!enabled) return false;

            if (!callingController.GetComponent<Fighter>().CanAttack(gameObject)) { return false; }

            if (Input.GetMouseButton(0))
            {
                callingController.GetComponent<Fighter>().Attack(gameObject);
            }

            return true;
        }

        private void OnMouseEnter()
        {
            if (infoAboveHead.isDamaged) return;
            infoAboveHead.RootCanvas.enabled = true;
        }

        private void OnMouseExit()
        {
            if (infoAboveHead.isDamaged) return;
            infoAboveHead.RootCanvas.enabled = false;
        }
    }
}
