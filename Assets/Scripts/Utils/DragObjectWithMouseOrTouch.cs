using System;
using UnityEngine;
using AssemblyCSharp;

public class DragObjectWithMouseOrTouch : MonoBehaviour
{
	private Renderer _renderer;

	/// <summary>
	/// Dont set
	/// </summary>
	public static GameObject Current = null;
	public float MouseDownDistance = 12f;
	public GameObject SheildObject;

	void Start(){
		_renderer = GetComponent<Renderer>();
	}

	// Update is called once per frame
	void Update () {
		if(!_renderer.enabled)
			return;

		var positionZ = transform.position.z;
		var inputPosition = GetInputPosition();

		var sheild = gameObject.GetChildrenByName<Transform>("Sheild");

		if (inputPosition != Vector3.zero) {
			inputPosition.z = positionZ;

			if(Current != null && Current != this.gameObject)
				return;

			var currentMouseDistance = Vector3.Distance(inputPosition, transform.position);

			if(currentMouseDistance > MouseDownDistance)
			{
				return;
			}

			Current = this.gameObject;

			if(sheild!=null)
			{
				var sheildAnimator = sheild.GetComponent<Animator>();
				sheildAnimator.SetBool("IsActive", true);
			}

			transform.position = inputPosition;
		}
		else
		{
			if(sheild != null)
			{
				var sheildAnimator = sheild.GetComponent<Animator>();
				sheildAnimator.SetBool("IsActive", false);
			}

			Current = null;
		}
	}

	Vector3 GetInputPosition() {
		var inputPosition = Vector3.zero;

		if(Input.touchSupported)
		{
			if(Input.touchCount != 1)
				return inputPosition;
			
			inputPosition = Input.GetTouch(0).position;
		}
		else if(Input.mousePresent && Input.GetMouseButton(0))
		{
			inputPosition = Input.mousePosition;
		}

		if(inputPosition == Vector3.zero)
			return inputPosition;

		return Camera.main.ScreenToWorldPoint(inputPosition);
	}
}

