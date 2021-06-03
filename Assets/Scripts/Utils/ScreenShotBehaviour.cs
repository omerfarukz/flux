using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Collections;

public class ScreenShotBehaviour : MonoBehaviour {
	
	public static List<string> _fileList = new List<string>();
	
	private string ScreenShotDirectoryPath;
	
	public int PeriodInSeconds = 3;
	
	void Start () {
		ScreenShotDirectoryPath = Application.dataPath + "/ScreenShots/";
		
		if(!Directory.Exists(ScreenShotDirectoryPath))
		{
			Directory.CreateDirectory(ScreenShotDirectoryPath);
		}
		
		StartCoroutine("ScreenShotCoroutine");
	}
	
	void Update() {
		var renderer = GetComponent<SpriteRenderer>();
		var texture = textureFromFile(ScreenShotDirectoryPath + "/s.png");
		
		renderer.sprite = Sprite.Create(texture, new Rect(0f,0f,Screen.width, Screen.height), Vector2.zero);
	}
	
	IEnumerator ScreenShotCoroutine()
	{
		for(;;){
			yield return new WaitForSeconds(PeriodInSeconds);
			
			//TakeScreenShot();
		}
	}
	
	void TakeScreenShot()
	{
		Application.CaptureScreenshot(ScreenShotDirectoryPath + "/s.png");
	}
	
	Texture2D textureFromFile(string filePath) {
		return textureFromBytes(textureBytesForFrame(filePath));
	}
	
	Texture2D textureFromBytes(byte[] bytes) {
		Texture2D texture2D = new Texture2D(1, 1);
		texture2D.LoadImage(bytes);
		return texture2D;
	}
	
	byte[] textureBytesForFrame(string imageURL) {
		return null; // todo:
		//return File.ReadAllBytes(imageURL);
	}
}