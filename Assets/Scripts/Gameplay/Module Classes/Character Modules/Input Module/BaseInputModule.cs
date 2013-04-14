using System;
using UnityEngine;

public delegate void AttemptStateTransition ();

public interface IInput {
	Vector3 MoveDir {get;}
	Vector3 LookDir {get;}

	event AttemptStateTransition ActivateRun;
	event AttemptStateTransition ActivateDodge;
	event AttemptStateTransition ActivateAlt;
	event AttemptStateTransition ActivatePrimary;
	event AttemptStateTransition ActivateSecondary;
	event AttemptStateTransition ActivateSpecial;
}

public class BaseInputModule : IInput {
	protected ICharacter user;
	protected Vector3 moveDir;
	protected Vector3 lookDir;
	
	public Vector3 MoveDir {get {return moveDir;}}
	public Vector3 LookDir {get {return lookDir;}}

	public event AttemptStateTransition ActivateRun;
	public event AttemptStateTransition ActivateDodge;
	public event AttemptStateTransition ActivateAlt;
	public event AttemptStateTransition ActivatePrimary;
	public event AttemptStateTransition ActivateSecondary;
	public event AttemptStateTransition ActivateSpecial;
}

///General
/// all input modules will consist of movement and targetting directions.
/// 
///Player
/// player modules will include events tied to button presses (i.e. lmb event attaches to activate primary delegate in state machine) 
/// 
///Enemy
/// enemy modules will include events tied to pathfinding and targetting data (i.e. distance of player from enemy, or their velocity to determine whether to sidestep an attack)
