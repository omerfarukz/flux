using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class LoadLevelGUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){
		GUIStyle gs = new GUIStyle();
		gs.fontSize = 18;
		gs.normal.textColor = Color.white;

//		if(GUI.Button(new Rect(20f, 20f, 100f, 100f), "Load level",gs)){
//			LevelManager.Instance.LoadLevel("Level1");
//		}
//		if(GUI.Button(new Rect(20f, 100f, 150f, 100f), "Reload master level",gs)){
//			ReloadMasterLevel();
//		}
	}

	void ReloadMasterLevel(){
		ParticleManagerBehavior.Conditioner.Forces.Clear();
		ParticleManagerBehavior.Conditioner.Particles.Clear();

		Application.LoadLevel("MasterLevel");
	}
}
