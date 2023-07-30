using System;
using System.ComponentModel;
using UnityEngine;

public class ImageUtility
{

    public static Texture2D DecodeBase64Image(string base64EncodedImage)
    {
        if (string.IsNullOrEmpty(base64EncodedImage)) return null;
        try
        {
            var imageBytes = Convert.FromBase64String(base64EncodedImage);
            var texture = new Texture2D(1, 1);

            texture.LoadImage(imageBytes);
            return texture;
        } catch (Exception ex) {
            
            Debug.Log(ex); 
            return null;         
        }

    }

    public static string EncodeBase64Image(Texture2D texture)
    {
        if (texture == null) return null;

        byte[] imageBytes = texture.EncodeToPNG();

        string base64EncodedImage = Convert.ToBase64String(imageBytes);

        return base64EncodedImage;
    }

    public static Sprite CreateSpriteFromTexture(Texture2D texture, float pixelPerUnits = 1f)
    {
        if (texture == null) return null;
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), pixelPerUnits);
    }

    public static Color GetColorFromHexEnum(HexEnum hexEnum)
    {
        var hexString = EnumUtility.GetDescription(hexEnum);

        if (ColorUtility.TryParseHtmlString(hexString, out var color))
        {
            return color;
        } else
        {
            throw new FormatException($"Invalid hex string: {hexString}");
        }
    }
}

public enum HexEnum
{
    [Description("#F5F5F5")]
    Highlight,
    [Description("#C8C8C8")]
    Pressed

}