public interface IEquipmentCard : ICard
{   
    public EquipmentClass EquipmentClass { get; }
    public int Price { get; }

    public string Stats { get; }
    public string Description { get; }
}