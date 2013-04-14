using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public interface ICharacter {
	MonoBehaviour CharBase {get;}

	//DEV
	IInput CharInput { get;}
	IAnimation CharAnimation {get;}
	IPhysics CharPhysics {get;}
	ICharacterStateMachine CharState {get;}

	//USER
	ICharacterStats CharStats {get;}
	IActions CharActions {get;}
}


[RequireComponent (typeof(CharacterController))]						//the active "body" of the character, used for movement and collision detection
[RequireComponent (typeof(Rigidbody))]									//the passive "body" of the character, used for physics effects such as in combat
//[RequireComponent (typeof(IRagePixel))]									//the animation plugin for the character

/// <summary>
/// Base Character script that acts as the foundation for all other character related scripts and components.
/// This abstract script lays down the basic functionality (stats, movement, basic states)for any in-game character, 
/// which will be fine tuned in later derived scripts. </summary>
public abstract class BaseCharacter : MonoBehaviour, ICharacter {
	#region Fields
	//Inspector Fields - values are initialized, but are set via the instructor view in Unity.
	//[SerializeField] protected CharState state;



	//Primary Vital Bars
	protected VitalBar healthBar;
	protected VitalBar staminaBar;
	protected VitalBar energyBar;

	//Internal Operational Fields
//	private Transform _coordinates;										//The primary identifier for Unity game objects
//	private CharacterController _body;									//Used for rudimentary collider and physics. Mainly used for movement and detecting collisions.
//	private Rigidbody _charPhysics;										//Used for more complex physics and collisions. Mainly used for combat.
//	protected Vector3 _target;											//The foreign game object that dictates the character's target. Use changes depending on derived scripts.
//
//	private IRagePixel _charAnimation;									//Contains sprite and animations for unit
//	private Animation _charAnimation3D;

	//Internal State Flags
//	private bool _hasTarget ;
//	private bool _weaponReady;
//
//	private bool _isRunning;
//	private bool _isDefending;
//	private bool _isCountering;
//	private bool _isEvading;
//	
//	//private Vector3 _moveDir;											//internal movement coordinates
//	//private Vector3 _lookDir;											//internal aiming coordinates
//	private Vector3 _knockDir;										//internal knockback coordinates
	#endregion Fields

	#region Functionality Modules
	//Input (created, but needs to be revised, and reworked to a base interface declared here, but instantiated in derived classes)
	//Animation (Still iffy on how this is all setup, need to research. Should plug into equipment and state machine to determine what animations are played)
	//Physics (will tie into character movement, non-statistical combat calculations, and many core unity functions)
	//State Machine (will tie into the majority of the modules, looking at equipment and MoveSet for available actions, animation to cue the correct animation, physics for calculations, etc.)
	//ActionsModule (This will connect to charEquipment to become a list of all available actions a character may perform.)

	#region Module Members
	protected IInput charInput;
	protected IAnimation charAnimation;
	protected IPhysics charPhysics;
	protected ICharacterStateMachine charState;
	protected IActions charActions;

	[SerializeField] protected ICharacterStats charStats; //This holds all gameplay and combat stats that are inherent to the character
	[SerializeField] protected BaseMovementModule charMovement; //This will likely get wrapped up into character stats and physics
	[SerializeField] protected BaseEquipmentLoadoutModule charEquipment; //This holds all gameplay and combat stats that are inherent to the loadout

