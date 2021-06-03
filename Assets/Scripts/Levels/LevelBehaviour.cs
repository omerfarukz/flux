using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Advertisements;
using System;

public class LevelBehaviour : MonoBehaviour {

	private Text _levelInfoText;
	private RectTransform _levelUpCanvas;
	private Image _star1;
	private Image _star2;
	private Image _star3;
	private Text _captionText;


	private Text _batteryEmptyInfoText;
	private float? _lastBatteryEmptyShowCloseTime;

	public Color StarColorComplated = Color.yellow;
	public Color StarColorNotComplated = Color.gray;

	
	// Use this for initialization
	void Awake (){
		_levelInfoText = this.gameObject.GetChildrenByName<RectTransform>("CanvasLevel").gameObject.GetChildrenByName<Text>("LevelInfo");
		_levelUpCanvas = this.gameObject.GetChildrenByName<RectTransform>("CanvasLevelUp");

		_star1 = this.gameObject.GetChildrenByName<Image>("Star1");
		_star2 = this.gameObject.GetChildrenByName<Image>("Star2");
		_star3 = this.gameObject.GetChildrenByName<Image>("Star3");
		_captionText = this.gameObject.GetChildrenByName<Text>("CaptionText");

		_levelUpCanvas.gameObject.SetActive(false);

		Drag2dObject.Enabled = true;

//		var statusPanel = _levelUpCanvas.gameObject.GetChildrenByName<RectTransform> ("Panel").gameObject.GetChildrenByName<RectTransform> ("SharePanel");
//		statusPanel.gameObject.SetActive(false);
		MusicSingleton.StartAudio("GamePlay");
	}

	void Start()
	{
		Time.timeScale = 1f;
	}

	void LateUpdate() {
		StartCoroutine(CheckForCaptureScreenShot());
	}

	void Update () {

		var avgPercentOfGoals = 0f;
		if(
			ParticleManagerBehavior.Conditioner.Goals!=null
			&& ParticleManagerBehavior.Conditioner.Goals.Count > 0
		)
		{
			avgPercentOfGoals = ParticleManagerBehavior.Conditioner.Goals.Select(s=>s.Percent).Average();
		}

		if(
			LevelManager.Instance.ActiveLevel != null
			&& !LevelManager.Instance.ActiveLevel.CurrentCompleted
		)
		{
			_levelInfoText.text = LevelManager.Instance.ActiveLevel.Title + "\r\n" + avgPercentOfGoals.ToString("##");

			if(avgPercentOfGoals == 100)
			{
				//Time.timeScale = 0f;

				if(!LevelManager.Instance.ActiveLevel.CurrentCompleted  && !CaptureScreenShot && ScreenShot == null && !Advertisement.isShowing)
				{
					CaptureScreenShot = true;
				}

				if(ScreenShot != null)
				{
					LevelManager.Instance.ActiveLevel.CurrentCompleted = true;
					LevelManager.Instance.ActiveLevel.Completed = true;

					var totalCount = ParticleManagerBehavior.Conditioner.Forces.Count(f=>f.ShowOnItemsPanel);
					var notVisibleCount = ParticleManagerBehavior.Conditioner.Forces.Count(f=> f.ShowOnItemsPanel && !f.IsVisible);

					if(totalCount <= 2)
					{
						SetStars(3);
					}
					else if(totalCount == 3 && notVisibleCount == 0)
					{
						SetStars(2);
					}
					else if(totalCount == 3 && notVisibleCount > 0)
					{
						SetStars(3);
					}
					else if(totalCount > 3)
					{
						if(notVisibleCount==0)
						{
							SetStars(1);
						}
						else if(notVisibleCount == 1)
						{
							SetStars(2);
						}
						else
						{
							SetStars(3);
						}
					}

					LevelManager.Instance.SetLevelState(LevelManager.Instance.ActiveLevel);

					ShowLevelUpCanvas ();
				}

				UpdateScreenShotImage (avgPercentOfGoals);
			}
		}
	}

	void SetStars (int i)
	{
		_star1.color = StarColorComplated;
		_star2.color = StarColorNotComplated;
		_star3.color = StarColorNotComplated;
		_captionText.text = "Done!";

		if(i == 2)
		{
			_star2.color = StarColorComplated;
			_captionText.text = "Good!";
		}
		else if(i == 3)
		{
			_star2.color = StarColorComplated;
			_star3.color = StarColorComplated;
			_captionText.text = "Perfect!";
		}

		var maxStarCount = Math.Max(LevelManager.Instance.ActiveLevel.CompletedWithStarsCount, i);

		LevelManager.Instance.ActiveLevel.CompletedWithStarsCount = maxStarCount;
	}

