using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDragFromDeckManager : CardEventManager, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform oldParent;
    private Vector2 oldLocalPosition;
    private Type clickEvent = typeof(CardShowcaseClickEventManager);

    private Vector2 distance;

    public void OnBeginDrag(PointerEventData eventData)
    {
        oldParent = transform.parent;

        oldLocalPosition = transform.localPosition;

        CardUtility.DetachEvent(transform, clickEvent);

        transform.SetParent(CardWarehouseManager.Instance.GetDragArea());

        transform.localScale = Vector3.one / 4;

        distance = GameObjectUtility.GetMousePositionRelativeToRectTransform((RectTransform)transform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        var mousePos = GameObjectUtility.GetMousePos();

        transform.position = mousePos - distance;

    }

    public void OnEndDrag(PointerEventData eventData)
    {
            var cardPosition = transform.position;

            var dictionary = CardDictionary.GetCardDictionary();

            var inPlayDeck = dictionary[CardName].Item1 != CardType.Character;

            if (inPlayDeck && !GameObjectUtility.IsPositionInsideRectTransformArea(cardPosition, (RectTransform)CardWarehouseManager.Instance.GetPlayDeckContainer()))
            {
                CardWarehouseManager.Instance.RemoveCard(CardName, ComponentDeckType.Play);
                Destroy(transform.gameObject);
            }
            else if (!inPlayDeck && !GameObjectUtility.IsPositionInsideRectTransformArea(cardPosition, (RectTransform)CardWarehouseManager.Instance.GetCharacterDeckContainer()))
            {
                CardWarehouseManager.Instance.RemoveCard(CardName, ComponentDeckType.Character);
                Destroy(transform.gameObject);
            }
            else
            {
                transform.SetParent(oldParent);
                transform.localPosition = oldLocalPosition;
                CardUtility.AttachEvent(transform, clickEvent);
        }  
    }
}

