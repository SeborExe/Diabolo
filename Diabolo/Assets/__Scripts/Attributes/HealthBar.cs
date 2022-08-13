using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using RPG.Stats;

namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        Slider slider;
        Canvas rootCanvas;
        [SerializeField] TMP_Text levelText;
        [SerializeField] BaseStats baseStats;

        private void Awake()
        {
            slider = GetComponent<Slider>();
            rootCanvas = GetComponent<Canvas>();
        }

        private void Start()
        {
            levelText.text = baseStats.CurrentLevel.ToString();
            rootCanvas.enabled = false;
        }

        public void UpdateHealthBar(float currentHealth, float maxHealth)
        {
            slider.value = (currentHealth / maxHealth);

            if (currentHealth != maxHealth)
            {
                rootCanvas.enabled = true;
            }

            if (Mathf.Approximately(currentHealth, 0))
            {
                rootCanvas.enabled = false;
            }
        }
    }

}