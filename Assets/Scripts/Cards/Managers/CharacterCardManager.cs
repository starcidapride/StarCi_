using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCardManager : CardManager
{
    [SerializeField]
    private Image cardImage;

    public void SetAttributes(CharacterCardAttributes attributes)
    {
        CardName = attributes.CardName;

        cardImage.sprite = ImageUtility.CreateSpriteFromTexture(attributes.CardImage);
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