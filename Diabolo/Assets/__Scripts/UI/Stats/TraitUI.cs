using RPG.Stats;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Stats
{
    public class TraitUI : MonoBehaviour
    {
        [SerializeField] TMP_Text unassignedPointsText;
        [SerializeField] Button commitButton;

        TraitStore traitStore;
        GameObject player;

        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            traitStore = player.GetComponent<TraitStore>();
        }

        private void OnEnable()
        {
            commitButton.onClick.AddListener(traitStore.Commit);
            commitButton.onClick.AddListener(RefreshUI);

            if (player != null)
                player.GetComponent<BaseStats>().OnLevelUp += RefreshUI;
        }

        private void OnDisable()
        {
            commitButton.onClick.RemoveListener(traitStore.Commit);
            commitButton.onClick.RemoveListener(RefreshUI);

            if (player != null)
                player.GetComponent<BaseStats>().OnLevelUp -= RefreshUI;
        }

        public void RefreshUI()
        {
            unassignedPointsText.text = traitStore.GetUnassignedPoints().ToString();

            TraitRowUI[] traitRows = GetComponentsInChildren<TraitRowUI>();

            foreach (TraitRowUI traitRow in traitRows)
            {
                traitRow.RefreshUI();
            }
        }
    }
}
