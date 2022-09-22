using UnityEngine;
using RPG.UI.Inventory;

namespace RPG.Crafting
{
    public class Craft : MonoBehaviour
    {
        public void CraftItem(Inventory inventory, InventoryItem inventoryItem, CraftingRecipe.Recipes recipe)
        {
            // Check if the player has the ingredients in the inventory.
            if(HasIngredients(inventory, recipe))
            {
                // If true
                // Rremove the items from the inventory.
                RemoveItems(inventory, recipe);
                // Add the recipe result item to the first empty slot in the inventory.
                inventory.AddToFirstEmptySlot(inventoryItem, 1);
            }
        }

        private void RemoveItems(Inventory inventory, CraftingRecipe.Recipes recipe)
        {
            // Iterate through all of the ingredients in the specified recipe in the parameter.
            foreach (CraftingRecipe.Ingredients ingredient in recipe.ingredients)
            {
                // Check if the current ingredient iteration's item is stackable.
                if (ingredient.item.IsStackable())
                {
                    // If the item is stackable, get the slot index number the item is found in.
                    // Assign the slot index number in a variable.
                    int itemSlot = inventory.GetItemSlot(ingredient.item, ingredient.number);
                    // Remove the items from the player's inventory slot with the amount number.
                    inventory.RemoveFromSlot(itemSlot, ingredient.number);
                }
                else
                {
                    // If the item is NOT stackable, loop through the current foreach loop iteration's ingredient amount number.
                    for (int i = 0; i < ingredient.number; i++)
                    {
                        // Get the slot index number that the item is found in.
                        // Because we know that the item is not stackable, we can simply say that the amount is 1.
                        int itemSlot = inventory.GetItemSlot(ingredient.item, 1);
                        // Remove the items from the player's inventory slot.
                        inventory.RemoveFromSlot(itemSlot, 1);
                    }
                }
            }
        }

        private bool HasIngredients(Inventory inventory, CraftingRecipe.Recipes recipe)
        {
            // Create boolean to store result.
            bool hasItem = false;
            // Iterate through all of the ingredients in the specified recipe in the parameter.
            foreach(CraftingRecipe.Ingredients ingredient in recipe.ingredients)
            {
                // Check if the current ingredient iteration's item is stackable.
                if(ingredient.item.IsStackable())
                {
                    // If the item is stackable, get the inventory slot index number that the stack is in.
                    // We check if the returned value equals to or is greater than 0, because GetItemSlot returns -1 by default if no slot is found.
                    // Assign the if statement value to the boolean variable.
                    hasItem = inventory.GetItemSlot(ingredient.item, ingredient.number) >= 0;
                }
                else
                {
                    // If the item is NOT Stackable, check if the required item is in the player's inventory.
                    // Assign the if statement value to the boolean variable.
                    hasItem = inventory.HasItem(ingredient.item, ingredient.number);
                }
                // If the assigned value is false, the method will return false.
                if(!hasItem) return false;
            }
            // The method will return true by default.
            return true;
        }
    }
}