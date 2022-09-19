using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace RPG.UI.Inventory.Editor
{
    public class InventoryItemEditor : EditorWindow
    {
        private InventoryItem selected;

        [MenuItem("Window/InventoryItem Editor")]
        public static void ShowEditorWindow()
        {
            GetWindow(typeof(InventoryItemEditor), false, "InventoryItem");
        }

        public static void ShowEditorWindow(InventoryItem candidate)
        {
            InventoryItemEditor window = GetWindow(typeof(InventoryItemEditor), false, "InventoryItem") as InventoryItemEditor;
            if (candidate)
            {
                window.OnSelectionChange();
            }
        }

        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            InventoryItem candidate = EditorUtility.InstanceIDToObject(instanceID) as InventoryItem;
            if (candidate != null)
            {
                ShowEditorWindow(candidate);
                return true;
            }

            return false;
        }

        void OnSelectionChange()
        {
            var candidate = EditorUtility.InstanceIDToObject(Selection.activeInstanceID) as InventoryItem;
            if (candidate == null) return;
            selected = candidate;
            Repaint();
        }

        void OnGUI()
        {
            if (!selected)
            {
                EditorGUILayout.HelpBox("No InventoryItem Selected", MessageType.Error);
                return;
            }

            EditorGUILayout.HelpBox($"{selected.name}/{selected.GetDisplayName()}", MessageType.Info);
            selected.DrawCustomInspector();
        }
    }
}
