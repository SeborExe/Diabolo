using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RPG.UI.Inventory
{
    public class ItemTooltip : MonoBehaviour
    {
        [SerializeField] TMP_Text titleText = null;
        [SerializeField] TMP_Text bodyText = null;

        public void Setup(InventoryItem item)
        {
            titleText.text = item.GetDisplayName();
            bodyText.text = item.GetDescription();
        }
    }
}
