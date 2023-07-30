using UnityEngine;

public class ShieldOfTheLost : IEquipmentCard
{
    public Texture2D Image { get; } = PathUtility.LoadCardImage(CardName.ShieldOfTheLost);
    public EquipmentClass EquipmentClass { get; } = EquipmentClass.Defense;

    public int Price { get; } = 1000;

    public string Stats { get; } = "[Attack: 5]";
    public string Description { get; } = "Kiếm đẹp lắm";
}