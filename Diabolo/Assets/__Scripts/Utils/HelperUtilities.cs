using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Utils
{
    public static class HelperUtilities
    {
        public static Color GetItemColorNameBasedOnRarity(ItemRarity rarity)
        {
            switch (rarity)
            {
                case ItemRarity.Common:
                    return Color.black;

                case ItemRarity.Rare:
                    return Color.blue;

                case ItemRarity.Unique:
                    return Color.green;

                case ItemRarity.Epic:
                    return Color.magenta;

                case ItemRarity.Legendary:
                    return Color.yellow;

                default:
                    return Color.black;
            }
        }
    }

}