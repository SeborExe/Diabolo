using RPG.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.MainMenu
{
    public class SaveLoadUI : MonoBehaviour
    {
        [SerializeField] Transform contentRoot;
        [SerializeField] GameObject saveButtonPrefab;

        private void OnEnable()
        {
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
            if (savingWrapper == null) return;

            foreach (Transform child in contentRoot)
            {
                Destroy(child.gameObject);
            }

            foreach (string save in savingWrapper.ListSaves())
            {
                GameObject saveInstance = Instantiate(saveButtonPrefab, contentRoot);
                TMP_Text saveName = saveInstance.GetComponentInChildren<TMP_Text>();
                saveName.text = save;
                Button button = saveInstance.GetComponentInChildren<Button>();
                button.onClick.AddListener(() => savingWrapper.LoadGame(save));
            }
        }
    }

}