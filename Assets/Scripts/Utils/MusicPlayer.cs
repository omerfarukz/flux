using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
	public Dictionary<string, AudioSource> AudioSources {
		get;
		set;
	}

	public MusicPlayer ()
	{
		AudioSources = new Dictionary<string, AudioSource>();
	}

	public void AddAudioClip(string key, AudioClip clip)
	{
		if(AudioSources.ContainsKey(key))
			return;

		var newAudioSource = gameObject.AddComponent<AudioSource>();
		newAudioSource.loop = true;
		newAudioSource.clip = clip;
		newAudioSource.volume = 0f;
		newAudioSource.Play();

		AudioSources.Add(key,  newAudioSource);
	}

	public void Play(string key)
	{
		foreach (var item in AudioSources) {
			if(item.Key != key)
			{
				item.Value.volume = 0f;
			}
			else
			{
				item.Value.volume = 1f;
			}
		}
	}
}