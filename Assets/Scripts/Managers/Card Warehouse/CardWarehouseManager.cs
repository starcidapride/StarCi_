using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CardWarehouseManager : Singleton<CardWarehouseManager> 
{
    [SerializeField]
    private Transform ui;

    [SerializeField]
    private TMP_InputField cardNameTextInput;

    [SerializeField]
    private TMP_Dropdown cardTypeDropdownInput;

    [SerializeField]
    private TMP_Dropdown characterRoleDropdownInput;

    [SerializeField]
    private TMP_Dropdown equipmentClassDropdownInput;

    [SerializeField]
    private Button findButton;

    [SerializeField]
    public Transform cardShowcaseContainer;

    [SerializeField]
    private Transform cardPreviewAreaContainer;

    [SerializeField]
    private Transform cardDetails;

    [SerializeField]
    private Transform dragArea;

    [SerializeField]
    private Transform playDeckContainer;

    [SerializeField]
    private Transform characterDeckContainer;

    private string cardName;
    private CardTypeDropdown cardType;
    private CharacterRoleDropdown characterRole;
    private EquipmentClassDropdown equipmentClass;
    private int currentPage = 0;
    public static bool IsFinishLoad { get; set; } = false;
    private IEnumerator Start()
    {
        yield return new WaitUntil(() => LoadingFadeEffectManager.IsEndFadeOut);

        cardName = cardNameTextInput.text;
        cardType = (CardTypeDropdown)cardTypeDropdownInput.value;
        characterRole = (CharacterRoleDropdown)characterRoleDropdownInput.value;
        equipmentClass = (EquipmentClassDropdown)equipmentClassDropdownInput.value;

        cardNameTextInput.onEndEdit.AddListener(value => cardName = value);

        cardTypeDropdownInput.onValueChanged.AddListener(value => cardType = (CardTypeDropdown)value);

        characterRoleDropdownInput.onValueChanged.AddListener(value => characterRole = (CharacterRoleDropdown)value);

        equipmentClassDropdownInput.onValueChanged.AddListener(value => equipmentClass = (EquipmentClassDropdown)value);

        findButton.onClick.AddListener(OnFindButtonClick);

        SelectDeckManager.Instance.OnSelectedDeckChanged += OnSelectedDeckChanged; ;

        IsFinishLoad = true;
    }

    private void OnSelectedDeckChanged()
    {
        UpdateComponentDeck(ComponentDeckType.Play, true);
        UpdateComponentDeck(ComponentDeckType.Character, true);
    }

    private void OnDestroy()
    {
        IsFinishLoad = false;
    }

    public Transform GetPlayDeckContainer()
    {
        return playDeckContainer;
    }

    public Transform GetCharacterDeckContainer()
    {
        return characterDeckContainer;
    }

    public Transform GetDragArea()
    {
        return dragArea;
    }

    public void SetActiveUI(bool isActive)
    {
        StartCoroutine(SetActiveUICoroutine(isActive));
    }

    public void SetActiveCardDetails(bool isEnable)
    {   
        cardDetails.gameObject.SetActive(isEnable);
    }

    public Transform GetCardShowcaseContainer()
    {
        return cardShowcaseContainer;
    }

    public Transform GetCardPreviewAreaContainer()
    {
        return cardPreviewAreaContainer;
    }

    private IEnumerator SetActiveUICoroutine(bool isActive)
    {
        if (isActive)
        {
            ui.gameObject.SetActive(true);

            yield return AnimationUtility.WaitForAnimationCompletion(ui);
        } else
        {
            ui.gameObject.SetActive(false);
        }
    }

    private void AddCardToDeckThenSave(CardName cardName, ComponentDeckType deckType)
    {
        var selectedDeck = LocalSessionManager.Instance.GetSelectedDeck();

        var componentDeck = deckType == ComponentDeckType.Play ? selectedDeck.PlayDeck : selectedDeck.CharacterDeck;

        componentDeck.Add(cardName);

        PlayerPrefsUtility.SaveToPlayerPrefs(Constants.PlayerPrefs.USER,
            LocalSessionManager.Instance.User);
    }

    public void AddCard(CardName cardName, ComponentDeckType deckType)
    {
        var selectedDeck = LocalSessionManager.Instance.GetSelectedDeck();

        var result = CardUtility.ValidateCardAddition(deckType,
            selectedDeck,
            cardName
        );

        if (result == CardAdditionResult.Success)
        {
            AddCardToDeckThenSave(cardName, deckType);

            UpdateComponentDeck(deckType);
            return;
        }

        var message = result switch
        {
            CardAdditionResult.CardNotAllowed => $"Cannot add <b>{cardName}</b> to your {deckType} deck. The card is not allowed in this deck.",
            CardAdditionResult.DeckReachedLimit => $"Cannot add <b>{cardName}</b> to your {deckType} deck. The deck has reached its card limit.",
            CardAdditionResult.MaxCardOccurrences => $"Cannot add <b>{cardName}</b> to your {deckType} deck. The maximum card occurrences have been reached.",
            _ => string.Empty
        };

        AlertManager.Instance.Show(AlertCaption.Failure,
            message,
            new List<AlertButton>()
            {
                    new AlertButton()
                    {
                        ButtonText = ButtonText.Cancel,
                        HandleOnClick = () => AlertManager.Instance.Hide()
                    }
            }
            );
    }

    public void RemoveCard(CardName cardName, ComponentDeckType deckType)
    {
        var selectedDeck = LocalSessionManager.Instance.GetSelectedDeck();

        if (deckType == ComponentDeckType.Play)
        {
            selectedDeck.PlayDeck.Remove(cardName);
        } else
        {
            selectedDeck.CharacterDeck.Remove(cardName);
        }

        UpdateComponentDeck(deckType, true);

        PlayerPrefsUtility.SaveToPlayerPrefs(Constants.PlayerPrefs.USER, LocalSessionManager.Instance.User);
    }

    private void UpdateComponentDeck(ComponentDeckType componentDeckType, bool firstUpdate = false)
    {
        StartCoroutine(UpdateComponentDeckCoroutine(componentDeckType, firstUpdate));
    }

    private IEnumerator UpdateComponentDeckCoroutine(ComponentDeckType componentDeckType, bool firstUpdate = false)
    {
        var container = componentDeckType == ComponentDeckType.Play ? 
            playDeckContainer : characterDeckContainer;

        GameObjectUtility.RemoveAllChildGameObjects(container);

        var deckCollection = LocalSessionManager.Instance.User.DeckCollection;

        var rows = componentDeckType switch
        {
            ComponentDeckType.Play => 4,
            _ => 1
        };

        var cols = 10;

        var grids = Grid.SplitSpriteIntoIndexedGrids(container, rows, cols);

        var selectedDeck = componentDeckType == ComponentDeckType.Play ?
            deckCollection.Decks[deckCollection.SelectedDeckIndex].PlayDeck :
            deckCollection.Decks[deckCollection.SelectedDeckIndex].CharacterDeck;

        if (selectedDeck.Count == 0) yield break;

        var isAnimations = new Dictionary<int, bool>
        {
            { selectedDeck.Count - 1, true }
        };

        if (selectedDeck.Count - 1 > 0)
        {
            for (int i = 0; i < selectedDeck.Count - 1; i++)
            {
                isAnimations.Add(i, firstUpdate);
            }
        }

        for (int i = 0; i < selectedDeck.Count; i++)
        {
            if (isAnimations[i])
            {
                StartCoroutine(CardUtility.InstantiateAndSetupCardCoroutine(selectedDeck[i],
                                  container,
                                  grids[i].Center,
                                  Vector2.one / 4,
                                  new List<Type> { typeof(CardDragFromDeckManager),
                                  typeof(CardShowcaseClickEventManager)
                                  }));
            }
            else
            {
                CardUtility.InstantiateAndSetupCard(selectedDeck[i],
                                   container,
                                   grids[i].Center,
                                   Vector2.one / 4,
                                   new List<Type> { typeof(CardDragFromDeckManager),
                                   typeof(CardShowcaseClickEventManager)
                                   });
            }
        }

        yield return new WaitForSeconds(0.6f);
    }


    private void OnFindButtonClick()
    {
        StartCoroutine(OnFindButtonClickCoroutine());
    }

    public class EnumDescriptionComparer<TEnum> : IComparer<TEnum> where TEnum : Enum
    {
        public int Compare(TEnum x, TEnum y)
        {
            string descriptionX = EnumUtility.GetDescription(x);
            string descriptionY = EnumUtility.GetDescription(y);

            return string.Compare(descriptionX, descriptionY, StringComparison.OrdinalIgnoreCase);
        }
    }


    private IEnumerator OnFindButtonClickCoroutine()
    {
        GameObjectUtility.RemoveAllChildGameObjects(cardShowcaseContainer);

        var dictionary = CardDictionary.GetCardDictionary();

    var sortedMap = new SortedDictionary<CardName, (CardType, Type)>(

            dictionary.Where(kvp =>
        {
            var cardNameFilter = EnumUtility.GetDescription(kvp.Key).ContainsInsensitive(cardName);

            var cardTypeFilter = cardType == CardTypeDropdown.None || kvp.Value.Item1 == (CardType)cardType;

            bool IsMatchCharacterRole(CharacterRoleDropdown characterRole)
            {
                var cardType = dictionary[kvp.Key].Item1;
                if (cardType != CardType.Character) return false;

                var cardClass = dictionary[kvp.Key].Item2;

                var card = (ICharacterCard)Activator.CreateInstance(cardClass);

                if (card.CharacterRole != (CharacterRole)characterRole && characterRole != CharacterRoleDropdown.None) return false;

                return true;
            }

            bool IsMatchEquipmentRole(EquipmentClassDropdown equipmentClass)
            {
                var cardType = dictionary[kvp.Key].Item1;
                if (cardType != CardType.Equipment) return false;

                var cardClass = dictionary[kvp.Key].Item2;

                var card = (IEquipmentCard)Activator.CreateInstance(cardClass);

                if (card.EquipmentClass != (EquipmentClass)equipmentClass && equipmentClass != EquipmentClassDropdown.None) return false;


                return true;
            }


            var characterRoleFilter =
            cardType != CardTypeDropdown.Character
            || characterRole == CharacterRoleDropdown.None
            || IsMatchCharacterRole(characterRole);

            var equipmentClassFilter =
            cardType != CardTypeDropdown.Equipment
            || equipmentClass == EquipmentClassDropdown.None
            || IsMatchEquipmentRole(equipmentClass);

            return cardNameFilter && cardTypeFilter && characterRoleFilter && equipmentClassFilter;

        }).ToDictionary(kvp => kvp.Key, kvp => kvp.Value)
        , new EnumDescriptionComparer<CardName>()
            );

        var cardNames = sortedMap.Select(kvp => kvp.Key).ToList();

        var grids = Grid.SplitSpriteIntoIndexedGrids(cardShowcaseContainer, 4, 3);

        for (int i = 12 * currentPage; i < math.min(12 * (currentPage + 1), cardNames.Count); i++)
        {

            StartCoroutine(CardUtility.InstantiateAndSetupCardCoroutine(cardNames[i], cardShowcaseContainer,
                grids[i].Center, Vector2.one / 4, new List<Type> { 
                    typeof(CardShowcaseClickEventManager),
                    typeof(CardDragToDeckManager)
            }));
        }

        SetUIInteractability(false);

        yield return new WaitForSeconds(0.6f);

        SetUIInteractability(true);
    }

    public void SetUIInteractability(bool isEnable)
    {
        GameObjectUtility.SetInteractability(ui, isEnable);
    }
}

public enum CardTypeDropdown
{
    None,

    Character,

    Equipment,

    Spell,

    Other
}

public enum CharacterRoleDropdown
{
    None,

    Warrior,

    Tank,

    Support,

    Mage,

    Marksman,

    Assassin
}

public enum EquipmentClassDropdown
{
    None,

    Attack,

    Magic,

    Defense
}
