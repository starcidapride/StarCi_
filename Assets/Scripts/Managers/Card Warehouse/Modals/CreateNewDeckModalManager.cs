
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateNewDeckModalManager: Singleton<CreateNewDeckModalManager>
{
    [SerializeField]
    private TMP_InputField deckNameTextInput;

    [SerializeField]
    private Button cancelButton;

    [SerializeField]
    private Button submitButton;

    private string deckName;
    private void Start()
    {
        deckName = deckNameTextInput.text;

        deckNameTextInput.onEndEdit.AddListener(value => deckName = value);

        cancelButton.onClick.AddListener(ModalManager.Instance.CloseNearestModal);

        submitButton.onClick.AddListener(OnSubmitButtonClick);
    }

    private void OnSubmitButtonClick()
    {
        var user = LocalSessionManager.Instance.User;

        user.DeckCollection.Decks.Add(new Deck()
        {
            DeckName = deckName,
            PlayDeck = new List<CardName>(),
            CharacterDeck = new List<CardName>()
        });

        user.DeckCollection.SelectedDeckIndex = user.DeckCollection.Decks.Count - 1;

        PlayerPrefsUtility.SaveToPlayerPrefs(Constants.PlayerPrefs.USER, user);

        SelectDeckManager.Instance.UpdateDeckDropdownOptions();

        SelectDeckManager.Instance.InvokeOnSelectedDeckChanged();

        ModalManager.Instance.CloseNearestModal();
    }
}