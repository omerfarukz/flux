using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShowHandLevel1 : MonoBehaviour {

	public Color ImageColor = Color.white;
	public Transform Force;
	public Transform ForcePlace0;
	public Transform ForcePlace;

	public float Delay = 1f;
	
	// Use this for initialization
	void Start () {
		Drag2dObject.Enabled = false;

		var te = new iTweenExt()
				.Delay(Delay)
				.MoveTo(gameObject, Force.position + new Vector3(0f, -3f, 0f), 1f)
				.AddAction(()=>{
					gameObject.GetComponent<AudioSource>().Play();
					gameObject.transform.SetParent(Force);
				}, 0.1f)
				.Delay(0.6f)
				.MoveTo(Force.gameObject, ForcePlace0.position, 0.5f)
				.Delay(0.3f)
				.MoveTo(gameObject, ForcePlace0.position + new Vector3(3f, -5f, 0f), 0.5f)
				.Delay(0.3f)
				.MoveTo(gameObject, ForcePlace0.position + new Vector3(0f, -3f, 0f), 0.5f)
				.Delay(0.3f)
				.AddAction(()=>{
					gameObject.GetComponent<AudioSource>().Play();
					gameObject.transform.SetParent(Force);
				}, 0.1f)
				.MoveTo(Force.gameObject, ForcePlace.position, 1f)
				.Delay(0.5f)
				.MoveTo(gameObject, Vector3.zero, 0.5f)
				.AddAction(()=>{
					gameObject.SetActive(false);
					Drag2dObject.Enabled = true;
				}, 0.1f);
		
		StartCoroutine(te.Run());
	}
}
