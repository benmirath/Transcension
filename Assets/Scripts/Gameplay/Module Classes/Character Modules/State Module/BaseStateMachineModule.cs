using UnityEngine;
using System;
using System.Collections;

public class BaseStateMachineModule {
	public enum CharState {
		Stunned,													//will disrupt character's ability to react when activated
		Idle,
		CombatReady,
		Dodge,														//will encapsulate any damage-mitigating movement state. 
		Defend,														//will encapsulate any damage-mitigating equipment state.
		Counter,													//will encapsulate any damage-mitigating counter-attack state.
		Attacking,													//will encapsulate any state that stems from using an equipped weapon.
		ComboAttack1,
		ComboAttack2,
		ComboAttack3,
		ComboAttack4,
		ComboAttack5,
		Finisher,
		AltFinisher,
		RangedAttack,
		RunAttack,
		DodgeAttack,
		Special1,
		Special2,
		Special3
	}

	#region Fields
	protected BaseCharacter user;
	protected CharState state;
	private bool hasTarget ;
	private bool weaponReady;
	
	private bool isRunning;
	private bool isDefending;
	private bool isCountering;
	private bool isEvading;
	#endregion

	#region Properties

	//State Flags
	
	public BaseCharacter User {
		get {return user;}
	}
	public CharState State {
		get {return state;}
		set {state = value;}
	}

	public bool HasTarget {
		get {return hasTarget;}
		set {hasTarget = value;}
	}
	public bool WeaponReady {
		get {return weaponReady;}
		set {weaponReady = value;}
	}

	public bool IsRunning {
		get { return isRunning;}
		set { isRunning = value;}
	}
	public bool IsDefending {
		get {return isDefending;}
		set {isDefending = value;}
	}
	public bool IsCountering {
		get {return isCountering;}
		set {isCountering = value;}
	}
	public bool IsEvading {
		get {return isEvading;}
		set {isEvading = value;}
	}
	#endregion

	#region Initialization
	public BaseStateMachineModule (BaseCharacter user) {
		this.user = user;
	}
	#endregion

	#region State Transitions

	protected virtual void StateTransition()
	{
		string methodName = state.ToString () + "State";
		Debug.Log (methodName);
		System.Reflection.MethodInfo info = GetType().GetMethod(methodName,System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
		Debug.LogWarning("activating method of name : " + methodName);
		user.StartCoroutine((IEnumerator)info.Invoke(this, null));
	}
	
	
	//	protected virtual void TransitionToCombatReady ()
	//	{
	//		if (GetState == CharState.Idle) state = CharState.CombatReady;
	//		if (GetState == CharState.CombatReady) state = CharState.Idle;
	//		else return;
	//	}
	//	protected abstract void StateTransition();
	//	protected abstract void TransitionToPrimary();
	//	protected abstract void TransitionToSecondary();
	//	protected abstract void TransitionToStunned();
	//	protected abstract void TransitionToDefend();
	//	protected virtual void TransitionToDodge () {
	//		if (state == CharState.Idle || state == CharState.CombatReady) 
	//			StartCoroutine (CharMovement.Dodge());
	//	}
	
	/// <summary>
	/// State transition that shifts the character between the two base states, idle and combat ready. </summary>
	protected virtual void SwitchCombatStances () {
		Debug.LogError ("SWITCHING STANCES");
		if (State == CharState.Idle) {
			WeaponReady = true;
			state = CharState.CombatReady;
		}
		else if (State == CharState.CombatReady) {
			WeaponReady = false;
			state = CharState.Idle;
		}
		else return;
	}
	
	/// <summary>
	/// State transition that activates the character's dodge if in the proper state </summary>
	/*protected virtual IEnumerator UseDodge () {
		if (state == CharState.Idle || state == CharState.CombatReady) {
			yield return StartCoroutine (CharMovement.Dodge ());
			if (_weaponReady) state = CharState.CombatReady;
			else state = CharState.Idle;
			StateTransition();
		}
		yield break;
	}*/
	#endregion State Transitions
	
	#region States
	protected virtual IEnumerator StunnedState()
	{
		if (user.CharPhysics != null) Debug.Log("rigidbody is attached");
		float _timer = 0; 
		_timer = Time.time + user.CharStats.StunDuration;
		user.CharPhysics.isKinematic = false;
		
		while (_timer - Time.time >= 0.05f)								//Initial Knockback
		{
			Debug.LogWarning("STUNNED AND MOVING");
			user.Body.Move(user.KnockDir);
			yield return new WaitForFixedUpdate();
		}
		while (_timer - Time.time >0)									//Stun Period
		{
			yield return null;
		}
		user.CharPhysics.isKinematic = true;
		user.KnockDir = Vector3.zero;
		
		if (WeaponReady) state = CharState.CombatReady;
		else state = CharState.Idle;
		StateTransition();
		yield break;
	}
	
	protected abstract IEnumerator IdleState();							//Base state with weapons' sheathed.		
	protected abstract IEnumerator CombatReadyState();					//Base state with weapons' drawn.
	
	//protected abstract IEnumerator DefendState();
	//protected abstract IEnumerator DodgeState();
	#endregion
}