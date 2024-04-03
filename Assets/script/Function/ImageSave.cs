using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class ImageSave
{
    public static string SpriteToBase64(Sprite sprite)
    {
        Texture2D texture = sprite.texture;
        byte[] textureBytes = texture.EncodeToPNG();
        return Convert.ToBase64String(textureBytes);
    }

    public static Sprite Base64ToSprite(string base64String)
    {
        byte[] textureBytes = Convert.FromBase64String(base64String);
        Texture2D texture = new Texture2D(1, 1);
        texture.LoadImage(textureBytes);
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
    }
}
