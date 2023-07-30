using UnityEngine;
public class Heal : ISpellCard
{
    public Texture2D Image { get; } = PathUtility.LoadCardImage(CardName.Heal);
    public string Description { get; } = "Heal";

}