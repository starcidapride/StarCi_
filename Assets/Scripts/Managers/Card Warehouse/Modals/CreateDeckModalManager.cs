
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateDeckModalManager: Singleton<CreateDeckModalManager>
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
        var user = LocalSessionManager.Instance.User;

        user.DeckCollection.Decks.Add(new Deck()
        {
            DeckName = deckName,
            PlayDeck = new List<CardName>(),
            CharacterDeck = new List<CardName>()
        });

        user.DeckCollection.SelectedDeckIndex = user.DeckCollection.Decks.Count - 1;

        LocalSessionManager.Instance.SaveToPlayerPrefs();

        SelectDeckManager.Instance.UpdateDeckDropdownOptions();

        SelectDeckManager.Instance.InvokeOnSelectedDeckChanged();

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