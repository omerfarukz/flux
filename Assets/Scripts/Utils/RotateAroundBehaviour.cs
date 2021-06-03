using UnityEngine;
using System.Collections;

public class RotateAroundBehaviour : MonoBehaviour {

	public float Factor = 0.1f;

	// Update is called once per frame
	void Update () {
		var rotation = transform.rotation;
		rotation.z += Factor;
		transform.Rotate(Vector3.forward * Factor);
	}
}
