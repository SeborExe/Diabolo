using RPG.Stats;
using RPG.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;

namespace RPG.Attributes
{
    public class Mana : MonoBehaviour, ISaveable
    {
        LazyValue<float> currentMana;

        private void Awake()
        {
            currentMana = new LazyValue<float>(GetMaxMana);
        }

        private void Update()
        {
            if (currentMana.value < GetMaxMana())
            {
                currentMana.value += Time.deltaTime * GetRegenerate();

                if (currentMana.value > GetMaxMana()) currentMana.value = GetMaxMana();
            }
        }

        public float GetMana()
        {
            return currentMana.value;
        }

        public float GetMaxMana()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Mana);
        }

        public float GetRegenerate()
        {
            return GetComponent<BaseStats>().GetStat(Stat.ManaRegeneration);
        }

        public bool UseMana(float amount)
        {
            if (amount > currentMana.value) return false;

            currentMana.value -= amount;
            return true;
        }

        public void RestoreMana(float amount)
        {
            currentMana.value = Mathf.Min(currentMana.value + amount, GetMaxMana());
        }

        public void StartRestoreManaCoroutine(float amoutToRestore, int perioid, float timeBetweenHeal)
        {
            StartCoroutine(RestoreManaRoutine(amoutToRestore, perioid, timeBetweenHeal));
        }

        private IEnumerator RestoreManaRoutine(float amoutToRestore, int perioid, float timeBetweenHeal)
        {
            for (int i = 0; i < perioid; i++)
            {
                RestoreMana(amoutToRestore);
                yield return new WaitForSeconds(timeBetweenHeal);
            }
        }

        public object CaptureState()
        {
            return currentMana.value;
        }

        public void RestoreState(object state)
        {
            currentMana.value = (float)state;
        }
    }
}
