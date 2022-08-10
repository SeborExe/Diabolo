using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RPG.UI.DamageText
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] TMP_Text damageText;

        public void DestroyText()
        {
            Destroy(gameObject);
        }

        public void SetValue(float amount)
        {
            damageText.text = String.Format("{0:0}", amount);
        }
    }
}

