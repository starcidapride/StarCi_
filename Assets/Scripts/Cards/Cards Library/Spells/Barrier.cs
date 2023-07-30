using UnityEngine;
public class Barrier : ISpellCard
{
    public Texture2D Image { get; } = PathUtility.LoadCardImage(CardName.Barrier);

    public string Description { get; } = "Barrier";

}