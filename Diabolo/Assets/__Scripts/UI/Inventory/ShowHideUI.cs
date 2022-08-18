using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI.Inventory
{
    public class ShowHideUI : MonoBehaviour
    {
        [SerializeField] KeyCode toggleKey = KeyCode.Escape;
        [SerializeField] GameObject uiContainer = null;
        [SerializeField] GameObject statsPanel = null;

        void Start()
        {
            uiContainer.SetActive(false);
            statsPanel.SetActive(true);
        }

        void Update()
        {
            if (Input.GetKeyDown(toggleKey))
            {
                uiContainer.SetActive(!uiContainer.activeSelf);
                statsPanel.SetActive(!statsPanel.activeSelf);
            }
        }
    }

}