using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class SpellCardManager : MonoBehaviour
{
    [SerializeField]
    private Image cardImage;

    [SerializeField]
    private TMP_Text cardNameText;

    [SerializeField]
    private TMP_Text descriptionText;

    public void SetAttributes(SpellCardAttributes attributes)
    {
        cardImage.sprite = ImageUtility.CreateSpriteFromTexture(attributes.CardImage);
        cardNameText.text = EnumUtility.GetDescription(attributes.CardName);
        descriptionText.text = attributes.Description;
    }
}

public class SpellCardAttributes
{
    public Texture2D CardImage { get; set; }
    public CardName CardName { get; set; }
    public string Description { get; set; }
}