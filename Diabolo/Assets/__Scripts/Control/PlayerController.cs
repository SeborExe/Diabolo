using RPG.Combat;
using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        Mover mover;
        Fighter fighter;

        private void Awake()
        {
            mover = GetComponent<Mover>();
            fighter = GetComponent<Fighter>();
        }

        private void Update()
        {
            if (InteractWithCombat()) return;
            if (MoveToTarget()) return; ;
        }

        private bool MoveToTarget()
        {
             RaycastHit hit;
             bool hasHit = Physics.Raycast(GetMouseRay(), out hit);

             if (hasHit)
             {
                 if (Input.GetMouseButton(0))
                 {
                     mover.StartMoveAction(hit.point);
                 }

                 return true;
             }

            return false;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits)
            {
                CombatTarget target =  hit.transform.GetComponent<CombatTarget>();
                if (!fighter.CanAttack(target)) continue;

                if (Input.GetMouseButtonDown(0))
                {
                    fighter.Attack(target);
                }

                return true;
            }

            return false;
        }
    }
}
