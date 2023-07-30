using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class CardUtils
{
    private static ICharacterCard GetCharacterCardObject(CardName cardName)
    {
        var dictionary = CardDictionary.GetCharacterCardDictionary();

        var cardClass = dictionary.ContainsKey(cardName) ? dictionary[cardName] : null;

        if (cardClass != null)
        {
            return (ICharacterCard)Activator.CreateInstance(cardClass);
        }
        return null;
    }
    private static Transform InstantiateCharaterCard(CardName cardName, Transform parent = null)
    {
        var characterCardPrefab = PathUtility.LoadCardPrefab(CardType.Character);

        characterCardPrefab.name = EnumUtility.GetDescription(cardName);

        var cardClassObject = GetCharacterCardObject(cardName);

        var card = UnityEngine.Object.Instantiate(characterCardPrefab, parent);

        var attributes = new CharacterCardAttributes()
        {
            CardName = cardName,

            CardImage = cardClassObject.Image,

            CharacterRole = cardClassObject.CharacterRole,

            Experience = cardClassObject.Experience,

            QImage = cardClassObject.QImage,

            EImage = cardClassObject.EImage,

            RImage = cardClassObject.RImage,
        };

        var manager = card.GetComponent<CharacterCardManager>();

        manager.SetAttributes(attributes);

        return card;
    }

    private static IEquipmentCard GetEquipmentCardObject(CardName cardName)
    {
        var dictionary = CardDictionary.GetEquipmentCardDictionary();

        var cardClass = dictionary.ContainsKey(cardName) ? dictionary[cardName] : null;

        if (cardClass != null)
        {
            return (IEquipmentCard)Activator.CreateInstance(cardClass);
        }
        return null;
    }
    private static Transform InstantiateEquipmentCard(CardName cardName, Transform parent = null)
    {
        var equipmentCardPrefab = PathUtility.LoadCardPrefab(CardType.Equipment);

        equipmentCardPrefab.name = EnumUtility.GetDescription(cardName);

        var cardClassObject = GetEquipmentCardObject(cardName);

        var card = UnityEngine.Object.Instantiate(equipmentCardPrefab, parent);

        var attributes = new EquipmentCardAttributes
        {
            CardName = cardName,

            CardImage = cardClassObject.Image,

            Price = cardClassObject.Price,

            Description = cardClassObject.Description
        };

        var manager = card.GetComponent<EquipmentCardManager>();

        manager.SetAttributes(attributes);

        return card;
    }

    private static ISpellCard GetSpellCardObject(CardName cardName)
    {
        var dictionary = CardDictionary.GetSpellCardDictionary();

        var cardClass = dictionary.ContainsKey(cardName) ? dictionary[cardName] : null;

        if (cardClass != null)
        {
            return (ISpellCard)Activator.CreateInstance(cardClass);
        }
        return null;
    }

    private static Transform InstantiateSpellCard(CardName cardName, Transform parent = null)
    {
        var spellCardPrefab = PathUtility.LoadCardPrefab(CardType.Spell);

        spellCardPrefab.name = EnumUtility.GetDescription(cardName);

        var cardClassObject = GetSpellCardObject(cardName);

        var card = UnityEngine.Object.Instantiate(spellCardPrefab, parent);

        var attributes = new SpellCardAttributes()
        {
            CardName = cardName,

            CardImage = cardClassObject.Image,

            Description = cardClassObject.Description,
        };

        var manager = card.GetComponent<SpellCardManager>();

        manager.SetAttributes(attributes);

        return card;
    }

    private static IOtherCard GetOtherCardObject(CardName cardName)
    {
        var dictionary = CardDictionary.GetOtherCardDictionary();

        var cardClass = dictionary.ContainsKey(cardName) ? dictionary[cardName] : null;

        if (cardClass != null)
        {
            return (IOtherCard)Activator.CreateInstance(cardClass);
        }
        return null;
    }

    private static Transform InstantiateOtherCard(CardName cardName, Transform parent = null)
    {
        var otherCardPrefab = PathUtility.LoadCardPrefab(CardType.Other);

        var cardClassObject = GetOtherCardObject(cardName);

        var card = UnityEngine.Object.Instantiate(otherCardPrefab, parent);

        var attributes = new OtherCardAttributes()
        {
            FrameImage = cardClassObject.Frame,

            CardImage = cardClassObject.Image,

            CardName = cardName,

            Description = cardClassObject.Description,
        };

        var manager = card.GetComponent<OtherCardManager>();

        manager.SetAttributes(attributes);

        return card;
    }

    public static Transform InstantiateCard(CardName cardName, Transform parent = null)
    {
        var cardType = CardDictionary.GetCardDictionary()[cardName].Item1;

        return cardType switch
        {
            CardType.Character => InstantiateCharaterCard(cardName, parent),

            CardType.Equipment => InstantiateEquipmentCard(cardName, parent),

            CardType.Spell => InstantiateSpellCard(cardName, parent),

            _ => InstantiateOtherCard(cardName, parent)
        };
    }

    public static Transform InstantiateAndSetupCard(CardName cardName, Vector2 position, Vector2 scale, Transform parent, List<Type> scripts = null)
    {
        var card = InstantiateCard(cardName, parent);

        card.localPosition = position;

        card.transform.localScale = scale;

        if (scripts != null)
        {
            foreach (Type script in scripts)
            {
                if (script != null && typeof(Component).IsAssignableFrom(script))
                {
                    var cardEventController = card.gameObject.AddComponent(script);

                    if (cardEventController is CardEventManager)
                    {
                        var cardEvent = (CardEventManager)cardEventController;
                        cardEvent.CardName = cardName;
                    }
                }
            }
        }

        return card;
    }


    public static IEnumerator InstantiateAndSetupCardCoroutine(CardName cardName, Vector2 position, Vector2 scale, Transform parent = null, List<Type> scripts = null)
    {
        var card = InstantiateAndSetupCard(cardName, position, scale, parent, scripts);

        yield return AnimationUtility.ExecuteTriggerThenWait(card, Constants.Triggers.FLIP_SELF);
    }

    public static Dictionary<AbilitySymbol, AbilityAttribute> GetAbilityAttributes(CardName cardName)
    {
        var characterCardClassObject = GetCharacterCardObject(cardName);

        if (characterCardClassObject == null) return null;

        return new Dictionary<AbilitySymbol, AbilityAttribute>()
        {
            {AbilitySymbol.Passive, new AbilityAttribute(){
                AbilityImage = characterCardClassObject.PassiveImage,
                AbilityName = characterCardClassObject.PassiveName,
                AbilityDescription = characterCardClassObject.PassiveDescription
            }},
            {AbilitySymbol.Q, new AbilityAttribute(){
                AbilityImage = characterCardClassObject.QImage,
                AbilityName = characterCardClassObject.QName,
                AbilityDescription = characterCardClassObject.QDescription
            }},
            {AbilitySymbol.E, new AbilityAttribute(){
                AbilityImage = characterCardClassObject.EImage,
                AbilityName = characterCardClassObject.EName,
                AbilityDescription = characterCardClassObject.EDescription
            }},
            {AbilitySymbol.R, new AbilityAttribute(){
                AbilityImage = characterCardClassObject.RImage,
                AbilityName = characterCardClassObject.RName,
                AbilityDescription = characterCardClassObject.RDescription
            }},
        };
    }

    public static CardAdditionResult ValidateCardAddition(ComponentDeckType deckType, Deck deck, CardName cardName)
    {
        var maxCards = deckType == ComponentDeckType.Play ? 
            Constants.CardLimits.MAX_CARDS_IN_PLAY_DECK :
            Constants.CardLimits.MAX_CARDS_IN_CHARACTER_DECK;
        
        var maxOccurrences = deckType == ComponentDeckType.Play ?
            Constants.CardLimits.MAX_OCCURRENCES_IN_PLAY_DECK :
            Constants.CardLimits.MAX_OCCURRENCES_IN_CHARACTER_DECK;

        var componentDeck = deckType == ComponentDeckType.Play ? deck.PlayDeck : deck.CharacterDeck;

        var dictionary = CardDictionary.GetCardDictionary();

        if (!(dictionary[cardName].Item1 == CardType.Character ^ deckType == ComponentDeckType.Play))
            return CardAdditionResult.CardNotAllowed;

        if (componentDeck.Count() >= maxCards) return CardAdditionResult.DeckReachedLimit;

        var cardOccurrences = componentDeck.Count(_cardName => _cardName == cardName);

        if (cardOccurrences >= (cardName == CardName.Invocation ? Constants.CardLimits.MAX_INVOCATION_CARD_OCCURRENCES : maxOccurrences))
            return CardAdditionResult.MaxCardOccurrences;

        return CardAdditionResult.Success;
    }

}

public enum ComponentDeckType
{
    Play,
    Character
}

public enum CardAdditionResult
{
    Success,
    CardNotAllowed,
    DeckReachedLimit,
    MaxCardOccurrences,
}

public class AbilityAttribute
{
    public Texture2D AbilityImage { get; set; }
    public string AbilityName { get; set; }
    public string AbilityDescription { get; set; }
}