using UnityEngine;
using System;
using System.Collections;

///<summary>
/// Character Movement Module. Holds movement-related values, as well as the relevant functions that use them. </summary>
[System.Serializable] public class BaseMovementModule {
	#region Fields
	//Inspector Fields
	[SerializeField, Range (0,10)] private float turnSpeed = 3;
	
	[SerializeField, Range (0,10)] private float walkSpeed = 5;
	[SerializeField, Range (0,10)] private float walkingBlockSpeed = 4;
	
	[SerializeField, Range (0,20)] private float runSpeed = 7;
	[SerializeField, Range (0,10)] private float runCost = 1;
	
	[SerializeField, Range (0,60)] private float dodgeSpeed = 30;
	[SerializeField, Range (0,20)] private float dodgeCost = 15;
	[SerializeField, Range (0,2)] private float dodgeStartup;
	[SerializeField, Range (0,2)] private float dodgeDuration;
	[SerializeField, Range (0,2)] private float dodgeCooldown;
	
	//Internal Fields
	private BaseCharacter _user;
	private Transform _coordinates;
	#endregion
	
	#region Properties
	public float TurnSpeed {
		get {return turnSpeed;}
	}
	public float WalkSpeed {
		get {return walkSpeed;}
	}
	public float RunSpeed {
		get {return runSpeed;}
	}
	public float RunCost {
		get {return runCost;}
	}
	public float DodgeSpeed {
		get {return dodgeSpeed;}
	}
	public float DodgeCost {
		get {return dodgeCost;}
	}
	public float DodgeStartup {
		get {return dodgeStartup;}
	}
	public float DodgeDuration {
		get {return dodgeDuration;}
	}
	public float DodgeCooldown {
		get {return dodgeCooldown;}
	}
	public float WalkingBlockSpeed {
		get {return walkingBlockSpeed;}
	}
	
	public BaseCharacter Char {
		get {return _user;}
	}
	#endregion
	
	#region Initialization
	//public CharacterMovementModule(){}
	public void Setup (BaseCharacter user) {
		_user = user;
		_coordinates = _user.Coordinates;
	}
	#endregion
	
	#region Character Movements
	public void Walk () {
		Turn ();
		_user.Body.Move (WalkSpeed * _user.CharInput.MoveDir.normalized * Time.deltaTime);		
	}
	
	public void Strafe () {
		Aim ();
		_user.Body.Move (WalkSpeed * _user.CharInput.MoveDir.normalized * Time.deltaTime);		
	}
	
	public void Run () {
		//if (CharMovement.RunCost > CharStats.Stamina.CurrentValue) return;
		_user.CharStats.Stamina.CurValue -= RunCost * Time.deltaTime;
		Turn ();
		_user.Body.Move (RunSpeed * _user.CharInput.MoveDir * Time.deltaTime);
	}
	
	/// <summary>
	/// Turn the character based on the direction they're moving. </summary>
	public void Turn ()
	{
		Vector3 dir = _user.CharInput.MoveDir;
		if (dir == Vector3.zero) return;
		
		float aim = Mathf.Atan2(-dir.x, dir.y) * Mathf.Rad2Deg;						//holds direction the character should be facing
		_coordinates.rotation = Quaternion.Euler(0, 0, aim);
	}
	/// <summary>
	/// Turns the character based on the direction of they're desired target. </summary>
	public void Aim ()
	{
		Vector3 target = _user.CharInput.LookDir;
		float angleX = target.x - _coordinates.position.x;
		float angleY = target.y - _coordinates.position.y;
		float targetAngle = Mathf.Atan2 (angleY, angleX) * Mathf.Rad2Deg;
		
		Quaternion fromRotation = _coordinates.rotation;
		Quaternion finalRotation = Quaternion.AngleAxis(targetAngle - 90, Vector3.forward);
		_coordinates.rotation = Quaternion.RotateTowards(fromRotation, finalRotation, turnSpeed);
	}
	/// <summary>
	/// Causes the character to Dodge. </summary>
	public IEnumerator Dodge ()
	{
		if (_user.CharStats.Stamina.CurValue < dodgeCost) yield break;
		else _user.CharStats.Stamina.CurValue -= dodgeCost;
		
		Vector3 dir = Char.CharInput.MoveDir;
		
		if (dodgeStartup > 0) yield return new WaitForSeconds(dodgeStartup);
		
		float curDuration = Time.time + dodgeDuration;												
		do {
			if (_user.State == BaseCharacter.CharState.Idle) Turn();
			if (_user.State == BaseCharacter.CharState.CombatReady) Aim();
			_user.Body.Move (dir * DodgeSpeed * Time.deltaTime);																		
			yield return null;
		} while (curDuration > Time.time);
		_user.IsEvading = false;
		
		if (dodgeCooldown > 0) yield return new WaitForSeconds(dodgeCooldown);
		
		if (_user.WeaponReady) _user.State = BaseCharacter.CharState.CombatReady;
		else _user.State = BaseCharacter.CharState.Idle;
		
		yield break;
	}
	#endregion
}