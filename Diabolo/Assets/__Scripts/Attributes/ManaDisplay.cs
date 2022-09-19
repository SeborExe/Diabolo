using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RPG.Attributes
{
    public class ManaDisplay : MonoBehaviour
    {
        Mana mana;

        private void Start()
        {
            mana = GameManager.Instance.GetPlayer().GetComponent<Mana>();
        }

        private void Update()
        {
            GetComponent<TMP_Text>().text = String.Format("{0:0}/{1:0}", mana.GetMana(), mana.GetMaxMana());
        }
    }
}
