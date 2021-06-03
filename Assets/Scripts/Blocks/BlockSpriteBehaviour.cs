using UnityEngine;
using System.Collections;

public class BlockSpriteBehaviour : MonoBehaviour {

	public Vector3 Position {
		get;
		set;
	}
	
	public Vector3 Rotation {
		get;
		set;
	}
	
	public Vector3 LocalScale {
		get;
		set;
	}
	
	public string SpriteResourceName {
		get;
		set;
	}
	
	public float MeshExtrusion {
		get;
		set;
	}
	
	public bool IsConvex {
		get;
		set;
	}

	void Start(){
		if(SpriteResourceName==null)
			return;

		var spriteMeshCollider = gameObject.GetComponentInChildren<SpriteMeshColliderBehaviour>();
		spriteMeshCollider.Extrusion = MeshExtrusion;
		spriteMeshCollider.IsConvex = IsConvex;

		var spriteRendererInstance = gameObject.GetComponentInChildren<SpriteRenderer>();
		spriteRendererInstance.sprite = Resources.Load<Sprite>(SpriteResourceName);

		transform.position = Position;
		transform.rotation = Quaternion.Euler(Rotation);
		transform.localScale = LocalScale;
	}
}