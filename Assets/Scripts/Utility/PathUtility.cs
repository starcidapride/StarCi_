using System;
using System.Collections.Generic;
using UnityEngine;
public class PathUtility
{
    public static SortedDictionary<int, Texture2D> LoadAllIndexedPictures()
    {
        try
        {
            var loadedPictures = Resources.LoadAll<Texture2D>("Images/Pictures");

            var pictureDictionary = new Dictionary<int, Texture2D>();

            foreach (var loadedPicture in loadedPictures)
            {
                var index = int.Parse(loadedPicture.name);
                pictureDictionary.Add(index, loadedPicture);
            }

            return new SortedDictionary<int, Texture2D>(pictureDictionary, Comparer<int>.Create((x, y) => x.CompareTo(y)));
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
            return null;
        }
    }

    public static Texture2D LoadIndexedPicture(int index)
    {
        try
        {
            return Resources.Load<Texture2D>($"Images/Pictures/{index}");
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
            return null;
        }
    }

    public static Texture2D LoadCardImage(CardName cardName)
    {
        try
        {
            var cardNameDescription = EnumUtility.GetDescription(cardName);

            var cardPath = "Images/Cards";
            var dictionary = CardDictionary.GetCardDictionary();
            
            var path = dictionary[cardName].Item1 switch
            {
                CardType.Character => $"{cardPath}/Characters/{cardNameDescription}/Image",
                CardType.Equipment => $"{cardPath}/Equipments/{cardNameDescription}",
                CardType.Spell => $"{cardPath}/Spells/{cardNameDescription}",
                _ => $"{cardPath}/Others/{cardNameDescription}",
            };

            return Resources.Load<Texture2D>(path);
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
            return null;
        }
    }

    public static Texture2D LoadAbilityImage(CardName cardName, AbilitySymbol abilitySymbol)
    {
        try
        {
            var cardNameDescription = EnumUtility.GetDescription(cardName);

            var characterPath = "Images/Cards/Characters";
            var dictionary = CardDictionary.GetCharacterCardDictionary();

            var path = abilitySymbol switch
            {
                AbilitySymbol.Passive => $"{characterPath}/{cardNameDescription}/Passive",
                AbilitySymbol.Q => $"{characterPath}/{cardNameDescription}/Q",
                AbilitySymbol.E => $"{characterPath}/{cardNameDescription}/E",
                _ => $"{characterPath}/{cardNameDescription}/R",
            };

            return Resources.Load<Texture2D>(path);

        }
        catch (Exception ex)
        {
            Debug.Log(ex);
            return null;
        }
    }

    public static Texture2D LoadCardFrame(CardName cardName)
    {
        try
        {
            return Resources.Load<Texture2D>($"Images/Cards/Frames/Others/{cardName}");
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
            return null;
        }
    }

    public static Transform LoadCardPrefab(CardType cardType)
    {
        try
        {
            var path = cardType switch
            {
                CardType.Character => $"Prefabs/Cards/Character Card",
                CardType.Equipment => $"Prefabs/Cards/Equipment Card",
                CardType.Spell => $"Prefabs/Cards/Spell Card",
                _ => $"Prefabs/Cards/Other Card",
            };
            return Resources.Load<Transform>(path);
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
            return null;
        }
    }
}
