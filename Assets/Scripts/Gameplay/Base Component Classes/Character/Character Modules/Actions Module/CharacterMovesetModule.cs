using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovesetModule : MonoBehaviour, IMoveSet {
	[SerializeField]protected BaseCharacter user;

	[SerializeField] protected BaseMovementModule charMovement;
	[SerializeField] protected BaseEquipmentLoadoutModule charEquipment;
	[SerializeField] protected CharacterStatusModule charStatus;
	[SerializeField] protected BaseStealthModule charStealth;

	public BaseMovementModule CharMovement {
		get {return charMovement;}
	}
	public BaseEquipmentLoadoutModule CharEquipment {
		get {return charEquipment;}
	}
	public CharacterStatusModule CharStatus {
		get {return charStatus;}
	}
	public IStealth CharStealth {
		get {return charStealth;}
	}

	public void Awake() 
	{
		user = GetComponent<BaseCharacter>();
		//construct ability classes
	}
	public void Start()
	{
		Debug.Log("MoveSet: Setting Values");
		if (user == null)
			Debug.Log ("Moveset user is currently null");
		charMovement.Setup (user);
		charStatus.Setup (user);
		charEquipment.Setup ();
	}
}

public interface IMoveSet {
	[SerializeField] BaseMovementModule CharMovement {get;}
	[SerializeField] BaseEquipmentLoadoutModule CharEquipment {get;}
	[SerializeField] CharacterStatusModule CharStatus {get;}
	[SerializeField] IStealth CharStealth {get;}
}