using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Dialogue;
using TMPro;
using UnityEngine.UI;

namespace RPG.UI.Dialogue
{
    public class DialogueUI : MonoBehaviour
    {
        PlayerConversant playerConversant;
        [SerializeField] TMP_Text AIText;
        [SerializeField] Button nextButton;
        [SerializeField] Button quitButton;
        [SerializeField] GameObject AIResponse;
        [SerializeField] Transform choiseRootTransform;
        [SerializeField] GameObject choisePrefab;

        private void Start()
        {
            playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
            playerConversant.OnConversationUpdated += UpdateUI;
            nextButton.onClick.AddListener(playerConversant.Next);
            quitButton.onClick.AddListener(playerConversant.Quit);

            UpdateUI();
        }

        private void UpdateUI()
        {
            gameObject.SetActive(playerConversant.IsActive());

            if (!playerConversant.IsActive()) return;
 
            AIResponse.SetActive(!playerConversant.IsChoosing());
            choiseRootTransform.gameObject.SetActive(playerConversant.IsChoosing());

            if (playerConversant.IsChoosing())
            {
                BuildChoiseList();
            }
            else
            {
                AIText.text = playerConversant.GetText();
                nextButton.gameObject.SetActive(playerConversant.HasNext());
            }
        }

        private void BuildChoiseList()
        {
            foreach (Transform child in choiseRootTransform)
            {
                Destroy(child.gameObject);
            }

            foreach (DialogueNode choise in playerConversant.GetChoices())
            {
                GameObject choiseInstance = Instantiate(choisePrefab, choiseRootTransform);
                var textComponent = choiseInstance.GetComponentInChildren<TMP_Text>();
                textComponent.text = choise.GetText();

                Button button = choiseInstance.GetComponentInChildren<Button>();
                button.onClick.AddListener(() => { playerConversant.SelectChoices(choise); });
            }
        }
    }
}