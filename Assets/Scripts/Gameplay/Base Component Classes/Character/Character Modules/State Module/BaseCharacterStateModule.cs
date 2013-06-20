using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacterStateModule : StateMachineBehaviourEx
{
	public enum CharacterActions
	{
		/// <summary>
		/// Base States. These are the first level of states that a character can exist in. They have no set time limit, and can 
		/// act as defaults. </summary>
				
		Idle
,
		SheatheWeapon
,
		Lockon
,
			/// <summary> Attacking State. This stops normal input recognition such as walking, and transfers control to the current weapon's state machine </summary>
		PrimaryAttack
,
		SecondaryAttack
,
			/// <summary> Sneaking State. This features mostly similar. </summary>
		Sneak
, 			//base state, weapon sheathed

			/// <summary> Action States. These are the next level of states that a character can exist in. They have some limitation on use and 
			/// duration, often with a clear beginning, middle and end. </summary>
		Stun
,
		KnockedBack
,
		Walk
,
		Run
,
		Dodge
,
	}
	#region Properties
	[SerializeField]protected CharacterStats stats;
	[SerializeField]protected CharacterMovesetModule moveSet;
//	protected BaseEquipmentLoadoutModule equipment;
//used to access actions for state activation

	[SerializeField]protected ICharacter user;
	[SerializeField]protected MeshRenderer _animation;
	[SerializeField]protected BaseInputModule input;
//used to monitor advanced state timing and activation

	public string LatestState;
	public string LastState;

	public IInput CharInput { get { return input; } }

	public IMoveSet MoveSet { get { return moveSet; } }
	#endregion

	//bool states. These are state flags that persist across multiple states.
	#region State Flags
	[SerializeField] bool armed;
	[SerializeField] bool attacking;
//	[SerializeField] bool invincible;
//	[SerializeField] bool sneaking;
//	[SerializeField] bool stunned;
//	[SerializeField] bool running;
//	[SerializeField] bool defending;
//	[SerializeField] bool countering;
	#endregion

	#region Setup
	protected override void OnAwake ()
	{
		base.OnAwake ();
		stats = GetComponent<CharacterStats> ();
		moveSet = GetComponent<CharacterMovesetModule> ();									//used to access actions for state activation
		//equipment = GetComponent<BaseEquipmentLoadoutModule> ();
		_animation = GetComponent<MeshRenderer> ();


		armed = false;
		attacking = false;
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
			//checks if ability is a special or not, determining vital type used.
			if (ability.UserVital.CurValue >= ability.Cost) {
				Debug.LogWarning ("Ability activated");
				ability.UserVital.CurValue -= ability.Cost;
				StartCoroutine (ability.UserVital.PauseRegen());
				return true;
			} else 
				Debug.LogWarning ("Not enough points in vital");
				return false;
//			switch (ability.VitalType)
//			{
//			case Vital.PrimaryVitalName.Stamina:
//				if (stats.Stamina.CurValue >= ability.Cost) {
//					Debug.LogWarning ("Standard Ability activated");
//					stats.Stamina.CurValue -= ability.Cost;
//					return true;
//				} else 
//					return false;
//				break;
//
//			case Vital.PrimaryVitalName.Energy:
//				if (stats.Energy.CurValue >= ability.Cost) {
//					Debug.LogWarning ("Special Ability activated");
//					stats.Energy.CurValue -= ability.Cost;
//					return true;
//				} else 
//					return false;
//				break;
//			case Vital.PrimaryVitalName.Health:
//			default:
//				return true;
//				break;
//			}
		} else {
			Debug.LogWarning ("Ability has no cost");
			return true;
		}
	}
		#endregion








	protected void TransitionToStunned ()
	{

	}

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




		#region Idle
	protected void Idle_EnterState ()
	{
		attacking = false;
		_animation.material.color = Color.white;
		//initialization for the state happens here
		Debug.Log ("entering idle state");
//		Debug.LogError("THIS BEGINS THE TEST");
//		while ((CharState)currentState == CharState.Idle) {
//			if (user.CharInput.MoveDir != Vector3.zero) {
//				if (dodgeSignal == true) user.CharActions.CharMovement.Dodge.Activate();
//
//				//if (dodgeSignal == true && user.CharActions.CharMovement.Dodge();
//				//else if (IsRunning == true && user.CharActions.CharMovement.RunCost < CharStats.Stamina.CurValue) CharMovement.Run ();
//				//else IActions.Walk ();
//			}
//					
//			Debug.LogWarning("Current state is : "+currentState);
//					//			Debug.LogWarning("Velocity: " + Body.velocity);
//					//			Debug.LogWarning("WalkSpeed: " + CharMovement.WalkSpeed);
//					//Debug.LogWarning();
//		yield return null;
//		}
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
		#endregion

		#region Walk
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
		#endregion

		#region Run
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

	}
		#endregion

		#region Dodge
		/// <summary>
		/// Activates the character's dodge. Returns from the enter state due to having a definite duration.
		/// </summary>
	protected void TransitionToDodge ()
	{
		Debug.Log ("Attempting Transition: Dodge");
		if (currentState.ToString () == CharacterActions.Idle.ToString () || currentState.ToString () == CharacterActions.Walk.ToString () || currentState.ToString () == CharacterActions.Run.ToString ()) 
			if (CheckAbilityVital (moveSet.CharMovement.Dodge))
				currentState = CharacterActions.Dodge;
//		else if (attacking) {
//			user.PrimaryWeapon.WeaponState.Followup = BaseEquipmentStateModule.FollowupType.Dodge;
//		}
		else {
		}
		//dont transition;
	}

	protected IEnumerator Dodge_EnterState ()
	{
		//Vector3 trajectory = input.MoveDir;
		moveSet.CharMovement.Dodge.Direction = input.MoveDir;
		_animation.material.color = Color.gray;
		float timer1 = moveSet.CharMovement.Dodge.EnterLength + Time.time;
		float timer2 = moveSet.CharMovement.Dodge.ActiveLength + timer1;
		while (timer1 >= Time.time) {
			moveSet.CharMovement.Dodge.EnterAbility ();
			yield return null;
		}
		while (timer2 >= Time.time) {
			moveSet.CharMovement.Dodge.ActiveAbility ();
			yield return null;
		}
		Debug.Log ("TEST123");
		currentState = CharacterActions.Idle;
		yield break;
	}

	protected void Dodge_ExitState ()
	{
		Debug.Log ("Exiting Dodge");
	}
		#endregion

	#region Sheathe Weapon
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
	#endregion

	#region Primary Attack
