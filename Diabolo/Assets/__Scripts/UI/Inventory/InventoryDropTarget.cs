using RPG.UI.Inventory.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI.Inventory
{
    public class InventoryDropTarget : MonoBehaviour, IDragDestination<InventoryItem>
    {
        public void AddItems(InventoryItem item, int number)
        {
            var player = GameManager.Instance.GetPlayer();
            player.GetComponent<ItemDropper>().DropItem(item, number);
        }

        public int MaxAcceptable(InventoryItem item)
        {
            return int.MaxValue;
        }
    }
}
