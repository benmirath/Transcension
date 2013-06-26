using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IInput {
	Vector3 MoveDir {get;}
	Vector3 LookDir {get;}
	bool LockedOn {get;}

	List <IInputAction> InputActions {get;}
	//void Setup (ICharacter user);

	event Action walkSignal;
	event Action runSignal;
	event Action dodgeSignal;

	event Action primarySignal;
	event Action secondarySignal;

	event Action sheatheSignal;
	event Action lockOnSignal;

	//event Action walkSignal;
	//event Action runSignal;
	//event Action stopRunSignal;
	//event Action dodgeSignal;
	//event Action sheatheSignal;
	//event Action primarySignal;
	//event Action secondarySignal;

}
public interface IEnemyInput : IInput {

}

public interface IInputAction {
	string Name {get;}
	bool Active { get;}
	IEnumerator CheckInput (float time, Action tapActivate, Action chargeActivate);
}
public interface IInputReaction : IInputAction {
	float Value { get;}
}

public abstract class BaseInputModule : MonoBehaviour, IInput 
{
	protected BaseCharacter user;
	[SerializeField]protected Vector3 moveDir;
	[SerializeField]protected Vector3 lookDir;
	protected bool lockedOn;

	public BaseCharacter User { get { return user; } }
	public abstract List <IInputAction> InputActions { get;}
	public virtual Vector3 MoveDir {get {return moveDir;}}
	public virtual Vector3 LookDir {get {return lookDir;}}

	public bool LockedOn {get { return lockedOn;}}

//	public abstract void Setup (ICharacter user);

	void Awake ()
	{
		moveDir = Vector3.zero;
		lookDir = Vector3.zero;
	}

	#region Ability Signals
	public event Action walkSignal;
	protected void ActivateWalk() 
	{
		walkSignal();
	}

	public event Action runSignal;
	protected void ActivateRun() {
		Debug.Log ("Activating Run");
		runSignal();
	}

	public event Action dodgeSignal;
	protected void ActivateDodge() {
		Debug.Log ("Activating Dodge");
		dodgeSignal();
	}

	public event Action primarySignal;
	protected void ActivatePrimary() {
		primarySignal();
	}

	public event Action secondarySignal;
	protected void ActivateSecondary() {
		secondarySignal();
	}
	
	public event Action lockOnSignal;
	protected void ActivateLockOn() {
		lockOnSignal();
	}

	public event Action sheatheSignal;
	protected void ActivateSheathe() {
		sheatheSignal();
	}



	#endregion
}

///General
/// all input modules will consist of movement and targetting directions.
/// 
///Player
/// player modules will include events tied to button presses (i.e. lmb event attaches to activate primary delegate in state machine) 
/// 
///Enemy
/// enemy modules will include events tied to pathfinding and targetting data (i.e. distance of player from enemy, or their velocity to determine whether to sidestep an attack)
