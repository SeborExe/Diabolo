using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Stats
{
    public class TraitRowUI : MonoBehaviour
    {
        [SerializeField] Trait trait;
        [SerializeField] TMP_Text traitText;
        [SerializeField] TMP_Text valueText;
        [SerializeField] Button minusButton;
        [SerializeField] Button plusButton;

        TraitStore traitStore;
        TraitUI traitUI;

        private void Awake()
        {
            traitStore = GameObject.FindGameObjectWithTag("Player").GetComponent<TraitStore>();
            traitUI = GetComponentInParent<TraitUI>();
        }

        private void Start()
        {
            traitUI.RefreshUI();
            traitText.text = trait.ToString();
        }

        private void OnEnable()
        {
            minusButton.onClick.AddListener(() => Allocate(-1));
            plusButton.onClick.AddListener(() => Allocate(+1));
        }

        private void OnDisable()
        {
            minusButton.onClick.RemoveAllListeners();
            plusButton.onClick.RemoveAllListeners();
        }

        public void Allocate(int points)
        {
            traitStore.AssignPoint(trait, points);
            traitUI.RefreshUI();
        }

        public void RefreshUI()
        {
            minusButton.interactable = traitStore.CanAssignPoints(trait, -1);
            plusButton.interactable = traitStore.CanAssignPoints(trait, 1);
            valueText.text = traitStore.GetProposedPoints(trait).ToString();
        }
    }
}
