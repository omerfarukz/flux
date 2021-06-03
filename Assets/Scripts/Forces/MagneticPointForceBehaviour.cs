using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System.Linq;

public class MagneticPointForceBehaviour : ForceBehaviourBase {	

	public float Factor;

	public float Radius;
	
	public bool Negate;

	public MagneticPointForceBehaviour ()
	{
		ForceInstance = new MagneticPointForce();
	}

	protected override void ForceUpdate(){
		var instance = (MagneticPointForce)ForceInstance;
		instance.ForceFactor = Factor;
		instance.ForceRadius = Radius;
		instance.ForceNegate = Negate;
	}
}
