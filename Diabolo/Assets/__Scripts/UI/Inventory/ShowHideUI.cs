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

        void Start()
        {
            uiContainer.SetActive(false);
            statsPanel.SetActive(true);
        }

        void Update()
        {
            foreach (KeyCode key in toggleKey)
            {
                if (Input.GetKeyDown(key))
                {
                    uiContainer.SetActive(!uiContainer.activeSelf);
                    statsPanel.SetActive(!statsPanel.activeSelf);
                }
            }

            if (uiContainer.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    uiContainer.SetActive(false);
                    statsPanel.SetActive(true);
                }
            }
        }
    }
}