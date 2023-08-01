using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class SpellCardManager : CardManager
{
    [SerializeField]
    private Image cardImage;

    public void SetAttributes(SpellCardAttributes attributes)
    {
        CardName = attributes.CardName;

        cardImage.sprite = ImageUtility.CreateSpriteFromTexture(attributes.CardImage);
    }
}

public class SpellCardAttributes
{
    public Texture2D CardImage { get; set; }
    public CardName CardName { get; set; }
    public string Description { get; set; }
}