	void ShowLevelUpCanvas ()
	{
		Drag2dObject.Enabled = false;

		_levelUpCanvas.gameObject.SetActive (true);
		var panel = _levelUpCanvas.gameObject.GetChildrenByName<RectTransform> ("Panel");
		var newPosition = Camera.main.WorldToScreenPoint (Vector3.zero);
		panel.transform.position = newPosition;

		var levelUpBg = _levelUpCanvas.gameObject.GetChildrenByName<RectTransform>("bg");
		levelUpBg.GetComponent<Animator>().SetBool("IsActive", true);
	}

	void UpdateScreenShotImage (float avgPercentOfGoals)
	{
		if (avgPercentOfGoals == 100 && ScreenShot != null) {
			var screenShotImage = _levelUpCanvas.gameObject.GetChildrenByName<RectTransform> ("Panel").gameObject.GetChildrenByName<Image> ("ScreenShotImage");
			if (screenShotImage == null) {
				Debug.LogError ("screenshot image component not found");
			}
			else {
				screenShotImage.sprite = ScreenShot;
			}
		}
	}


	private bool CaptureScreenShot;
	private Sprite ScreenShot;

	void OnPostRender()
	{
		Debug.LogWarning (33);
	}

	IEnumerator CheckForCaptureScreenShot()
	{
		if(CaptureScreenShot && ScreenShot == null)
		{
			CaptureScreenShot = false;
			yield return new WaitForEndOfFrame();

			
			print ("kepcir");

			Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
			texture.ReadPixels(new Rect(0,0,Screen.width,Screen.height),0,0);
			texture.Apply();


			ScreenShot = Texture2DHelper.CreateSprite(texture);
		}
	}

	public void LoadNextLevel()
	{
		LevelManager.Instance.LoadNextLevel(false);
	}

	public void ShareWithFacebook()
	{
		MusicSingleton.PlayButtonClick();

		if(ScreenShot == null || ScreenShot.texture == null)
		{
			Debug.LogError("Screenshot or it's texture is null");
			return;
		}

		var screenShotImage = _levelUpCanvas.gameObject.GetChildrenByName<RectTransform> ("Panel").gameObject.GetChildrenByName<Image> ("ScreenShotImage");

		string[] messages = new string[] {
			"Very entertaining and thought-provoking games. See how I solve",
			"This game is designed for very clever people. I finished the course...",
			"See what I did. The solution here it is",
			"This game is burning up the human brain. I finally finished",
			"Incredible! See how I did it.",
			"Amazing! See how I did it.",
			"Wonderful! See how I did it.",
		};

		int randomIndex = UnityEngine.Random.Range(0, messages.Length - 1);

		string fbMessage = messages[randomIndex];// + " http://goo.gl/mW0zun";

		//Text shareText = sharePanel.GetChildrenByName<Text>("ShareText");

		StartCoroutine(FacebookHelper.Instance.AutoPublishImageAndText(fbMessage, "http://goo.gl/DFn88a", screenShotImage.sprite.texture));
	}

	public void RestartLevel(){
		MusicSingleton.PlayButtonClick();
		LevelManager.Instance.LoadLevel(LevelManager.Instance.ActiveLevel);
	}

	public void BackToMenu()
	{
		MusicSingleton.PlayButtonClick();
		Application.LoadLevel("StartupScene");
	}

	public void CloseCanvas(GameObject @GameObject)
	{
		MusicSingleton.PlayButtonClick();
		@GameObject.SetActive(false);
		Drag2dObject.Enabled = true;
	}

	public void WatchVideoAddHintCloseCanvas(GameObject canvasToHide)
	{
		Drag2dObject.Enabled = true;

		#if UNITY_EDITOR
		HintManager.Instance.Add(10);
		canvasToHide.gameObject.SetActive(false);
		return;
		#endif

		if(Advertisement.IsReady())
		{
			Advertisement.Show(null, new ShowOptions {
				pause = true,
				resultCallback = result => {
					if(result == ShowResult.Finished)
					{
						HintManager.Instance.Add(3);
						canvasToHide.SetActive(false);
					}
					
					Debug.Log(result.ToString());
				}
			});
		}
	}

	
	public void LevelSelect()
	{
		MusicSingleton.PlayButtonClick();
		Application.LoadLevel("SelectLevel");
	}

}
