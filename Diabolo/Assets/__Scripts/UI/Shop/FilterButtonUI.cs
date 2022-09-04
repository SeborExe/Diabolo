using RPG.Shops;
using RPG.UI.Inventory;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Shops
{
    public class FilterButtonUI : MonoBehaviour
    {
        [SerializeField] ItemCategory category = ItemCategory.None;

        Button button;
        Shop currentShop;

        private void Awake()
        {
            button = GetComponent<Button>();
        }

        public void SetShop(Shop currentShop)
        {
            this.currentShop = currentShop;
        }

        private void OnEnable()
        {
            button.onClick.AddListener(SelectFilter);
        }

        private void OnDisable()
        {
            button.onClick.RemoveListener(SelectFilter);
        }

        public void RefreshUI()
        {
            button.interactable = currentShop.GetFilter() != category;
        }

        private void SelectFilter()
        {
            currentShop.SelectFilter(category);
        }
    }

}