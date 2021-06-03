using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Facebook.Unity;

public class FacebookDemoBehaviour : MonoBehaviour {

	public Image img;

	void OnGUI()
	{
		if(GUI.Button(new Rect(10f, 10f, 100f, 100f), "FB INIT"))
		{
			CallFBInit();
		}
		
		if(GUI.Button(new Rect(10f, 140f, 100f, 100f), "FB LOGIN"))
		{
			CallFBLogin();
		}
		
		if(GUI.Button(new Rect(10f, 250f, 100f, 100f), "FB SCREEN SHOT"))
		{
			StartCoroutine("TakeScreenshot");
		}

	}
	
	private void CallFBLogin()
	{
		FB.LogInWithPublishPermissions(new string[] {"user_about_me","public_profile"}, LoginCallbackV2);
	}

	void LoginCallbackV2 (ILoginResult result)
	{
		if (result.Error != null)
			Debug.LogError("Error Response:\n" + result.Error);
		else if (!FB.IsLoggedIn)
		{
			Debug.LogWarning("Login cancelled by Player");
		}
		else
		{
			Debug.Log("Login was successful!");
		}
	}
//	
//	void LoginCallback(FBResult result)
//	{
//		if (result.Error != null)
//			Debug.LogError("Error Response:\n" + result.Error);
//		else if (!FB.IsLoggedIn)
//		{
//			Debug.LogWarning("Login cancelled by Player");
//		}
//		else
//		{
//			Debug.Log("Login was successful!");
//		}
//	}
	
	private void CallFBInit()
	{
		if(!FB.IsInitialized)
		{
			FB.Init(OnInitComplete, OnHideUnity);
		}
	}
	
	private void OnInitComplete()
	{
		Debug.Log("FB.Init completed: Is user logged in? " + FB.IsLoggedIn);
	}
	
	private void OnHideUnity(bool isGameShown)
	{
		Debug.Log("Is game showing? " + isGameShown);
	}
	
	private IEnumerator TakeScreenshot()
	{
		yield return new WaitForEndOfFrame();
		
		var width = Screen.width;
		var height = Screen.height;
		var tex = new Texture2D(width, height, TextureFormat.RGB24, false);
		// Read screen contents into the texture
		tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
		tex.Apply();
		byte[] screenshot = tex.EncodeToPNG();
		
		var wwwForm = new WWWForm();
		wwwForm.AddBinaryData("image", screenshot, "InteractiveConsole.png");
		wwwForm.AddField("message", "herp derp.  I did a thing!  Did I do this right?");
		
		FB.API("me/photos", HttpMethod.POST, CallbackV2, wwwForm);
	}

	void CallbackV2 (IGraphResult result)
	{
		Debug.Log("screenshot callback!");
		
		// Some platforms return the empty string instead of null.
		if (!string.IsNullOrEmpty (result.Error))
		{
			Debug.LogError("Error Response:\n" + result.Error);
		}
		else
		{
			Debug.LogError("Success:\n" + result.RawResult);
		}
	}
//	
//	protected void Callback(FBResult result)
//	{
//		Debug.Log("screenshot callback!");
//		
//		// Some platforms return the empty string instead of null.
//		if (!string.IsNullOrEmpty (result.Error))
//		{
//			Debug.LogError("Error Response:\n" + result.Error);
//		}
//		else if (!string.IsNullOrEmpty (result.Text))
//		{
//			Debug.Log("Success Response:\n" + result.Text);
//		}
//		else if (result.Texture != null)
//		{
//			Debug.Log("Success Response: texture\n");
//		}
//		else
//		{
//			Debug.LogError("Empty Response\n");
//		}
//	}
}
