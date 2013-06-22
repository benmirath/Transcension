using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateMachine : StateMachineBehaviourEx
{
	public enum CharacterActions
	{
		/// <summary>
		/// Base States. These are the first level of states that a character can exist in. They have no set time limit, and can 
		/// act as defaults. </summary>
		Dead
,
		Idle
,
		Walk
,
		Run
,
		Dodge
,
		/// <summary> Attacking State. This stops normal input recognition such as walking, and transfers control to the current weapon's state machine </summary>
		PrimaryAttack
,
		SecondaryAttack
,
		/// <summary> Sneaking State. This features mostly similar. </summary>
		Sneak
, 		
		/// <summary> Action States. These are the next level of states that a character can exist in. They have some limitation on use and 
		/// duration, often with a clear beginning, middle and end. </summary>
		SheatheWeapon
,
		Lockon
,
		Stun
,
		Knockback
,
	
	}
	#region Properties

	[SerializeField]protected BaseCharacter user;
	[SerializeField]protected CharacterStats stats;
	[SerializeField]protected CharacterMovesetModule moveSet;
	[SerializeField]protected MeshRenderer _animation;
	[SerializeField]protected BaseInputModule input;			//used to monitor advanced state timing and activation

	public string LatestState;
	public string LastState;

	public BaseInputModule CharInput { get { return input; } }

	public IMoveSet MoveSet { get { return moveSet; } }
	#endregion

	//bool states. These are state flags that persist across multiple states.
	#region State Flags
	[SerializeField] bool armed;
	[SerializeField] bool attacking;
	#endregion

	#region Setup
	protected override void OnAwake ()
	{
		base.OnAwake ();
		user = GetComponent<BaseCharacter> ();
		stats = GetComponent<CharacterStats> ();
		moveSet = GetComponent<CharacterMovesetModule> ();									//used to access actions for state activation
		_animation = GetComponent<MeshRenderer> ();
		input = GetComponent<BaseInputModule> ();

		input = GetComponent<BaseInputModule> ();


		armed = false;
		attacking = false;
	}


	public virtual void SetInputs () {
//		user.CharStats.Health.MinValueEffect += Death;
//		sneaking = false;
//
//		stunned = false;
//		running = false;
//		defending = false;
//		countering = false;
	}

	public virtual void Start ()
	{
		//input.Setup ();
		StartCoroutine (monitorState());

		user.CharStats.Stun.MaxValueEffect = TransitionToStun;
		user.CharStats.Health.MinValueEffect = TransitionToDead;

		switch (user.CharType) {
			case BaseCharacter.CharacterType.Player:
			input = GetComponent<PlayerInput>();
			CharInput.walkSignal += TransitionToWalk;
			CharInput.runSignal += TransitionToRun;
			CharInput.dodgeSignal += TransitionToDodge;
			CharInput.primarySignal += TransitionToPrimary;
			CharInput.sheatheSignal += TransitionToSheatheWeapon;
			break;
		case BaseCharacter.CharacterType.Enemy:

			break;
		}
		currentState = CharacterActions.Idle;
	}
	/// <summary> States
	/// To change state, simply change the currentState variable. To creat a nested sub-state, use Call (name of state).
	/// 
	/// Idle
	/// Walk
	/// Run
	/// Dodge
	/// Sheathe Weapon (also Unsheathe Weapon)
	/// Attack
	/// 
	///// For later
	/// 
	/// Counter
	/// Defend
	/// 
	/// </summary>
	private IEnumerator monitorState ()
	{
		while (true) {
			LatestState = currentState.ToString ();
//						LastState = lastState.ToString ();
			yield return null;
		}
	}
	/// <summary>
	/// Checks the ability vital, making sure there's enough to activate the ability.
	/// </summary>
	/// <returns><c>true</c>, if ability meets activation parameters, <c>false</c> otherwise.</returns>
	/// <param name="ability">Ability.</param>
	private bool CheckAbilityVital (AbilityProperties ability)
	{
		Debug.Log ("3");
		if (ability.Cost > 0) {
			if (ability.UserVital.CurValue >= ability.Cost) {
				Debug.LogWarning ("Ability activated");
				ability.UserVital.CurValue -= ability.Cost;
				StartCoroutine (ability.UserVital.PauseRegen());
				return true;
			} else 
				Debug.LogWarning ("Not enough points in vital");
				return false;
		} else {
			Debug.LogWarning ("Ability has no cost");
			return true;
		}
	}
	#endregion

	protected void TransitionToKnockback ()
	{

	}

	protected void TransitionToSecondary ()
	{

	}
		#region Alive

		#endregion

		#region Dead

		#endregion


	protected IEnumerator ActivateStateAbility (AbilityProperties ability) {
		float timer;

		if (ability.Cost > 0)
			ability.UserVital.StopRegen = true;

		if (ability.EnterLength > 0) {
			timer = Time.time + ability.EnterLength;
			while (timer > Time.time) {
				ability.EnterAbility ();
				yield return null;
			}
		}
		if (ability.ActiveLength > 0) {
			timer = Time.time + ability.ActiveLength;
			while (timer > Time.time) {
				ability.ActiveAbility ();
				yield return null;
			}
		}
		if (ability.ExitLength > 0) {
			timer = Time.time + ability.ExitLength;
			while (timer > Time.time) {
				ability.ExitAbility ();
				yield return null;
			}
		}

		if (ability.UserVital.StopRegen == true)
			ability.UserVital.StopRegen = false;
		yield break;
	}

	#region Movement States
	protected void Idle_EnterState ()
	{
		attacking = false;

		if (user.CharType == BaseCharacter.CharacterType.Player)
			_animation.material.color = Color.white;
		else if (user.CharType == BaseCharacter.CharacterType.Enemy)
			_animation.material.color = Color.red; 
		//initialization for the state happens here
		Debug.Log ("entering idle state");
	}
	protected void Idle_Update ()
	{
		Debug.Log ("Currently idling");
		Debug.Log ("Current velocity is :"+input.MoveDir);

		if (input.LockedOn) {
			moveSet.CharMovement.Aim.ActiveAbility ();
		}
		if (input.MoveDir != Vector3.zero) {
			TransitionToWalk ();
		}

	}
	protected void Idle_ExitState ()
	{
		Debug.LogWarning ("leaving idle state");
	}

	protected void TransitionToWalk ()
	{
		Debug.Log ("Attempting Transition: Walk");
		if (currentState.ToString () == CharacterActions.Idle.ToString () || currentState.ToString () == CharacterActions.Run.ToString ()) {
			currentState = CharacterActions.Walk;

		} else {
		}//dont transition;
	}
	protected void Walk_EnterState ()
	{
		_animation.material.color = Color.cyan;
		Debug.LogWarning ("Entering Walk State");
	}
	public void Walk_FixedUpdate ()
	{
		Debug.Log ("currently walking");

		if (input.MoveDir == Vector3.zero) 					//no directional input, stop walking
			currentState = CharacterActions.Idle;
		else if (!armed)									//not armed, simply walk
			moveSet.CharMovement.Walk.ActiveAbility ();
		else 												//armed, will walk/strafe
			moveSet.CharMovement.Strafe.ActiveAbility ();
	}
	protected void Walk_ExitState ()
	{

	}
		
	protected void TransitionToRun ()
	{
		Debug.Log ("Attempting Transition: Run");
		if (currentState.ToString () == CharacterActions.Walk.ToString ()) {
			currentState = CharacterActions.Run;
		} else {
		}//dont transition;

	}
	protected IEnumerator Run_EnterState ()
	{
		_animation.material.color = Color.blue;
		Debug.Log ("1");
		float timer = moveSet.CharMovement.Run.EnterLength + Time.time;
		while (timer >= Time.time) {
			moveSet.CharMovement.Run.EnterAbility ();
			yield return null;
		}
		yield break;
	}
	protected void Run_FixedUpdate ()
	{
		Debug.Log ("2");

		if (input.MoveDir == Vector3.zero)					//Character stopped moving
			currentState = CharacterActions.Idle;
		else {
			if (moveSet.CharMovement.Run.UserVital.CurValue > moveSet.CharMovement.Run.Cost)//Character has enough stamina to activate run
				moveSet.CharMovement.Run.ActiveAbility ();
			else 											//otherwise return to walk
				TransitionToWalk ();
		}
	}
	protected void Run_ExitState ()
	{
		moveSet.CharMovement.Run.ExitAbility ();
	}

	/// <summary>
	/// Activates the character's dodge. Returns from the enter state due to having a definite duration.
	/// </summary>
	protected void TransitionToDodge ()
	{
		Debug.Log ("Attempting Transition: Dodge");
		if (currentState.ToString () == CharacterActions.Idle.ToString () || currentState.ToString () == CharacterActions.Walk.ToString () || currentState.ToString () == CharacterActions.Run.ToString ()) 
			if (CheckAbilityVital (moveSet.CharMovement.Dodge))
				currentState = CharacterActions.Dodge;
		else {
		}
		//dont transition;
	}
	protected IEnumerator Dodge_EnterState ()
	{
		moveSet.CharMovement.Dodge.Direction = input.MoveDir;
		_animation.material.color = Color.gray;
		yield return StartCoroutine (moveSet.CharMovement.Dodge.ActivateAbility());

		currentState = CharacterActions.Idle;
		yield break;
	}
	protected void Dodge_ExitState ()
	{
		Debug.Log ("Exiting Dodge");
	}
	#endregion

	#region Status States
	protected void TransitionToSheatheWeapon ()
	{
		if (currentState.ToString() == CharacterActions.Idle.ToString () || currentState.ToString() == CharacterActions.Walk.ToString () || currentState.ToString() == CharacterActions.Run.ToString ())
			currentState = CharacterActions.SheatheWeapon;
	}

	protected IEnumerator SheatheWeapon_EnterState ()
	{
		armed = !armed;
		_animation.material.color = Color.magenta;
		yield return new WaitForSeconds (.2f);
		currentState = CharacterActions.Idle;
		yield break;
	}

	protected void TransitionToStun () {
		Debug.LogError ("Should be stunned");
		Call (CharacterActions.Stun);

	}
	protected IEnumerator Stun_EnterState () 
	{
		yield return StartCoroutine (moveSet.CharStatus.HitStun.ActivateAbility());
		currentState = CharacterActions.Idle;
		Return ();
	}
	protected void TransitionToDead () {
		Call (CharacterActions.Dead);
	}
	protected void Dead_EnterState () {
		GameObject.Destroy (user.gameObject);
	}
	#endregion

	#region Equipment State Delegates
	protected void TransitionToPrimary ()
	{
		if (armed) {
			Debug.Log ("Entering Primary Attack");
			_animation.material.color = Color.green;
			attacking = true;	

			if (moveSet == null) Debug.LogError("no moveset set");
			if (moveSet.CharEquipment == null) Debug.LogError("no moveset equipment set");
			if (moveSet.CharEquipment.PrimaryMoveset == null) Debug.LogError("no primary moveset set");

			moveSet.CharEquipment.PrimaryMoveset.ActivateMoveset ();
			Debug.Log ("Succesfully returned from equipment action");
		}

	}
	protected IEnumerator SecondaryAttack_EnterState ()
	{
		yield break;
	}
	#endregion
}