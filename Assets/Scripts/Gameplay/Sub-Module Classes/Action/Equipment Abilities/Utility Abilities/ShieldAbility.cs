using UnityEngine;
using System.Collections;

//public class ShieldAbility : WeaponAbility
//{
//	protected override void Awake ()
//	{
//		base.Awake ();
//		Stats.Speed = 15;
//	}
//	
//	protected override void Start ()
//	{
//		base.Start ();
//		Effect = ShieldEffect;
//	}
//	
//	
//	private void ShieldEffect()
//	{
//		Debug.Log ("the character is looking at " + Char.CharInput.LookDir);
//		//while(Char.MoveDir != Vector3.zero)
////		Char.Movement.UseAim();
//		if (Char.CharInput.MoveDir != Vector3.zero)
//		{
//			Debug.Log (Char.CharInput.LookDir);
////			Char.Movement.UseAim(Char.LookDir);
////			Char.Body.Move (Char.CharInput.MoveDir * Stats.Speed * Time.deltaTime);
////			Char.CurrentAnimState = BaseCharacter.AnimState.Defend;	
//		}
//		else
//		{
////			Char.CurrentAnimState = BaseCharacter.AnimState.Defend;
//		}	
//	}
//	
////	public void SetValues (string id, BaseCharacter.AnimState anim, float spd,  Vector3 rng, float start, float mid, float end)
////	{
////		name = id;
////		_anim = anim;
////		speed = spd;
////		range = rng;
////		
////		startupLength = start;
////		durationLength = mid;
////		cooldownLength = end;
////	}
//	
////	protected override void SetHitBox (bool active)
////	{
////		if (active == true)
////		{
////			Debug.LogWarning("shield activating");		
////			_hitBox.enabled = true;	
////			HitBox.size = Stats.Range;
////		}
////		if (active == false)
////		{
////			Debug.LogWarning("shield deactivating");
////			_hitBox.enabled = false;			
////			HitBox.size = Vector3.zero;
////		}
////	}
//	
//	
///*	protected void OnTriggerEnter (Collider hit)
//	{
//		if (hit.GetComponent<BaseWeapon>() == true && !hit.GetComponent<BaseShield>())
//		{
//			BaseWeapon weapon = hit.GetComponent<BaseWeapon>();
//			
//			Vector3 hitDir;
//			hitDir = (weapon.transform.position - _curWeapon.User.position).normalized;
//
//			weapon.Char.ApplyAttackRecoil(hitDir);
//		}
//	}*/
//	
//	protected override IEnumerator ActivateAbilityDuration ()
//	{
//		SetHitBox(true);
//		//float curDuration = Time.time + durationLength;												
//		do {
//			//Empty function that will hold the ability effect. Will be filled in at ability's initialization.
//			//Char.Movement.UseAim(Char.LookDir);
//			Effect();
//
//			yield return null;
//		} while (Char.CharState.IsDefending);
//		SetHitBox(false);	
//	}	
//}

