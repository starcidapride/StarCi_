using UnityEngine;

public class Invocation : IOtherCard
{
    public Texture2D Frame { get; } = PathUtility.LoadCardFrame(CardName.Invocation);
    public Texture2D Image { get; } = PathUtility.LoadCardImage(CardName.Invocation);
    public string Description { get; } = "Invocation";

}