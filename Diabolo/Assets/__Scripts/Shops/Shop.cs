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

        public event Action OnChange;

        [System.Serializable]
        class StockItemConfig
        {
            public InventoryItem item;
            public int initialStock;
            [Range(0, 100)] public float buyingDiscountPercentage;
        }

        public void SetShopper(Shopper shopper)
        {
            currentShopper = shopper;
        }

        public IEnumerable<ShopItem> GetFilteredItems() 
        {
            foreach (StockItemConfig config in stockConfig)
            {
                float price = config.item.GetPrice() * (1 - (config.buyingDiscountPercentage/100));
                int quantityInTransaction = 0;
                transaction.TryGetValue(config.item, out quantityInTransaction);
                yield return new ShopItem(config.item, config.initialStock, price, quantityInTransaction);
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
            if (shopperInventory == null) return;

            Dictionary<InventoryItem, int> transactionSnapshop = new Dictionary<InventoryItem, int>(transaction);

            foreach (InventoryItem item in transactionSnapshop.Keys)
            {
                int quantity = transactionSnapshop[item];
                for (int i = 0; i < quantity; i++)
                {
                    bool success = shopperInventory.AddToFirstEmptySlot(item, 1);
                    if (success)
                    {
                        AddToTransaction(item, -1);
                    }
                }
            }
        }

        public float TransactionTotal() { return 0; }

        public void AddToTransaction(InventoryItem item, int quantity) 
        {
            if (!transaction.ContainsKey(item))
            {
                transaction[item] = 0;
            }

            transaction[item] += quantity;

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
