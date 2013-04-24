using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public interface IEquipmentLoadout {
	BaseEquipmentLoadoutModule.EquipmentStance Stance {
		get;
	}
	BaseEquipmentLoadoutModule.IMovesetHandler MoveSet {
		get;
	}
}

[System.Serializable] public class BaseEquipmentLoadoutModule
{
	public enum EquipmentStance {
		Balanced,				//two weapon slots available, for mainhand and offhand weapon.
		Focused					//one weapon slot available, for two-handed and dualwielded weapons.
	}
	//[SerializeField] private BaseEquipment equipmentSlot1;
	//[SerializeField] private BaseEquipment equipmentSlot2;
	
	#region Fields	
	private BaseCharacter _user;
	[SerializeField] private EquipmentStance combatStance;
	private IEquippable primary;
	private IEquippable secondary;

	
	// Class Abilities (Class Ability)
	
	// Style (Balanced, Power, Finesse)
	//[SerializeField] protected BaseEquipment.BaseEquipmentStats.EquipmentStyle combatStyle;
	
	// Weapon Slots (General Moveset)
	
	
	// Special Abilities (Specific Moveset)
	
	
//	[SerializeField] protected WeaponAbility.SpecialAttack special1;
//	[SerializeField] protected WeaponAbility.SpecialAttack special2;
//	[SerializeField] protected WeaponAbility.SpecialAttack special3;
	#endregion Fields
	
	#region Properties
//	public virtual WeaponAbility.SpecialAttack Special1 
//	{
//		get {return special1;}
//		set {special1 = value;}
//	}
//	public virtual WeaponAbility.SpecialAttack Special2 
//	{
//		get {return special2;}
//		set {special2 = value;}
//	}
//	public virtual WeaponAbility.SpecialAttack Special3 
//	{
//		get {return special3;}
//		set {special3 = value;}
//	}
	#endregion Properties

	#region Initialization
	public BaseEquipmentLoadoutModule () {}
	public void Setup (BaseCharacter user) {
		_user = user;

		//primary = _user.GetComponent();

		
//		switch (combatStance) {
//		case EquipmentStance.Balanced:
//			
//			break;
//			
//		case EquipmentStance.Power:
//			
//			break;
//			
//		case EquipmentStance.Finesse:
//			
//			break;
//		}
		
		//			Primary = equipmentSlot1;
		//			Secondary = equipmentSlot2;
	}
	#endregion Initialization
	
	#region Methods
	public void SetMoveset () {

	}
	public void ActivatePrimary () {


	}
	public void ActivateSecondary () {

	}
	
	#endregion
	public interface IMovesetHandler {
		IEquippable Primary {
			get;
		}
		IEquippable Secondary {
			get;
		}
	}
	///<summary>
	/// Equipment Module that will contain the weapon(s) currently equipped, and translate them into a moveset that the character's
	/// state machine can utilize. </summary>
	///
	private class MovesetHandler : IMovesetHandler {
		IEquipmentLoadout loadout;
		EquipmentStance stance;

		IEquippable primary;
		IEquippable secondary;

		public IEquippable Primary {
			get {return primary;}
		}
		public IEquippable Secondary {
			get {return secondary;}
		}

		MovesetHandler (IEquipmentLoadout _loadout) {
			loadout = _loadout;
			stance = _loadout.Stance;



		}
	}
}