using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        Mover mover;

        private void Awake()
        {
            mover = GetComponent<Mover>();
        }

        private void Update()
        {
            MoveToTarget();
        }

        private void MoveToTarget()
        {
            if (Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                bool hasHit = Physics.Raycast(ray, out hit);

                if (hasHit)
                {
                    mover.MoveTo(hit.point);
                }
            }
        }
    }
}
