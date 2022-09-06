using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Abilities;
using System;

namespace RPG.Abilities.Effects
{
    [CreateAssetMenu(fileName = "SpawnTargetEffect_", menuName = "Abilities/Effects/Spawn Efect Prefab")]
    public class SpawnTargetPrefabEffect : EffectStrategy
    {
        [SerializeField] Transform effectPrefab;
        [SerializeField] float destroyDelay = -1;

        public override void StartEffect(AbilityData data, Action finished)
        {
            data.StartCoroutine(Effect(data, finished));
        }

        private IEnumerator Effect(AbilityData data, Action finished)
        {
            yield return new WaitForSeconds(data.GetTimeToCast());

            Transform instance = Instantiate(effectPrefab);
            instance.position = data.GetTargetedPoint();
            if (destroyDelay > 0)
            {
                yield return new WaitForSeconds(destroyDelay);
                Destroy(instance.gameObject);
            }

            finished();
        }
    }

}