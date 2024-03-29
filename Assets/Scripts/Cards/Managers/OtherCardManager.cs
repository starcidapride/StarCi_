﻿using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class OtherCardManager : CardManager
{
    [SerializeField]
    private Image frameImage;

    [SerializeField]
    private Image cardImage;

    public void SetAttributes(OtherCardAttributes attributes)
    {
        CardName = attributes.CardName;

        frameImage.sprite = ImageUtility.CreateSpriteFromTexture(attributes.FrameImage);
        cardImage.sprite = ImageUtility.CreateSpriteFromTexture(attributes.CardImage);
    }
}

public class OtherCardAttributes
{
    public Texture2D FrameImage { get; set; }
    public Texture2D CardImage { get; set; }
    public CardName CardName { get; set; }
}