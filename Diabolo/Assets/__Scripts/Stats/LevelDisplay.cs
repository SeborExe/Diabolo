using System;
using TMPro;
using UnityEngine;

namespace RPG.Stats
{
    public class LevelDisplay : MonoBehaviour
    {
        [SerializeField]
        BaseStats baseStats;

        private void Update()
        {
            GetComponent<TMP_Text>().text = String.Format("{0:0}", baseStats.GetLevel());
        }
    }
}
