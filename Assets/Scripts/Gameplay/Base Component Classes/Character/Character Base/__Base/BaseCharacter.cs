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
[RequireComponent (typeof(IAnimation), typeof(CharacterController))]		//Engine Logic
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

	// The core modules that provide unique functionality to the character. Each of these should be present in some form on the character.
	//Functionality Modules
	//Input (created, but needs to be revised, and reworked to a base interface declared here, but instantiated in derived classes)
	//Animation (Still iffy on how this is all setup, need to research. Should plug into equipment and state machine to determine what animations are played)
	//State Machine (will tie into the majority of the modules, looking at equipment and MoveSet for available actions, animation to cue the correct animation, physics for calculations, etc.)
	//ActionsModule (This will connect to charEquipment to become a list of all available actions a character may perform.)

	//[SerializeField] protected IAnimation charAnimation;
	[SerializeField] protected CharacterType charType;
	public Material oldCharAnimation;
	protected BaseInputModule charInput;
	protected CharacterStateMachine charState;
	protected CharacterStats charStats; //This holds all gameplay and combat stats that are inherent to the character
	protected CharacterMovesetModule charMoveSet;
	protected BaseEquipmentLoadoutModule charEquipment;
	protected Animator charAnimation;
	#endregion

	#region Properties

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

	public CharacterType CharType 				{ get { return charType; } }
	public BaseInputModule CharInput 			{ get { return charInput; } }
	public CharacterStateMachine CharState 			{ get { return charState; } }
	public CharacterMovesetModule CharActions 		{ get { return charMoveSet; } }
	public CharacterStats CharStats 			{ get { return charStats; } }
	public BaseEquipmentLoadoutModule CharEquipment 	{ get { return charEquipment; } }
	public Animator CharAnimation 				{ get { return charAnimation; } }
	#endregion Properites
	
	#region Initialization
	/// <summary>
	/// Initializes the values associated to this character instance. </summary>
	protected virtual void Awake () 
	{
		//Unity Components
		_coordinates = GetComponent<Transform>();
		_controller = GetComponent<CharacterController>();
		_rigidbody = GetComponent<Rigidbody>();

		//Basic Character Components
		charAnimation = GetComponent<Animator> ();
		charStats = GetComponent<CharacterStats> ();
		charMoveSet = GetComponent<CharacterMovesetModule> ();
		if (charType == CharacterType.Enemy)
			oldCharAnimation = GetComponent <MeshRenderer> ().material;
		else
			oldCharAnimation = transform.Find ("Renderer").GetComponent<SkinnedMeshRenderer>().material;

		if (oldCharAnimation == null)
			Debug.LogError (name+"'s material is null");

//			GetComponentInChildren<MeshRenderer> ().material;

		//Add specialized character components
		switch (charType) {
		case CharacterType.Player:
			charState = gameObject.AddComponent <CharacterStateMachine> ();			//dictates character's moveset and availble actions
			charInput = gameObject.AddComponent <PlayerInput> ();				//dictates characters focus and behaviour
			break;

		case CharacterType.Enemy:
			charState = gameObject.AddComponent <EnemyStateModule> ();
			charInput = gameObject.AddComponent <AIInput> ();
			break;

		default:
			break;
		}
	}	

	/// <summary>
	/// Starts the instance of this character, beginning a coroutine for both determing 
	/// and executing character actions as well as their associated animations. </summary>
	protected virtual void Start () {
	}
	#endregion
}


public interface ICharacter {
	Transform Coordinates {get;}
	CharacterController Controller {get;}
	Rigidbody Rigid {get;}

	//DEV
	BaseInputModule CharInput { get;}
	//IAnimation CharAnimation {get;}
	CharacterStateMachine CharState {get;}
	//IGUI (will turn into a full module down the line, handle health bars and other gui elements)

	//USER
	BaseCharacter.CharacterType CharType {get;}
	CharacterStats CharStats {get;}
	CharacterMovesetModule CharActions {get;}
	BaseEquipmentLoadoutModule CharEquipment { get;}
}