	#endregion
	
//	public interface IStateMachine {
//		void ActivateRun ();
//		void ActivateDodge ();
//		void ActivatePrimary ();
//		void ActivateSecondary ();
//		void ActivateSpecial (int accessSlot);
//		//Signal for State Transition
//		//	events from input (such as the shift key to prompt ActivateDodge)
//
//		//Transitions
//		//	 this consists of the methods that prompt a transition (ActivatePrimary) and the single method that then facilitates the transition (StateTransition)
//		// ActivateRun
//		// ActivateDodge
//		// ActivateInteract
//		// ActivatePrimary
//		// ActivateSecondary
//		// ActivateSpecial
//
//		//States
//		//	this consists of a handful of base states that determine the actions available to the player at any given time.
//		// In-Menu
//		// In-Cutscene
//		//
//		// At-Ease
//		// Combat-Ready
//	}
//	public class StateMachineModule {
//		void ActivateRun () {
//
//		}
//		void ActivateDodge () {
//
//		}
//		void ActivatePrimary () {
//
//		}
//		void ActivateSecondary () {
//
//		}
//		void ActivateSpecial (int accessSlot) {
//
//		}
//
//		protected IEnumerator AtEase () {
//
//		}
//		protected IEnumerator CombatReady () {
//
//		}
//
//		protected IEnumerator IdleState () {					//Starting state, and where other states eventually return to. 
//			while (state == CharState.Idle) {
//				if (CharInput.MoveDir != Vector3.zero) {
//					if (IsEvading == true && CharMovement.DodgeCooldown < CharStats.Stamina.CurValue) yield return StartCoroutine (CharMovement.Dodge ());
//					else if (IsRunning == true && CharMovement.RunCost < CharStats.Stamina.CurValue) CharMovement.Run ();
//					else CharMovement.Walk ();
//				}
//				
//				//			Debug.LogWarning("Velocity: " + Body.velocity);
//				//			Debug.LogWarning("WalkSpeed: " + CharMovement.WalkSpeed);
//				//Debug.LogWarning();
//				yield return null;
//			}
//			StateTransition ();
//			yield break;
//		}
//		
//		protected IEnumerator CombatReadyState () {
//			while (state == CharState.CombatReady) {
//				
//				//if () {}						//if initiating an attack
//				if (CharInput.MoveDir != Vector3.zero) {
//					if (IsEvading == true) StartCoroutine (CharMovement.Dodge ());
//					else if (IsRunning == true && CharMovement.RunCost < CharStats.Stamina.CurValue) CharMovement.Run ();
//					else CharMovement.Strafe ();
//				}
//				else CharMovement.Aim();
//				yield return null;
//			}
//			StateTransition ();
//			yield break;
//		}
//		
//		protected IEnumerator AttackingState () {
//			while (state == CharState.Attacking) {
//				//yield return StartCoroutine ();
//			}
//			yield break;
//		}
//	}
	
//	public interface IStats {
//
//	}
//	public BaseStatsModule CharStats {
//		get {return charStats;}
//	}

	public BaseMovementModule CharMovement {
		get { return charMovement;}
	}

	#endregion

	#region Properties
	public abstract IInput CharInput {
		get;
	}



//	public BaseCharacter.BaseCharacterEquipmentModule CharEquipment 
//	{
//		get {return charEquipment;}
//		set {charEquipment = value;}
//	}
//	public Transform Coordinates 
//	{
//		get {return _coordinates;}
//		set {_coordinates = value;}
//	}
//	public CharacterController Body 
//	{
//		get {return _body;}
//		set {_body = value;}
//	}
//	public Rigidbody CharPhysics
//	{
//		get {return _charPhysics;}
//		set {_charPhysics = value;}
//	}
//	public IRagePixel CharAnimation
//	{
//		get {return _charAnimation;}
//		set {_charAnimation = value;}
//	}
//	public Animation CharAnimation3D {
//		get {return _charAnimation3D;}
//		set {_charAnimation3D = value;}
//	}
//	public virtual Vector3 Target
//	{
//		get {return _target;}
//		set {_target = value;}
//	}
//
//	public Vector3 KnockDir {
//		get {return _knockDir;}
//		set {_knockDir = value;}
//	}
	
	//Internal State
//	public Vector3 MoveDir
//	{
//		get {return _moveDir;}
//		set {_moveDir = value;}
//	}
//
//	public Vector3 LookDir
//	{
//		get {return _lookDir;}
//		set {_lookDir = value;}
//	}
	
	//State Flags
//	public CharState State {
//		get {return state;}
//		set {state = value;}
//	}
//	public void DrawWeapon() {
//		_weaponReady = !_weaponReady;
//	}
//	public bool WeaponReady {
//		get {return _weaponReady;}
//		set {_weaponReady = value;}
//	}	
//	public bool IsRunning {
//		get { return _isRunning;}
//		set { _isRunning = value;}
//	}
//	public bool IsDefending {
//		get {return _isDefending;}
//		set {_isDefending = value;}
//	}
//	public bool IsEvading {
//		get {return _isEvading;}
//		set {_isEvading = value;}
//	}

	public MonoBehaviour CharBase {
		get {return this;}
	}

