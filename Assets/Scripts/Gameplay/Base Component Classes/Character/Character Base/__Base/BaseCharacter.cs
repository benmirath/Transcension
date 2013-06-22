using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

//[RequireComponent (typeof(CharacterController))]						//the active "body" of the character, used for movement and collision detection
//[RequireComponent (typeof(Rigidbody))]									//the passive "body" of the character, used for physics effects such as in combat
//[RequireComponent (typeof(IRagePixel))]									//the animation plugin for the character

/// <summary>
/// Base Character script that acts as the foundation for all other character related scripts and components.
/// This script lays down the basic functionality (stats, movement, basic states)for any in-game character, 
/// which will be fine tuned in later derived scripts. </summary>
[RequireComponent (typeof(IAnimation), typeof(CharacterController), typeof(Rigidbody))]		//Engine Logic
[RequireComponent (typeof(CharacterStats), typeof(CharacterMovesetModule))]
public class BaseCharacter : MonoBehaviour, ICharacter 
{
	public enum CharacterType
	{
		Player,
		Enemy,
		Ally,
		Neutral,
	}

	#region Fields
	//Primary Vital Bars
	protected VitalBar healthBar;
	protected VitalBar staminaBar;
	protected VitalBar energyBar;

	//Internal Operational Fields
	private Transform _coordinates;										//The primary identifier for Unity game objects
	private CharacterController _controller;							//Used for rudimentary collider and physics. Mainly used for movement and detecting collisions.
	private Rigidbody _rigidbody;										//Used for more complex physics and collisions. Mainly used for combat.

	/// The core modules that provide unique functionality to the character. Each of these should be present in some form on the character.
	//Functionality Modules
	//Input (created, but needs to be revised, and reworked to a base interface declared here, but instantiated in derived classes)
	//Animation (Still iffy on how this is all setup, need to research. Should plug into equipment and state machine to determine what animations are played)
	//State Machine (will tie into the majority of the modules, looking at equipment and MoveSet for available actions, animation to cue the correct animation, physics for calculations, etc.)
	//ActionsModule (This will connect to charEquipment to become a list of all available actions a character may perform.)

	//[SerializeField] protected IAnimation charAnimation;
	protected Material charAnimation;
	protected BaseInputModule charInput;
	protected BaseCharacterStateModule charState;
	protected CharacterStats charStats; //This holds all gameplay and combat stats that are inherent to the character
	protected CharacterMovesetModule charMoveSet;
	protected BaseEquipmentLoadoutModule charEquipment;

	[SerializeField] protected CharacterType charType;

//	[SerializeField]
//	protected BaseEquipment primaryWeapon;
//	[SerializeField]
//	protected BaseEquipment secondaryWeapon;

	//[SerializeField] protected BaseMovementModule charMovement; //This will likely get wrapped up into character stats and physics
	//[SerializeField] protected BaseEquipmentLoadoutModule charEquipment; //This holds all gameplay and combat stats that are inherent to the loadout
	#endregion

	#region Properties

	public CharacterType CharType { get { return charType; } }
	public Transform Coordinates 
	{
		get {return _coordinates;}
		set {_coordinates = value;}
	}
	public CharacterController Controller 
	{
		get {return _controller;}
		set {_controller = value;}
	}
	public Rigidbody Rigid
	{
		get {return _rigidbody;}
		set {_rigidbody = value;}
	}
	
//	public IAnimation CharAnimation {
//		get {return charAnimation;}
//	}
	public BaseInputModule CharInput {
		get {return charInput;}
	}
	public BaseCharacterStateModule CharState {
		get {return charState;}
	}
	public CharacterMovesetModule CharActions {
		get {return charMoveSet;}
	}
	public CharacterStats CharStats {
		get {return charStats;}
	}
	public BaseEquipmentLoadoutModule CharEquipment {
		get {return charEquipment;}
	}

//	public BaseEquipment PrimaryWeapon {
//		get {return primaryWeapon;}
//	}
//
//	public BaseEquipment SecondaryWeapon {
//		get {return secondaryWeapon;}
//	}
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
	/// Initializes the values associated to this character instance. </summary>
	protected virtual void Awake () 
	{
		//Unity Components
		_coordinates = GetComponent<Transform>();
		_controller = GetComponent<CharacterController>();
		_rigidbody = GetComponent<Rigidbody>();

		//Basic Character Components
		charStats = GetComponent<CharacterStats>();
		charMoveSet = GetComponent<CharacterMovesetModule>();

		//Add specialized character components
		switch (charType) {
		case CharacterType.Player:
			charState = gameObject.AddComponent <PlayerStateModule> ();			//dictates character's moveset and availble actions
			charInput = gameObject.AddComponent <PlayerInput> ();				//dictates characters focus and behaviour
			break;

		case CharacterType.Enemy:
			charState = gameObject.AddComponent <EnemyStateModule> ();
			charInput = gameObject.AddComponent <AIInput> ();
//			charAnimation.color = Color.red;
//			charAnimation.color = ;
//			charAnimation.color = 
			break;

		default:
			break;
		}


		//primaryWeapon = transform.GetComponentInChildren<>();
	}	
	
	protected virtual void Setup () {
		//CharMovement.Setup(this);
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
}


public interface ICharacter {
	Transform Coordinates {get;}
	CharacterController Controller {get;}
	Rigidbody Rigid {get;}

	//DEV
	BaseInputModule CharInput { get;}
	//IAnimation CharAnimation {get;}
	BaseCharacterStateModule CharState {get;}
	//IGUI (will turn into a full module down the line, handle health bars and other gui elements)

	//USER
	BaseCharacter.CharacterType CharType {get;}
	CharacterStats CharStats {get;}
	CharacterMovesetModule CharActions {get;}
	BaseEquipmentLoadoutModule CharEquipment { get;}
//	BaseEquipment PrimaryWeapon {get;}
//	BaseEquipment SecondaryWeapon {get;}
}
