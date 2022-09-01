using RPG.Control;
using RPG.UI.Inventory;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Shops
{
    public class Shop : MonoBehaviour, IRaycastable
    {
        public class ShopItem
        {
            InventoryItem item;
            int availability;
            float price;
            int quantityInTransaction;
        }

        public event Action OnChange;

        public IEnumerable<ShopItem> GetFilteredItems() { return null; }

        public void SelectFilter(ItemCategory category) { }

        public ItemCategory GetFilter() { return ItemCategory.None; }

        public void SelectMode(bool isBuying) { }

        public bool IsBuyingMode() { return true; }

        public bool CanTransact() { return true; }

        public void ConfirmTransation() { }

        public float TransactionTotal() { return 0; }

        public void AddToTransaction(InventoryItem item, int quantity) { }

        public bool HandleRaycast(PlayerController callingController)
        {
            callingController.GetComponent<Shopper>().SetActiveShop(this);

            return true;
        }

        public void OpenShop()
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Shopper>().SetActiveShop(this);
        }

        public CursorType GetCursorType()
        {
            return CursorType.Dialogue;
        }
    }
}
