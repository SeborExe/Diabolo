using UnityEngine;
using RPG.Saving;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField]
        private float experiencePoints = 0;

        //public float ExperiencePoints { get => experiencePoints; set => experiencePoints = value; }

        public void GainExperience(float exp)
        {
            experiencePoints += exp;
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
