using System;
using UnityEngine;
using System.IO;
using System.Collections;

public class ScreenShotHelper : MonoBehaviour
{
	public Texture2D Current;

	private string  _directoryFullPath;

	public ScreenShotHelper (string directoryPath)
	{
		_directoryFullPath = Application.dataPath + directoryPath;
	}

	public Texture2D RenderScreenToTexture()
	{
		Texture2D texture = new Texture2D(Screen.width, Screen.height);
		texture.ReadPixels(new Rect(0,0,Screen.width,Screen.height),0,0);
		texture.Apply();

		return texture;
	}

	public void Capture()
	{
		var frame = new WaitForEndOfFrame();

		Current = RenderScreenToTexture();

	}

	public void Initialize()
	{
		if(!Directory.Exists(_directoryFullPath))
			return;

		var filesInDirectory = Directory.GetFiles(_directoryFullPath);
		if(filesInDirectory != null)
		{
			foreach (var filePath in filesInDirectory) {
				File.Delete(filePath);
			}
		}
	}
}

