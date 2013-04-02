using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable] public class BaseEquipmentModule
{
	public enum EquipmentStance {
		Balanced,
		Power,
		Finesse
	}
	[SerializeField] private EquipmentStance combatStance;
	[SerializeField] private BaseEquipment equipmentSlot1;
	[SerializeField] private BaseEquipment equipmentSlot2;
	
	#region Fields
	//	public IBaseEquipment<IBaseEquipment> primary;
	private IMoveset primaryMoveSet;
	private IMoveset secondaryMoveSet;
	
	private BaseCharacter _user;
	//	[SerializeField] EquipmentLoadoutType combatStance;
	
	// Class Abilities (Class Ability)
	
	// Style (Balanced, Power, Finesse)
	//[SerializeField] protected BaseEquipment.BaseEquipmentStats.EquipmentStyle combatStyle;
	
	// Weapon Slots (General Moveset)
	
	
	// Special Abilities (Specific Moveset)
	
	
	[SerializeField] protected WeaponAbility.SpecialAttack special1;
	[SerializeField] protected WeaponAbility.SpecialAttack special2;
	[SerializeField] protected WeaponAbility.SpecialAttack special3;
	#endregion Fields
	
	#region Properties
	public virtual WeaponAbility.SpecialAttack Special1 
	{
		get {return special1;}
		set {special1 = value;}
	}
	public virtual WeaponAbility.SpecialAttack Special2 
	{
		get {return special2;}
		set {special2 = value;}
	}
	public virtual WeaponAbility.SpecialAttack Special3 
	{
		get {return special3;}
		set {special3 = value;}
	}
	#endregion Properties
	
	#region Initialization
	public BaseEquipmentModule () {}
	public void Setup (BaseCharacter user)
	{
		_user = user;
		
		switch (combatStance) {
		case EquipmentStance.Balanced:
			
			break;
			
		case EquipmentStance.Power:
			
			break;
			
		case EquipmentStance.Finesse:
			
			break;
		}
		
		//			Primary = equipmentSlot1;
		//			Secondary = equipmentSlot2;
	}
	#endregion Initialization
	
	#region Setup
	
	#endregion
	
	#region Loadout Specialization
	interface IMoveset {
		IBaseEquipment Primary {get;}
		IBaseEquipment Secondary {get;}
	}
	public class OneWeaponMoveset : IMoveset {
		[SerializeField] protected BaseEquipment primary;
		
		public virtual IBaseEquipment Primary {
			get {return primary;}
		}
		public virtual IBaseEquipment Secondary {
			get {return primary;}
		}
	}
	public class TwoWeaponMoveset : OneWeaponMoveset {
		private IBaseEquipment secondary;
		
		public override IBaseEquipment Secondary {
			get {return secondary;}
		}
	}
	#endregion Loadout Specilization
}