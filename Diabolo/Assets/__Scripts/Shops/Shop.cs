using RPG.Control;
using RPG.Stats;
using RPG.Saving;
using RPG.UI.Inventory;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Shops
{
    public class Shop : MonoBehaviour, ISaveable
    {
        [SerializeField] string shopName = "Shop";
        [SerializeField, Range(0, 100)] float sellingDiscount = 80f;

        [SerializeField] StockItemConfig[] stockConfig;

        Shopper currentShopper = null;
        ItemCategory filter = ItemCategory.None;
        bool isBuyingMode = true;

        Dictionary<InventoryItem, int> transaction = new Dictionary<InventoryItem, int>();
        Dictionary<InventoryItem, int> stockSold = new Dictionary<InventoryItem, int>();

        public event Action OnChange;

        [System.Serializable]
        class StockItemConfig
        {
            public InventoryItem item;
            public int initialStock;
            [Range(0, 100)] public float buyingDiscountPercentage;
            public int levelToUnlock = 0;
        }

        public void SetShopper(Shopper shopper)
        {
            currentShopper = shopper;
        }

        public IEnumerable<ShopItem> GetFilteredItems() 
        {
            foreach (ShopItem shopItem in GetAllItems())
            {
                InventoryItem item = shopItem.GetInventoryItem();
                if (filter == ItemCategory.None || item.GetCategory() == filter)
                {
                    yield return shopItem;
                }
            }
        }

        public IEnumerable<ShopItem> GetAllItems()
        {
            Dictionary<InventoryItem, float> prices = GetPrices();
            Dictionary<InventoryItem, int> availabilities = GetAvailabilities();

            foreach (InventoryItem item in availabilities.Keys)
            {
                if (availabilities[item] <= 0) continue;

                float price = prices[item];
                int quantityInTransaction = 0;
                transaction.TryGetValue(item, out quantityInTransaction);
                int availability = availabilities[item];
                yield return new ShopItem(item, availability, price, quantityInTransaction);
            }
        }

        private int CountItemsInInventory(InventoryItem item)
        {
            Inventory inventory = currentShopper.GetComponent<Inventory>();
            if (inventory == null) return 0;

            int total = 0;
            for (int i = 0; i < inventory.GetSize(); i++)
            {
                if (inventory.GetItemInSlot(i) == item)
                {
                    total += inventory.GetNumberInSlot(i);
                }
            }

            return total;
        }

        public void SelectFilter(ItemCategory category) 
        {
            filter = category;
            OnChange?.Invoke();
        }

        public ItemCategory GetFilter() 
        {
            return filter; 
        }

        public void SelectMode(bool isBuying) 
        {
            isBuyingMode = isBuying;
            OnChange?.Invoke();
        }

        public bool IsBuyingMode() 
        { 
            return isBuyingMode; 
        }

        public bool CanTransact() 
        {
            if (IsTransactionEmpty()) return false;
            if (!HasSufficientFund()) return false;
            if (!HasInventorySpace()) return false;

            return true;
        }

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
                    if (isBuyingMode)
                    {
                        BuyItem(shopperInventory, shopperPurse, item, price);
                    }
                    else
                    {
                        SellItem(shopperInventory, shopperPurse, item, price);
                    }

                }
            }

            OnChange?.Invoke();
        }

        private void BuyItem(Inventory shopperInventory, Purse shopperPurse, InventoryItem item, float price)
        {
            if (shopperPurse.GetBalance() < price) return;

            bool success = shopperInventory.AddToFirstEmptySlot(item, 1);
            if (success)
            {
                AddToTransaction(item, -1);

                if (!stockSold.ContainsKey(item))
                {
                    stockSold[item] = 0;
                }

                stockSold[item]++;
                shopperPurse.UpdateBalance(-price);
            }
        }

        private void SellItem(Inventory shopperInventory, Purse shopperPurse, InventoryItem item, float price)
        {
            int slot = FindFirstItemSlot(shopperInventory, item);
            if (slot == -1) return;

            AddToTransaction(item, -1);
            shopperInventory.RemoveFromSlot(slot, 1);

            if (!stockSold.ContainsKey(item))
            {
                stockSold[item] = 0;
            }

            stockSold[item]--;
            shopperPurse.UpdateBalance(price);
        }

        private int FindFirstItemSlot(Inventory shopperInventory, InventoryItem item)
        {
            for (int i = 0; i < shopperInventory.GetSize(); i++)
            {
                if (shopperInventory.GetItemInSlot(i) == item)
                {
                    return i;
                }
            }

            return -1;
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

            Dictionary<InventoryItem, int> availabilities = GetAvailabilities();
            int availability = availabilities[item];

            if (transaction[item] + quantity > availability)
            {
                transaction[item] = availability; //Set to maximum avaiable quantity
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

        public bool HasSufficientFund()
        {
            if (!isBuyingMode) return true;

            Purse purse = currentShopper.GetComponent<Purse>();
            if (purse == null) return false;

            return purse.GetBalance() >= TransactionTotal();
        }

        public bool IsTransactionEmpty()
        {
            return transaction.Count == 0;
        }

        public bool HasInventorySpace()
        {
            if (!isBuyingMode) return true;

            Inventory shopperInventory = currentShopper.GetComponent<Inventory>();
            if (shopperInventory == null) return false;

            List<InventoryItem> flatItems = new List<InventoryItem>();
            foreach (ShopItem shopItem in GetAllItems())
            {
                InventoryItem item = shopItem.GetInventoryItem();
                int quantity = shopItem.GetQuantityInTransaction();
                for (int i = 0; i < quantity; i++)
                {
                    flatItems.Add(item);
                }
            }

            return shopperInventory.HasSpaceFor(flatItems);
        }

        private int GetShopperLevel()
        {
            BaseStats stats = currentShopper.GetComponent<BaseStats>();
            if (stats == null) return 0;

            return stats.GetLevel();
        }

        private Dictionary<InventoryItem, int> GetAvailabilities()
        {
            Dictionary<InventoryItem, int> availabilities = new Dictionary<InventoryItem, int>();

            foreach (StockItemConfig config in GetAvailableConfig())
            {
                if (isBuyingMode)
                {
                    if (!availabilities.ContainsKey(config.item))
                    {
                        int soldAmount = 0;
                        stockSold.TryGetValue(config.item, out soldAmount);
                        availabilities[config.item] = -soldAmount;
                    }

                    availabilities[config.item] += config.initialStock;
                }
                else
                {
                    availabilities[config.item] = CountItemsInInventory(config.item);
                }
            }

            return availabilities;
        }

        private Dictionary<InventoryItem, float> GetPrices()
        {
            Dictionary<InventoryItem, float> prices = new Dictionary<InventoryItem, float>();

            foreach (StockItemConfig config in GetAvailableConfig())
            {
                if (isBuyingMode)
                {
                    if (!prices.ContainsKey(config.item))
                    {
                        prices[config.item] = config.item.GetPrice();
                    }

                    prices[config.item] *= (1 - (config.buyingDiscountPercentage / 100));
                }
                else
                {
                    prices[config.item] = config.item.GetPrice() * (sellingDiscount / 100);
                }
            }

            return prices;
        }

        private IEnumerable<StockItemConfig> GetAvailableConfig()
        {
            int shopperLevel = GetShopperLevel();
            foreach (StockItemConfig config in stockConfig)
            {
                if (config.levelToUnlock > shopperLevel) continue;
                yield return config;
            }
        }

        public object CaptureState()
        {
            Dictionary<string, int> saveObject = new Dictionary<string, int>();

            foreach (var pair in stockSold)
            {
                saveObject[pair.Key.GetItemID()] = pair.Value;
            }

            return saveObject;
        }

        public void RestoreState(object state)
        {
            Dictionary<string, int> saveObject = (Dictionary<string, int>)state;

            stockSold.Clear();
            foreach (var pair in saveObject)
            {
                stockSold[InventoryItem.GetFromID(pair.Key)] = pair.Value;
            }
        }
    }
}
