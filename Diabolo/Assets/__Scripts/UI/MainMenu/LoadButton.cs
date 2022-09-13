using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using RPG.SceneManagement;
using RPG.UI.MainMenu;

public class LoadButton : MonoBehaviour
{
    [SerializeField] GameObject confirmPanel;
    [SerializeField] TMP_Text title;

    [Header("Buttons")]
    [SerializeField] Button deleteButton;
    [SerializeField] Button yesButton;
    [SerializeField] Button noButton;

    private string saveName;

    private void Start()
    {
        confirmPanel.SetActive(false);
        saveName = GetComponentInChildren<TMP_Text>().text;

        yesButton.onClick.AddListener(() => DeleteSave(saveName));
    }

    private void OnEnable()
    {
        deleteButton.onClick.AddListener(() => ShowHideConfirmPanel(true));
        noButton.onClick.AddListener(() => ShowHideConfirmPanel(false));
    }

    private void OnDisable()
    {
        yesButton.onClick.RemoveAllListeners();
        deleteButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();
    }

    private void DeleteSave(string fileName)
    {
        SavingWrapper savingWrapper = GameManager.Instance.GetSavingWrapper();
        savingWrapper.Delete(fileName);

        SaveLoadUI saveLoad = FindObjectOfType<SaveLoadUI>();
        saveLoad.OnMenuChanges_Invoke();
    }

    private void ShowHideConfirmPanel(bool show)
    {
        confirmPanel.SetActive(show);
        title.text = $"Are you sure you want to delete the record: {saveName}";
    }
}
