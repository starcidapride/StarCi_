using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardShowcaseClickEventManager : CardEventManager, IPointerClickHandler
{
    private Transform cardDetails;
    private Transform characterCardDetailsPrefab;

    private void Start()
    {
        cardDetails = GameObject.Find(Constants.GameObjectNames.CARD_DETAILS).transform;
        characterCardDetailsPrefab = PathUtility.GetCardDetails(CardType.Character);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        StartCoroutine(OnPointerClickCoroutine());
    }

    public IEnumerator OnPointerClickCoroutine()
    {
        var cardObject = CardUtility.GetCharacterCardObject(CardName);

        var cardType = CardUtility.GetCardType(CardName);

        var cardDetailsPrefab = cardType switch
        {
            CardType.Character => characterCardDetailsPrefab,
            _ => null
        };

        GameObjectUtility.RemoveAllChildGameObjects(cardDetails);

        var cardDetailsInstance = Instantiate(cardDetailsPrefab, cardDetails);

        cardDetailsInstance.GetComponent<CharacterCardDetailsManager>()
            .SetProperties(new CharacterCardDetailsProperties()
        {
            PassiveImage = cardObject.PassiveImage,
            EImage = cardObject.EImage,
            QImage = cardObject.QImage,
            RImage = cardObject.RImage,

            CardName = CardName,
            MaxHealth = cardObject.MaxHealth,
            AttackDamage = cardObject.AttackDamage,
            Armor = cardObject.Armor,
            MagicResistance = cardObject.MagicResistance,

        });

        GameObjectUtility.RemoveAllChildGameObjects(CardWarehouseManager.Instance.GetCardPreviewAreaContainer());

        CardWarehouseManager.Instance.SetUIInteractability(false);

        CardWarehouseManager.Instance.SetActiveCardDetails(true);

        yield return CardUtility.InstantiateAndSetupCardCoroutine(CardName, 
            CardWarehouseManager.Instance.GetCardPreviewAreaContainer(), 
            Vector2.zero, Vector2.one * 3 / 4);

        CardWarehouseManager.Instance.SetUIInteractability(true);
    }



}

