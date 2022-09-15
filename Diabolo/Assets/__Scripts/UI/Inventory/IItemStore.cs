namespace RPG.UI.Inventory
{
    public interface IItemStore
    {
        int AddItems(InventoryItem item, int number);
    }
}