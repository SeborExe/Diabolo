using RPG.Movement;
using RPG.UI.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    [RequireComponent(typeof(Pickup))]
    public class RunOverPickUp : MonoBehaviour, IRaycastable
    {
        Pickup pickup;

        private void Awake()
        {
            pickup = GetComponent<Pickup>();
        }

        public CursorType GetCursorType()
        {
            if (pickup.CanBePickedUp())
            {
                return CursorType.PickUp;
            }
            else
            {
                return CursorType.FullPickUp;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                GetComponent<Pickup>().PickupItem();
            }
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                callingController.GetComponent<Mover>().MoveTo(transform.position, 1f);
            }

            return true;
        }
    }
}