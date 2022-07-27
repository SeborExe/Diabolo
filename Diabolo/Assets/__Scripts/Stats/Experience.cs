using UnityEngine;
using RPG.Saving;
using System;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField]
        private float experiencePoints = 0;

        public event Action onExperienceGained;

        public void GainExperience(float exp)
        {
            experiencePoints += exp;
            onExperienceGained();
        }

        public float GetExp()
        {
            return experiencePoints;
        }

        public object CaptureState()
        {
            return experiencePoints;
        }

        public void RestoreState(object state)
        {
            experiencePoints = (float)state;
        }
    }
}
