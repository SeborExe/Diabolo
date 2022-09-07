using RPG.Control;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Abilities.Targeting
{
    [CreateAssetMenu(fileName = "DirectionalTargeting_", menuName = "Abilities/Targeting/Directional Targeting")]
    public class DirectionalTargeting : TargetingStrategy
    {
        [SerializeField] LayerMask layerMask;
        [SerializeField] float groundOffset = 1f;

        public override void StartTargeting(AbilityData data, Action finished)
        {
            RaycastHit raycasyHit;
            Ray ray = PlayerController.GetMouseRay();
            if (Physics.Raycast(ray, out raycasyHit, 1000, layerMask))
            {
                data.SetTargetedPoint(raycasyHit.point + ray.direction * groundOffset / ray.direction.y);
            }

            finished();
        }
    }
}


