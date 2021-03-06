using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IInput {
	Vector3 MoveDir {get;}
	Vector3 LookDir {get;}
	float TurnDir {get;}
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
	protected Animator animator;
	[SerializeField]protected Vector3 moveDir;
	[SerializeField]protected Vector3 lookDir;
	protected float turnDir;
	protected bool lockedOn;

	public BaseCharacter User { get { return user; } }
	public abstract List <IInputAction> InputActions { get;}
	public virtual Vector3 MoveDir {get {return moveDir;}}
	public virtual Vector3 LookDir {get {return lookDir;}}
	public virtual float TurnDir {
		get {return turnDir;}
		set {turnDir = value;}}

	public bool LockedOn {get { return lockedOn;}}

//	public abstract void Setup (ICharacter user);

	public virtual void Awake ()
	{
		user = GetComponent<BaseCharacter>();
		animator = GetComponent <Animator> ();
		moveDir = Vector3.zero;
		lookDir = Vector3.zero;
	}
	public virtual void FixedUpdate () {
		turnDir = UpdateTurnDir ();
		Debug.LogError ("the turn dir is : " + turnDir);
	}

	#region Ability Signals
	public event Action walkSignal;
	protected void ActivateWalk() 
	{
		walkSignal();
	}

	public event Action runSignal;
	protected void ActivateRun() {
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

	#region Movement Data
	protected float UpdateTurnDir ()
	{
		//		Debug.LogWarning ("aim activating");
		//User.Coordinates.rotation = Quaternion.Euler (CharInput.LookDir);

		Vector3 target; 
		if (User.CharState.Armed)
			target = lookDir;
		else
			target = User.transform.position + moveDir;

		Vector3 newAngle = new Vector3 (target.x - User.Coordinates.position.x, 0, target.z - User.Coordinates.position.z);
		float targetAngle = Mathf.Atan2 (newAngle.z, - newAngle.x) * Mathf.Rad2Deg;
		Quaternion finalRotation = Quaternion.AngleAxis (targetAngle + 90, Vector3.up);

//		float angleX = target.x - User.Coordinates.position.x;
//		float angleZ = target.z - User.Coordinates.position.z;
//
//		float targetAngle = Mathf.Atan2 (angleZ, - angleX) * Mathf.Rad2Deg;

		float newTurnDir = Quaternion.Dot (User.Coordinates.rotation, finalRotation);
		return newTurnDir;


//		Quaternion fromRotation = User.Coordinates.rotation;
//		Quaternion finalRotation = Quaternion.AngleAxis (targetAngle - 90, Vector3.up);
//		User.Coordinates.rotation = Quaternion.RotateTowards (fromRotation, finalRotation, lookSpeed);


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
