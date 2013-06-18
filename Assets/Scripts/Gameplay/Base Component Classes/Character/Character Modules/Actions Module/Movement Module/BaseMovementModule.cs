using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public interface IMovement {
	MovementProperties Aim {
		get;
	}
	MovementProperties Walk {
		get;
	}
	MovementProperties Strafe {
		get;
	}
	MovementProperties Run {
		get;
	}
	MovementProperties Dodge {
		get;
	}
}

///<summary>
/// Character Movement Module. Holds movement-related values, as well as the relevant functions that use them. </summary>
[System.Serializable] public class BaseMovementModule : IMovement{
	#region Properties

	[SerializeField]protected MovementProperties aim;
	[SerializeField]protected MovementProperties walk;
	[SerializeField]protected MovementProperties strafe;
	[SerializeField]protected MovementProperties run;
	[SerializeField]protected MovementProperties dodge;
	//Internal Fields
	private ICharacter _user;
	//private Transform _coordinates;
	//private List<MovementProperties> moveSet;
	
	public ICharacter Char {
		get {return _user;}
	}

	public MovementProperties Aim {
		get {return aim;}
	}
	public MovementProperties Walk {
		get {return walk;}
	}
	public MovementProperties Strafe {
		get {return strafe;}
	}
	public MovementProperties Run {
		get {return run;} 
	}
	public MovementProperties Dodge {
		get {return dodge;}
	}
	#endregion
	
	#region Initialization
	//public CharacterMovementModule(){}

	/// <summary>
	/// Setup both this module, and all the abilities stored within.
	/// </summary>
	/// <param name="user">User.</param>
	public void Setup (ICharacter user) {
		Debug.Log("BaseMovementModule: Setting Values");
		_user = user;
		//_coordinates = _user.Coordinates;

		walk.SetValue (user);
		strafe.SetValue(user);
		run.SetValue(user);
		dodge.SetValue(user);
	}
	#endregion
}