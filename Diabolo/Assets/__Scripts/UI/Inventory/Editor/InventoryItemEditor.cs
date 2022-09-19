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

        GUIStyle previewStyle;
        GUIStyle descriptionStyle;
        GUIStyle headerStyle;

        void OnEnable()
        {
            previewStyle = new GUIStyle();
            previewStyle.normal.background = EditorGUIUtility.Load("Assets/Assets/UI/Fantasy RPG UI/UI/Parts/Background_06.png") as Texture2D;
            previewStyle.padding = new RectOffset(40, 40, 40, 40);
            previewStyle.border = new RectOffset(0, 0, 0, 0);
        }

        bool stylesInitialized = false;

        void OnGUI()
        {
            if (selected == null)
            {
                EditorGUILayout.HelpBox("No Item Selected", MessageType.Error);
                return;
            }
            if (!stylesInitialized)
            {
                descriptionStyle = new GUIStyle(GUI.skin.label)
                {
                    richText = true,
                    wordWrap = true,
                    stretchHeight = true,
                    fontSize = 14,
                    alignment = TextAnchor.MiddleCenter
                };
                headerStyle = new GUIStyle(descriptionStyle) { fontSize = 24 };
                stylesInitialized = true;
            }
            Rect rect = new Rect(0, 0, position.width * .65f, position.height);
            DrawInspector(rect);
            rect.x = rect.width;
            rect.width /= 2.0f;

            DrawPreviewTooltip(rect);
        }

        Vector2 scrollPosition;
        void DrawInspector(Rect rect)
        {
            GUILayout.BeginArea(rect);
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);
            selected.DrawCustomInspector();
            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }

        void DrawPreviewTooltip(Rect rect)
        {
            GUILayout.BeginArea(rect, previewStyle);
            if (selected.GetIcon() != null)
            {
                float iconSize = Mathf.Min(rect.width * .33f, rect.height * .33f);
                Rect texRect = GUILayoutUtility.GetRect(iconSize, iconSize);
                GUI.DrawTexture(texRect, selected.GetIcon().texture, ScaleMode.ScaleToFit);
            }

            EditorGUILayout.LabelField(selected.GetDisplayName(), headerStyle);
            EditorGUILayout.LabelField(selected.GetDescription(), descriptionStyle);
            GUILayout.EndArea();
        }
    }
}
