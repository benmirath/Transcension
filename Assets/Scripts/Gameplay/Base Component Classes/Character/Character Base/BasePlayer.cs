/// <summary>
/// Base player script. Expands on the base character script, adding in more specific functionality for the player character.
/// This includes input (currently just from keyboard), targetting (may later get rolled into input script), and states. </summary>
using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(PlayerInput))]
public class BasePlayer : BaseCharacter, ICharacter
{
	//[SerializeField] protected new CharState state;
	
	#region Properties
	public override BaseInputModule CharInput 
	{
		get {return charInput;}
	}

/*	public CharState CurrentState
	{
		get {return state;}
		set {state = value;}
	}
	public override string GetState 
	{
		get {return CurrentState.ToString();}
	}*/
	#endregion Properties
	
	#region Initialization
	protected override void Awake ()
	{
		base.Awake ();
		charState = GetComponent<PlayerStateModule>();
		charInput = GetComponent<PlayerInput>();
	}
	protected override void Initialization ()
	{
		base.Initialization ();
		//charInput = new PlayerInput (this);
	}


	/// <summary>
	/// This is called once all components are initialized. This method then sets the values necessary for functioning. Currently
	/// used primarily for connecting inputs to their relevant events. </summary>
	protected override void Setup ()
	{
		base.Setup ();
		if (charInput == null) Debug.LogError("Input Is Null");

		//charInput.Setup (this);

//		charInput.stance.buttonTapped += charInput.Targetting.LockOn;				//tab - tap - lock on
//		
//		charInput.stance.buttonCharged += SwitchCombatStances;						//tab - press - draw weapon
//
//		charInput.evasion.buttonTapped += UseDodge;				//command - tap = dodge
//		charInput.evasion.buttonCharged += StartRunning;//TransitionToRun;
//		charInput.evasion.buttonUp += StopRunning;//TransitionToWalk;
//		
//		charInput.mainHand.buttonDown += UsePrimary;	
//		charInput.offHand.buttonDown += UseSecondary;
//		
//		charInput.special1.buttonDown += UseSpecial1;
	}
	#endregion Initialization
	
	protected void Update ()
	{
//		Target = Target;
	}
	
	#region GUI
	private void ActivateLockOn()
	{
//		if (WeaponReady) charInput.Targetting.LockOn();
//		else return;
	}
/*	private void DrawWeapon()
	{
		if (CurrentState == CharState.Idle) 
		{
			if (WeaponReady == true) WeaponReady = false;
			else WeaponReady = true;
		}
		else return;
	}*/
	#endregion GUI
}
