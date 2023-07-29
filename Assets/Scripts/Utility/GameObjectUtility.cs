using System.ComponentModel;
using System;
using UnityEngine;

public class GameObjectUtility
{
    public static void SetInteractability(Transform transform, bool isEnabled = true)
    {
        var canvasGroup = transform.GetComponent<CanvasGroup>();

        canvasGroup.interactable = isEnabled;
        canvasGroup.blocksRaycasts = isEnabled;
    }

    public static Transform GetClosestSiblingGameObject(Transform transform, bool isOlder)
    {
        var parent = transform.transform.parent;
        var index = transform.transform.GetSiblingIndex();
        var siblingIndex = isOlder ? index - 1 : index + 1;

        if (siblingIndex < 0 || siblingIndex >= parent.childCount) return null;

        return parent.GetChild(siblingIndex);
    }

    public static void RemoveAllChildGameObjects(Transform transform)
    {
        foreach (Transform child in transform)
        {
            UnityEngine.Object.Destroy(child.gameObject);
        }
    }

}