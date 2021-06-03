using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System.Linq;

public class DirectionalForceBehaviour : ForceBehaviourBase {	

	public Vector3 ForceVector;

	public float Radius;

	public DirectionalForceBehaviour ()
	{
		ForceInstance = new DirectionalForce();
	}

	protected override void ForceUpdate(){
		var instance = (DirectionalForce)ForceInstance;
		instance.ForceVector = ForceVector;
		instance.ForceRadius = Radius;
	}
}
