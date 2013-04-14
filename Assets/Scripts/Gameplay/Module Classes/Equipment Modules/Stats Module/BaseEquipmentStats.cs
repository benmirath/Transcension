using System;
using UnityEngine;

public interface IEquipmentStats {
	float BaseDamage {
		get;
	}
	float AdjustedBaseDamage {
		get;
	}
	ScalingStat ScalingBuff {
		get;
	}
}

///<summary>
/// Elements required for (all) equipment stats.
/// Lists (specified for each weapon)
/// -Equipment Type (sword vs spear)
/// -Attribute Type (physical vs fire)
/// -Wielding Type (onehanded vs dualwielded)
/// -Style Type (Power vs Balanced)
/// 
/// Base Stats (standard, and does not normally change during the course of play)
/// -Name
/// -Scaling Stat
/// -Scaling Ratio (percentage of scaling stat that gets added to weapon's base damage)
/// -Base Damage
/// -Base Defense
/// 
/// Attack Stats
/// -Stat instance of all attacks available to weapon.
/// 
/// Implemented Stats
/// -Scaling Buff (determined by calculating the value of scaling stat and ratio)
/// -Ability Buff (determined by current attack being used, modifying the base damage (i.e. a finisher attack hitting for 1.75 the base damage value))
/// -Weapon Buff (determined based on any abilities/items/effects that boost the weapons damage)
/// -Real Damage (((base damage * scaling buff) * ability buff) + Weapon Buff)
/// </summary>	  	
[System.Serializable] public class BaseEquipmentStats {
//	public enum EquipmentType {
//		//DW only
//		PrayerSeal,
//		//1 Hand (balanced, finesse)
//		Knife,
//		BoltShooter,
//		//Universal (balanced, power, finesse)
//		ChiFocus,
//		Sword,
//		SpearScythe,
//		//1.5 Hand (balanced, power)
//		Mace,
//		CrystalOrb,
//		//2 Hand only
//		GreatShield,
//		GreatSword,
//		GreatBow,
//		
//		//Off Hand
//		PrayerBeads,
//		TotemShield,
//		BladeRelic
//	}
//	public enum AttributeType {
//		Normal,
//		Poisoned,													//poison (build - once meter is full, causes gradual damage)
//		Tearing,													//bleeding (build - once meter is full, causes burst damage)
//		Piercing,													//armor piercing (passive - ignores armor)
//		Crushing,													//greater stun/knockback distance (passive)		
//		Spirit,
//		Fire,														//ablaze (build - once meter is full, causes gradual damage for a short time that also spreads to nearby units)
//		Ice,														//chilled (build - once meter is full, causes slowdown)
//		Lightning,
//		Earth
//	}
//	public enum EquipmentSlotType {
//		OffHanded,							//Prayer Beads, Shield-Totem, Blade-Relic
//		OneHanded,							//Knives, Seals, BoltShooter, Orb,
//		OneHalfHanded,						//Sword, Mace, SpearScyhe, Chakra
//		DualWielded,						//Chi Focus, Prayer Seals
//		TwoHanded							//GreatShield, GreatSword, GreatBow
//	}
//	public enum EquipmentStyle {
//		Balanced,													//
//		Power,
//		Finesse,
//		Universal
//	}	
	
	#region Fields
	//Inspector Fields
	[SerializeField] protected string name;
	[SerializeField] protected float baseDamage;					//Base offensive efficacy of equipment, modified by stats and abilities
//	[SerializeField] protected float baseDefense;					//Base defensive efficacy of equipment, modified by stats
	
	[SerializeField] protected ScalingStat scalingBuff;
	
	//[SerializeField] protected Attribute scalingstat;				//Buff to weapon's efficacy based on user's selected stat and its scaling ratio
	
	//Internal Fields
//	private float abilityBuff;										//percentage of attack adjustment from ability being used.
	protected BaseEquipment curEquipment;
	#endregion
	
	#region Initializers
	public BaseEquipmentStats () {
		//abilityBuff = 0;
	}
	public void Setup (BaseEquipment thisWeapon) {
		curEquipment = thisWeapon;
		//scalingBuff.SetScaling(curEquipment.user.CharStats);
	}
	#endregion Initializerss
	
	#region Properties
	public float BaseDamage {
		get {return baseDamage;}
		set {baseDamage = value;}
	}
	public float AdjustedBaseDamage {
		get {return baseDamage * scalingBuff.BaseValue;}
	}
//	public float BaseDefense {
//		get {return baseDefense;}
//		set {baseDefense = value;}
//	}
	public ScalingStat ScalingBuff {
		get {return scalingBuff;}
	}
//	public float AbilityBuff {
//		get {return abilityBuff;}
//		set {abilityBuff = value;}
//	}
	public BaseEquipment CurEquipment {
		get {return curEquipment;}
		set {curEquipment = value;}
	}
	#endregion Properties
}