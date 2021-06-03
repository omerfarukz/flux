using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Facebook.Unity;

public class FacebookHelper
{
	public static FacebookHelper Instance = new FacebookHelper();

	private static object _lockObject = new object();

	private bool _autoInitAndLogin;
	private bool _autoLoginAndPublish;

	public bool DownloadProfileImage { get; set; }
	public string DownloadProfileImageUrl { get; set; }

	public string[] DefaultLoginOptions = new string[] {"user_about_me","public_profile","publish_actions"};

	public Texture2D Picture {
		get;
		set;
	}

	public string FirstName{
		get;
		set;
	}

	public bool IsReady {
		get
		{
			return FB.IsInitialized && FB.IsLoggedIn;
		}
	}

	public bool IsLoggedIn {
		get
		{
			return FB.IsLoggedIn;
		}
	}

	public bool IsInitialized {
		get
		{
			return FB.IsInitialized;
		}
	}

	public void Init()
	{
		if(!FB.IsInitialized)
		{
			FB.Init(OnInitComplete, OnHideUnity);
		}
	}

	public void Login(string[] options)
	{
		if(options==null)
			options = DefaultLoginOptions;

		lock (_lockObject) {
			if(!FB.IsLoggedIn)
			{
				FB.LogInWithPublishPermissions(options, LoginCallback);
			}
		}
	}

	public void LoadProfile ()
	{
		FB.API("me?fields=first_name,name,picture", HttpMethod.GET, (result) => {
			if(result.Error == null)
			{
				Debug.Log("get profile name and picture");

				IDictionary dict = Facebook.MiniJSON.Json.Deserialize(result.RawResult) as IDictionary;
				if(dict.Contains("first_name"))
				{
					FirstName = dict["first_name"].ToString();
				}

				if(dict.Contains("picture"))
				{
					var pictureDict = (IDictionary)dict["picture"];
					if(pictureDict.Contains("data"))
					{
						var pictureDataDict = (IDictionary)pictureDict["data"];
						if(pictureDataDict.Contains("url"))
						{
							DownloadProfileImageUrl = pictureDataDict["url"].ToString();
							DownloadProfileImage = true;
						}
					}
				}
			}
		});

	}

	public IEnumerator AutoDownloadProfilePicture(string url)
	{
		if(!string.IsNullOrEmpty(url))
		{
			DownloadProfileImage = false;

			var w = new WWW(url);
			while (!w.isDone) {
				yield return new WaitForSeconds(0.5f);
			}

			if(w.texture!=null)
				Picture = w.texture;
		}
	}
	
	public IEnumerator AutoFacebookLogin(string[] options)
	{
		_autoInitAndLogin = true;

		Init ();

		while (_autoInitAndLogin && !FB.IsInitialized) {
			yield return new WaitForSeconds(1);
		}

		Login(options);


		_autoInitAndLogin = false;
	}

	public IEnumerator AutoPublishImageAndText(string message, string url, Texture2D texture)
	{
		_autoLoginAndPublish = true;

		//statusText.text = "Initializing";

		Init ();
		
		yield return new WaitForSeconds(1);
		
		while(_autoLoginAndPublish && !FB.IsInitialized)
		{
			yield return new WaitForSeconds(1);
		}

		if(_autoLoginAndPublish)
		{
			//statusText.text = "Connecting to facebook account";

			Login(DefaultLoginOptions);
			
			while(_autoLoginAndPublish && !FB.IsLoggedIn)
			{	
				yield return new WaitForSeconds(1);
			}
			
			if(_autoLoginAndPublish)
			{
				PublishImageAndText(message, url, texture);
				//statusText.text = "Sharing completed";
			}
		}
		
		_autoLoginAndPublish = false;

		//statusPanel.gameObject.SetActive(false);
	}

	public void PublishImageAndText(string message, string url, Texture2D texture)
	{
		Init();
		Login(DefaultLoginOptions);

//		if(message==null && texture == null)
//		{
//			Debug.LogError("FacebookHelper: message and texture is null, request ignored");
//			return;
//		}
//
//		var wwwForm = new WWWForm();
//
//		if(texture==null)
//		{
//			Debug.LogWarning("Texture is null");
//		}
//		else
//		{
//			byte[] screenshot = texture.EncodeToPNG();
//			wwwForm.AddBinaryData("image", screenshot, "InteractiveConsole.png");
//		}
//
//		if(message==null)
//		{
//			Debug.LogWarning("message is null");
//		}
//		else
//		{
//			wwwForm.AddField("message", message);
//		}

		//FB.API("me/photos", HttpMethod.POST, PublishImageAndTextCallback, wwwForm);

		FB.Mobile.ShareDialogMode = ShareDialogMode.NATIVE;

		FB.ShareLink(
			new Uri(url),
			"Flux Game",
			message,
			callback: PublishImageAndTextCallback);
	}

	private void PublishImageAndTextCallback(IShareResult result)
	{
		if(result == null)
		{
			Debug.LogError("result returned as null");
		}
		else if(!string.IsNullOrEmpty(result.Error))
		{
			Debug.LogError(string.Concat("publish image and text is not worked, returned error message is ", result.Error, " and text is ", result.RawResult));
		}
		else
		{
			Debug.Log("content published to facebook successufuly");
		}
	}
	
	void LoginCallback(ILoginResult result)
	{
		if (result.Error != null)
		{
			Debug.LogError("Error Response:\n" + result.Error);
			_autoLoginAndPublish = false;
		}
		else if (!FB.IsLoggedIn)
		{
			Debug.LogWarning("Login cancelled by Player");
			_autoLoginAndPublish = false;
		}
		else if(!string.IsNullOrEmpty(result.RawResult))
		{
			Debug.Log("Login was successful!");
			LoadProfile();
		}
	}

	private void OnInitComplete()
	{
		Debug.Log("v3 - FB.Init completed: Is user logged in? " + FB.IsLoggedIn);
	}
	
	private void OnHideUnity(bool isGameShown)
	{
		Debug.Log("FacebookHelper: Is game showing? " + isGameShown);
	}
}