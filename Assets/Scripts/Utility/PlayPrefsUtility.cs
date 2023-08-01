using Newtonsoft.Json;
using System.ComponentModel;
using UnityEngine;

public class PlayerPrefsUtility
{
    public static T LoadFromPlayerPrefs<T>(PlayerPrefsKey key) where T : class
    {
        string jsonString = PlayerPrefs.GetString(EnumUtility.GetDescription(key));

        if (!string.IsNullOrEmpty(jsonString))
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }
        else 
        {
            return null;
        }
    }

    public static void SaveToPlayerPrefs(PlayerPrefsKey key, object value)
    {
        PlayerPrefs.SetString(EnumUtility.GetDescription(key), JsonConvert.SerializeObject(value));
    }
}

public enum PlayerPrefsKey
{
    [Description("User")]
    User,

    [Description("Lobby")]
    Lobby
}