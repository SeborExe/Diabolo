using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Abilities.Targeting
{
    [CreateAssetMenu(fileName = "SelfTargeting_", menuName = "Abilities/Targeting/Self Targeting")]
    public class SelfTargeting : TargetingStrategy
    {
        public override void StartTargeting(AbilityData data, Action finished)
        {
            data.SetTargets(new GameObject[]{ data.GetUser()});
            data.SetTargetedPoint(data.GetUser().transform.position);
            finished();
        }
    }
}
