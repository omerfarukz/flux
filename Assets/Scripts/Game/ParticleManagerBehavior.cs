using System;
using UnityEngine;
using AssemblyCSharp;
using System.Linq;

public class ParticleManagerBehavior : MonoBehaviour
{
	public bool BlendColors;

	public static ParticleManager Conditioner = new ParticleManager();
	
	void Start()
	{

	}

	void FixedUpdate(){
		Conditioner.Apply();
	}
}