	public IAnimation CharAnimation {
		get {return charAnimation;}
	}
	public IPhysics CharPhysics {
		get {return charPhysics;}
	}
	public ICharacterStats CharStats {
		get {return charStats;}
	}
	public ICharacterStateMachine CharState {
		get {return charState;}
	}
	public IActions CharActions {
		get {return charActions;}
	}
	#endregion Properites
	
	#region Initialization


//	protected virtual void ModuleInitialization ()
//	{
//		//Initialize Components
//		_coordinates = GetComponent<Transform>();
//		_body = GetComponent<CharacterController>();
//		_charPhysics = GetComponent<Rigidbody>();
//		_charAnimation = GetComponent<RagePixelSprite>();	
//
//		CharMovement.Setup(this);
//		
//		//Set Internal Values
////		MoveDir = Vector3.zero;
////		LookDir = Vector3.zero;
//	}
//	protected virtual void ModuleSetup () {
//		SetVitals();
//		//StartVitalRegen();
//	}
	
	/// <summary>
	/// Sets the values of all vitals associated with the character. </summary>
	/// <remarks>
	/// Uses the serialized Attributes to generate all relevant values for Vitals, and thus must be set before hand. </remarks>
//	protected void SetVitals()
//	{
//		//Primary Vitals
////		CharStats.Health.SetScaling(CharStats.SelectStatScaling(CharStats.Health.ScalingAttribute), CharStats.SelectStatScaling(CharStats.Health.RegenAttribute));
////		CharStats.Stamina.SetScaling(CharStats.SelectStatScaling(CharStats.Stamina.ScalingAttribute), CharStats.SelectStatScaling(CharStats.Stamina.RegenAttribute));
////		CharStats.Energy.SetScaling(CharStats.SelectStatScaling(CharStats.Energy.ScalingAttribute), CharStats.SelectStatScaling(CharStats.Energy.RegenAttribute));
////		
////		//Status Vitals
////		CharStats.StunResistance.SetDecay(CharStats.SelectStatScaling(CharStats.StunResistance.decayAttribute));
//	}

	/// <summary>
	/// Initialization method for the character, from which all components and members are defined and set. </summary>
	protected virtual void Initialization () {
		//Get Components in Physics Module
		//Create attributes and vitals in Stats Module
		//Create all actions/abilities
	}

	/// <summary>
	/// Initializes the values associated to this character instance. </summary>
	protected virtual void Awake () 
	{
		Initialization();
	}	
	
	protected virtual void Setup () {
		CharMovement.Setup(this);
		//CharStats.Setup(this);
	}

	/// <summary>
	/// Starts the instance of this character, beginning a coroutine for both determing 
	/// and executing character actions as well as their associated animations. </summary>
	protected virtual void Start () {
		Setup ();
		//StartCoroutine (DecideAnimation());
		//StartCoroutine (IdleState());
	}
	#endregion
	
	#region Combat Status Maintenance
	/// <summary>
	/// Applies damage to the character. </summary>
	/// <param name='atkStrength'> Strength of attack being received. </param>
//	public void ApplyDamage(float atkStrength) 
//	{
//		float damage = atkStrength;
//		CharStats.Health.CurValue -= damage;
//	}
//	/// <summary>
//	/// Decreases the current value of the character's stamina vital. </summary>
//	/// <param name='cost'> The value to be decreased from character's vital, from an ability used or an attack received. </param>
//	public void ApplyStaminaUse(float cost)
//	{
//		CharStats.Stamina.CurValue -= cost;
//	}
//	/// <summary>
//	/// Decreases the current value of the character's energy vital. </summary>
//	/// <param name='cost'> The value to be decreased from character's vital, typically from using a special ability. </param>
//	public void ApplyEnergyUse(float cost)
//	{
//		CharStats.Energy.CurValue -= cost;
//	}
//	
	/// <summary>
	/// Applies recoil to character from a failed attack (if blocked for example). </summary>
	/// <param name='hitDir'> Direction in which the character is knocked. </param>
//	public void ApplyAttackRecoil (Vector3 hitDir)
//	{
//		StartCoroutine (StunnedState());
//		//TransitionToStunned();
//	}
//	
//	/// <summary>
//	/// Applies hit stun to the character, and activates the character's stunned state if the max value is exceeded.
//	/// </summary>
//	/// <param name='hitStrength'> Strength (physical, not statistical) of the attack. </param>
//	/// <param name='hitDir'> Direction in which the attack pushes the character. </param>
//	public void ApplyHitStun(float hitStrength, Vector3 hitDir)
//	{
//		CharStats.StunResistance.CurValue += hitStrength;
//		
//		if (CharStats.StunResistance.CurValue >= CharStats.StunResistance.MaxValue)
//		{
//			Debug.Log ("Beginning stunned");
//			CharStats.StunResistance.CurValue = CharStats.StunResistance.MinValue;
//			_knockDir = hitDir * Time.deltaTime * CharStats.StunStrength;				//new Vector3(hitDir.x, hitDir.y, hitDir);
//
//			StartCoroutine (StunnedState());
//			//TransitionToStunned();
//		}
//	}
	#endregion

