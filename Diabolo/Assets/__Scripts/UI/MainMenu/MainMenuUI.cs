using RPG.SceneManagement;
using RPG.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.MainMenu
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] Button continueButton;
        [SerializeField] Button newGameButton;
        [SerializeField] Button loadGameButton;
        [SerializeField] Button settingsButton;
        [SerializeField] Button quitButton;

        LazyValue<SavingWrapper> savingWrapper;

        private void Awake()
        {
            savingWrapper = new LazyValue<SavingWrapper>(GetSavingWrapper);
        }

        private void OnEnable()
        {
            continueButton.onClick.AddListener(ContinueGame);
        }
            
        private void OnDisable()
        {
            continueButton.onClick.RemoveListener(ContinueGame);
        }

        private SavingWrapper GetSavingWrapper()
        {
            return FindObjectOfType<SavingWrapper>();
        }

        public void ContinueGame()
        {
            savingWrapper.value.ContinueGame();
        }
    }
}
