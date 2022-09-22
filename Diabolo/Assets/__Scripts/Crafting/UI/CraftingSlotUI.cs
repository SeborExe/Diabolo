using UnityEngine;
using RPG.UI.Inventory;

namespace RPG.Crafting.UI
{
    public class CraftingSlotUI : MonoBehaviour, IItemHolder
    {
        [SerializeField] InventoryItemIcon icon = null;
 
        InventoryItem item;

        public void Setup(InventoryItem item, int number)
        {
            // Store the item parameter value in the local item variable.
            this.item = item;
 
            // Set both the item image and the amount number on the UI.
            icon.SetItem(item, number);
        }

        public InventoryItem GetItem()
        {
            // Return the set value in the item variable for the tooltip system
            return item;
        }
    }
}