using AssemblyCSharp;
using UnityEngine;

public abstract class ForceBehaviourBase : MonoBehaviour {

	protected bool _initialized;
	protected IParticleForce _particleForce;

	public string Name;
	public bool ShowOnItemsPanel = true;
	public bool IsVisible;
	public bool IsHinted;
	public int HintOrder;
	public bool IsConstant;
	public bool IsGoal;

	internal IParticleForce ForceInstance { get; set; }

	void Awake(){
		ParticleManagerBehavior.Conditioner.AddForce(this);
	}

	protected abstract void ForceUpdate();

	void Update(){
		ForceUpdate();
	}
}