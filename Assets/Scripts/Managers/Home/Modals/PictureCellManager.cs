using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PictureCellManager : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Image image;

    private int pictureIndex;

    public void SetImage(int index)
    {
        pictureIndex = index;

        image.sprite = ImageUtility.CreateSpriteFromTexture(PathUtility.LoadIndexedPicture(index));
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        var user = LocalSessionManager.Instance.User;
        try
        {
            user.PictureIndex = pictureIndex;

            HomeManager.Instance.SetPictureImage(pictureIndex);
        } finally
        {
            ModalManager.Instance.CloseNearestModal();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        var color = ImageUtility.GetColorFromHexEnum(HexEnum.LightGray);

        image.color = color;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        var color = Color.white;

        image.color = color;
    }
}
