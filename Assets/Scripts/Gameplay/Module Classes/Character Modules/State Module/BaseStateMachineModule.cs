using UnityEngine;
using System;
using System.Collections;

public interface ICharacterStateMachine {
	BaseStateMachineModule.CharState State {
		get;
	}
	bool WeaponReady {
		get;
	}

	bool IsRunning {
		get;
		//set;
	}
	bool IsDefending {
		get;
		//set;
	}
	bool IsCountering {
		get;
		//set;
	}
	bool IsEvading {
		get;
		//set;
	}
	
}

public abstract class BaseStateMachineModule : ICharacterStateMachine {
	///<summary>
	/// Base Character State. This is the baseline that any character operates on, determining the actions available to them at any one time.</summary>
	/// <remarks>
	/// Idle (The basic starting state. Character's weapon is sheathed, they're just walking around/exploring, etc.)
	/// CombatReady (The combat state. Character's weapon is drawn, and activating primary will )
	/// Attacking
	/// Countering
	/// Sneaking
	/// InMenu
	/// </remarks>
	public enum CharState {
		Idle,
		CombatReady,
		//Attacking,
		//Countering,
		Sneaking,
		InMenu,
	}
	/// <summary>
	/// Common events that lead out from the base character states. These are either contant actions that are too 
	/// standalone for a state (running), or single actions that branch off from the current state. </summary>
	/// <remarks>
	/// None, (Charater is doing nothing. Base event state)
	/// Walking (Character Walks. Usable from Idle, CombatReady and Sneaking)
	/// Running (Chararter Runs. Usable from Idle, CombatReady)
	/// Dodging (Character Dodges. Usable from Idle, CombatReady, and Attacking)
	/// ActivatingClassSpecial (Character uses their Class Special. Usability depends on Ability)
	/// AltStance (Character readies to use their equipment's alt actions. Usable from CombatReady and Attacking)
	/// ActivatingPrimary (Character activates their primary weapon, with outputted action depending first on current subState (if combatReady), and then current weapon's state if Attacking)
	/// ActivatingSecondary (Character activates their secondary weapon/moveset, with outputted action depending first on current subState)
	/// </remarks>
	public enum CharSubState {
		None,
		Stunned,				//accessed from combat actions module
		Walking,				//accessed from movement actions module
		Running,				//accessed from movement actions module
		Dodging,				//accessed from movement actions module
		ActivatingClassSpecial,	//accessed from class actions module
		//AltStance,				//accessed 

		ActivatingPrimary,		//accessed from equipment loadout actions module
		ActivatingSecondary,	//accessed from equipment loadout actions module
		ActivatingSpecial		//accessed from equipment loadout actions module
	}

	#region External Members
	protected ICharacter user;
	protected IInput charInput;
	protected IPhysics charPhysics;
	protected IActions charActions;
	#endregion

	#region Initialization
	public BaseStateMachineModule (ICharacter user) {
		this.user = user;
	}

	protected virtual void SetTransitionMethods (ICharacter _user) {
		IInput _input = _user.CharInput;
		IActions _actions = _user.CharActions;
		
		_input.ActivateRun += ActivateRun;
		_input.ActivateDodge += ActivateDodge;
		//_input.ActivatePrimary
		
	}
	#endregion

	#region Internal Fields
	protected CharState state;
	protected CharSubState subState;
	#endregion

	
	#region State Flags
	private bool hasTarget;
	private bool weaponReady;
	private bool isRunning;
	private bool isStunned;
	private bool isAttacking;
	private bool isDefending;
	private bool isCountering;
	private bool isEvading;
	#endregion

	#region Properties

	//State Flags
	
