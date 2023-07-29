using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ModalManager : SingletonPersistent<ModalManager>
{
    [SerializeField]
    private Image backdropImage;

    private bool modalActive;
    private void Hide()
    {
        backdropImage.gameObject.SetActive(false);

        modalActive = false;
    }

    private void Show()
    {
        StartCoroutine(ShowCoroutine());
    }

    private IEnumerator ShowCoroutine()
    {
        modalActive = true;

        backdropImage.gameObject.SetActive(true);

        yield return AnimationUtility.ExecuteTriggerThenWait(
            backdropImage.transform, 
            Constants.Triggers.FADE_IN
            );
    }

    private bool HasActived()
    {
        return modalActive;
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
        var numSiblings = backdropImage.transform.childCount;

        if (numSiblings == 0) return;

        var youngestSibling = backdropImage.transform.GetChild(numSiblings - 1);

        var closestYoungerSibling = GameObjectUtility.GetClosestSiblingGameObject(youngestSibling, true);

        Destroy(youngestSibling.gameObject);

        if (closestYoungerSibling != null)
        {
            GameObjectUtility.SetInteractability(closestYoungerSibling, true);
            return;
        }

        Hide();
    }
}
