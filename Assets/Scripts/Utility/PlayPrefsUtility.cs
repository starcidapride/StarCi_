using Newtonsoft.Json;
using UnityEngine;

public class PlayerPrefsUtility
{
    public static T LoadFromPlayerPrefs<T>(string key) where T : class
    {
        string jsonString = PlayerPrefs.GetString(key);

        if (!string.IsNullOrEmpty(jsonString))
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }
        else 
        {
            return null;
        }
    }

    public static void SaveToPlayerPrefs(string key, object value)
    {
        PlayerPrefs.SetString(key, JsonConvert.SerializeObject(value));
    }
}