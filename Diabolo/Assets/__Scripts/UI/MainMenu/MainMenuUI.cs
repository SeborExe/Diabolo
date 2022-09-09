using RPG.SceneManagement;
using RPG.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RPG.UI.MainMenu
{
    public class MainMenuUI : MonoBehaviour
    {
        [Header("Views")]
        [SerializeField] GameObject mainMenu;
        [SerializeField] GameObject newGame;
        [SerializeField] GameObject loadGame;

        [Header("Buttons")]
        [SerializeField] Button continueButton;
        [SerializeField] Button newGameButton;
        [SerializeField] Button loadGameButton;
        [SerializeField] Button settingsButton;
        [SerializeField] Button quitButton;
        [SerializeField] TMP_InputField gameNameInputField;
        [SerializeField] Button[] backToMainMenuButtons;
        [SerializeField] Button startNewGameButton;

        LazyValue<SavingWrapper> savingWrapper;
        UISwitcher uISwitcher;

        private void Awake()
        {
            savingWrapper = new LazyValue<SavingWrapper>(GetSavingWrapper);
            uISwitcher = GetComponentInChildren<UISwitcher>();
        }

        private void OnEnable()
        {
            continueButton.onClick.AddListener(ContinueGame);
            newGameButton.onClick.AddListener(() => uISwitcher.SwitchTo(newGame));
            loadGameButton.onClick.AddListener(() => uISwitcher.SwitchTo(loadGame));
            startNewGameButton.onClick.AddListener(NewGame);

            foreach (Button button in backToMainMenuButtons)
            {
                button.onClick.AddListener(() => uISwitcher.SwitchTo(mainMenu));
            }
        }
            
        private void OnDisable()
        {
            continueButton.onClick.RemoveListener(ContinueGame);
            newGameButton.onClick.RemoveAllListeners();
            loadGameButton.onClick.RemoveAllListeners();
            startNewGameButton.onClick.RemoveListener(NewGame);

            foreach (Button button in backToMainMenuButtons)
            {
                button.onClick.RemoveAllListeners();
            }
        }

        private SavingWrapper GetSavingWrapper()
        {
            return FindObjectOfType<SavingWrapper>();
        }

        public void ContinueGame()
        {
            savingWrapper.value.ContinueGame();
        }

        public void NewGame()
        {
            savingWrapper.value.NewGame(gameNameInputField.text);
        }
    }
}
