using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

public class BlowUpBuilding : StateMachineBehaviourEx {
	
	public static List<BlowUpBuilding> buildings = new List<BlowUpBuilding>();
	public static int count;
	
	Transform _explosions;
	Transform _fire;
	
	public enum BuildingStates
	{
		Normal,
		BlowingUp
	}
	
	void Start()
	{
		buildings.Add(this);
		count++;
		_explosions = transform.Find("Explosions");
		_fire = transform.Find("Fire");
	}
	
	void OnDestroy()
	{
		buildings.Remove(this);
		count = 0;
	}
	
	
	
	void TakeDamage()
	{
		currentState = BuildingStates.BlowingUp;
	}
	
	IEnumerator BlowingUp_EnterState()
	{
		foreach(var lp in GetComponentsInChildren<ActivateOnTrigger>())
		{
			lp.enabled = false;
		}
		_explosions.gameObject.SetActiveRecursively(true);
		yield return new WaitForSeconds(1);
		_fire.gameObject.SetActiveRecursively(true);
		yield return MoveObject(transform, transform.position - Vector3.up * 8, 3.4f);
		count--;
		if(count == 0)
		{
			var winner = GameObject.Find("Winner");
			winner.guiText.enabled = true;
			yield return new WaitForSeconds(8);
			winner.guiText.enabled = false;
			yield return new WaitForSeconds(4);
			Application.LoadLevel(Application.loadedLevel);
		}
		
	}
}
