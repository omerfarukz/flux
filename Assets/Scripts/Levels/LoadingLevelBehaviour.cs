using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using UnityEngine.UI;

public class LoadingLevelBehaviour : MonoBehaviour {

	private Text percentText;
	private RectTransform progressbarForeground;
	public float WaitSecondsBeforeLoad = 1.0f;
	public float WaitSecondsAfterLoad = 1.0f;

	void Awake(){
		percentText = gameObject.GetChildrenByName<Text>("percent");
		percentText.text = null;

		progressbarForeground = gameObject.GetChildrenByName<RectTransform>("progressbarForeground");

		var currentRect = progressbarForeground.rect;
		currentRect.width = 0f;

		//progressbarForeground.rect.Set(currentRect.x, currentRect.y, 0f, currentRect.height);
		progressbarForeground.transform.localScale = new Vector3(0.1f,progressbarForeground.transform.localScale.y,progressbarForeground.transform.localScale.z);
		Drag2dObject.Enabled = true;
	}

	void Start ()
	{
		MusicSingleton.StartAudio("General");

		Time.timeScale = 1f;

		if(LevelManager.Instance.ActiveLevel == null)
		{
			Debug.LogError("LoadingSceneParameters is not assigned");
			return;
		}

		ParticleManagerBehavior.Conditioner.Forces.Clear();
		ParticleManagerBehavior.Conditioner.Particles.Clear();

		var loadingText = gameObject.GetChildrenByName<Text>("title");
		loadingText.text = LevelManager.Instance.ActiveLevel.Title;

		var levelEnumerator = loadLevel();
		StartCoroutine(levelEnumerator);
	}

	IEnumerator loadLevel()
	{
		yield return new WaitForSeconds(WaitSecondsBeforeLoad);

		AsyncOperation levelAsync = Application.LoadLevelAsync(LevelManager.Instance.ActiveLevel.Name);
		levelAsync.allowSceneActivation = false;

		while (!levelAsync.isDone)
		{
			var percent = (levelAsync.progress * 110) + 1;

			percentText.text = string.Format("{0:##}%", percent);
			//progressbarForeground.rect.width = progressbarDefaultWidth * percent;
			var currentRect = progressbarForeground.rect;
			currentRect.width = percent;
			
			//progressbarForeground.rect = currentRect;
			progressbarForeground.transform.localScale = new Vector3(levelAsync.progress +0.1f,progressbarForeground.transform.localScale.y,progressbarForeground.transform.localScale.z);
			if(levelAsync.progress > 0.89f)
				break;

			yield return(0);
		}

		yield return new WaitForSeconds(WaitSecondsAfterLoad);

		levelAsync.allowSceneActivation = true;

		DestroyImmediate(this);
	}
	
}
