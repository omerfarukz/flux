using UnityEngine;
using System.Collections;
using AssemblyCSharp;

[ExecuteInEditMode]
public class ParticleSystemSettingBehaviour : MonoBehaviour {
	
	private ParticleSystem _birthEmitter;

	public ParticleSystem MainEmitter {
		get;
		set;
	}
	
	public bool UseCustomPositonAndRotation;
	public Vector3 Position = Vector3.zero;
	public Vector3 Rotation = Vector3.zero;
	public bool PlayOnAwake = true;
	public bool WorldCollisionEnabled = false;
	
	public Color StartColor = Color.cyan;
	public float MainShapeRadius = 0.9f;
	public float MainLifeTime = 1.5f;
	public float MainStartSpeed = 80.0f;
	public float MainSize = 1f;
	public int MainMaxParticles = 50;
	public float MainEmissionRate = 28.5f;
	
	public float BirthParticleColorAlpha = 0.2f;
	public float BirthShapeRadius = 0.1f;
	public float BirthLifeTime = 0.9f;
	public float BirthStartSpeed = 0f;
	public float BirthStartSize = 1f;
	public int BirthMaxParticles = 900;
	public float BirthEmissionRate = 30;

	// Use this for initialization
	void Awake () {
		MainEmitter = this.GetComponent<ParticleSystem>();
		_birthEmitter = gameObject.GetChildrenByName<ParticleSystem>("BirthEmitter");

		MainEmitter.playOnAwake = PlayOnAwake;
	}

	void Start(){
		ParticleManagerBehavior.Conditioner.AddParticleSystem(this);

		InternalUpdate();
	}

	void Update()
	{}

	void FixedUpdate()
	{
		if(MainEmitter.emissionRate > float.Epsilon)
		{
			if(LevelManager.Instance.IsInGamePlay && !LevelManager.Instance.IsPaused)
			{
				//EnergyManager.Instance.AddEnergy( -0.006f );
			}
		}

		if(EnergyManager.Instance.GetPercent() <= 0 || LevelManager.Instance.IsPaused)
		{
			MainEmitter.emissionRate = 0f;
		}
		else
		{
			MainEmitter.emissionRate = MainEmissionRate;
		}
	}

	void InternalUpdate ()
	{
		if (UseCustomPositonAndRotation) {
			transform.position = Position;
			transform.rotation = Quaternion.Euler (Rotation);
		}
		MainEmitter.startColor = StartColor;
		MainEmitter.startLifetime = MainLifeTime;
		MainEmitter.startSpeed = MainStartSpeed;
		MainEmitter.startSize = MainSize;
		MainEmitter.maxParticles = MainMaxParticles;
		MainEmitter.emissionRate = MainEmissionRate;
		_birthEmitter.startColor = new Color (StartColor.r, StartColor.g, StartColor.b, BirthParticleColorAlpha);
		_birthEmitter.startSize = BirthStartSize;
		_birthEmitter.startLifetime = BirthLifeTime;
		_birthEmitter.emissionRate = BirthEmissionRate;
	}
	//		#if UNITY_EDITOR
	//
	//		var soBirth = new UnityEditor.SerializedObject(_birthEmitter);
	//		soBirth.FindProperty("ShapeModule.radius").floatValue = BirthShapeRadius;
	//		soBirth.ApplyModifiedProperties();
	//
	//		var soMain = new UnityEditor.SerializedObject(_mainEmitter);
	//		soMain.FindProperty("ShapeModule.radius").floatValue = MainShapeRadius;
	//		soMain.FindProperty("CollisionModule.enabled").boolValue = WorldCollisionEnabled;
	//		
	//		soMain.ApplyModifiedProperties();
	//		#endif
}
