using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStatus {
	void ApplyAttack (AttackProperties attack, Transform instigator);
	void ApplyDamage (float damage);
	void ApplyHitStun (float hitStrength, Vector3 hitDir);

}

[System.Serializable]
public class CharacterStatusModule : IStatus {
	#region Properties
	protected BaseCharacter user;
<<<<<<< HEAD
=======
//	protected IPhysics charPhysics;
>>>>>>> 4dc69985bd335f0692f84f530e82348a9e76a8b3
	protected CharacterStats charStats;
	
	[SerializeField] protected MovementProperties hitStun;			//when enough hits max out the stun meter, puts the character in hitstun
	[SerializeField] protected MovementProperties knockback;			//when an attack has knockback and stuns/hits a stunned character, puts the character in knockback
	[SerializeField] protected MovementProperties attackRecoil;			//when an attack is deflected, puts the character in attackRecoil

	public MovementProperties HitStun {
		get { return hitStun; }
	}
	public MovementProperties Knockback {
		get { return knockback; }
	}
	public MovementProperties AttackRecoil {
		get { return attackRecoil; }
	}		



	public BaseCharacter User {
		get {return user;}
	}
<<<<<<< HEAD
=======
//	public IPhysics CharPhysics {
//		get {return charPhysics;}
//	}
>>>>>>> 4dc69985bd335f0692f84f530e82348a9e76a8b3
	public CharacterStats CharStats {
		get {return charStats;}
	}
	#endregion

	#region Initialization
	public void Setup (BaseCharacter _user) {
		user = _user;
		charStats = user.CharStats;
	}
	#endregion

	#region Methods
	public void ApplyAttack (AttackProperties attack, Transform instigator) {
		Vector3 attackDir = user.transform.position - instigator.position;

<<<<<<< HEAD
		ApplyVitalUse (attack.AdjustedDamageValue, charStats.Health);
=======
		//Apply attack damage
		ApplyVitalUse (attack.AdjustedDamageValue, charStats.Health);
		//Apply attack stun
>>>>>>> 4dc69985bd335f0692f84f530e82348a9e76a8b3
		ApplyHitStun (attack.AdjustedImpactValue, attackDir);
	}
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
<<<<<<< HEAD
		ApplyVitalUse (-hitStrength, charStats.Stun);
=======
		ApplyVitalUse (-hitStrength, charStats.StunResistance);
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

>>>>>>> 4dc69985bd335f0692f84f530e82348a9e76a8b3
	}
	protected IEnumerator RecoilStun () {
		#if DEBUG
		if (user.Rigid != null) Debug.Log("rigidbody is attached");
		#endif
		float _timer = 0; 

		while (_timer - Time.time >0)									//Stun Period
		{
			yield return null;
		}
		yield break;

	}
	#endregion

	#region Vital Effect Modifiers
<<<<<<< HEAD
=======
	public void SetVitalEffects () {
		user.CharStats.Health.MinValueEffect += Death;
		user.CharStats.StunResistance.MaxValueEffect += Stunned;
	}
>>>>>>> 4dc69985bd335f0692f84f530e82348a9e76a8b3

	protected void Death () {
		Debug.LogError (user.name+" has been destroyed!");
		GameObject.Destroy (user.gameObject);
	}
<<<<<<< HEAD
//
//	protected void Stunned () {
//		Debug.LogError ("stunned");
//		user.CharState.Call (CharacterStateMachine.CharacterActions.Stun);
//	}
//
//	protected void Knockback () {
//
//	}
=======

	protected void Stunned () {
		Debug.LogError ("stunned");
		user.StartCoroutine(hitStun.ActivateAbility());
	}

	protected void Knockback () {

	}
>>>>>>> 4dc69985bd335f0692f84f530e82348a9e76a8b3

	protected void Poisoned () {

	}


	#endregion
}


