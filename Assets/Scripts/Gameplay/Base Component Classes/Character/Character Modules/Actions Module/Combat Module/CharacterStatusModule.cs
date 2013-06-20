using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStatus {
	void ApplyDamage (float damage);
	void ApplyHitStun (float hitStrength, Vector3 hitDir);
}

[System.Serializable]
public class CharacterStatusModule : IStatus {
	#region Properties
	protected BaseCharacter user;
//	protected IPhysics charPhysics;
	protected ICharacterClassicStats charStats;
	
	[SerializeField] protected MovementProperties hitStun;			//when enough hits max out the stun meter, puts the character in hitstun
	[SerializeField] protected MovementProperties knockback;			//when an attack has knockback and stuns/hits a stunned character, puts the character in knockback
	[SerializeField] protected MovementProperties attackRecoil;		//when an attack is deflected, puts the character in attackRecoil



	public BaseCharacter User {
		get {return user;}
	}
//	public IPhysics CharPhysics {
//		get {return charPhysics;}
//	}
	public ICharacterClassicStats CharStats {
		get {return charStats;}
	}
	#endregion

	#region Methods
	public void ApplyVitalUse (float cost, IVital vit) {
		vit.CurValue -= cost;
	}

	public void ApplyDamage (float damage) {
		user.CharStats.Health.CurValue -= damage;
	}

	/// <summary>
	/// Applies hit stun to the character, and activates the character's stunned state if the max value is exceeded.
	/// </summary>
	/// <param name='hitStrength'> Strength (physical, not statistical) of the attack. </param>
	/// <param name='hitDir'> Direction in which the attack pushes the character. </param>
	public void ApplyHitStun (float hitStrength, Vector3 hitDir) {
		ApplyVitalUse (hitStrength, charStats.StunResistance);
//		if (charStats.StunResistance.CurValue >= charStats.StunResistance.MaxValue)
//		{
//			Debug.Log ("Beginning stunned");
//			charStats.StunResistance.CurValue = charStats.StunResistance.MinValue;
//			//charPhysics.KnockDir = hitDir * Time.deltaTime * charStats.StunStrength;				//new Vector3(hitDir.x, hitDir.y, hitDir);
//
//			//user.CharBase.StartCoroutine (StunnedState());
//			//TransitionToStunned();
//		}
	}
	public void ApplyAttack (AttackProperties attack) {
		ApplyDamage (attack.AdjustedDamageValue);
//		ApplyHitStun (attack.ImpactModifier)
	}
	
	protected IEnumerator HitStun () {
#if DEBUG
		if (user.Rigid != null) Debug.Log("rigidbody is attached");
#endif
		float _timer = 0; 
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
//		_timer = Time.time + user.CharStats.StunDuration;
		//user.CharPhysics.Body.isKinematic = false;

		while (_timer - Time.time >0)									//Stun Period
		{
			yield return null;
		}
		yield break;

	}
	#endregion
}


