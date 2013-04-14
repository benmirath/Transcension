using UnityEngine;
using System.Collections;

//public class Stinger : WeaponAbility.SpecialAttack
//{
//	private bool _stopAbility;
//	private Vector3 dir;
//	
//	protected override void Awake ()
//	{
//		base.Awake ();
//		Stats.Speed = 20;
//		_stopAbility = false;
////		Debug.LogWarning("STINGER TEST");
//	}
//
//	protected override void SpecialEffect ()
//	{
//		Debug.Log ("STINGER EFFECT ACTIVE");
////		Char.Body.Move (Char.Coordinates.up * Stats.Speed * Time.deltaTime);
//	}
//	
//	protected override IEnumerator ActivateAbilityDuration ()
//	{
//		Debug.Log ("STINGER ACTIVATED");
//		//Sets length of time at which ability's duration will be finished
//		SetHitBox(true);
//		float curDuration = Time.time + Stats.DurationLength;		
//		//dir = Char.LookDir;
//		do {
//			//empty function that will hold the ability effect. Will be filled in at ability's initialization.
//			if (_stopAbility == true) yield break;
//			Effect();																		
//			yield return null;
//		} while (curDuration > Time.time);
//		SetHitBox(false);
//		yield break;
//	}
//	
//	protected override void OnTriggerEnter (Collider hit)
//	{
//		base.OnTriggerEnter (hit);
//		if (hit.GetComponent<BaseEnemy>())
//			_stopAbility = true;
//	}
//}

