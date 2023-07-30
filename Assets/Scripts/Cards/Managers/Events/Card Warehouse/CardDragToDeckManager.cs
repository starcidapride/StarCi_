//using UnityEngine;
//using UnityEngine.EventSystems;

//using static CardUtils;
//using static GameObjectUtils;

//public class CardDragToDeckManager : CardEventManager, IBeginDragHandler, IDragHandler, IEndDragHandler
//{      
//    private Transform dragCard;
//    private Vector2 localPosition;
//    public void OnBeginDrag(PointerEventData eventData)
//    {
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

//        if (IsPositionInsideRectTransformArea(cardPosition, (RectTransform)  CardWarehouseManager.Instance.PlayDeckContainer))
//        {
//            CardWarehouseManager.Instance.AddCard(CardName, ComponentDeckType.Play);
//        } 
//        else if (IsPositionInsideRectTransformArea(cardPosition, (RectTransform)CardWarehouseManager.Instance.CharacterDeckContainer))
//        {
//            CardWarehouseManager.Instance.AddCard(CardName, ComponentDeckType.Character);
//        }

//        Destroy(dragCard.gameObject);
//    }
//}

