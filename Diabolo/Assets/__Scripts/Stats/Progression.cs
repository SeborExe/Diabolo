using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression")]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] characterClasses;

        Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookUpTable = null;

        public float GetStat(Stat stat, CharacterClass characterClass, int level)
        {
            BuildLookUp();

            float[] levels = lookUpTable[characterClass][stat];

            if (levels.Length < level)
            {
                return 0;
            }

            return levels[level - 1];
        }

        public int GetLevels(Stat stat, CharacterClass characterClass)
        {
            BuildLookUp();

            float[] levels = lookUpTable[characterClass][stat];
            return levels.Length;
        }

        private void BuildLookUp()
        {
            if (lookUpTable != null) { return; }

            lookUpTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();

            foreach (ProgressionCharacterClass progressionClass in characterClasses)
            {
                var statLookUpTable = new Dictionary<Stat, float[]>();

                foreach (ProgressionStats progressionStats in progressionClass.stats)
                {
                    statLookUpTable[progressionStats.stat] = progressionStats.levels;
                }

                lookUpTable[progressionClass.characterClass] = statLookUpTable;
            }
        }

        [System.Serializable]
        class ProgressionCharacterClass
        {
            public CharacterClass characterClass;
            public ProgressionStats[] stats; 
        }

        [System.Serializable]
        class ProgressionStats
        { 
            public Stat stat;
            public float[] levels;
        }
    }
}
