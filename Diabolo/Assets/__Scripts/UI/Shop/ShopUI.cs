using RPG.Dialogue;
using RPG.Shops;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Shops
{
    public class ShopUI : MonoBehaviour
    {
        [SerializeField] TMP_Text shopName;
        [SerializeField] Button quitButton;
        [SerializeField] Button confirmBuyButton;
        [SerializeField] Button swithcButton;
        [SerializeField] Transform contentContainer;
        [SerializeField] RowUI rowPrefab;
        [SerializeField] TMP_Text totalText;

        GameObject player;
        Shopper shopper = null;
        Shop currentShop = null;
        Color originalTotalTextColor;

        private void OnEnable()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            shopper = player.GetComponent<Shopper>();
            originalTotalTextColor = totalText.color;

            quitButton.onClick.AddListener(Close);
            confirmBuyButton.onClick.AddListener(ConfirmTransaction);
            swithcButton.onClick.AddListener(SwitchMode);

            ShopChanged();

            if (shopper == null) return;

            shopper.OnActiveShopChange += ShopChanged;
        }

        private void OnDisable()
        {
            shopper.OnActiveShopChange -= ShopChanged;
            quitButton.onClick.RemoveListener(Close);
            confirmBuyButton.onClick.RemoveListener(ConfirmTransaction);
            swithcButton.onClick.RemoveListener(SwitchMode);
        }

        private void ShopChanged()
        {
            if (currentShop != null) currentShop.OnChange -= RefreshUI;

            player.GetComponent<PlayerConversant>().Quit();
            currentShop = shopper.GetActiveShop();
            gameObject.SetActive(currentShop != null);

            foreach (FilterButtonUI button in GetComponentsInChildren<FilterButtonUI>())
            {
                button.SetShop(currentShop);
            }

            if (currentShop == null) return;
            shopName.text = currentShop.GetShopeName();

            currentShop.OnChange += RefreshUI;

            RefreshUI();
        }

        private void RefreshUI()
        {
            foreach (Transform child in contentContainer)
            {
                Destroy(child.gameObject);
            }

            foreach (ShopItem item in currentShop.GetFilteredItems())
            {
                RowUI row = Instantiate(rowPrefab, contentContainer);
                row.SetUp(currentShop, item);
            }

            totalText.text = $"Total: ${currentShop.TransactionTotal():N2}";
            totalText.color = currentShop.HasSufficientFund() ? originalTotalTextColor : Color.red;
            confirmBuyButton.interactable = currentShop.CanTransact();
            TMP_Text swithcText = swithcButton.GetComponentInChildren<TMP_Text>();
            TMP_Text confirmText = confirmBuyButton.GetComponentInChildren<TMP_Text>();

            if (currentShop.IsBuyingMode())
            {
                swithcText.text = "Switch To Selling";
                confirmText.text = "Buy";
            }
            else
            {
                swithcText.text = "Switch To Buying";
                confirmText.text = "Sell";
            }

            foreach (FilterButtonUI button in GetComponentsInChildren<FilterButtonUI>())
            {
                button.RefreshUI();
            }
        }

        public void Close()
        {
            shopper.SetActiveShop(null);
        }

        public void ConfirmTransaction()
        {
            currentShop.ConfirmTransation();
        }

        private void SwitchMode()
        {
            currentShop.SelectMode(!currentShop.IsBuyingMode());
        }
    }
}
