using UnityEngine.EventSystems;
using UnityEngine;
using System.Linq;

public class ItemButtonPointerDownHandler : MonoBehaviour, IPointerDownHandler
{
	public string Name {
		get;
		set;
	}

	public void OnPointerDown (PointerEventData eventData)
	{
		var nextItem = ParticleManagerBehavior.Conditioner.Forces.FirstOrDefault(f=>f.ShowOnItemsPanel && f.IsVisible == false && f.Name == Name);
		if(nextItem == null)
		{
			Debug.LogWarning("there is no item");
			return;
		}
		
		var newPosition =  Camera.main.ScreenToWorldPoint(eventData.position);
		newPosition.z = 0f;

		nextItem.IsVisible = true;
		nextItem.transform.position = newPosition;

		Drag2dObject.Current = nextItem.gameObject;
	}
}