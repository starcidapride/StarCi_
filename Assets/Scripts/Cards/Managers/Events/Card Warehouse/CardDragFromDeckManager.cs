//using TMPro;
//using UnityEngine;
//using UnityEngine.EventSystems;

//using static CardUtils;

//public class CardDragFromDeckManager : CardEventManager, IBeginDragHandler, IDragHandler, IEndDragHandler
//{      
//    private Transform dragCard;
//    private Vector2 localPosition;
//    public void OnBeginDrag(PointerEventData eventData)
//    {
//        SetVisibility(transform, false);

//        dragCard = InstantiateCard(CardName, CardWarehouseManager.Instance.DragArea);
        
//        dragCard.localScale = Vector3.one / 4;

//        localPosition = GetMousePositionRelativeToRectTransform((RectTransform)transform);
//    }

//    public void OnDrag(PointerEventData eventData)
//    {
//        var mousePos = GetMousePos();
      
//        dragCard.position = mousePos - localPosition;

//    }

//    public void OnEndDrag(PointerEventData eventData)
//    {
//        var cardPosition = dragCard.position;

//        var map = GetMap();

//        var inPlayDeck = map[CardName].Item1 != CardType.Character;

//        if (inPlayDeck && !IsPositionInsideRectTransformArea(cardPosition, (RectTransform) CardWarehouseManager.Instance.PlayDeckContainer))
//        {
//            CardWarehouseManager.Instance.RemoveCard(CardName, ComponentDeckType.Play);
//        }
//        else if (!inPlayDeck && !IsPositionInsideRectTransformArea(cardPosition, (RectTransform) CardWarehouseManager.Instance.CharacterDeckContainer))
//        {
//            CardWarehouseManager.Instance.RemoveCard(CardName, ComponentDeckType.Character);
//        } else
//        {
//            SetVisibility(transform, true);
//        }

//        Destroy(dragCard.gameObject);
//    }
//}

