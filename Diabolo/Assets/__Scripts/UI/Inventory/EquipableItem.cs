using RPG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.UI.Inventory
{
    public class EquipableItem : InventoryItem
    {
        [Tooltip("Where are we allowed to put this item.")]
        [SerializeField] EquipLocation allowedEquipLocation = EquipLocation.Weapon;
        [SerializeField] Condition equipCondition;

        public bool CanEquip(EquipLocation equipLocation, Equipment equipment)
        {
            if (equipLocation != allowedEquipLocation) return false;

            return equipCondition.Check(equipment.GetComponents<IPredicateEvaluator>());
        }

        public bool CanEquip(Equipment equipment)
        {
            return equipCondition.Check(equipment.GetComponents<IPredicateEvaluator>());
        }

        public EquipLocation GetAllowedEquipLocation()
        {
            return allowedEquipLocation;
        }

        public Condition GetConditions()
        {
            return equipCondition;
        }

#if UNITY_EDITOR
        public void SetAllowedEquipLocation(EquipLocation newLocation)
        {
            if (allowedEquipLocation == newLocation) return;
            SetUndo("Change Equip Location");
            allowedEquipLocation = newLocation;
            Dirty();
        }

        bool drawInventoryItem = true;
        public override void DrawCustomInspector()
        {
            base.DrawCustomInspector();

            foldoutStyle = new GUIStyle(EditorStyles.foldout);
            foldoutStyle.fontStyle = FontStyle.Bold;
            drawInventoryItem = EditorGUILayout.Foldout(drawInventoryItem, "Equipable Data", foldoutStyle);
            if (!drawInventoryItem) return;
            SetAllowedEquipLocation((EquipLocation)EditorGUILayout.EnumPopup(new GUIContent("Equip Location"), allowedEquipLocation, IsLocationSelectable, false));
        }

        public virtual bool IsLocationSelectable(Enum location)
        {
            EquipLocation candidate = (EquipLocation)location;
            return candidate != EquipLocation.Weapon;
        }
#endif
    }

}