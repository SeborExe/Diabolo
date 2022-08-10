using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        Slider slider;
        Canvas rootCanvas;

        private void Awake()
        {
            slider = GetComponent<Slider>();
            rootCanvas = GetComponent<Canvas>();
        }

        private void Start()
        {
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