using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MusicSingleton : MonoBehaviour 
{
	private static MusicPlayer player;
	private static bool IsInitialized = false;

	public AudioClip GamePlayAudio;
	public AudioClip GeneralAudio;
	public AudioClip ButtonClickAudio;
	
	private static AudioSource ButtonClickSource;

	public static void StartAudio(string key)
	{
		if(IsInitialized && player!=null)
			player.Play(key);
	}

	public static void PlayButtonClick()
	{
		if(ButtonClickSource==null)
			return;

		ButtonClickSource.time = 0f;
		ButtonClickSource.Play();
	}

	void Awake()
	{
		if(IsInitialized)
		{
			//TODO;
			return;
		}

		DontDestroyOnLoad(this);


		player = gameObject.AddComponent<MusicPlayer>();
		player.AddAudioClip("General", GeneralAudio);
		player.AddAudioClip("GamePlay", GamePlayAudio);


		ButtonClickSource = this.gameObject.AddComponent<AudioSource>();
		ButtonClickSource.clip = ButtonClickAudio;
		ButtonClickSource.loop = false;
		ButtonClickSource.playOnAwake = false;

//		AudioSource.PlayClipAtPoint(ButtonClickAudio, Vector3.zero);

		IsInitialized = true;
	}
}