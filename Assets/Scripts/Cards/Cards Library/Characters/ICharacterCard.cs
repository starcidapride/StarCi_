using UnityEngine;
public interface ICharacterCard : ICard
{
    public int Experience { get; }
    public CharacterRole CharacterRole { get; }
    public int MaxHealth { get; }
    public int AttackDamage { get; }
    public int Armor { get; }
    public int MagicResistance { get; }
    public string PassiveName { get; }
    public Texture2D PassiveImage { get; }
    public string PassiveDescription { get; }
    public string QName { get; }
    public Texture2D QImage { get; }
    public string QDescription { get; }
    public string EName { get; }
    public Texture2D EImage { get; }
    public string EDescription { get; }
    public string RName { get; }
    public Texture2D RImage { get; }
    public string RDescription { get; }
}

public enum AbilitySymbol
{
    Passive,
    Q,
    E,
    R
}
