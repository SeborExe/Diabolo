using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.UI.Inventory
{
    public class ActionItem : InventoryItem
    {
        [Tooltip("Does an instance of this item get consumed every time it's used.")]
        [SerializeField] bool consumable = false;

        [NonSerialized] GUIStyle contentStyle;

        public virtual bool Use(GameObject user)
        {
            return false;
        }

        public bool isConsumable()
        {
            return consumable;
        }

        public virtual bool CanUse(GameObject user)
        {
            return true;
        }

#if UNITY_EDITOR


        void SetIsConsumable(bool value)
        {
            if (consumable == value) return;
            SetUndo(value ? "Set Consumable" : "Set Not Consumable");
            consumable = value;
            Dirty();
        }

        bool drawActionItem = true;
        public override void DrawCustomInspector()
        {
            contentStyle = new GUIStyle { padding = new RectOffset(15, 15, 0, 0) };

            base.DrawCustomInspector();
            drawActionItem = EditorGUILayout.Foldout(drawActionItem, "Action Item Data");
            if (!drawActionItem) return;
            EditorGUILayout.BeginVertical(contentStyle);
            SetIsConsumable(EditorGUILayout.Toggle("Is Consumable", consumable));
            EditorGUILayout.EndVertical();
        }

#endif
    }
}
