using UnityEngine;
using UnityEngine.UI;

public class CardSleeveManager : MonoBehaviour
{
    [SerializeField]
    private Image cardSleeveImage;

    public void SetVisibility(bool isVisible)
    {
        cardSleeveImage.gameObject.SetActive(isVisible);
    }

    public void SetImage(int cardSleeveIndex)
    {
        cardSleeveImage.sprite = ImageUtility.CreateSpriteFromTexture(
            PathUtility.LoadIndexedCardSleeve(cardSleeveIndex));
    }
}
