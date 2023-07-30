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
}