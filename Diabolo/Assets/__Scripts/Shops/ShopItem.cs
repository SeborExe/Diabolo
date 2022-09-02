using RPG.UI.Inventory;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Shops
{
    public class ShopItem
    {
        InventoryItem item;
        int availability;
        float price;
        int quantityInTransaction;

        public ShopItem(InventoryItem item, int availability, float price, int quantityInTransaction)
        {
            this.item = item;
            this.availability = availability;
            this.price = price;
            this.quantityInTransaction = quantityInTransaction;
        }

        public string GetAvailability()
        {
            return availability.ToString();
        }

        public float GetPrice()
        {
            return price;
        }

        public Sprite GetImage()
        {
            return item.GetIcon();
        }

        public string GetName()
        {
            return item.GetDisplayName();
        }

        public InventoryItem GetInventoryItem()
        {
            return item;
        }

        public int GetQuantityInTransaction()
        {
            return quantityInTransaction;
        }
    }
}
