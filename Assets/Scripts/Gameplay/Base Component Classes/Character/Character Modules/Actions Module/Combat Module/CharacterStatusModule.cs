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

		ApplyVitalUse (attack.AdjustedDamageValue, charStats.Health);
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
		ApplyVitalUse (-hitStrength, charStats.Stun);
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

	protected void Death () {
		Debug.LogError (user.name+" has been destroyed!");
		GameObject.Destroy (user.gameObject);
	}
//
//	protected void Stunned () {
//		Debug.LogError ("stunned");
//		user.CharState.Call (CharacterStateMachine.CharacterActions.Stun);
//	}
//
//	protected void Knockback () {
//
//	}

	protected void Poisoned () {

	}


	#endregion
}


