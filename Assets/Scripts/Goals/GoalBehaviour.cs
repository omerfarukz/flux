using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GoalBehaviour : MonoBehaviour {

	private SpriteRenderer _spriteRenderer;
	private SpriteRenderer _spriteRenderer_2;
	private SpriteRenderer _spriteRenderer_3;

	private bool _isStarted;
	private Text _percentText;
	private float _lastHitDeltaTime;
	private bool _audioStoped = false;

	public float StartAlpha = 0.4f;
	public Color Color = Color.white;
	public float MaxDistance = 5f;
	public float HitFactor = 0.05f;
	public float DownFactor = 0.05f;
	public float TimeToDown = 0.2f;
	public float MaxAlpha = 1.5f;
	public bool IgnoreColor;
	public float CurrentAlpha;
	public float? LastHit;
	public float PercentAnimationStartDeltaTime = 1f;
	public AudioClip HitAudio;

	private AudioSource _hitAudioSource;

	public float Percent{
		get{
			if(!LastHit.HasValue)
				return 0f;

			if(_spriteRenderer.color.a>=1f)
			{
				return 100f;
			}
			else
			{
				var start = 1f - StartAlpha;
				var current  = _spriteRenderer.color.a - StartAlpha;
				
				return (100f * current) / start;
			}
		}
	}

	public bool Apply(ParticleSystem.Particle particle){
		if(!IgnoreColor && particle.color != this.Color)
			return false;

		var distance = Vector3.Distance(transform.position, particle.position);
		if(distance > MaxDistance)
			return false;

		var newAlpha = _spriteRenderer.color.a + HitFactor;
		if(newAlpha > MaxAlpha)
			newAlpha = MaxAlpha;

		setAlpha(newAlpha);

		LastHit = Time.time;
		return true;
	}

	void Start(){
		_spriteRenderer = this.gameObject.GetChildrenByName<SpriteRenderer>("front");
		_spriteRenderer_2 = this.gameObject.GetChildrenByName<SpriteRenderer>("back");
		_spriteRenderer_3 = this.gameObject.GetChildrenByName<SpriteRenderer>("front2");

		_spriteRenderer.color = this.Color;
		_spriteRenderer_2.color = this.Color;
		_spriteRenderer_3.color = this.Color;

		_percentText = this.gameObject.GetChildrenByName<Canvas>("CanvasGoal").gameObject.GetChildrenByName<Text>("ForcePercentText");


		setAlpha(StartAlpha);
		ParticleManagerBehavior.Conditioner.AddGoal(this);
		_isStarted = true;
		LastHit = Time.time;

		_hitAudioSource = gameObject.AddComponent<AudioSource>();
		_hitAudioSource.volume = 0f;
		_hitAudioSource.time = Random.Range(0f,HitAudio.length*0.8f);
		_hitAudioSource.clip = HitAudio;
		_hitAudioSource.loop = true;
		_hitAudioSource.Play();
	}

	void FixedUpdate () {
		if(!LastHit.HasValue)
			return;

		_lastHitDeltaTime = Time.time - LastHit.Value;
		if(_lastHitDeltaTime < TimeToDown)
			return;

		var newAlpha = _spriteRenderer.color.a - (DownFactor);
		if(newAlpha < StartAlpha)
			newAlpha = StartAlpha;
		
		setAlpha(newAlpha);
	}

	void Update()
	{
		_percentText.transform.position = transform.position;
		_percentText.text = string.Format("{0:##}", Percent);

		var anim = _percentText.GetComponent<Animator>();
		anim.SetBool("IsActive", LastHit.HasValue && _lastHitDeltaTime < PercentAnimationStartDeltaTime);

		var animator = _spriteRenderer.GetComponent<Animator>();
		animator.SetBool("IsActive", CurrentAlpha>0.6f);

		if(_audioStoped)
		{
			_hitAudioSource.volume = Mathf.Lerp(_hitAudioSource.volume, 0f, 2f * Time.deltaTime);
			return;
		}

		if(LevelManager.Instance != null && LevelManager.Instance.ActiveLevel != null &&
		   LevelManager.Instance.ActiveLevel.CurrentCompleted && !_audioStoped)
		{
			_audioStoped = true;
		}
		else
		{
			_hitAudioSource.volume = Mathf.Max(0f, Mathf.Min(0.7f,(Percent/100f-0.1f)));
		}
	}

	void setAlpha(float value){
		//swap
		var currentColor = _spriteRenderer.color;
		currentColor.a = value;

		_spriteRenderer.color = currentColor;
		_spriteRenderer_2.color = currentColor;
		_spriteRenderer_3.color = currentColor;


		CurrentAlpha = value;
	}

	public bool IsCompleted(){
		if(!_isStarted)
			return false;

		if(_spriteRenderer == null)
			return false;

		return Percent == 100f;
	}

}
