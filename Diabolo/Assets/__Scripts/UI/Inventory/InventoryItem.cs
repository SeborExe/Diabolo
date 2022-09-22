using RPG.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.UI.Inventory
{
    [CreateAssetMenu(menuName = ("Equipment/Inventory/Item"))]
    public abstract class InventoryItem : ScriptableObject, ISerializationCallbackReceiver
    {
        [Tooltip("Auto-generated UUID for saving/loading. Clear this field if you want to generate a new one.")]
        [SerializeField] string itemID = null;
        [Tooltip("Item name to be displayed in UI.")]
        [SerializeField] string displayName = null;
        [Tooltip("Item description to be displayed in UI.")]
        [SerializeField][TextArea] string description = null;
        [Tooltip("The UI icon to represent this item in the inventory.")]
        [SerializeField] Sprite icon = null;
        [Tooltip("The prefab that should be spawned when this item is dropped.")]
        [SerializeField] Pickup pickup = null;
        [Tooltip("If true, multiple items of this type can be stacked in the same inventory slot.")]
        [SerializeField] bool stackable = false;
        [SerializeField] float price;
        [SerializeField] ItemCategory category = ItemCategory.None;
        [SerializeField] ItemRarity rarity = ItemRarity.Common;
        [SerializeField] bool isAbility = false;

        [NonSerialized] GUIStyle contentStyle;

        static Dictionary<string, InventoryItem> itemLookupCache;

        public static InventoryItem GetFromID(string itemID)
        {
            if (itemLookupCache == null)
            {
                itemLookupCache = new Dictionary<string, InventoryItem>();
                var itemList = Resources.LoadAll<InventoryItem>("");
                foreach (var item in itemList)
                {
                    if (itemLookupCache.ContainsKey(item.itemID))
                    {
                        Debug.LogError(string.Format("Looks like there's a duplicate Item ID for objects: {0} and {1}", itemLookupCache[item.itemID], item));
                        continue;
                    }

                    itemLookupCache[item.itemID] = item;
                }
            }

            if (itemID == null || !itemLookupCache.ContainsKey(itemID)) return null;
            return itemLookupCache[itemID];
        }

        public Pickup SpawnPickup(Vector3 position, int number)
        {
            var pickup = Instantiate(this.pickup);
            pickup.transform.position = position;
            pickup.Setup(this, number);
            return pickup;
        }

        public Sprite GetIcon()
        {
            return icon;
        }

        public string GetItemID()
        {
            return itemID;
        }

        public bool IsStackable()
        {
            return stackable;
        }

        public string GetDisplayName()
        {
            return displayName;
        }

        public virtual string GetDescription()
        {
            return description;
        }

        public string GetRawDescription()
        {
            return description;
        }

        public float GetPrice()
        {
            return price;
        }

        public ItemCategory GetCategory()
        {
            return category;
        }

        public ItemRarity GetItemRarity()
        {
            return rarity;
        }

        public Pickup GetPickup()
        {
            return pickup;
        }

        public bool IsAbility()
        {
            return isAbility;
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            // Generate and save a new UUID if this is blank.
            if (string.IsNullOrWhiteSpace(itemID))
            {
                itemID = System.Guid.NewGuid().ToString();
            }
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            // Require by the ISerializationCallbackReceiver but we don't need
            // to do anything with it.
        }

#if UNITY_EDITOR

        public void SetUndo(string message)
        {
            Undo.RecordObject(this, message);
        }

        public void Dirty()
        {
            EditorUtility.SetDirty(this);
        }

        public bool FloatEquals(float value1, float value2)
        {
            return Math.Abs(value1 - value2) < .001f;
        }

        public void SetItemRarity(ItemRarity newRarity)
        {
            if (rarity == newRarity) return;
            SetUndo("Change Item Rarity");
            rarity = newRarity;
            Dirty();
        }

        #region Setters

        public void SetDisplayName(string newDisplayName)
        {
            if (newDisplayName == displayName) return;
            SetUndo("Change Display Name");
            displayName = newDisplayName;
            Dirty();
        }

        public void SetDescription(string newDescription)
        {
            if (newDescription == description) return;
            SetUndo("Change Description");
            description = newDescription;
            Dirty();
        }

        public void SetIcon(Sprite newIcon)
        {
            if (icon == newIcon) return;
            SetUndo("Change Icon");
            icon = newIcon;
            Dirty();
        }

        public void SetPickup(Pickup newPickup)
        {
            if (pickup == newPickup) return;
            SetUndo("Change Pickup");
            pickup = newPickup;
            Dirty();
        }

        public void SetItemID(string newItemID)
        {
            if (itemID == newItemID) return;
            SetUndo("Change ItemID");
            itemID = newItemID;
            Dirty();
        }

        public void SetStackable(bool newStackable)
        {
            if (stackable == newStackable) return;
            SetUndo(stackable ? "Set Not Stackable" : "Set Stackable");
            stackable = newStackable;
            Dirty();
        }

        #endregion

        bool drawInventoryItem = true;
        public GUIStyle foldoutStyle;
        public virtual void DrawCustomInspector()
        {
            contentStyle = new GUIStyle { padding = new RectOffset(15, 15, 0, 0) };

            foldoutStyle = new GUIStyle(EditorStyles.foldout);
            foldoutStyle.fontStyle = FontStyle.Bold;
            drawInventoryItem = EditorGUILayout.Foldout(drawInventoryItem, "InventoryItem Data", foldoutStyle);
            if (!drawInventoryItem) return;
            EditorGUILayout.BeginVertical(contentStyle);
            SetItemID(EditorGUILayout.TextField("ItemID (clear to reset", GetItemID()));
            SetDisplayName(EditorGUILayout.TextField("Display name", GetDisplayName()));
            SetDescription(EditorGUILayout.TextField("Description", GetRawDescription()));
            SetIcon((Sprite)EditorGUILayout.ObjectField("Icon", GetIcon(), typeof(Sprite), false));
            SetPickup((Pickup)EditorGUILayout.ObjectField("Pickup", pickup, typeof(Pickup), false));
            SetItemRarity((ItemRarity)EditorGUILayout.EnumPopup(new GUIContent("Item Rarity"), rarity));
            SetStackable(EditorGUILayout.Toggle("Stackable", IsStackable()));
            EditorGUILayout.EndVertical();
        }
#endif
    }
}
