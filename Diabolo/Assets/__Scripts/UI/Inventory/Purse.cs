using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI.Inventory
{
    public class Purse : MonoBehaviour
    {
        [SerializeField] float startingBalance = 100f;

        float balance = 0;

        public event Action OnChange;

        private void Awake()
        {
            balance = startingBalance;
        }

        public float GetBalance()
        {
            return balance;
        }

        public void UpdateBalance(float amount)
        {
            balance = Mathf.Max(0, balance + amount);
            OnChange?.Invoke();
        }
    }
}
