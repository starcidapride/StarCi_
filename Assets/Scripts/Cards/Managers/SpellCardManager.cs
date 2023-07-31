using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class SpellCardManager : CardManager
{
    [SerializeField]
    private Image cardImage;

    [SerializeField]
    private TMP_Text cardNameText;

    public void SetAttributes(SpellCardAttributes attributes)
    {
        CardName = attributes.CardName;

        cardImage.sprite = ImageUtility.CreateSpriteFromTexture(attributes.CardImage);
        cardNameText.text = EnumUtility.GetDescription(attributes.CardName);
    }
}

public class SpellCardAttributes
{
    public Texture2D CardImage { get; set; }
    public CardName CardName { get; set; }
    public string Description { get; set; }
}