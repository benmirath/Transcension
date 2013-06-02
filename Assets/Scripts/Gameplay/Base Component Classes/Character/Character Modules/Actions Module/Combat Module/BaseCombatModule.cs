using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICombat {
	void ApplyDamage (float damage);
	void ApplyHitStun (float hitStrength, Vector3 hitDir);
}

public class BaseCombatModule : ICombat {
	protected ICharacter user;
//	protected IPhysics charPhysics;
	protected ICharacterClassicStats charStats;

	public ICharacter User {
		get {return user;}
	}
//	public IPhysics CharPhysics {
//		get {return charPhysics;}
//	}
	public ICharacterClassicStats CharStats {
		get {return charStats;}
	}


	public BaseCombatModule () {

	}

	public void ApplyDamage (float damage) {

	}

	/// <summary>
	/// Applies hit stun to the character, and activates the character's stunned state if the max value is exceeded.
	/// </summary>
	/// <param name='hitStrength'> Strength (physical, not statistical) of the attack. </param>
	/// <param name='hitDir'> Direction in which the attack pushes the character. </param>
	public void ApplyHitStun (float hitStrength, Vector3 hitDir) {
		charStats.StunResistance.CurValue += hitStrength;
		if (charStats.StunResistance.CurValue >= charStats.StunResistance.MaxValue)
		{
			Debug.Log ("Beginning stunned");
			charStats.StunResistance.CurValue = charStats.StunResistance.MinValue;
			//charPhysics.KnockDir = hitDir * Time.deltaTime * charStats.StunStrength;				//new Vector3(hitDir.x, hitDir.y, hitDir);
			
			//user.CharBase.StartCoroutine (StunnedState());
			//TransitionToStunned();
		}
	}

	protected IEnumerator HitStun () {
#if DEBUG
		if (user.Rigid != null) Debug.Log("rigidbody is attached");
#endif
		float _timer = 0; 
		_timer = Time.time + user.CharStats.StunDuration;
		user.Rigid.isKinematic = false;
		
		while (_timer - Time.time >= 0.05f)								//Initial Knockback
		{
			Debug.LogWarning("STUNNED AND MOVING");
			//user.Controller(user.CharPhysics.KnockDir);
			yield return new WaitForFixedUpdate();
		}
		while (_timer - Time.time >0)									//Stun Period
		{
			yield return null;
		}
		user.Rigid.isKinematic = true;
		//user.CharPhysics.KnockDir = Vector3.zero;
		
//		if (WeaponReady) state = CharState.CombatReady;
//		else state = CharState.Idle;
//		StateTransition();
		yield break;

	}
	protected IEnumerator RecoilStun () {
		#if DEBUG
		if (user.Rigid != null) Debug.Log("rigidbody is attached");
		#endif
		float _timer = 0; 
		_timer = Time.time + user.CharStats.StunDuration;
		//user.CharPhysics.Body.isKinematic = false;

		while (_timer - Time.time >0)									//Stun Period
		{
			yield return null;
		}
		//user.CharPhysics.Body.isKinematic = true;
		//user.CharPhysics.KnockDir = Vector3.zero;
		
		//		if (WeaponReady) state = CharState.CombatReady;
		//		else state = CharState.Idle;
		//		StateTransition();
		yield break;

	}
}


