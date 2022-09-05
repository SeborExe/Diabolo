using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Abilities;
using RPG.Control;
using System;

namespace RPG.Abilities.Targeting
{
    [CreateAssetMenu(fileName = "DelayedClickStrategy_", menuName = "Abilities/Targeting/Delayed Targeting")]
    public class DelayedClickTargeting : TargetingStrategy
    {
        [SerializeField] Texture2D cursorTexture;
        private Vector2 cursorHotSpot;
        [SerializeField] LayerMask layerMask;
        [SerializeField] float areaAffectRadius;
        [SerializeField] Transform targetingPrefab;

        Transform targetingPrefabInstance = null;

        public override void StartTargeting(AbilityData data, Action finished)
        {
            PlayerController playerController = data.GetUser().GetComponent<PlayerController>();
            playerController.StartCoroutine(Targeting(data, playerController, finished));
        }

        private IEnumerator Targeting(AbilityData data, PlayerController playerController, Action finished)
        {
            playerController.enabled = false;

            if (targetingPrefabInstance == null)
            {
                targetingPrefabInstance = Instantiate(targetingPrefab);
            }
            else
            {
                targetingPrefabInstance.gameObject.SetActive(true);
            }

            targetingPrefabInstance.localScale = new Vector3(areaAffectRadius * 2f, 1f, areaAffectRadius * 2f);

            while (true)
            {
                Cursor.SetCursor(cursorTexture, cursorHotSpot, CursorMode.Auto);

                RaycastHit raycasyHit;
                if (Physics.Raycast(PlayerController.GetMouseRay(), out raycasyHit, 1000, layerMask))
                {
                    targetingPrefabInstance.position = raycasyHit.point;

                    if (Input.GetMouseButton(0))
                    {
                        yield return new WaitWhile(() => Input.GetMouseButton(0));
                        playerController.enabled = true;
                        targetingPrefabInstance.gameObject.SetActive(false);
                        data.SetTargetedPoint(raycasyHit.point);
                        data.SetTargets(GetGameObjectsInRadius(raycasyHit.point));
                        finished();
                        yield break;
                    }
                }

                yield return null;
            }
        }

        private IEnumerable<GameObject> GetGameObjectsInRadius(Vector3 point)
        {
            RaycastHit[] hits = Physics.SphereCastAll(point, areaAffectRadius, Vector3.up, 0);
            foreach (RaycastHit hit in hits)
            {
                yield return hit.collider.gameObject;
            }
        }
    }
}