	//TODO: convert into State Module
	#region State Transitions
//	public enum CharState {
//		Stunned,													//will disrupt character's ability to react when activated
//		Idle,
//		CombatReady,
//		Dodge,														//will encapsulate any damage-mitigating movement state. 
//		Defend,														//will encapsulate any damage-mitigating equipment state.
//		Counter,													//will encapsulate any damage-mitigating counter-attack state.
//		Attacking,													//will encapsulate any state that stems from using an equipped weapon.
//		ComboAttack1,
//		ComboAttack2,
//		ComboAttack3,
//		ComboAttack4,
//		ComboAttack5,
//		Finisher,
//		AltFinisher,
//		RangedAttack,
//		RunAttack,
//		DodgeAttack,
//		Special1,
//		Special2,
//		Special3
//	}
//	protected virtual void StateTransition()
//	{
//		string methodName = state.ToString () + "State";
//		Debug.Log (methodName);
//		System.Reflection.MethodInfo info = GetType().GetMethod(methodName,System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
//		Debug.LogWarning("activating method of name : " + methodName);
//		StartCoroutine((IEnumerator)info.Invoke(this, null));
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
//	protected virtual void SwitchCombatStances () {
//		Debug.LogError ("SWITCHING STANCES");
//		if (State == CharState.Idle) {
//			WeaponReady = true;
//			state = CharState.CombatReady;
//		}
//		else if (State == CharState.CombatReady) {
//			WeaponReady = false;
//			state = CharState.Idle;
//		}
//		else return;
//	}

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
/*	protected virtual IEnumerator StunnedState() {
		if (_charPhysics != null) Debug.Log("rigidbody is attached");
		float _timer = 0; 
		_timer = Time.time + CharStats.StunDuration;
		_charPhysics.isKinematic = false;
		
		while (_timer - Time.time >= 0.05f) {								//Initial Knockback
			Debug.LogWarning("STUNNED AND MOVING");
			Body.Move(_knockDir);
			yield return new WaitForFixedUpdate();
		}
		while (_timer - Time.time >0) {									//Stun Period
			yield return null;
		}
		_charPhysics.isKinematic = true;
		_knockDir = Vector3.zero;

		if (WeaponReady) state = CharState.CombatReady;
		else state = CharState.Idle;
		StateTransition();
		yield break;
	}*/

//	protected abstract IEnumerator IdleState();							//Base state with weapons' sheathed.		
//	protected abstract IEnumerator CombatReadyState();					//Base state with weapons' drawn.

	//protected abstract IEnumerator DefendState();
	//protected abstract IEnumerator DodgeState();
	#endregion States
	
//	protected abstract IEnumerator DecideAnimation ();

