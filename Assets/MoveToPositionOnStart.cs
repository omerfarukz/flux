using UnityEngine;
using System.Collections;

public class MoveToPositionOnStart : MonoBehaviour {

	private Vector3 _startupPosition;

	public Vector3 SlideDelta = Vector3.zero;
	public float Duration = 2f;

	void Start () {
		_startupPosition = transform.position;
		transform.position = _startupPosition + SlideDelta;
	}

	void Update () {
		iTween.MoveTo(this.gameObject, _startupPosition, Duration);
	}
}