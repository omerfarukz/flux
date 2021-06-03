using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class ItemButtonBehaviour : MonoBehaviour {

	private int _itemsCount;
	private Dictionary<string, GameObject> _buttons;
	private Dictionary<Vector3, string> _buttonHintPositions;

	public RectTransform NotEnoughHintCanvas; // bundan da kurtul

	
	public GameObject ItemButtonPrefab;
	public GameObject SpritePrefab;

	public Button HintButton;
	public Text HintCounterText;

	public float DistanceToHide = 30f;

	#region ItemsPanelIcons

	public Sprite ForceLeftIcon;
	public Sprite ForceLeft2xIcon;
	public Sprite ForceRightIcon;
	public Sprite ForceRight2xIcon;
	public Sprite ForceUpIcon;
	public Sprite ForceUp2xIcon;
	public Sprite ForceDownIcon;
	public Sprite ForceDown2xIcon;
	public Sprite ForceMagnetIcon;
	public Sprite ForceMagnetNegateIcon;
	public Sprite UnkownIcon;

	#endregion

	#region Hints
	
	public Sprite ForceLeftHint;
	public Sprite ForceLeft2xHint;
	public Sprite ForceRightHint;
	public Sprite ForceRight2xHint;
	public Sprite ForceUpHint;
	public Sprite ForceUp2xHint;
	public Sprite ForceDownHint;
	public Sprite ForceDown2xHint;
	public Sprite ForceMagnetHint;
	public Sprite ForceMagnetNegateHint;

	#endregion

	void Start () {
		_buttons = new Dictionary<string, GameObject>();
		_buttonHintPositions = new Dictionary<Vector3, string>();

		CreateOrUpdateItemButtons(true);

		HintManager.Instance.Initialize();
	}
	
	void Update () {
		CreateOrUpdateItemButtons(false);

		CheckItemsForHide();

		UpdateHintButton();
	}

	void UpdateHintButton ()
	{
		HintCounterText.text = HintManager.Instance.Current.ToString();

		if(_buttonHintPositions == null || _buttonHintPositions.Count == 0)
		{
			HintButton.interactable = false;
		}
	}

	public void ShowHint ()
	{
		if (_buttonHintPositions == null || _buttonHintPositions.Count == 0)
		{
			// there is no hint abailable in this level
			return;
		}

		MusicSingleton.PlayButtonClick();

		if(HintManager.Instance.Current <= 0)
		{
			Drag2dObject.Enabled = false;

			Debug.Log("not enough hint");
			NotEnoughHintCanvas.gameObject.SetActive(true);
			
			var panel = NotEnoughHintCanvas.gameObject.GetChildrenByName<RectTransform>("Panel");
			
			var newPosition = Camera.main.WorldToScreenPoint(Vector3.zero);
			panel.transform.position = newPosition;

			return;
		}
		else
		{
			HintManager.Instance.Add(-1);
		}

		ShowHintInternal ();
	}

	void ShowHintInternal ()
	{
		var nextForce = _buttonHintPositions.First ();
		GameObject newHintObject = Instantiate (SpritePrefab);
		newHintObject.transform.position = nextForce.Key;
		newHintObject.GetComponent<SpriteRenderer> ().sprite = GetHintImageByName (nextForce.Value);
		var lerpBehaviour = newHintObject.AddComponent<LerpBehaviour> ();
		var startPosition = Vector3.zero;
		if (_buttons.Keys.Contains (nextForce.Value))
			startPosition = Camera.main.ScreenToWorldPoint (_buttons [nextForce.Value].transform.position);
		lerpBehaviour.StartLerp (startPosition, nextForce.Key, 3f);
		_buttonHintPositions.Remove (nextForce.Key);
	}

	/// <summary>
	/// Creates the or update item buttons.
	/// </summary>
	/// <param name="create">If set to <c>true</c> create.</param>
	void CreateOrUpdateItemButtons (bool create)
	{
		if (ParticleManagerBehavior.Conditioner.Forces == null)
			return;

		var kindOfParticleNames = ParticleManagerBehavior.Conditioner.Forces.Where(f=>f.ShowOnItemsPanel).Select(f => f.Name).Distinct();

		if (kindOfParticleNames != null)
		{
			foreach (var name in kindOfParticleNames)
			{
				var namedItems = ParticleManagerBehavior.Conditioner.Forces.Where(f=>f.Name == name);

				string text = null;

				if(create)
				{
					text = namedItems.Count().ToString();

					CreateButton (name, text);
				}
				else // update
				{
					// TODO: Update
					var hiddenItemsCount = namedItems.Count(f=>!f.IsVisible);

					if(hiddenItemsCount == 0)
					{
						SetButtonState(name, false);
					}
					else
					{
						SetButtonState(name, true);
					}

					SetButtonText(name, hiddenItemsCount.ToString());
				}
			}
		}


		if(ParticleManagerBehavior.Conditioner.Forces != null)
		{
			var itemsPanelButtons = ParticleManagerBehavior.Conditioner.Forces.Where(f=>f.ShowOnItemsPanel);

			foreach (var item in itemsPanelButtons)
			{
				item.gameObject.SetActive(item.IsVisible);
			}

			//_buttonHintPositions.Add(name, new

		}


		if(create)
		{
			var hintableItems = ParticleManagerBehavior.Conditioner.HintableForces;
			foreach (var item in hintableItems) {
				_buttonHintPositions.Add(item.transform.position, item.Name);
			}
		}
	}

	void CheckItemsForHide ()
	{
		if (ParticleManagerBehavior.Conditioner.Forces == null)
			return;

		if(Drag2dObject.Current != null)
			return;


		foreach (var item in ParticleManagerBehavior.Conditioner.Forces.Where(f=>f.ShowOnItemsPanel && f.IsVisible)) {

			if(Drag2dObject.Current == item)
				continue;

			var buttonScreenPosition = _buttons[item.Name].transform.position;
			var forcePositon = Camera.main.WorldToScreenPoint(item.transform.position);

			var itemToButtonDistance = Vector2.Distance(forcePositon, buttonScreenPosition);
			if(itemToButtonDistance < DistanceToHide)
			{
				item.IsVisible = false;
			}
		}
	}

	void SetButtonState(string name, bool enabled)
	{
		var button = _buttons[name];
		var buttonComponent = button.GetComponent<Button>();

		buttonComponent.interactable = enabled;
	}

	void SetButtonText(string name, string text)
	{
		var button = _buttons[name];
		var textComponent = button.GetComponentInChildren<Text>();
		textComponent.text = text;
	}

	GameObject CreateButton (string name, string text)
	{
		var newButton = Instantiate(ItemButtonPrefab);
		newButton.transform.SetParent(this.transform);

		var imageComponent = newButton.GetComponent<Image>();
		imageComponent.sprite = GetIconByName(name);

//		var buttonRectTransform = newButton.GetComponent<RectTransform>();
//
//		buttonRectTransform.pivot = new Vector2(0f, 0f);
//		
//		var buttonRectTransformPosition = buttonRectTransform.transform.position;
//		buttonRectTransformPosition.x =  15f + _itemsCount  * 10f + (buttonRectTransform.rect.width * _itemsCount);
//		buttonRectTransformPosition.y = 15f;
//		buttonRectTransform.transform.position = buttonRectTransformPosition;	 

//		var textComponent = newButton.GetComponentInChildren<Text>();
//		textComponent.text = text;
//
		var pointerDownHandler = newButton.AddComponent<ItemButtonPointerDownHandler>();
		pointerDownHandler.Name = name;

		_buttons.Add(name, newButton);

		++_itemsCount;
		return newButton;
	}

	private Sprite GetIconByName(string name)
	{
		Sprite sprite;

		switch (name) {
		case "ForceLeft":
			sprite = ForceLeftIcon;
			break;
		case "ForceLeft2x":
			sprite = ForceLeft2xIcon;
			break;
		case "ForceRight":
			sprite = ForceRightIcon;
			break;
		case "ForceRight2x":
			sprite = ForceRight2xIcon;
			break;
		case "ForceUp":
			sprite = ForceUpIcon;
			break;
		case "ForceUp2x":
			sprite = ForceUp2xIcon;
			break;
		case "ForceDown":
			sprite = ForceDownIcon;
			break;
		case "ForceDown2x":
			sprite = ForceDown2xIcon;
			break;
		case "ForceMagnet":
			sprite = ForceMagnetIcon;
			break;
		case "ForceMagnetNegate":
			sprite = ForceMagnetNegateIcon;
			break;
		default:
			sprite = UnkownIcon;
			break;
		}

		return sprite;
	}

	private Sprite GetHintImageByName(string name)
	{
		Sprite sprite;
		
		switch (name) {
		case "ForceLeft":
			sprite = ForceLeftHint;
			break;
		case "ForceLeft2x":
			sprite = ForceLeft2xHint;
			break;
		case "ForceRight":
			sprite = ForceRightHint;
			break;
		case "ForceRight2x":
			sprite = ForceRight2xHint;
			break;
		case "ForceUp":
			sprite = ForceUpHint;
			break;
		case "ForceUp2x":
			sprite = ForceUp2xHint;
			break;
		case "ForceDown":
			sprite = ForceDownHint;
			break;
		case "ForceDown2x":
			sprite = ForceDown2xHint;
			break;
		case "ForceMagnet":
			sprite = ForceMagnetHint;
			break;
		case "ForceMagnetNegate":
			sprite = ForceMagnetNegateHint;
			break;
		default:
			sprite = UnkownIcon;
			break;
		}
		
		return sprite;
	}

}
