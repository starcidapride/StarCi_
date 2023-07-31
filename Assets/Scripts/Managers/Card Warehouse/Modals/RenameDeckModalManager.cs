using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RenameDeckModalManager : Singleton<RenameDeckModalManager>
{
    [SerializeField]
    private TMP_InputField deckNameTextInput;

    [SerializeField]
    private Button cancelButton;

    [SerializeField]
    private Button confirmButton;

    private string deckName;
    private void Start()
    {
        deckName = deckNameTextInput.text;

        deckNameTextInput.onEndEdit.AddListener(value => deckName = value);

        cancelButton.onClick.AddListener(ModalManager.Instance.CloseNearestModal);

        confirmButton.onClick.AddListener(OnConfirmButtonClick);
    }

    private void OnConfirmButtonClick()
    {
        var selectedDeck = LocalSessionManager.Instance.GetSelectedDeck();

        selectedDeck.DeckName = deckName;

        SelectDeckManager.Instance.UpdateDeckDropdownOptions();

        ModalManager.Instance.CloseNearestModal();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            OnConfirmButtonClick();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ModalManager.Instance.CloseNearestModal();
        }
    }
}