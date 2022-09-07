using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Abilities.Effects
{
    [CreateAssetMenu(fileName = "PlayerPrefabEffect_", menuName = "Abilities/Effects/Player Prefab Effect")]
    public class SpawnPlayerPrefabEffect : EffectStrategy
    {
        [SerializeField] Transform effectPrefab;
        [SerializeField] float destroyDelay = -1;
        [SerializeField] Vector3 offset;

        public override void StartEffect(AbilityData data, Action finished)
        {
            data.StartCoroutine(Effect(data, finished));
        }

        private IEnumerator Effect(AbilityData data, Action finished)
        {
            Transform instance = Instantiate(effectPrefab);
            instance.position = data.GetUser().transform.position + offset;
            if (destroyDelay > 0)
            {
                yield return new WaitForSeconds(destroyDelay);
                Destroy(instance.gameObject);
            }

            finished();
        }
    }
}
