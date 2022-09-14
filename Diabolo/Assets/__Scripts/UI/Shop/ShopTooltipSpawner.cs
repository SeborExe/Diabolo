using RPG.UI.Inventory;
using RPG.UI.Shops;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Shops
{
    public class ShopTooltipSpawner : TooltipSpawner
    {
        [SerializeField] RowUI rowUI;

        public override bool CanCreateTooltip()
        {
            return true;
        }

        public override void UpdateTooltip(GameObject tooltip)
        {
            ShopItem item = rowUI.GetShopItem();
            tooltip.GetComponent<ItemTooltip>().Setup(item.GetInventoryItem());
        }
    }
}
