using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PictureCellManager : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Image image;

    private int cellIndex;

    public void SetAttributes(int index)
    {
        cellIndex = index;

        image.sprite = ImageUtility.CreateSpriteFromTexture(PathUtility.LoadIndexedPicture(index));
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        EditPictureModalManager.Instance.SelectedPictureIndex = cellIndex;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
