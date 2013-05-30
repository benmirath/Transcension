using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevisedStateMachine : StateMachineBehaviourEx
{
		public enum CharacterActions
		{
		/// <summary>
		/// Base States. These are the first level of states that a character can exist in. They have no set time limit, and can 
		/// act as defaults. </summary>
				
				Idle,
		/// <summary> Attacking State. This stops normal input recognition such as walking, and transfers control to the current weapon's state machine </summary>
				Attack,			
		/// <summary> Sneaking State. This features mostly similar. </summary>
				Sneak, 			//base state, weapon sheathed

		/// <summary> Action States. These are the next level of states that a character can exist in. They have some limitation on use and 
		/// duration, often with a clear beginning, middle and end. </summary>
				Stun,
				KnockedBack,
				Walk,
				Run,
				Dodge,
		}
	#region Properties
		[SerializeField]protected BaseCharacterStats stats;
		[SerializeField]protected MoveSet moveSet;					//used to access actions for state activation

		[SerializeField]protected ICharacter user;
		[SerializeField]protected MeshRenderer _animation;
		[SerializeField]protected BaseInputModule input;			//used to monitor advanced state timing and activation

		public string State;

		public IInput CharInput { get { return input; } }

		public IMoveSet MoveSet { get { return moveSet; } }
	#endregion

	//bool states. These are state flags that persist across multiple states.
	#region State Flags
		bool armed;
		bool invincible;
		bool attacking;
		bool sneaking;
		bool stunned;
		bool running;
		bool defending;
		bool countering;
	#endregion

	#region Setup
		protected override void OnAwake ()
		{
				base.OnAwake ();
				stats = GetComponent<BaseCharacterStats> ();
				moveSet = GetComponent<MoveSet> ();									//used to access actions for state activation
				_animation = GetComponent<MeshRenderer>();


				armed = false;
				attacking = false;
				sneaking = false;

				stunned = false;
				running = false;
				defending = false;
				countering = false;
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
						State = currentState.ToString ();
						yield return null;
				}
		}

		/// <summary>
		/// Checks the ability vital, making sure there's enough to activate the ability.
		/// </summary>
		/// <returns><c>true</c>, if ability vital has enough to activate, <c>false</c> otherwise.</returns>
		/// <param name="ability">Ability.</param>
		private bool CheckAbilityVital (AbilityProperties ability)
		{
				Debug.Log ("3");
				if (ability.Cost > 0) {
						//checks if ability is a special or not, determining vital type used.
						if (ability.IsSpecial) {
								if (stats.Energy.CurValue > ability.Cost) {
										Debug.Log ("Special Ability activated");
										stats.Energy.CurValue -= ability.Cost;
										return true;
								} else 
										return false;
						} else {
								if (stats.Stamina.CurValue > ability.Cost) {
										Debug.Log ("Standard Ability activated");
										stats.Stamina.CurValue -= ability.Cost;
										return true;
								} else 
										return false;
						}
				}
				else 
				{
						Debug.Log ("Ability did not activate");
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

		protected void TransitionToPrimary ()
		{
				if (currentState.ToString () == CharacterActions.Idle.ToString () || currentState.ToString () == CharacterActions.Walk.ToString () && armed) {
						//launch standard attack
				} else if (currentState.ToString () == CharacterActions.Run.ToString () && armed) {
						//launch running attack
				} else if (currentState.ToString () == CharacterActions.Dodge.ToString () && armed) {
						//launch dodging attack
				}
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
						moveSet.CharMovement.Aim.durationAbility ();
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
				if (currentState == CharacterActions.Idle.ToString () || currentState == CharacterActions.Run.ToString ()) {
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
						moveSet.CharMovement.Walk.durationAbility ();
				else 												//armed, will walk/strafe
						moveSet.CharMovement.Strafe.durationAbility ();
		}

		protected void Walk_ExitState ()
		{

		}
		#endregion

		#region Run
		protected void TransitionToRun ()
		{
				Debug.Log ("Attempting Transition: Run");
				if (currentState == CharacterActions.Walk.ToString ()) {
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
						moveSet.CharMovement.Run.enterAbility ();
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
						if (CheckAbilityVital (moveSet.CharMovement.Run))//Character has enough stamina to activate run
								moveSet.CharMovement.Run.durationAbility ();
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
						currentState = CharacterActions.Dodge;
				else {
				}//dont transition;
		}

		protected IEnumerator Dodge_EnterState ()
		{
				_animation.material.color = Color.gray;
				float timer1 = moveSet.CharMovement.Dodge.EnterLength + Time.time;
				float timer2 = moveSet.CharMovement.Dodge.DurationLength + timer1;
				while (timer1 >= Time.time) {
						moveSet.CharMovement.Dodge.enterAbility ();
						yield return null;
				}
				while (timer2 >= Time.time) {
						moveSet.CharMovement.Dodge.durationAbility ();
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

		protected IEnumerator SheatheWeapon_EnterState ()
		{
				armed = !armed;
				_animation.material.color = Color.magenta;
				currentState = CharacterActions.Idle;
				yield break;
		}

		protected IEnumerator Primary_EnterState ()
		{
				yield break;
		}

		protected IEnumerator Secondary_EnterState ()
		{
				yield break;
		}
}