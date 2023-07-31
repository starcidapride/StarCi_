using UnityEngine;
using UnityEngine.EventSystems;

public class CardDragToDeckManager : CardEventManager, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform dragCard;
    private Vector2 localPosition;
    public void OnBeginDrag(PointerEventData eventData)
    {
        dragCard = CardUtility.InstantiateCard(CardName, CardWarehouseManager.Instance.GetDragArea());

        dragCard.localScale = Vector3.one / 4;

        localPosition = GameObjectUtility.GetMousePositionRelativeToRectTransform((RectTransform)transform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        var mousePos = GameObjectUtility.GetMousePos();

        dragCard.position = mousePos - localPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        var cardPosition = dragCard.position;

        if (GameObjectUtility.IsPositionInsideRectTransformArea(cardPosition, (RectTransform)CardWarehouseManager.Instance.GetPlayDeckContainer()))
        {
            CardWarehouseManager.Instance.AddCard(CardName, ComponentDeckType.Play);
        }
        else if (GameObjectUtility.IsPositionInsideRectTransformArea(cardPosition, (RectTransform)CardWarehouseManager.Instance.GetCharacterDeckContainer()))
        {
            CardWarehouseManager.Instance.AddCard(CardName, ComponentDeckType.Character);
        }

        Destroy(dragCard.gameObject);
    }
}

