using System.Collections;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class CardWarehouseManager : Singleton<CardWarehouseManager> 
{
    [SerializeField]
    private Transform ui;

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => LoadingFadeEffectManager.IsEndFadeOut);
    }

    public void SetActiveUI(bool isActive)
    {
        StartCoroutine(SetActiveUICoroutine(isActive));
    }

    public IEnumerator SetActiveUICoroutine(bool isActive)
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
}
