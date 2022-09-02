using RPG.Control;
using RPG.UI.Inventory;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Shops
{
    public class Shop : MonoBehaviour
    {
        [SerializeField] string shopName = "Shop";

        [SerializeField] StockItemConfig[] stockConfig;

        Shopper currentShopper = null;

        Dictionary<InventoryItem, int> transaction = new Dictionary<InventoryItem, int>();
        Dictionary<InventoryItem, int> stock = new Dictionary<InventoryItem, int>();

        public event Action OnChange;

        [System.Serializable]
        class StockItemConfig
        {
            public InventoryItem item;
            public int initialStock;
            [Range(0, 100)] public float buyingDiscountPercentage;
        }

        private void Awake()
        {
            foreach (StockItemConfig config in stockConfig)
            {
                stock[config.item] = config.initialStock;
            }
        }

        public void SetShopper(Shopper shopper)
        {
            currentShopper = shopper;
        }

        public IEnumerable<ShopItem> GetFilteredItems() 
        {
            return GetAllItems();
        }

        public IEnumerable<ShopItem> GetAllItems()
        {
            foreach (StockItemConfig config in stockConfig)
            {
                float price = config.item.GetPrice() * (1 - (config.buyingDiscountPercentage / 100));
                int quantityInTransaction = 0;
                transaction.TryGetValue(config.item, out quantityInTransaction);
                int currentStock = stock[config.item];
                yield return new ShopItem(config.item, currentStock, price, quantityInTransaction);
            }
        }

        public void SelectFilter(ItemCategory category) { }

        public ItemCategory GetFilter() { return ItemCategory.None; }

        public void SelectMode(bool isBuying) { }

        public bool IsBuyingMode() { return true; }

        public bool CanTransact() { return true; }

        public void ConfirmTransation()
        {
            Inventory shopperInventory = currentShopper.GetComponent<Inventory>();
            Purse shopperPurse = currentShopper.GetComponent<Purse>();

            if (shopperInventory == null || shopperPurse == null) return;

            foreach (ShopItem shopItem in GetAllItems())
            {
                InventoryItem item = shopItem.GetInventoryItem();
                int quantity = shopItem.GetQuantityInTransaction();
                float price = shopItem.GetPrice();

                for (int i = 0; i < quantity; i++)
                {
                    if (shopperPurse.GetBalance() < price) break;

                    bool success = shopperInventory.AddToFirstEmptySlot(item, 1);
                    if (success)
                    {
                        AddToTransaction(item, -1);
                        stock[item]--;
                        shopperPurse.UpdateBalance(-price);
                    }
                }
            }

            OnChange?.Invoke();
        }

        public float TransactionTotal() 
        {
            float total = 0;
            foreach (ShopItem item in GetAllItems())
            {
                total += item.GetPrice() * item.GetQuantityInTransaction();
            }

            return total;
        }

        public void AddToTransaction(InventoryItem item, int quantity) 
        {
            if (!transaction.ContainsKey(item))
            {
                transaction[item] = 0;
            }

            if (transaction[item] + quantity > stock[item])
            {
                transaction[item] = stock[item]; //Set to maximum avaiable quantity
            }
            else
            {
                transaction[item] += quantity;
            }

            if (transaction[item] <= 0)
            {
                transaction.Remove(item);
            }

            OnChange?.Invoke();
        }

        public void OpenShop()
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Shopper>().SetActiveShop(this);
        }

        public string GetShopeName()
        {
            return shopName;
        }
    }
}
