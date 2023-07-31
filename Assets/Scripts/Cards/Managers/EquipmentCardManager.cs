using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class EquipmentCardManager : CardManager
{
    [SerializeField]
    private Image cardImage;

    [SerializeField]
    private TMP_Text cardNameText;

    [SerializeField]
    private TMP_Text priceText;

    public void SetAttributes(EquipmentCardAttributes attributes)
    {
        CardName = attributes.CardName;

        cardImage.sprite = ImageUtility.CreateSpriteFromTexture(attributes.CardImage);
        cardNameText.text = EnumUtility.GetDescription(attributes.CardName);
        priceText.text = attributes.Price.ToString();
    }
}

public class EquipmentCardAttributes
{
    public Texture2D CardImage { get; set; }
    public CardName CardName { get; set; }
    public int Price { get; set; }
}