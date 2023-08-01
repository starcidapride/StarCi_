using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class EquipmentCardManager : CardManager
{
    [SerializeField]
    private Image cardImage;

    public void SetAttributes(EquipmentCardAttributes attributes)
    {
        CardName = attributes.CardName;

        cardImage.sprite = ImageUtility.CreateSpriteFromTexture(attributes.CardImage);
    }
}

public class EquipmentCardAttributes
{
    public Texture2D CardImage { get; set; }
    public CardName CardName { get; set; }
    public int Price { get; set; }
}