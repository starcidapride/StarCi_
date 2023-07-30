using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ModalManager : SingletonPersistent<ModalManager>
{
    [SerializeField]
    private Image backdropImage;

    public static bool ModalActive { get; set; } = false;
    private void Hide()
    {
        StartCoroutine(HideCoroutine());
    }

    private IEnumerator HideCoroutine()
    {
        yield return AnimationUtility.ExecuteTriggerThenWait(
            backdropImage.transform,
            Constants.Triggers.FADE_OUT
            );

        backdropImage.gameObject.SetActive(false);

        ModalActive = false;
    }

    private void Show()
    {
        StartCoroutine(ShowCoroutine());
    }

    private IEnumerator ShowCoroutine()
    {
        ModalActive = true;

        backdropImage.gameObject.SetActive(true);

        yield return AnimationUtility.ExecuteTriggerThenWait(
            backdropImage.transform, 
            Constants.Triggers.FADE_IN
            );
    }

    private bool HasActived()
    {
        return ModalActive;
    }

    public void CreateModal(Transform modalPrefab)
    {
        StartCoroutine(CreateModalCoroutine(modalPrefab));
    }
    private IEnumerator CreateModalCoroutine(Transform modalPrefab)
    {
        if (!HasActived())
        {
            Show();
        }

        var modalInstance = Instantiate(modalPrefab, backdropImage.transform);

        yield return AnimationUtility.WaitForAnimationCompletion(modalInstance);

        modalInstance.name = modalPrefab.name;

        var closestYoungerSibling = GameObjectUtility.GetClosestSiblingGameObject(modalInstance, true);
        
        if (closestYoungerSibling != null)
        {
            GameObjectUtility.SetInteractability(closestYoungerSibling, false);
        }
    }

    public void CloseNearestModal()
    {
        StartCoroutine(CloseNearestModalCoroutine());
    }

    private IEnumerator CloseNearestModalCoroutine()
    {
        if (HasActived())
        {
            Hide();
        }

        var numSiblings = backdropImage.transform.childCount;

        if (numSiblings == 0) yield break;

        var youngestSibling = backdropImage.transform.GetChild(numSiblings - 1);

        var closestYoungerSibling = GameObjectUtility.GetClosestSiblingGameObject(youngestSibling, true);

        yield return AnimationUtility.ExecuteTriggerThenWait(youngestSibling, Constants.Triggers.END);
        
        Destroy(youngestSibling.gameObject);

        if (closestYoungerSibling != null)
        {
            GameObjectUtility.SetInteractability(closestYoungerSibling, true);
            yield break;
        }
    }
}
