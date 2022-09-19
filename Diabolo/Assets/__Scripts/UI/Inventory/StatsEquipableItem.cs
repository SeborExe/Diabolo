using RPG.Stats;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.UI.Inventory
{
    [CreateAssetMenu(menuName = ("Equipment/Inventory/EQ Item"))]
    public class StatsEquipableItem : EquipableItem, IModifierProvider
    {
        [SerializeField] List<Modifier> additiveModifiers = new List<Modifier>();
        [SerializeField] List<Modifier> percentageModifiers = new List<Modifier>();

        [System.Serializable]
        struct Modifier
        {
            public Stat stat;
            public float value;
        }

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            foreach (var modifier in additiveModifiers)
            {
                if (modifier.stat == stat)
                {
                    yield return modifier.value;
                }
            }
        }

        public IEnumerable<string> GetAdditiveModifiers()
        {
            foreach (var modifier in additiveModifiers)
            {
                yield return $"{modifier.stat}: +{modifier.value}";
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            foreach (var modifier in percentageModifiers)
            {
                if (modifier.stat == stat)
                {
                    yield return modifier.value;
                }
            }
        }

        public IEnumerable<string> GetPercentageModifiers()
        {
            foreach (var modifier in percentageModifiers)
            {
                yield return $"{modifier.stat}: +{modifier.value}%";
            }
        }

        public string AdditiveModifier()
        {
            for (int i = 0; i < additiveModifiers.Count; i++)
            {
                return $"Increase {additiveModifiers[i].stat} : {additiveModifiers[i].value} \n";
            }

            return null;
        }

        public string PercentageModifier()
        {
            for (int i = 0; i < percentageModifiers.Count; i++)
            {
                return $"Increase {percentageModifiers[i].stat} : {percentageModifiers[i].value}% \n";
            }

            return null;
        }

        string FormatAttribute(Modifier mod, bool percent)
        {
            if ((int)mod.value == 0.0f) return "";
            string percentString = percent ? "percent" : "point";
            string bonus = mod.value > 0.0f ? "<color=#8888ff>bonus</color>" : "<color=#ff8888>penalty</color>";
            return $"{Mathf.Abs((int)mod.value)} {percentString} {bonus} to {mod.stat}\n";
        }

        public override string GetDescription()
        {
            string result = GetRawDescription() + "\n\n";
            foreach (Modifier mod in additiveModifiers)
            {
                result += FormatAttribute(mod, false);
            }

            foreach (Modifier mod in percentageModifiers)
            {
                result += FormatAttribute(mod, true);
            }
            return result;
        }

#if UNITY_EDITOR

        void AddModifier(List<Modifier> modifierList)
        {
            SetUndo("Add Modifier");
            modifierList?.Add(new Modifier());
            Dirty();
        }

        void RemoveModifier(List<Modifier> modifierList, int index)
        {
            SetUndo("Remove Modifier");
            modifierList?.RemoveAt(index);
            Dirty();
        }

        void SetStat(List<Modifier> modifierList, int i, Stat stat)
        {
            if (modifierList[i].stat == stat) return;
            SetUndo("Change Modifier Stat");
            Modifier mod = modifierList[i];
            mod.stat = stat;
            modifierList[i] = mod;
            Dirty();
        }

        private void SetValue(List<Modifier> modifierList, int i, int v)
        {
            SetUndo("Change Modifier Value");
            Modifier mod = modifierList[i];
            mod.value = v;
            modifierList[i] = mod;
            Dirty();
        }

        void DrawModifierList(List<Modifier> modifierList)
        {
            int modifierToDelete = -1;
            GUIContent statLabel = new GUIContent("Stat");
            for (int i = 0; i < modifierList.Count; i++)
            {
                Modifier modifier = modifierList[i];
                EditorGUILayout.BeginHorizontal();
                SetStat(modifierList, i, (Stat)EditorGUILayout.EnumPopup(statLabel, modifier.stat, IsStatSelectable, false));
                SetValue(modifierList, i, EditorGUILayout.IntSlider("Value", (int)modifier.value, -20, 100));
                if (GUILayout.Button("-"))
                {
                    modifierToDelete = i;
                }

                EditorGUILayout.EndHorizontal();
            }

            if (modifierToDelete > -1)
            {
                RemoveModifier(modifierList, modifierToDelete);
            }

            if (GUILayout.Button("Add Modifier"))
            {
                AddModifier(modifierList);
            }
        }

        bool IsStatSelectable(Enum candidate)
        {
            Stat stat = (Stat)candidate;
            if (stat == Stat.ExperienceReward || stat == Stat.ExperienceToLevelUp) return false;
            return true;
        }

        bool drawStatsEquipableItemData = true;
        bool drawAdditive = true;
        bool drawPercentage = true;

        public override void DrawCustomInspector()
        {
            base.DrawCustomInspector();
            drawStatsEquipableItemData =
                EditorGUILayout.Foldout(drawStatsEquipableItemData, "StatsEquipableItemData", foldoutStyle);
            if (!drawStatsEquipableItemData) return;

            drawAdditive = EditorGUILayout.Foldout(drawAdditive, "Additive Modifiers");
            if (drawAdditive)
            {
                DrawModifierList(additiveModifiers);
            }
            drawPercentage = EditorGUILayout.Foldout(drawPercentage, "Percentage Modifiers");
            if (drawPercentage)
            {
                DrawModifierList(percentageModifiers);
            }

        }

#endif
    }
}
