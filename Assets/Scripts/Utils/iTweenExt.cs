using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class iTweenExt
{
	public List<iTweenExtItem> Items {
		get;
		set;
	}

	public iTweenExt ()
	{
		Items = new List<iTweenExtItem>();

	}

	public iTweenExt AddAction(Action action, float time)
	{
		var txi = new iTweenExtItem();
		txi.TargetAction = action; //()=> { iTween.MoveTo(target, hash); };
		txi.Time = time;

		Items.Add(txi);

		return this;
	}

	public iTweenExt Delay(float time)
	{
		AddAction(null, time);
		
		return this;
	}

	public iTweenExt MoveTo(GameObject target, Vector3 position, float time)
	{
		AddAction(()=> { iTween.MoveTo(target, position, time); }, time);

		return this;
	}

	public iTweenExt FadeTo(GameObject target, float alpha, float time)
	{
		AddAction(()=> { iTween.FadeTo(target, alpha, time); }, time);
		return this;
	}

	public IEnumerator Run()
	{
		if(Items==null)
			yield return null;

		foreach (var item in Items) {

			if(item.TargetAction != null)
			{
				item.TargetAction();
			}

			yield return new WaitForSeconds(item.Time);
		}
	}

}

public class iTweenExtItem
{
	public Action TargetAction {
		get;
		set;
	}
	
	public float Time {
		get;
		set;
	}
}