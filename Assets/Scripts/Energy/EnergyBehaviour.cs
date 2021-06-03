using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBehaviour : MonoBehaviour {

	//private int ENERGY_FULL_NOTIFICATION_ANDROID_ID = 5;

	public int MinFullNotificationSeconds = 3;

	// Use this for initialization
	void Start () {
		EnergyManager.Instance.Initialize();
		InvokeRepeating("UpdatePercent", 0.5f, 2f);
	}

	void UpdatePercent()
	{
		EnergyManager.Instance.UpdateStatus();
	}

	void Update () {
		var textComponent = this.gameObject.GetChildrenByName<Text>("energy_text").GetComponent<Text>();
		var currentPercent = EnergyManager.Instance.GetPercent();

		textComponent.text = ((int)currentPercent).ToString();

		var cellComponent = this.gameObject.GetChildrenByName<Image>("battery_cells").GetComponent<Image>();
		cellComponent.fillAmount = (currentPercent + 0.0001f) / 100f;

		if(currentPercent> 70)
			cellComponent.color = Color.green;
		else if(currentPercent> 20)
			cellComponent.color = Color.yellow;
		else
			cellComponent.color = Color.red;
	}

	void OnApplicationQuit() {
		PlayerPrefs.Save();
	}

	public void OnApplicationPause(bool pauseStatus)
	{
		if(pauseStatus)
		{
			if(EnergyManager.Instance.GetPercent() < 100f)
			{
				long seconds = (long)EnergyManager.Instance.GetNextFullSeconds();

				Debug.Log("notification scheduled " + seconds + " seconds later");

				//UnityEngine.iOS.LocalNotification.SendNotification
			}
			else {
				//UnityEngine.iOS.LocalNotification.SendNotification
			}

			PlayerPrefs.Save();
		}
		else
		{
			// resume
			EnergyManager.Instance.UpdateStatus( forced:true ); // ekran kapandi acildi, oynama modunda olsanda enerjiyi update et
		}
	
	}
}