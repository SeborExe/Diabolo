using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RPG.Attributes
{
    public class HealthDisplay : MonoBehaviour
    {
        [SerializeField]
        Health health;

        private void Update()
        {
            GetComponent<TMP_Text>().text = String.Format("{0:0}%", health.GetPercentage());
        }
    }
}
