using DG.Tweening;
using UnityEngine;


public class LoadingFadeEffectManager : SingletonPersistent<LoadingFadeEffectManager>
{
    [SerializeField]
    private CanvasGroup transitionBackgroundCanvasGroup;

    [SerializeField]
    [Range(0f, 5f)]
    private float fadeDuration;

    public static bool IsEndFadeIn { get; set; } = false;
    public static bool IsEndFadeOut { get; set; } = false;

    public void FadeIn()
    {
        IsEndFadeOut = false;

        transitionBackgroundCanvasGroup.gameObject.SetActive(true);

        transitionBackgroundCanvasGroup.DOFade(1f, fadeDuration).OnComplete(
            () =>
            {
                IsEndFadeIn = true;
            }
        );
    }

    public void FadeOut()
    {
        IsEndFadeIn = false;

        transitionBackgroundCanvasGroup.DOFade(0f, fadeDuration).OnComplete(
            () =>
            {
                transitionBackgroundCanvasGroup.gameObject.SetActive(false);
                IsEndFadeOut = true;
            }
        );
    }

    public void FadeAll()
    {
        transitionBackgroundCanvasGroup.DOFade(1f, fadeDuration).OnComplete(
            () =>
            {
                DOVirtual.DelayedCall(1f, () =>
                {
                    transitionBackgroundCanvasGroup.DOFade(0f, fadeDuration);
                }
                );
            }
        );
    }
}