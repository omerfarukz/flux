using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteMeshColliderBehaviour : MonoBehaviour {

	private MeshCollider _customMeshCollider;

	public float Extrusion = 1.0f;
	public bool IsConvex = true;

	void Start ()
	{
		var currentMeshCollider = gameObject.GetComponent<MeshCollider>();
		
		if(currentMeshCollider != null)
			GameObject.DestroyImmediate(currentMeshCollider);
		
		var ploygonCollider = gameObject.GetComponent<PolygonCollider2D>();

		Mesh mesh = Triangulator.CreateMesh(ploygonCollider.points, Extrusion);

		DestroyImmediate(ploygonCollider);

		_customMeshCollider = gameObject.AddComponent(typeof(MeshCollider)) as MeshCollider;
		_customMeshCollider.sharedMesh = mesh;
		_customMeshCollider.convex = IsConvex;
		_customMeshCollider.isTrigger = false;
	}
}

