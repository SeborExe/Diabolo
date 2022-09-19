using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        const string currentSaveFile = "currentSaveName";
        [SerializeField] float fadeInTime = 0.3f;
        [SerializeField] float fadeOutTime = 0.2f;

        public void ContinueGame()
        {
            if (!PlayerPrefs.HasKey(currentSaveFile)) return;
            if (!GetComponent<SavingSystem>().SavefileExist(GetCurrentSave())) return; 

            StartCoroutine(LoadLastScene());
        }

        public void NewGame(string saveFile)
        {
            if (String.IsNullOrEmpty(saveFile)) return;

            SetCurrentSave(saveFile);
            StartCoroutine(LoadFirstScene());
        }

        public void LoadGame(string saveName)
        {
            SetCurrentSave(saveName);
            ContinueGame();
            Time.timeScale = 1;
        }

        public void LoadMenu()
        {
            StartCoroutine(LoadMenuScene());
        }

        private void SetCurrentSave(string saveFile)
        {
            PlayerPrefs.SetString(currentSaveFile, saveFile);
        }

        private string GetCurrentSave()
        {
            return PlayerPrefs.GetString(currentSaveFile);
        }

        IEnumerator LoadLastScene()
        {
            Fader fader = GameManager.Instance.GetFader();

            yield return fader.FadeOut(fadeOutTime);
            yield return GetComponent<SavingSystem>().LoadLastScene(GetCurrentSave());
            yield return fader.FadeIn(fadeInTime);
        }

        IEnumerator LoadFirstScene()
        {
            Fader fader = GameManager.Instance.GetFader();

            yield return fader.FadeOut(fadeOutTime);
            yield return SceneManager.LoadSceneAsync(1);
            yield return fader.FadeIn(fadeInTime);
        }

        IEnumerator LoadMenuScene()
        {
            Fader fader = GameManager.Instance.GetFader();

            yield return fader.FadeOut(fadeOutTime);
            yield return SceneManager.LoadSceneAsync(0);
            yield return fader.FadeIn(fadeInTime);
        }

        public void Save()
        {
            GetComponent<SavingSystem>().Save(GetCurrentSave());
        }

        public void Load()
        {
            GetComponent<SavingSystem>().Load(GetCurrentSave());
        }

        public void Delete(string fileName)
        {
            GetComponent<SavingSystem>().Delete(fileName);
        }

        public IEnumerable<string> ListSaves()
        {
            return GetComponent<SavingSystem>().ListSaves();
        }
    }
}
