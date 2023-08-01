using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCardDetailsManager : MonoBehaviour
{
    [SerializeField]
    private Image passiveImage;

    [SerializeField]
    private Image qImage;

    [SerializeField]
    private Image eImage;

    [SerializeField]
    private Image rImage;

    [SerializeField]
    private TMP_Text cardNameText;

    [SerializeField]
    private TMP_Text maxHealthText;

    [SerializeField]
    private TMP_Text attackDamageText;

    [SerializeField]
    private TMP_Text armorText;

    [SerializeField]
    private TMP_Text magicResistanceText;

    private CardName cardName;
    private void Start()
    {
        var abilities = CardUtility.GetAbilityAttributes(cardName);

        passiveImage.GetComponent<AbilityHoverManager>().SetProperties(
            AbilitySymbol.Passive,
            abilities[AbilitySymbol.Passive].AbilityName,
            abilities[AbilitySymbol.Passive].AbilityDescription
            );

        qImage.GetComponent<AbilityHoverManager>().SetProperties(
            AbilitySymbol.Q,
            abilities[AbilitySymbol.Q].AbilityName,
            abilities[AbilitySymbol.Q].AbilityDescription
            );

        eImage.GetComponent<AbilityHoverManager>().SetProperties(
            AbilitySymbol.E,
            abilities[AbilitySymbol.E].AbilityName,
            abilities[AbilitySymbol.E].AbilityDescription
            );

        rImage.GetComponent<AbilityHoverManager>().SetProperties(
            AbilitySymbol.R,
            abilities[AbilitySymbol.R].AbilityName,
            abilities[AbilitySymbol.R].AbilityDescription
            );
    }

    public void SetProperties(CharacterCardDetailsProperties properties)
    {
        cardName = properties.CardName;

        passiveImage.sprite = ImageUtility.CreateSpriteFromTexture(properties.PassiveImage);
        qImage.sprite = ImageUtility.CreateSpriteFromTexture(properties.QImage);
        eImage.sprite = ImageUtility.CreateSpriteFromTexture(properties.EImage);
        rImage.sprite = ImageUtility.CreateSpriteFromTexture(properties.RImage);

        cardNameText.text = EnumUtility.GetDescription(properties.CardName);
        maxHealthText.text = properties.MaxHealth.ToString();
        attackDamageText.text = properties.AttackDamage.ToString();
        armorText.text = properties.Armor.ToString();
        magicResistanceText.text = properties.MagicResistance.ToString();
    }


}

public class CharacterCardDetailsProperties
{
    public Texture2D PassiveImage { get; set; }
    public Texture2D QImage { get; set; }
    public Texture2D EImage { get; set; }
    public Texture2D RImage { get; set; }
    public int MaxHealth { get; set; }
    public int AttackDamage { get; set;} 
    public int Armor { get; set; }
    public int MagicResistance { get; set;}

    public CardName CardName { get; set; }
}