//		protected void TransitionToPrimary ()
//		{
//			if (armed) {
////				if (currentState.ToString () == CharacterActions.Idle.ToString () || 
////				    currentState.ToString () == CharacterActions.Walk.ToString () || 
////				    currentState.ToString () == CharacterActions.Run.ToString () ||
////				    currentState.ToString () == CharacterActions.Dodge.ToString ()) {
////					currentState = CharacterActions.PrimaryAttack;
//					Debug.Log ("Entering Primary Attack");
//					_animation.material.color = Color.green;
//					//Call (BaseEquipmentStateModule.EquipmentActions.ActivateWeapon, user.PrimaryWeapon.WeaponState);
//					user.PrimaryWeapon.WeaponState.ActivateWeapon();
//					Debug.Log ("Succesfully returned from equipment action");
////				}	
//			}
//
//		}
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
//					if (currentState.ToString () == CharacterActions.Idle.ToString () || currentState.ToString () == CharacterActions.Walk.ToString ())
//							Call (BaseEquipmentStateModule.EquipmentActions.StartPrimary, user.PrimaryWeapon.WeaponState);
//
//					else if(currentState.ToString () == CharacterActions.Run.ToString ())
//							Call (BaseEquipmentStateModule.EquipmentActions.RunAttack, user.PrimaryWeapon.WeaponState);
//						    
//					else if (currentState.ToString () == CharacterActions.Dodge.ToString ()) 
//							Call (BaseEquipmentStateModule.EquipmentActions.DodgeAttack, user.PrimaryWeapon.WeaponState);
//					else
//							user.PrimaryWeapon.WeaponState.Followup = BaseEquipmentStateModule.FollowupType.Primary;
					
//					attacking = false;
			//Call (BaseEquipmentStateModule.EquipmentActions.ActivateWeapon, user.PrimaryWeapon.WeaponState);
//				user.PrimaryWeapon.WeaponState.ActivateWeapon();
			Debug.Log ("Succesfully returned from equipment action");
			//				}	
		}

	}

//	protected IEnumerator PrimaryAttack_EnterState ()
//	{
//		Debug.Log ("Entering Primary Attack");
//		_animation.material.color = Color.green;
//		//Call (BaseEquipmentStateModule.EquipmentActions.ActivateWeapon, user.CharEquipment.Primary.WeaponState);
//		Debug.Log ("Succesfully returned from equipment action");
//		currentState = CharacterActions.Idle;
//				
//		yield break;
//	}
//
//	protected void PrimaryAttack_ExitState ()
//	{
//		Debug.Log ("Exiting from equipment action");
//		currentState = CharacterActions.Idle;
//	}
	#endregion

	protected IEnumerator SecondaryAttack_EnterState ()
	{
		yield break;
	}
}