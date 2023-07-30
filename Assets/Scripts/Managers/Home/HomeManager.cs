using System.Collections;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HomeManager : Singleton<HomeManager> 
{
    [SerializeField]
    private Transform ui;

    [SerializeField]
    private Image pictureImage;

    [SerializeField]
    private TMP_Text usernameText;

    [SerializeField]
    private Button editPictureButton;

    [SerializeField]
    private Transform editPictureModalPrefab;

    private User user;
    private IEnumerator Start()
    {
        yield return new WaitUntil(() => LoadingFadeEffectManager.IsEndFadeOut);
        user = LocalSessionManager.Instance.User;

        pictureImage.sprite = ImageUtility.CreateSpriteFromTexture(PathUtility.LoadIndexedPicture(user.PictureIndex));

        usernameText.text = user.Username;

        editPictureButton.onClick.AddListener(() => ModalManager.Instance.CreateModal(editPictureModalPrefab));
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
