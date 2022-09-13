using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI.Inventory
{
    public class ShowHideUI : MonoBehaviour
    {
        [SerializeField] KeyCode[] toggleKey;
        [SerializeField] GameObject uiContainer = null;
        [SerializeField] GameObject statsPanel = null;
        [SerializeField] bool isPauseMenu = false;

        void Start()
        {
            if (uiContainer != null)
                uiContainer.SetActive(false);

            if (statsPanel != null)
                statsPanel.SetActive(true);
        }

        void Update()
        {
            foreach (KeyCode key in toggleKey)
            {
                if (Input.GetKeyDown(key))
                {
                    Toogle();
                }
            }

            if (uiContainer.activeSelf && !isPauseMenu)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    if (uiContainer != null)
                        uiContainer.SetActive(false);

                    if (statsPanel != null)
                        statsPanel.SetActive(true);
                }
            }
        }

        public void Toogle()
        {
            if (uiContainer != null)
                uiContainer.SetActive(!uiContainer.activeSelf);

            if (statsPanel != null)
                statsPanel.SetActive(!statsPanel.activeSelf);
        }
    }
}