using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCardManager : CardManager
{
    [SerializeField]
    private Image cardImage;

    [SerializeField]
    private TMP_Text cardNameText;

    [SerializeField]
    private TMP_Text characterRoleText;

    [SerializeField]
    private Image passiveImage;

    [SerializeField]
    private Image QImage;

    [SerializeField]
    private Image EImage;

    [SerializeField]
    private Image RImage;

    public void SetAttributes(CharacterCardAttributes attributes)
    {
        CardName = attributes.CardName;

        cardImage.sprite = ImageUtility.CreateSpriteFromTexture(attributes.CardImage);
        cardNameText.text = EnumUtility.GetDescription(attributes.CardName);
        characterRoleText.text = EnumUtility.GetDescription(attributes.CharacterRole);
        passiveImage.sprite = ImageUtility.CreateSpriteFromTexture(attributes.PassiveImage);
        QImage.sprite = ImageUtility.CreateSpriteFromTexture(attributes.QImage);
        EImage.sprite = ImageUtility.CreateSpriteFromTexture(attributes.EImage);
        RImage.sprite = ImageUtility.CreateSpriteFromTexture(attributes.RImage);
    }
}

public class CharacterCardAttributes
{
    public Texture2D CardImage { get; set; }
    public CardName CardName { get; set; }
    public CharacterRole CharacterRole { get; set; }
    public Texture2D PassiveImage { get; set; }
    public Texture2D QImage { get; set; }
    public Texture2D EImage { get; set; }
    public Texture2D RImage { get; set; }
}