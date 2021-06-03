using System;
using UnityEngine;

public class LerpBehaviour : MonoBehaviour
{
	private bool _isStarted;
	private float _speed;

	public Vector3 FinalPosition;

	void Update()
	{
		if(!_isStarted)
			return;

		transform.position = Vector3.Lerp(transform.position, FinalPosition, _speed * Time.smoothDeltaTime);

		if(transform.position == FinalPosition)
			_isStarted = false;
	}

	public void StartLerp(Vector3 startPosition, Vector3 finalPosition, float speed)
	{
		transform.position = startPosition;
		FinalPosition = finalPosition;

		_speed = speed;
		_isStarted = true;
	}
}