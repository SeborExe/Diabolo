using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using RPG.SceneManagement;
using RPG.UI.MainMenu;

public class LoadButton : MonoBehaviour
{
    [SerializeField] Button deleteButton;

    private string saveName;

    private void Start()
    {
        saveName = GetComponentInChildren<TMP_Text>().text;
        deleteButton.onClick.AddListener(() => DeleteSave(saveName));
    }

    private void OnDisable()
    {
        deleteButton.onClick.RemoveAllListeners();
    }

    private void DeleteSave(string fileName)
    {
        SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
        savingWrapper.Delete(fileName);

        SaveLoadUI saveLoad = FindObjectOfType<SaveLoadUI>();
        saveLoad.OnMenuChanges_Invoke();
    }
}
