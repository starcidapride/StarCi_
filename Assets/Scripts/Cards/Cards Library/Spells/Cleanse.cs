using UnityEngine;
public class Cleanse : ISpellCard
{
    public Texture2D Image { get; } = PathUtility.LoadCardImage(CardName.Cleanse);

    public string Description { get; } = "Cleanse";

}