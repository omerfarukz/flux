using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms.GameCenter;

public class StartupSceneBehaviour : MonoBehaviour {

	public bool AdsTestMode = false;

	public Button FacebookButton;
	public Text HelloText;
	public RawImage ProfileImage;

	void Awake () {
		if (Advertisement.isSupported && !Advertisement.isInitialized) {

			#if UNITY_IOS
			Advertisement.Initialize ("34775"); // prod icin test modu u tamemen sil
			#else
			Advertisement.Initialize ("34778"); // prod icin test modu u tamemen sil
			#endif
		}
		else
		{
			Debug.Log("Platform not supported");
		}

		//EnergyManager.Instance.AddEnergy(-70);
	}

	void Start()
	{
		Application.targetFrameRate = 30;
		MusicSingleton.StartAudio("General");

		LevelManager.Instance.Initialize();
		//FacebookHelper.Instance.Init();
	}
	
	// Update is called once per frame
	void Update () {
		EnergyManager.Instance.UpdateStatus();

		if(FacebookHelper.Instance.IsLoggedIn)
		{
			if(FacebookButton.gameObject.activeSelf)
			{
				FacebookButton.gameObject.SetActive(false);
			}

			if(HelloText!=null && FacebookHelper.Instance.FirstName != null)
			{
				HelloText.gameObject.SetActive(true);

				HelloText.text = "Hello, " + FacebookHelper.Instance.FirstName;
			}

//			if(false && ProfileImage!=null && FacebookHelper.Instance.Picture != null)
//			{
//				ProfileImage.gameObject.SetActive(true);
//
//				ProfileImage.texture = FacebookHelper.Instance.Picture;
//			}
//			else
//			{
//				var hwRect = HelloText.rectTransform.rect;
//				//hwRect.x = 0f;
//
//				//HelloText.rectTransform.rect.Set(60f, hwRect.top, hwRect.width, hwRect.height);
//			}
		}
		else
		{
			HelloText.gameObject.SetActive(false);
			ProfileImage.gameObject.SetActive(false);
		}
	}

	void LateUpdate()
	{
		if(FacebookHelper.Instance.DownloadProfileImage)
		{
			StartCoroutine(FacebookHelper.Instance.AutoDownloadProfilePicture(FacebookHelper.Instance.DownloadProfileImageUrl));
		}
	}

	void OnGUI()
	{
		if(Debug.isDebugBuild){
			if(GUI.Button(new Rect(10, 60, 130,50),"music first"))
			{
				MusicSingleton.StartAudio("General");
			}

			if(GUI.Button(new Rect(10, 130, 130,50),"music second"))
			{
				MusicSingleton.StartAudio("GamePlay");
			}
		}
	}

	public void LoadLevel_1()
	{
		MusicSingleton.PlayButtonClick();
		LevelManager.Instance.LoadNextLevel(true);
	}

	public void FacebookLogin()
	{
		StartCoroutine(FacebookHelper.Instance.AutoFacebookLogin(null));
	}
	
	public void LevelSelect()
	{
		MusicSingleton.PlayButtonClick();
		Application.LoadLevel("SelectLevel");
	}

	public void GotoThankYouScene()
	{
		MusicSingleton.PlayButtonClick();
		Application.LoadLevel("ThankYouScene");
	}

}