using System;
using System.Collections.Generic;
using System.ComponentModel;

public class CardDictionary
{
    public static Dictionary<CardName, Type> GetCharacterCardDictionary()
    {
        return new Dictionary<CardName, Type>()
    {
        { CardName.Arthur, typeof(Arthur) },
        { CardName.Baldum, typeof(Baldum) },
        { CardName.Cresht, typeof(Cresht) },
        { CardName.TelAnnas, typeof(TelAnnas) },
        { CardName.Peura, typeof(Peura) }
    };
    }

    public static Dictionary<CardName, Type> GetEquipmentCardDictionary()
    {
        return new Dictionary<CardName, Type>()
    {
        { CardName.FafnirsTalon, typeof(FafnirsTalon) },
        { CardName.ShieldOfTheLost, typeof(ShieldOfTheLost) },
    };
    }

    public static Dictionary<CardName, Type> GetSpellCardDictionary()
    {
        return new Dictionary<CardName, Type>()
    {
        { CardName.Barrier, typeof(Barrier) },
        { CardName.Cleanse, typeof(Cleanse) },
        { CardName.Heal, typeof(Heal) },
    };
    }

    public static Dictionary<CardName, Type> GetOtherCardDictionary()
    {
        return new Dictionary<CardName, Type>()
    {
        { CardName.Invocation, typeof(Invocation) },
    };
    }

    public static Dictionary<CardName, (CardType, Type)> GetCardDictionary()
    {
        var dictionary = new Dictionary<CardName, (CardType, Type)>();

        foreach (var kvp in GetCharacterCardDictionary())
        {
            dictionary.Add(kvp.Key, (CardType.Character, kvp.Value));
        }

        foreach (var kvp in GetEquipmentCardDictionary())
        {
            dictionary.Add(kvp.Key, (CardType.Equipment, kvp.Value));
        }

        foreach (var kvp in GetSpellCardDictionary())
        {
            dictionary.Add(kvp.Key, (CardType.Spell, kvp.Value));
        }

        foreach (var kvp in GetOtherCardDictionary())
        {
            dictionary.Add(kvp.Key, (CardType.Other, kvp.Value));
        }

        return dictionary;  
    }
}


public enum CardType
{
    Character = 1,
    Equipment = 2,
    Spell = 3,
    Other = 4
}

public enum CharacterRole
{
    Warrior = 1,
    Tank = 2,
    Support = 3,
    Mage = 4,
    Marksman = 5,
    Assassin = 6
}

public enum EquipmentClass
{
    Attack = 1,
    Magic = 2,
    Defense = 3
}

public enum CardName
{
    [Description("Arthur")]
    Arthur,
    [Description("Baldum")]
    Baldum,
    [Description("Cresht")]
    Cresht,
    [Description("Peura")]
    Peura,
    [Description("Tel'Annas")]
    TelAnnas,


    [Description("Fafnir's Talon")]
    FafnirsTalon,
    [Description("Shield Of The Lost")]
    ShieldOfTheLost,


    [Description("Barrier")]
    Barrier,
    [Description("Cleanse")]
    Cleanse,
    [Description("Heal")]
    Heal,

    [Description("Invocation")]
    Invocation
}