	/// <summary>
	/// Basic equipment module for characters. Nested classes contained within further specify
	/// the kind of equipment that can be set to this character. </summary>	
	/// <remarks> When character is created, their combat style/equipment loadout is specified. This
	/// will then determine what weapons may be equipped. </remarks>


//	public interface IEquipmentModule {
//		IBaseEquipment Primary {get;}
//		IBaseEquipment Secondary {get;}
//	}


//
//	[System.Serializable] public class EquipmentModule {
//		[SerializeField] private BaseCharacterEquipmentModule equipment;
//
//		public BaseCharacterEquipmentModule CharEquipment {
//			get { return equipment;}
//			set {equipment = value;}
//		}
//	}
//
//	[System.Serializable] public class BaseCharacterEquipmentModule
//	{
//		public enum EquipmentStance {
//			Balanced,
//			Power,
//			Finesse
//		}
//		[SerializeField] private EquipmentStance combatStance;
//		[SerializeField] private BaseEquipment equipmentSlot1;
//		[SerializeField] private BaseEquipment equipmentSlot2;
//
//		#region Fields
//	//	public IBaseEquipment<IBaseEquipment> primary;
//		private IMoveset primaryMoveSet;
//		private IMoveset secondaryMoveSet;
//
//		private BaseCharacter _user;
//	//	[SerializeField] EquipmentLoadoutType combatStance;
//
//		// Class Abilities (Class Ability)
//
//		// Style (Balanced, Power, Finesse)
//		//[SerializeField] protected BaseEquipment.BaseEquipmentStats.EquipmentStyle combatStyle;
//
//		// Weapon Slots (General Moveset)
//
//
//		// Special Abilities (Specific Moveset)
//
//		
//		[SerializeField] protected WeaponAbility.SpecialAttack special1;
//		[SerializeField] protected WeaponAbility.SpecialAttack special2;
//		[SerializeField] protected WeaponAbility.SpecialAttack special3;
//		#endregion Fields
//
//		#region Properties
//		public virtual WeaponAbility.SpecialAttack Special1 
//		{
//			get {return special1;}
//			set {special1 = value;}
//		}
//		public virtual WeaponAbility.SpecialAttack Special2 
//		{
//			get {return special2;}
//			set {special2 = value;}
//		}
//		public virtual WeaponAbility.SpecialAttack Special3 
//		{
//			get {return special3;}
//			set {special3 = value;}
//		}
//		#endregion Properties
//
//		#region Initialization
//		public BaseCharacterEquipmentModule () {}
//		public void Setup (BaseCharacter user)
//		{
//			_user = user;
//
//			switch (combatStance) {
//			case EquipmentStance.Balanced:
//
//				break;
//
//			case EquipmentStance.Power:
//
//				break;
//
//			case EquipmentStance.Finesse:
//
//				break;
//			}
//
////			Primary = equipmentSlot1;
////			Secondary = equipmentSlot2;
//		}
//		#endregion Initialization
//
//		#region Setup
//
//		#endregion
//
//		#region Loadout Specialization
//		interface IMoveset {
//			IBaseEquipment Primary {get;}
//			IBaseEquipment Secondary {get;}
//		}
//		public class OneWeaponMoveset : IMoveset {
//			[SerializeField] protected BaseEquipment primary;
//
//			public virtual IBaseEquipment Primary {
//				get {return primary;}
//			}
//			public virtual IBaseEquipment Secondary {
//				get {return primary;}
//			}
//		}
//		public class TwoWeaponMoveset : OneWeaponMoveset {
//			private IBaseEquipment secondary;
//
//			public override IBaseEquipment Secondary {
//				get {return secondary;}
//			}
//		}
//		#endregion Loadout Specilization
//	}
//
//	public interface IInput {
//		//protected Vector3 _moveDir;
//		Vector3 MoveDir {get;}
//
//		//protected Vector3 _lookDir;
//		Vector3 LookDir {get;}
//	}
//
//	///<summary>
//	/// Character Movement Module. Holds movement-related values, as well as the relevant functions that use them. </summary>
//	[System.Serializable] public class CharacterMovementModule {
//		#region Fields
//		//Inspector Fields
//		[SerializeField, Range (0,10)] private float turnSpeed = 3;
//
//		[SerializeField, Range (0,10)] private float walkSpeed = 5;
//		[SerializeField, Range (0,10)] private float walkingBlockSpeed = 4;
//
//		[SerializeField, Range (0,20)] private float runSpeed = 7;
//		[SerializeField, Range (0,10)] private float runCost = 1;
//
//		[SerializeField, Range (0,60)] private float dodgeSpeed = 30;
//		[SerializeField, Range (0,20)] private float dodgeCost = 15;
//		[SerializeField, Range (0,2)] private float dodgeStartup;
//		[SerializeField, Range (0,2)] private float dodgeDuration;
//		[SerializeField, Range (0,2)] private float dodgeCooldown;
//
//		//Internal Fields
//		private BaseCharacter _user;
//		private Transform _coordinates;
//		#endregion
//		
//		#region Properties
//		public float TurnSpeed {
//			get {return turnSpeed;}
//		}
//		public float WalkSpeed {
//			get {return walkSpeed;}
//		}
//		public float RunSpeed {
//			get {return runSpeed;}
//		}
//		public float RunCost {
//			get {return runCost;}
//		}
//		public float DodgeSpeed {
//			get {return dodgeSpeed;}
//		}
//		public float DodgeCost {
//			get {return dodgeCost;}
//		}
//		public float DodgeStartup {
//			get {return dodgeStartup;}
//		}
//		public float DodgeDuration {
//			get {return dodgeDuration;}
//		}
//		public float DodgeCooldown {
//			get {return dodgeCooldown;}
//		}
//		public float WalkingBlockSpeed {
//			get {return walkingBlockSpeed;}
//		}
//		
//		public BaseCharacter Char {
//			get {return _user;}
//		}
//		#endregion
//		
//		#region Initialization
//		//public CharacterMovementModule(){}
//		public void Setup (BaseCharacter user) {
//			_user = user;
//			_coordinates = _user.Coordinates;
//		}
//		#endregion
//		
//		#region Character Movements
//		public void Walk () {
//			Turn ();
//			_user.Body.Move (WalkSpeed * _user.CharInput.MoveDir.normalized * Time.deltaTime);		
//		}
//
//		public void Strafe () {
//			Aim ();
//			_user.Body.Move (WalkSpeed * _user.CharInput.MoveDir.normalized * Time.deltaTime);		
//		}
//
//		public void Run () {
//			//if (CharMovement.RunCost > CharStats.Stamina.CurrentValue) return;
//			_user.CharStats.Stamina.CurValue -= RunCost * Time.deltaTime;
//			Turn ();
//			_user.Body.Move (RunSpeed * _user.CharInput.MoveDir * Time.deltaTime);
//		}
//
//		/// <summary>
//		/// Turn the character based on the direction they're moving. </summary>
//		public void Turn ()
//		{
//			Vector3 dir = _user.CharInput.MoveDir;
//			if (dir == Vector3.zero) return;
//			
//			float aim = Mathf.Atan2(-dir.x, dir.y) * Mathf.Rad2Deg;						//holds direction the character should be facing
//			_coordinates.rotation = Quaternion.Euler(0, 0, aim);
//		}
//		/// <summary>
//		/// Turns the character based on the direction of they're desired target. </summary>
//		public void Aim ()
//		{
//			Vector3 target = _user.CharInput.LookDir;
//			float angleX = target.x - _coordinates.position.x;
//			float angleY = target.y - _coordinates.position.y;
//			float targetAngle = Mathf.Atan2 (angleY, angleX) * Mathf.Rad2Deg;
//			
//			Quaternion fromRotation = _coordinates.rotation;
//			Quaternion finalRotation = Quaternion.AngleAxis(targetAngle - 90, Vector3.forward);
//			_coordinates.rotation = Quaternion.RotateTowards(fromRotation, finalRotation, turnSpeed);
//		}
//		/// <summary>
//		/// Causes the character to Dodge. </summary>
//		public IEnumerator Dodge ()
//		{
//			if (_user.CharStats.Stamina.CurValue < dodgeCost) yield break;
//			else _user.CharStats.Stamina.CurValue -= dodgeCost;
//
//			Vector3 dir = Char.CharInput.MoveDir;
//
//			if (dodgeStartup > 0) yield return new WaitForSeconds(dodgeStartup);
//			
//			float curDuration = Time.time + dodgeDuration;												
//			do {
//				if (_user.GetState == CharState.Idle) Turn();
//				if (_user.GetState == CharState.CombatReady) Aim();
//				_user.Body.Move (dir * DodgeSpeed * Time.deltaTime);																		
//				yield return null;
//			} while (curDuration > Time.time);
//			_user.IsEvading = false;
//			
//			if (dodgeCooldown > 0) yield return new WaitForSeconds(dodgeCooldown);
//
//			if (_user.WeaponReady) _user.state = CharState.CombatReady;
//			else _user.state = CharState.Idle;
//
//			yield break;
//		}
//		#endregion
//	}
}