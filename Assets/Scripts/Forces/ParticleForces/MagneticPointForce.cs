using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class MagneticPointForce : IParticleForce {

	public float ForceFactor {
		get;
		set;
	}

	public float ForceRadius {
		get;
		set;
	}

	public bool ForceNegate {
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
			var forceVector = (forceVectorNormalized * (distanceDifference + ForceFactor));

			if(ForceNegate)
				forceVector *= -1f;

			particleInstance.velocity += new Vector3(forceVector.x, forceVector.y, forceVector.z);
		}
	}

	#endregion

}