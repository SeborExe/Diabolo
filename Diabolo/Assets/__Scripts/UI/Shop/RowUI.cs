using RPG.Shops;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Shops
{
    public class RowUI : MonoBehaviour
    {
        [SerializeField] Image itemIcon;
        [SerializeField] TMP_Text itemName;
        [SerializeField] TMP_Text itemAvailability;
        [SerializeField] TMP_Text itemPrice;
        [SerializeField] TMP_Text itemQuantity;
        [SerializeField] Button plusButton;
        [SerializeField] Button minusButton;

        Shop currentShop = null;
        ShopItem item = null;

        public void SetUp(Shop currentShop ,ShopItem item)
        {
            this.currentShop = currentShop;
            this.item = item;
            itemIcon.sprite = item.GetImage();
            itemName.text = item.GetName();
            itemAvailability.text = item.GetAvailability();
            itemPrice.text = $"${item.GetPrice():N2}";
            itemQuantity.text = item.GetQuantityInTransaction().ToString();
        }

        private void OnEnable()
        {
            plusButton.onClick.AddListener(Add);
            minusButton.onClick.AddListener(Remove);
        }

        private void OnDisable()
        {
            plusButton.onClick.RemoveListener(Add);
            minusButton.onClick.RemoveListener(Remove);
        }

        public void Add()
        {
            currentShop.AddToTransaction(item.GetInventoryItem(), 1);
        }

        public void Remove()
        {
            currentShop.AddToTransaction(item.GetInventoryItem(), -1);
        }
    }
}
