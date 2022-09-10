using RPG.Attributes;
using RPG.SceneManagement;
using RPG.UI.Inventory;
using RPG.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Control
{
    public class GameOver : MonoBehaviour
    {
        [Header("Views")]
        [SerializeField] GameObject gameOverView;
        [SerializeField] GameObject loadGameView;
        [SerializeField] GameObject[] viewsToDisable;
        [SerializeField] GameObject UICanvas;

        [Header("Buttons")]
        [SerializeField] Button loadGameButton;
        [SerializeField] Button quitButton;
        [SerializeField] Button[] backToGameOverViewButton;

        LazyValue<SavingWrapper> savingWrapper;
        GameObject player;
        bool isGameOver = false;

        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<Health>().OnDie += OnPlayerDied;
            savingWrapper = new LazyValue<SavingWrapper>(GetSavingWrapper);
        }

        private void OnDisable()
        {
            if (isGameOver)
            {
                loadGameButton.onClick.RemoveListener(ShowLoadGameMenu);
                quitButton.onClick.RemoveListener(QuitGame);

                foreach (Button button in backToGameOverViewButton)
                {
                    button.onClick.RemoveListener(ShowGameOverMenu);
                }
            }
        }

        public void OnPlayerDied()
        {
            isGameOver = true;
            player.GetComponent<PlayerController>().enabled = false;
            gameOverView.SetActive(true);

            foreach (GameObject view in viewsToDisable)
            {
                view.SetActive(false);
            }

            UICanvas.GetComponent<ShowHideUI>().enabled = false;

            AddListenersToButtons();
        }

        private void AddListenersToButtons()
        {
            loadGameButton.onClick.AddListener(ShowLoadGameMenu);
            quitButton.onClick.AddListener(QuitGame);

            foreach (Button button in backToGameOverViewButton)
            {
                button.onClick.AddListener(ShowGameOverMenu);
            }
        }

        public void ShowLoadGameMenu()
        {
            gameOverView.SetActive(false);
            loadGameView.SetActive(true);
        }

        public void ShowGameOverMenu()
        {
            loadGameView.SetActive(false);
            gameOverView.SetActive(true);
        }

        public void QuitGame()
        {
            savingWrapper.value.LoadMenu();
        }

        private SavingWrapper GetSavingWrapper()
        {
            return FindObjectOfType<SavingWrapper>();
        }
    }
}
