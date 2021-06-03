using UnityEngine;
using System;

public class EnergyManager
{
	private bool _initialized = false;
	
	public static EnergyManager Instance = new EnergyManager();
	
	const string ENERGY_LAST_UPDATE_STATUS_TIME_KEY = "ENERGY_LAST_UPDATE_STATUS_TIME_KEY";
	const string ENERGY_PERCENT = "ENERGY_PERCENT";
	
	public int TotalSecondsForOnePercent {
		get;
		set;
	}
	
	public EnergyManager ()
	{
		TotalSecondsForOnePercent = 3;
	}
	
	public void Initialize()
	{
		if(_initialized)
			return;
		
		_initialized = true;
		
		if(!PlayerPrefs.HasKey(ENERGY_LAST_UPDATE_STATUS_TIME_KEY))
		{
			Debug.LogWarning("ilk defa acildi, enerji full yapiliyor");
			SetFull();
		}
	}
	
	public void SetFull ()
	{
		PlayerPrefs.SetInt(ENERGY_LAST_UPDATE_STATUS_TIME_KEY, OmrTime.GetCurrent());
		PlayerPrefs.SetFloat(ENERGY_PERCENT, 100f);
	}
	
	public void UpdateStatus(bool forced = false)
	{
		float currentPercent = PlayerPrefs.GetFloat(ENERGY_PERCENT);
		
		int currentTime = OmrTime.GetCurrent();
		int secondsSpanToLastUpdateStatus = currentTime - PlayerPrefs.GetInt(ENERGY_LAST_UPDATE_STATUS_TIME_KEY);

		// level disinda da energy update edilmeli
		if(forced || (!LevelManager.Instance.IsInGamePlay && !LevelManager.Instance.IsPaused))
		{
			float newEnergyPercentBetweenTwoUpdates = (1f / TotalSecondsForOnePercent) * secondsSpanToLastUpdateStatus;
			
			if(newEnergyPercentBetweenTwoUpdates < 0f) // if user changed the system time: yes! we are hacked
			{
				newEnergyPercentBetweenTwoUpdates = 0f;
			}
			
			currentPercent += newEnergyPercentBetweenTwoUpdates;
			
			if(currentPercent > 100f)
				currentPercent = 100f;
			
			if(currentPercent < 0f) // we are hacked
				currentPercent = 0f;
		}

		PlayerPrefs.SetInt(ENERGY_LAST_UPDATE_STATUS_TIME_KEY, currentTime);
		PlayerPrefs.SetFloat(ENERGY_PERCENT, currentPercent);
	}
	
	public float GetPercent()
	{
		return PlayerPrefs.GetFloat(ENERGY_PERCENT);
	}
	
	public DateTime GetNextFullDateTime()
	{
		return DateTime.Now.AddSeconds(GetNextFullSeconds());
	}
	
	public float GetNextFullSeconds()
	{
		var remainingPercent = 100f - GetPercent();
		var remainingSecondsToFull = Mathf.Ceil(remainingPercent * TotalSecondsForOnePercent);
		
		return remainingSecondsToFull;
	}

	internal void AddEnergy(float energy)
	{
		float currentPercent = PlayerPrefs.GetFloat(ENERGY_PERCENT);
		
		currentPercent += energy;
		
		if(currentPercent>100f)
			currentPercent = 100f;

		if(currentPercent<0f)
			currentPercent = 0f;


		PlayerPrefs.SetFloat(ENERGY_PERCENT, currentPercent);
	}
}