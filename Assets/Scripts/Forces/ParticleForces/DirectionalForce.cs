using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class DirectionalForce : IParticleForce {

	public Vector3 ForceVector {
		get;
		set;
	}

	public float ForceRadius {
		get;
		set;
	}

	#region IParticleForce implementation
	 
	public void Apply (Vector3 forcePosition, Vector3 forceRotation, ref ParticleSystem.Particle particleInstance)
	{
		var distance = Vector3.Distance(forcePosition, particleInstance.position);

		if(distance < ForceRadius){
			var forceVectorNormalized = (forcePosition - particleInstance.position).normalized;
			
			var distanceDifference = ForceRadius - distance;
			var forceVector = ForceVector + (forceVectorNormalized * distanceDifference);
			
			particleInstance.velocity += new Vector3(forceVector.x, forceVector.y, forceVector.z);
		}
	}

	#endregion

}