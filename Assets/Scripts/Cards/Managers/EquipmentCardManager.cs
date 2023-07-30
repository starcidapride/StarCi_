using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class EquipmentCardManager : MonoBehaviour
{
    [SerializeField]
    private Image cardImage;

    [SerializeField]
    private TMP_Text cardNameText;

    [SerializeField]
    private TMP_Text priceText;

    [SerializeField]
    private TMP_Text descriptionText;

    public void SetAttributes(EquipmentCardAttributes attributes)
    {
        cardImage.sprite = ImageUtility.CreateSpriteFromTexture(attributes.CardImage);
        cardNameText.text = EnumUtility.GetDescription(attributes.CardName);
        priceText.text = attributes.Price.ToString();
        descriptionText.text = attributes.Description;
    }
}

public class EquipmentCardAttributes
{
    public Texture2D CardImage { get; set; }
    public CardName CardName { get; set; }
    public int Price { get; set; }
    public string Description { get; set; }
}