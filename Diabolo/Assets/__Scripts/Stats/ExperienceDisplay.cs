using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RPG.Stats
{
    public class ExperienceDisplay : MonoBehaviour
    {
        Experience experience;

        private void Start()
        {
            experience = GameManager.Instance.GetPlayer().GetComponent<Experience>();
        }

        private void Update()
        {
            GetComponent<TMP_Text>().text = String.Format("{0:0}", experience.GetExp());
        }
    }
}
