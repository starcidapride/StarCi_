using UnityEngine;
public class TelAnnas : ICharacterCard
{
    public Texture2D Image { get; } = PathUtility.LoadCardImage(CardName.TelAnnas);
    public int Experience { get; } = 1;

    public CharacterRole CharacterRole { get; } = CharacterRole.Marksman;

    public int MaxHealth { get; } = 1;

    public int AttackDamage { get; } = 1;

    public int Armor { get; } = 1;

    public int MagicResistance { get; } = 1;

    public string PassiveName { get; } = "The Morning Star";

    public Texture2D PassiveImage { get; } = PathUtility.LoadAbilityImage(CardName.TelAnnas, AbilitySymbol.Passive);
    public string PassiveDescription { get; } = "TC";

    public string QName { get; } = "Eagle Eye";

    public Texture2D QImage { get; } = PathUtility.LoadAbilityImage(CardName.TelAnnas, AbilitySymbol.Q);

    public string QDescription { get; } = "ASTT";

    public string EName { get; } = "Penetrating Shot";

    public Texture2D EImage { get; } = PathUtility.LoadAbilityImage(CardName.TelAnnas, AbilitySymbol.E);

    public string EDescription { get; } = "CMP";

    public string RName { get; } = "Arrow Of Chaos";

    public Texture2D RImage { get; } = PathUtility.LoadAbilityImage(CardName.TelAnnas, AbilitySymbol.R);
    public string RDescription { get; } = "KGSM";
}