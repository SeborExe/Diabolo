using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RPG.Stats
{
    public class ExperienceDisplay : MonoBehaviour
    {
        [SerializeField]
        Experience experience;

        private void Update()
        {
            GetComponent<TMP_Text>().text = String.Format("{0:0}", experience.GetExp());
        }
    }
}
