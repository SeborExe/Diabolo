using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Abilities.Effects
{
    [CreateAssetMenu(fileName = "DelayEffect_", menuName = "Abilities/Effects/Delay Effect")]
    public class DelayCompositeEffect : EffectStrategy
    {
        [SerializeField] float delay = 0.5f;
        [SerializeField] EffectStrategy[] effectsToDelay;
        [SerializeField] bool abortIfCancelled = false;

        public override void StartEffect(AbilityData data, Action finished)
        {
            data.StartCoroutine(DeleyedEffect(data, finished));
        }

        private IEnumerator DeleyedEffect(AbilityData data, Action finished)
        {
            yield return new WaitForSeconds(delay);

            if (abortIfCancelled && data.IsCancelled()) yield break;

            foreach (EffectStrategy effect in effectsToDelay)
            {
                effect.StartEffect(data, finished);
            }
        }
    }
}
