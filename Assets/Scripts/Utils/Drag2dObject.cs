using System;
using UnityEngine;
using AssemblyCSharp;

public class Drag2dObject : MonoBehaviour
{
	private Renderer _renderer;
	
	/// <summary>
	/// Dont set
	/// </summary>
	public static GameObject Current = null;
	public static bool Enabled = true;

	public float MouseDownDistance = 5f;
	public GameObject SheildObject;

	private Transform _sheild;

	void Start(){
		_renderer = GetComponent<Renderer>();
		_sheild = gameObject.GetChildrenByName<Transform>("Sheild");

		var lerpComponent = GetComponent<LerpBehaviour>();
		if(lerpComponent == null)
		{
			this.gameObject.AddComponent<LerpBehaviour>();
		}
	}

	private bool _dragging = false;
	private Vector3 _dragStartDiffPosition;

	// Update is called once per frame
	void Update () {
		if(!_renderer.enabled)
			return;

		if(!Enabled)
			return;

		var positionZ = transform.position.z;
		var inputPosition = GetInputPosition();
		

		if (inputPosition.HasValue) {
			var currentInputPosition = inputPosition.Value;
			currentInputPosition.z = positionZ;
			
			if(Current != null && Current != this.gameObject)
				return;

			if(!_dragging)
			{
				var currentMouseDistance = Vector3.Distance(currentInputPosition, transform.position);
				
				if(currentMouseDistance > MouseDownDistance)
				{
					return;
				}
				
				_dragging = true;
				_dragStartDiffPosition = transform.position - currentInputPosition;
			}

			currentInputPosition += _dragStartDiffPosition;

			Current = this.gameObject;
			
			if(_sheild!=null)
			{
				var sheildAnimator = _sheild.GetComponent<Animator>();
				sheildAnimator.SetBool("IsActive", true);
			}

			var lerpComponent = GetComponent<LerpBehaviour>();
			lerpComponent.StartLerp(transform.position, currentInputPosition, 10f);
			//transform.position = Vector3.Slerp(transform.position,;
		}
		else
		{
			if(_sheild != null)
			{
				var sheildAnimator = _sheild.GetComponent<Animator>();
				sheildAnimator.SetBool("IsActive", false);
			}

			Current = null;
			_dragging = false;
			_dragStartDiffPosition = Vector3.zero;
		}
	}
	
	Vector3? GetInputPosition() {
		Vector3? inputPosition = null;
		
		if(Input.touchSupported)
		{
			if(Input.touchCount != 1)
				return null;
			
			inputPosition = Input.GetTouch(0).position;
		}
		else if(Input.mousePresent && Input.GetMouseButton(0))
		{
			inputPosition = Input.mousePosition;
		}
		
		if(!inputPosition.HasValue)
			return null;
		
		return Camera.main.ScreenToWorldPoint(inputPosition.Value);
	}


}

