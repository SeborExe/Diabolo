using System;
using TMPro;
using UnityEngine;

namespace RPG.Stats
{
    public class LevelDisplay : MonoBehaviour
    {
        BaseStats baseStats;

        private void Start()
        {
            baseStats = GameManager.Instance.GetPlayer().GetComponent<BaseStats>();
        }

        private void Update()
        {
            GetComponent<TMP_Text>().text = String.Format("{0:0}", baseStats.GetLevel());
        }
    }
}
