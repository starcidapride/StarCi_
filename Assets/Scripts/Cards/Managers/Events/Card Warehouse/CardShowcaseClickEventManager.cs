using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardShowcaseClickEventManager : CardEventManager, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        StartCoroutine(OnPointerClickCoroutine());
    }

    public IEnumerator OnPointerClickCoroutine()
    {
        GameObjectUtility.RemoveAllChildGameObjects(CardWarehouseManager.Instance.GetCardPreviewAreaContainer());

        CardWarehouseManager.Instance.SetUIInteractability(false);

        CardWarehouseManager.Instance.SetActiveCardDetails(true);

        yield return CardUtility.InstantiateAndSetupCardCoroutine(CardName, 
            CardWarehouseManager.Instance.GetCardPreviewAreaContainer(), 
            Vector2.zero, Vector2.one * 3 / 4, new List<Type> {
         //   typeof(CardPreviewPanelHoverEventManager)
        });

        CardWarehouseManager.Instance.SetUIInteractability(true);
    }



}

