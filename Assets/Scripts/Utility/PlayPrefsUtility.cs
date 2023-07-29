using Newtonsoft.Json;
using UnityEngine;

public class PlayerPrefsUtility
{
    public static T LoadFromPlayPrefs<T>(string key)
    {
        return JsonConvert.DeserializeObject<T>(PlayerPrefs.GetString(key));
    }

    public static void SaveToPlayPrefs(string key, object value)
    {
        PlayerPrefs.SetString(key, JsonConvert.SerializeObject(value));
    }
}