using UnityEngine;
using System.Collections;

public class DisplayFPSOnGUI : MonoBehaviour {

	int frameCount = 0;
	float dt = 0f;
	float fps = 0f;
	float updateRate = 100f;
	
	void Update()
	{
		frameCount++;
		dt += Time.deltaTime;
		if (dt > 1f/updateRate)
		{
			fps = frameCount / dt;
			frameCount = 0;
			dt -= 1f/updateRate;
		}
	}

	void OnGUI () {
		GUI.Label(new Rect(5f,5f,100,20f), string.Format("FPS: {0:n2}", fps));
	}
}
