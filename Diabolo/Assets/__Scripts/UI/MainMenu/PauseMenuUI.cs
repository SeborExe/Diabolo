using RPG.Control;
using RPG.SceneManagement;
using RPG.UI.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.MainMenu
{
    public class PauseMenuUI : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] Button closePauseMenuBtn;
        [SerializeField] Button resumeBtn;
        [SerializeField] Button saveBtn;
        [SerializeField] Button saveAndQuitBtn;

        PlayerController playerController;
        ShowHideUI showHideUI;
        SavingWrapper savingWrapper;

        private void Awake()
        {
            playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            showHideUI = GetComponentInParent<ShowHideUI>();
            savingWrapper = FindObjectOfType<SavingWrapper>();
        }

        private void OnEnable()
        {
            closePauseMenuBtn.onClick.AddListener(showHideUI.Toogle);
            resumeBtn.onClick.AddListener(showHideUI.Toogle);
            saveBtn.onClick.AddListener(Save);
            saveAndQuitBtn.onClick.AddListener(SaveAndQuit);

            Time.timeScale = 0;
            playerController.enabled = false;
        }

        private void OnDisable()
        {
            closePauseMenuBtn.onClick.RemoveListener(showHideUI.Toogle);
            resumeBtn.onClick.RemoveListener(showHideUI.Toogle);
            saveBtn.onClick.RemoveListener(Save);
            saveAndQuitBtn.onClick.RemoveListener(SaveAndQuit);

            Time.timeScale = 1;

            if (playerController != null)
                playerController.enabled = true;
        }

        public void Save()
        {
            savingWrapper.Save();
        }

        public void SaveAndQuit()
        {
            savingWrapper.Save();
            savingWrapper.LoadMenu();
        }
    }
}
