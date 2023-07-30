using UnityEngine;

public interface IOtherCard : ICard
{
    public Texture2D Frame { get; }
    public string Description { get; }
}