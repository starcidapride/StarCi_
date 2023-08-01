using UnityEngine;

public class LoadingManager : SingletonPersistent<LoadingManager>
{
    [SerializeField]
    private Transform loadingSpinner;

    public void Hide()
    {
        loadingSpinner.gameObject.SetActive(false);

        LoadingSceneManager.IsInputBlocked = false;
    }
    public void Show()
    {
        LoadingSceneManager.IsInputBlocked = true;

        loadingSpinner.gameObject.SetActive(true);
    }
}