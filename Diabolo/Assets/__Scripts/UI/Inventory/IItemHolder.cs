using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI.Inventory
{
    public interface IItemHolder
    {
        InventoryItem GetItem();
    }
}