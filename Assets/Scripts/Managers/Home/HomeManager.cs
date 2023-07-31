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
    private Button goToCardWarehouseButton;

    [SerializeField]
    private Transform editPictureModalPrefab;

    private User user;

    public static bool IsFinishLoad { get; set; } = false;
    private IEnumerator Start()
    {
        yield return new WaitUntil(() => LoadingFadeEffectManager.IsEndFadeOut);
        user = LocalSessionManager.Instance.User;

        pictureImage.sprite = ImageUtility.CreateSpriteFromTexture(PathUtility.LoadIndexedPicture(user.PictureIndex));

        usernameText.text = user.Username;

        editPictureButton.onClick.AddListener(() => ModalManager.Instance.CreateModal(editPictureModalPrefab));

        goToCardWarehouseButton.onClick.AddListener(() => LoadingSceneManager.Instance.LoadScene(SceneName.CardWarehouse, false));

        IsFinishLoad = true;
    }

    private void OnDestroy()
    {
        IsFinishLoad = false;
    }

    public void SetActiveUI(bool isActive)
    {
        StartCoroutine(SetActiveUICoroutine(isActive));
    }

    private IEnumerator SetActiveUICoroutine(bool isActive)
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

    public void SetPictureImage(int index)
    {
        pictureImage.sprite = ImageUtility.CreateSpriteFromTexture(PathUtility.LoadIndexedPicture(index));
    }
}