	public ICharacter User {
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
		//set {weaponReady = value;}
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


	#region States
	protected IEnumerator Idle () {
		while (state == CharState.Idle) {
			if (User.CharInput.MoveDir != Vector3.zero) {
				//if () charActions.CharMovement.Dodge

//				if (IsEvading == true && charActions.CharMovement.DodgeCooldown < CharStats.Stamina.CurValue) yield return StartCoroutine (CharMovement.Dodge ());
//				else if (IsRunning == true && CharMovement.RunCost < CharStats.Stamina.CurValue) CharMovement.Run ();
//				else charActions.CharMovement.Walk ();
			}
			
			//			Debug.LogWarning("Velocity: " + Body.velocity);
			//			Debug.LogWarning("WalkSpeed: " + CharMovement.WalkSpeed);
			//Debug.LogWarning();
			yield return null;
		}
		StateTransition ();
		yield break;
	}
	protected IEnumerator CombatReady () {
		while (state == CharState.CombatReady) {
			
			//if () {}						//if initiating an attack
			//			if (User.CharInput.MoveDir != Vector3.zero) {
			//				if (IsEvading == true) User.CharBase.StartCoroutine (CharMovement.Dodge ());
			//				else if (IsRunning == true && CharMovement.RunCost < CharStats.Stamina.CurValue) CharMovement.Run ();
			//				else CharMovement.Strafe ();
			//			}
			//			else CharMovement.Aim();
			yield return null;
		}
		StateTransition ();
		yield break;
	}
	protected IEnumerator Sneaking () {
		while (state == CharState.Sneaking) {
			//yield return StartCoroutine ();
			yield return null;
		}
		yield break;
	}
//	protected IEnumerator Attacking () {
//		while (state == CharState.Attacking) {
//			//yield return StartCoroutine ();
//			yield return null;
//		}
//		yield break;
//	}
	protected IEnumerator InMenu () {
		while (state == CharState.InMenu) {
			//yield return StartCoroutine ();
			yield return null;
		}
		yield break;
	}
	#endregion

	#region Transitions
	protected virtual void StateTransition() {
		string methodName = state.ToString () + "State";
		Debug.Log (methodName);
		System.Reflection.MethodInfo info = GetType().GetMethod(methodName,System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
		Debug.LogWarning("activating method of name : " + methodName);
		user.CharBase.StartCoroutine((IEnumerator)info.Invoke(this, null));
	}

	protected void DrawWeapon () {

	}
	protected void SheatheWeapon () {

	}
	protected void ActivateRun () {

	}
	protected void ActivateDodge () {
		if (state==CharState.Idle||state==CharState.CombatReady) {
			charActions.CharMovement.Dodge.Activate();
		}
	}
	protected void ActivatePrimary () {

	}
	protected void ActivateSecondary () {

	}

	#endregion

	#region StateEvents
	//protected delegate IEnumerator Dodge(Vector3 dir);
	//private 

//	private AttemptStateTransition activateRun;
//	private AttemptStateTransition activateDodge;
//	private AttemptStateTransition activateAlt;
//	private AttemptStateTransition activatePrimary;
//	private AttemptStateTransition activateSecondary;
//	private AttemptStateTransition activateSpecial;

	/// <summary>
	/// Used to activate any base actions by the character in the state machine, flipping a bool first to true, then false.
	/// </summary>
	/// <returns>The action.</returns>
	/// <param name="state">State.</param>
//	protected IEnumerator ActivateAction (CharStateEvent state) {
//		bool activeSwitch;
//		switch (state) {
//		case CharStateEvent.Running:
//			activeSwitch=runActive;
//			break;
//		case CharStateEvent.Dodging:
//			activeSwitch=dodgeActive;
//			break;
//		case CharStateEvent.ActivatingAlt:
//			activeSwitch=altActive;
//			break;
//		case CharStateEvent.ActivatingPrimary:
//			activeSwitch=primaryActive;
//			break;
//		case CharStateEvent.ActivatingSecondary:
//			activeSwitch=secondaryActive;
//			break;
//		case CharStateEvent.ActivatingSpecial:
//			activeSwitch=specialActive;
//			break;
//		default:
//			break;
//		}
//		activeSwitch=true;
//		yield return new WaitForEndOfFrame();
//		activeSwitch=false;
//		yield break;
//	}
//	protected void ActivatePrimary () {
//
//	}
//	protected void ActivateSecondary () {
//
//	}

	#endregion
	



	#region State Transitions

//	protected virtual void StateTransition() {
//		string methodName = state.ToString () + "State";
//		Debug.Log (methodName);
//		System.Reflection.MethodInfo info = GetType().GetMethod(methodName,System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
//		Debug.LogWarning("activating method of name : " + methodName);
//		user.CharBase.StartCoroutine((IEnumerator)info.Invoke(this, null));
//	}
	
	
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
//			WeaponReady = true;
			state = CharState.CombatReady;
		}
		else if (State == CharState.CombatReady) {
//			WeaponReady = false;
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
	
	#endregion
}

//	protected virtual IEnumerator StunnedState()
//	{
//		if (user.CharPhysics != null) Debug.Log("rigidbody is attached");
//		float _timer = 0; 
//		_timer = Time.time + user.CharStats.StunDuration;
//		user.CharPhysics.Body.isKinematic = false;
//		
//		while (_timer - Time.time >= 0.05f)								//Initial Knockback
//		{
//			Debug.LogWarning("STUNNED AND MOVING");
//			user.CharPhysics.Controller.Move(user.CharPhysics.KnockDir);
//			yield return new WaitForFixedUpdate();
//		}
//		while (_timer - Time.time >0)									//Stun Period
//		{
//			yield return null;
//		}
//		user.CharPhysics.Body.isKinematic = true;
//		//user.CharPhysics.KnockDir = Vector3.zero;
//		
//		if (WeaponReady) state = CharState.CombatReady;
//		else state = CharState.Idle;
//		StateTransition();
//		yield break;
//	}
//	
//	//protected abstract IEnumerator IdleState();							//Base state with weapons' sheathed.		
//	//protected abstract IEnumerator CombatReadyState();					//Base state with weapons' drawn.
//
//	protected IEnumerator IdleState () {					//Starting state, and where other states eventually return to. 
//		while (state == CharState.Idle) {
//			if (User.CharInput.MoveDir != Vector3.zero) {
//				if (IsEvading == true && charActions.CharMovement.DodgeCooldown < CharStats.Stamina.CurValue) yield return StartCoroutine (CharMovement.Dodge ());
//				else if (IsRunning == true && CharMovement.RunCost < CharStats.Stamina.CurValue) CharMovement.Run ();
//				else IActions.Walk ();
//			}
//			
//			//			Debug.LogWarning("Velocity: " + Body.velocity);
//			//			Debug.LogWarning("WalkSpeed: " + CharMovement.WalkSpeed);
//			//Debug.LogWarning();
//			yield return null;
//		}
//		StateTransition ();
//		yield break;
//	}
//	
//	protected IEnumerator CombatReadyState () {
//		while (state == CharState.CombatReady) {
//			
//			//if () {}						//if initiating an attack
////			if (User.CharInput.MoveDir != Vector3.zero) {
////				if (IsEvading == true) User.CharBase.StartCoroutine (CharMovement.Dodge ());
////				else if (IsRunning == true && CharMovement.RunCost < CharStats.Stamina.CurValue) CharMovement.Run ();
////				else CharMovement.Strafe ();
////			}
////			else CharMovement.Aim();
//			yield return null;
//		}
//		StateTransition ();
//		yield break;
//	}
//	
//	protected IEnumerator AttackingState () {
//		while (state == CharState.Attacking) {
//			//yield return StartCoroutine ();
//		}
//		yield break;
//	}
//
//	//protected abstract IEnumerator DefendState();
//	//protected abstract IEnumerator DodgeState();

