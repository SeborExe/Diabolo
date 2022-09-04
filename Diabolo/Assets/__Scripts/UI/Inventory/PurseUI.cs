using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RPG.UI.Inventory
{
    public class PurseUI : MonoBehaviour
    {
        [SerializeField] TMP_Text balanceText;

        Purse playerPurse = null;
            
        private void Awake()
        {
            playerPurse = GameObject.FindGameObjectWithTag("Player").GetComponent<Purse>();
            RefreshUI();
        }

        private void OnEnable()
        {
            RefreshUI();
            playerPurse.OnChange += RefreshUI;
        }

        private void OnDisable()
        {
            playerPurse.OnChange -= RefreshUI;
        }

        private void RefreshUI()
        {
            balanceText.text = $"${playerPurse.GetBalance():N2}";
        }
    }
}
