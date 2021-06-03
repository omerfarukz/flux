using System;
using UnityEngine;
using System.IO;

public class Texture2DHelper
{
	public static Texture2D textureFromFile(string filePath) {
		return textureFromBytes(textureBytesForFrame(filePath));
	}
	
	public static Texture2D textureFromBytes(byte[] bytes) {
		Texture2D texture2D = new Texture2D(1, 1);
		texture2D.LoadImage(bytes);
		return texture2D;
	}
	
	public static byte[] textureBytesForFrame(string imageURL) {
		return null; // todo:
		//return File.ReadAllBytes(imageURL);
	}

	public static Sprite CreateSprite(byte[] bytes)
	{
		var texture = textureFromBytes(bytes);
		return CreateSprite(texture);
	}

	public static Sprite CreateSprite(Texture2D texture)
	{
		return Sprite.Create(texture, new Rect(0f,0f,Screen.width, Screen.height), Vector2.zero);
	}
}