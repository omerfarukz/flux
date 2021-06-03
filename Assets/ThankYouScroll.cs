using UnityEngine;
using System.Collections;

public class ThankYouScroll : MonoBehaviour {

	private float StartTime = 0f;

	public float Delay = 2f;
	public float Speed = 1.2f;
	public Transform EndObject;

	void Start(){
		StartTime = Time.time;

		var currentPos = gameObject.transform.position;
		currentPos.y = Screen.height / 2;

		gameObject.transform.position = currentPos;

		MusicSingleton.StartAudio("General");
	}

	void Update () {
		if(Input.touchCount>0 || (Input.mousePresent && Input.GetMouseButtonDown(0)))
		{
			Application.LoadLevel("StartupScene");
			return;
		}

		if(EndObject.position.y > Screen.height)
		{
			//TODO: would you like to vote this game?
			Application.LoadLevel("StartupScene");
		}
		else if(Time.time - StartTime > Delay)
		{
			var currentPos = gameObject.transform.position;
			currentPos.y += Speed;


			gameObject.transform.position = currentPos;
		}
	}
}
