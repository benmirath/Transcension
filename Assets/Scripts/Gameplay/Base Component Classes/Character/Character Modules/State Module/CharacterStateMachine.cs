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
	[SerializeField]protected Material _animation;
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
	protected bool running;
	protected bool evading;

	public bool Armed { get { return armed; } }
	public bool Running { get { return running; } }
	public bool Evading { get { return evading; } }
	public bool Attacking { 
		get { return attacking; } 
		set { attacking = value; }
	}

	#endregion

	#region Setup
	protected override void OnAwake ()
	{
		base.OnAwake ();
		user = GetComponent<BaseCharacter> ();
		stats = GetComponent<CharacterStats> ();
		moveSet = GetComponent<CharacterMovesetModule> ();									//used to access actions for state activation

//		_animation = transform.Find ("Renderer").GetComponent<SkinnedMeshRenderer>();
		_animation = user.oldCharAnimation;

		input = GetComponent<BaseInputModule> ();


		armed = false;
		attacking = false;
		running = false;
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
		if (stats.Stun != null)
			user.CharStats.Stun.MaxValueEffect = TransitionToStun;
		else
			Debug.LogError ("no stun stat set!");

		stats.Health.MinValueEffect = TransitionToDead;

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
			input = GetComponent<AIInput>();
			break;
		}
		currentState = CharacterActions.Idle;
		StartCoroutine (monitorState());
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
			LatestState = name +" : "+ currentState.ToString ();
//						LastState = lastState.ToString ();
			yield return null;
		}
	}
	/// <summary>
	/// Checks the ability vital, making sure there's enough to activate the ability.
	/// </summary>
	/// <returns><c>true</c>, if ability meets activation parameters, <c>false</c> otherwise.</returns>
	/// <param name="ability">Ability.</param>
	public bool CheckAbilityVital (AbilityProperties ability)
	{
		if (ability.Cost > 0) {
			if (ability.UserVital.CurValue >= ability.Cost) {
				ability.UserVital.CurValue -= ability.Cost;
//				StartCoroutine (ability.UserVital.PauseRegen());
				return true;
			} else 
				Debug.LogWarning ("Not enough points in vital");
				return false;
		} else {
			Debug.LogWarning ("Ability has no cost");
			return true;
		}
	}
	public bool MonitorAbilityVital (AbilityProperties ability)
	{
		if (ability.Cost > 0) {
			if (ability.UserVital.CurValue >= ability.Cost) {
				ability.UserVital.CurValue -= ability.Cost * Time.deltaTime;
				//				StartCoroutine (ability.UserVital.PauseRegen());
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


	#region Movement States
	protected void Idle_EnterState ()
	{
		attacking = false;

		if (user.CharType == BaseCharacter.CharacterType.Player)
			_animation.color = Color.white;
		else if (user.CharType == BaseCharacter.CharacterType.Enemy)
			_animation.color = Color.red; 
		//initialization for the state happens here
	}
	protected void Idle_Update ()
	{
//		Debug.Log ("Currently idling");
//		Debug.Log ("Current velocity is :"+input.MoveDir);

		if (armed) {
			moveSet.CharMovement.Aim.ActiveAbility ();
		}
		if (input.MoveDir != Vector3.zero) {
			TransitionToWalk ();
		}

	}
	protected void Idle_ExitState ()
	{
	}

	protected void TransitionToWalk ()
	{
		if (currentState.ToString () == CharacterActions.Idle.ToString () || currentState.ToString () == CharacterActions.Run.ToString ()) {
			currentState = CharacterActions.Walk;

		} else {
		}//dont transition;
	}
	protected void Walk_EnterState ()
	{
		_animation.color = Color.cyan;
	}
	public void Walk_FixedUpdate ()
	{

		if (input.MoveDir == Vector3.zero) 					//no directional input, stop walking
			currentState = CharacterActions.Idle;

		else if (!armed)							//not armed, simply walk
			moveSet.CharMovement.Walk.ActiveAbility ();
		else 									//armed, will walk/strafe
			moveSet.CharMovement.Strafe.ActiveAbility ();
	}
	protected void Walk_ExitState ()
	{

	}
		
	protected void TransitionToRun ()
	{
		if (currentState.ToString () == CharacterActions.Walk.ToString ()) {
			currentState = CharacterActions.Run;
		} else {
		}//dont transition;

	}
	protected IEnumerator Run_EnterState ()
	{
		_animation.color = Color.blue;
		running = true;

		yield return StartCoroutine (moveSet.CharMovement.Run.ActivateSustainedAbility(input.InputActions.Find(i => i.Name == "Evasion")));

		if (input.MoveDir != Vector3.zero)
			currentState = CharacterActions.Walk;
		else
			currentState = CharacterActions.Idle;
	}
//	protected void Run_FixedUpdate ()
//	{
//		Debug.Log ("2");
//
//		if (input.MoveDir == Vector3.zero)					//Character stopped moving
//			currentState = CharacterActions.Idle;
//		else {
//			if (moveSet.CharMovement.Run.UserVital.CurValue > moveSet.CharMovement.Run.Cost)//Character has enough stamina to activate run
//				moveSet.CharMovement.Run.ActiveAbility ();
//			else 											//otherwise return to walk
//				TransitionToWalk ();
//		}
//	}
	protected void Run_ExitState ()
	{
		running = false;
//		moveSet.CharMovement.Run.ExitAbility ();


	}

	/// <summary>
	/// Activates the character's dodge. Returns from the enter state due to having a definite duration.
	/// </summary>
	protected void TransitionToDodge ()
	{
		if (currentState.ToString () == CharacterActions.Idle.ToString () || currentState.ToString () == CharacterActions.Walk.ToString () || currentState.ToString () == CharacterActions.Run.ToString ()) 
			if (moveSet.CharMovement.Dodge.UserVital.CurValue > moveSet.CharMovement.Dodge.Cost)
				currentState = CharacterActions.Dodge;
		else {
		}
		//dont transition;
	}
	protected IEnumerator Dodge_EnterState ()
	{
//		moveSet.CharMovement.Dodge.Direction = input.MoveDir;
		_animation.color = Color.gray;
		evading = true;
		yield return StartCoroutine (moveSet.CharMovement.Dodge.ActivateDurationalAbility());

		currentState = CharacterActions.Idle;
		yield break;
	}
	protected void Dodge_ExitState ()
	{
		evading = false;
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

		if (armed == true)
			user.CharAnimation.SetBool ("Armed", true);
		else
			user.CharAnimation.SetBool ("Armed", false);

		_animation.color = Color.magenta;
		yield return new WaitForSeconds (.2f);
		currentState = CharacterActions.Idle;
		yield break;
	}


	protected void TransitionToStun () {
//		Call (CharacterActions.Stun);
		Return (CharacterActions.Stun);
	}
	protected IEnumerator Stun_EnterState () 
	{
		yield return StartCoroutine (moveSet.CharStatus.HitStun.ActivateDurationalAbility());
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
			_animation.color = Color.green;
//			attacking = true;	

			if (moveSet == null) Debug.LogError("no moveset set");
			if (moveSet.CharEquipment == null) Debug.LogError("no moveset equipment set");
			if (moveSet.CharEquipment.PrimaryMoveset == null) Debug.LogError("no primary moveset set");

			moveSet.CharEquipment.PrimaryMoveset.ActivateMoveset ();
		}

	}
	protected IEnumerator SecondaryAttack_EnterState ()
	{
		yield break;
	}
	#endregion
}