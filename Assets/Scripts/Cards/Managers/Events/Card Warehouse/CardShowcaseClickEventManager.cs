//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.EventSystems;

//using static CardUtils;
//using static GameObjectUtils;


//public class CardShowcaseClickEventManager : CardEventManager, IPointerClickHandler
//{
//    public void OnPointerClick(PointerEventData eventData)
//    {
//        StartCoroutine(OnPointerClickCoroutine());
//    }

//    public IEnumerator OnPointerClickCoroutine()
//    {
//        DestroyAllChildGameObjects(CardWarehouseManager.Instance.CardPreviewPanel);

//        CardWarehouseManager.Instance.SetUIInteractability(false);

//        yield return InstantiateAndSetupCardCoroutine(CardName, Vector2.zero, Vector2.one * 3/4, CardWarehouseManager.Instance.CardPreviewPanel, new List<Type> { typeof(CardPreviewPanelHoverEventManager) });
        
//        CardWarehouseManager.Instance.SetUIInteractability();
//    }



//}

