using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.UI.Inventory;

namespace RPG.Control
{
    [RequireComponent(typeof(Pickup))]
    public class ClickablePickUp : MonoBehaviour, IRaycastable
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

        public bool HandleRaycast(PlayerController callingController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                pickup.PickupItem();
            }
            return true;
        }
    }

}