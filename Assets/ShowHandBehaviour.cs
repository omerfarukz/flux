using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShowHandBehaviour : MonoBehaviour {

	public Color ImageColor = Color.white;
	public Transform Button;
	public float Delay = 1.5f;

	// Use this for initialization
	void Start () {
		StartCoroutine(Show());
	}

	IEnumerator Show(){
		//Drag2dObject.Enabled = false;

		yield return new WaitForEndOfFrame();

		var imageComponent = GetComponent<Image>();
		var zeroAlphaColor = ImageColor;
		zeroAlphaColor.a = 0.1f;
		
		imageComponent.color = zeroAlphaColor;
		var bPos = new Vector3(0f, 0f, 0f);
		
		print (Button.position);
		print ("100");
		//var buttonPos = ItemButtonBehaviour.
		var te = new iTweenExt()
			.Delay(Delay)
			.MoveTo(gameObject, Camera.main.WorldToScreenPoint(new Vector3(0f, 4f, 0f)), 0f)
				.AddAction(()=>{
					imageComponent.color = ImageColor;
				}, 1f)
				.Delay(0.3f)
				.MoveTo(gameObject, currentPos() + bPos, 1f)
				.AddAction(()=>{
					gameObject.GetComponent<AudioSource>().Play();
				}, 1f)
				.MoveTo(gameObject, currentPos() + new Vector3(-20f, -60f, 0f), 1f)
				.Delay(0.5f)
				.MoveTo(gameObject, currentPos() + bPos, 1f)
				.AddAction(()=>{
					gameObject.GetComponent<AudioSource>().Play();
				}, 1f)
				.Delay(0.5f)
				.AddAction(()=>{
					gameObject.SetActive(false);
					//Drag2dObject.Enabled = true;
				}, 0f);

		StartCoroutine(te.Run());
	}

	Vector3 currentPos(){
		print ("200 - " + Button.position);
		return Button.position;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
