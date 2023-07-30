using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class OtherCardManager : MonoBehaviour
{
    [SerializeField]
    private Image frameImage;

    [SerializeField]
    private Image cardImage;

    [SerializeField]
    private TMP_Text cardNameText;

    [SerializeField]
    private TMP_Text descriptionText;

    public void SetAttributes(OtherCardAttributes attributes)
    {
        frameImage.sprite = ImageUtility.CreateSpriteFromTexture(attributes.FrameImage);
        cardImage.sprite = ImageUtility.CreateSpriteFromTexture(attributes.CardImage);
        cardNameText.text = EnumUtility.GetDescription(attributes.CardName);
        descriptionText.text = attributes.Description;
    }
}

public class OtherCardAttributes
{
    public Texture2D FrameImage { get; set; }
    public Texture2D CardImage { get; set; }
    public CardName CardName { get; set; }
    public string Description { get; set; }
}