using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Abilities;
using RPG.Control;
using System;

namespace RPG.Abilities.Targeting
{
    [CreateAssetMenu(fileName = "DelayedClickStrategy", menuName = "Abilities/Targeting/Delayed Targeting")]
    public class DelayedClickTargeting : TargetingStrategy
    {
        [SerializeField] Texture2D cursorTexture;
        private Vector2 cursorHotSpot;
        [SerializeField] LayerMask layerMask;
        [SerializeField] float areaAffectRadius;

        public override void StartTargeting(GameObject user, Action<IEnumerable<GameObject>> finished)
        {
            PlayerController playerController = user.GetComponent<PlayerController>();
            playerController.StartCoroutine(Targeting(user, playerController, finished));
        }

        private IEnumerator Targeting(GameObject user, PlayerController playerController, Action<IEnumerable<GameObject>> finished)
        {
            playerController.enabled = false;
            while (true)
            {
                Cursor.SetCursor(cursorTexture, cursorHotSpot, CursorMode.Auto);
                if (Input.GetMouseButton(0))
                {
                    yield return new WaitWhile(() => Input.GetMouseButton(0));
                    playerController.enabled = true;
                    finished(GetGameObjectsInRadius());
                    yield break;
                }

                yield return null;
            }
        }

        private IEnumerable<GameObject> GetGameObjectsInRadius()
        {
            RaycastHit raycasyHit;
            if (Physics.Raycast(PlayerController.GetMouseRay(), out raycasyHit, 1000, layerMask))
            {
                RaycastHit[] hits = Physics.SphereCastAll(raycasyHit.point, areaAffectRadius, Vector3.up, 0);
                foreach (RaycastHit hit in hits)
                {
                    yield return hit.collider.gameObject;
                }
            }
        }
    }
}
