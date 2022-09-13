using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.MainMenu
{
    public class PauseManager : MonoBehaviour
    {
        [Header("Views")]
        [SerializeField] GameObject pauseMenu;
        [SerializeField] GameObject loadGameMenu;

        [Header("Buttons")]
        [SerializeField] Button loadGameBtn;
        [SerializeField] Button backToPauseBtn;
        [SerializeField] Button quitBtn;

        private void OnEnable()
        {
            loadGameBtn.onClick.AddListener(ShowLoadGameMenu);
            quitBtn.onClick.AddListener(ShowPauseMenu);
            backToPauseBtn.onClick.AddListener(ShowPauseMenu);
        }

        private void OnDisable()
        {
            loadGameBtn.onClick.RemoveListener(ShowLoadGameMenu);
            quitBtn.onClick.RemoveListener(ShowPauseMenu);
            backToPauseBtn.onClick.RemoveListener(ShowPauseMenu);
        }

        public void ShowLoadGameMenu()
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 0;
            loadGameMenu.SetActive(true);
        }

        public void ShowPauseMenu()
        {
            loadGameMenu.SetActive(false);
            pauseMenu.SetActive(true);
        }
    }
}
