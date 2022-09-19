using RPG.Attributes;
using RPG.UI.Inventory;
using System;
using UnityEditor;
using UnityEngine;

namespace RPG.Actions
{
    [CreateAssetMenu(fileName = "New Healing Spell", menuName = "RPG/Actions/HealingSpell")]
    public class HealingSpell : ActionItem
    {
        [SerializeField] float amountToHeal;
        [SerializeField] bool isPercentage;

        [NonSerialized] GUIStyle contentStyle;

        public override bool CanUse(GameObject user)
        {
            if (!user.TryGetComponent(out Health health))
                return false;
            if (health.IsDead() || health.GetPercentage() >= 100.0f) return false;
            return true;
        }

        public override bool Use(GameObject user)
        {
            if (!user.TryGetComponent(out Health health)) return false;
            if (health.IsDead()) return false;
            health.Heal(isPercentage ? health.GetMaxHealthPoints() * amountToHeal / 100.0f : amountToHeal);
            return true;
        }

        public override string GetDescription()
        {
            string result = GetRawDescription() + "\n";
            string spell = isConsumable() ? "potion" : "spell";
            string percent = isPercentage ? "percent of your Max Health" : "Health Points.";
            result += $"This {spell} will restore {(int)amountToHeal} {percent}";
            return result;
        }

#if UNITY_EDITOR
        void SetAmountToHeal(float value)
        {
            if (FloatEquals(amountToHeal, value)) return;
            SetUndo("Change Amount To Heal");
            amountToHeal = value;
            Dirty();
        }

        void SetIsPercentage(bool value)
        {
            if (isPercentage == value) return;
            SetUndo(value ? "Set as Percentage Heal" : "Set as Absolute Heal");
            isPercentage = value;
        }

        bool drawHealingData = true;
        public override void DrawCustomInspector()
        {
            contentStyle = new GUIStyle { padding = new RectOffset(15, 15, 0, 0) };

            base.DrawCustomInspector();
            drawHealingData = EditorGUILayout.Foldout(drawHealingData, "HealingSpell Data");
            if (!drawHealingData) return;
            EditorGUILayout.BeginVertical(contentStyle);
            SetAmountToHeal(EditorGUILayout.IntSlider("Amount to Heal", (int)amountToHeal, 1, 100));
            SetIsPercentage(EditorGUILayout.Toggle("Is Percentage", isPercentage));
            EditorGUILayout.EndVertical();
        }

#endif

    }
}