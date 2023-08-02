using UnityEngine;
using UnityEngine.UI;

public class LoadingManager : SingletonPersistent<LoadingManager>
{
    [SerializeField]
    private Image backdropImage;

    private readonly float a = 0.5f;

    public void Start()
    {
        var color = backdropImage.color;
        color.a = a;
        backdropImage.color = color;
    }

    public void Hide()
    {
        backdropImage.gameObject.SetActive(false);

        LoadingSceneManager.IsInputBlocked = false;
    }
    public void Show()
    {

        backdropImage.gameObject.SetActive(true);

        LoadingSceneManager.IsInputBlocked = true;
    }
}