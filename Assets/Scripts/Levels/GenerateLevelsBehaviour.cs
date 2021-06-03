using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GenerateLevelsBehaviour : MonoBehaviour {

	private bool NextLevelIsDetected = false;
	private int NextLevelIndex = -1;

	public GameObject LevelMenuButtonInstance;
	public Color CompletedColor = Color.green;
	public Color NextLevelColor = Color.yellow;
	public Color NotCompletedColor = Color.gray;


	// Use this for initialization
	void Start () {
		LevelManager.Instance.Initialize();
		


		for (int i = 0; i < LevelManager.Instance.Levels.Count; i++) {
			AddLevel (i, LevelManager.Instance.Levels[i]);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void AddLevel (int i, Level level)
	{
		var newInstance = Instantiate (LevelMenuButtonInstance);
		var textComponent = newInstance.gameObject.GetChildrenByName<Text>("number");
		textComponent.text = (i+1).ToString();
		textComponent.fontSize = 54;
		textComponent.color = Color.black;

		newInstance.transform.SetParent (transform);
		newInstance.transform.localScale = Vector3.one;

		var buttonComponent = newInstance.GetChildrenByName<Button>("back");
		buttonComponent.onClick.AddListener(() => {
			LevelButtonClicked(i);
		});

		var star1 = newInstance.GetChildrenByName<Image>("star1");
		var star2 = newInstance.GetChildrenByName<Image>("star2");
		var star3 = newInstance.GetChildrenByName<Image>("star3");

		var lockedIcon = newInstance.GetChildrenByName<Image>("lock");
		lockedIcon.gameObject.SetActive(false);
		
		star1.gameObject.SetActive(false);
		star2.gameObject.SetActive(false);
		star3.gameObject.SetActive(false);

		var imageComponent = newInstance.GetChildrenByName<Image>("back");
		if(level.Completed)
		{
			imageComponent.color = CompletedColor;

			if(level.CompletedWithStarsCount == 1)
			{
				star1.gameObject.SetActive(true);
			}
			else if(level.CompletedWithStarsCount == 2)
			{
				star2.gameObject.SetActive(true);
			}
			else if(level.CompletedWithStarsCount == 3)
			{
				star3.gameObject.SetActive(true);
			}
		}
		else
		{
			if(!NextLevelIsDetected)
			{
				NextLevelIsDetected = true;
				NextLevelIndex = i;
				imageComponent.color = NextLevelColor;
				//textComponent.color = Color.black;
			}
			else
			{
				buttonComponent.interactable = false;
				imageComponent.color = NotCompletedColor;
				lockedIcon.gameObject.SetActive(true);
				textComponent.text = string.Empty;
			}
		}
	}

	public void LevelButtonClicked(int index)
	{
		var clickedLevel = LevelManager.Instance.Levels[index];
		if(clickedLevel.Completed || index == NextLevelIndex)
		{
			MusicSingleton.PlayButtonClick();
			LevelManager.Instance.LoadLevel(clickedLevel);
		}
	}

	
	public void BackToMenu()
	{
		MusicSingleton.PlayButtonClick();
		Application.LoadLevel("StartupScene");
	}

}
