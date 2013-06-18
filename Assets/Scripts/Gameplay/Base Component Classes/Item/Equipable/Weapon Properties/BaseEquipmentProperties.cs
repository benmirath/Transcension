using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEquipmentProperties
{
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
[System.Serializable] public abstract class BaseEquipmentProperties : StateMachineBehaviourEx
{
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
	
	#region Properties
	public enum EquipmentActions
	{
		Idle
		,
		//		ActivateWeapon
		//,
		//		//Primary Actions
		//Basic startup attack for weapon. Might start a combo, fire a projectile, etc.
		Combo1
		,		//The alt attack for the primary moveset. 			
		AltPrimary
		,					
		Combo2
		,
		Combo3
		,
		Combo4
		,
		Combo5
		,
		RunAttack
		,
		DodgeAttack
		,
		RangeAttack
		,
		//Secondary Actions
		StartSecondary
		,					//Basic secondary moveset attack. Might activate a shield, use a finisher, or start a counter.
		AltSecondary
		,
		InterruptSecondary
	}
	//Internal Fields
	public enum FollowupType
	{
		None
		,
		Primary
		,
		Secondary
		,
		Dodge
		,
	}
	[SerializeField] protected FollowupType followup = FollowupType.None;

	protected BaseCharacter user;
	protected PlayerStateModule userState;
	protected BaseEquipment curEquipment;
	protected MeshRenderer anim;					//Will be turned into the animation element of this class once we have that more in place

	//Inspector Fields
	[SerializeField] protected string equipmentName;
	[SerializeField] protected float baseDamage;			//Base offensive efficacy of equipment, modified by stats and abilities
	[SerializeField] protected ScalingStat scalingBuff;

	protected List<BaseEquipmentMoveset> availableMovesets;
	protected BaseEquipmentMoveset activeMoveset;

	protected List<AttackProperties> availableActions;

	public FollowupType Followup {
		get { return followup;}
		set { followup = value;}
	}
	public List<BaseEquipmentMoveset> AvailableMovesets { 
		get { return availableMovesets;} 
	}

	public BaseEquipmentMoveset ActiveMoveset {
		get { return activeMoveset;}
		set { activeMoveset = value; }
	}

	public float BaseDamage { 
		get { return baseDamage;} 
	}
	public ScalingStat ScalingBuff { 
		get { return scalingBuff;} 
	}
	public float AdjustedBaseDamage { 
		get { return baseDamage * scalingBuff.BaseValue;} 
	}
	public BaseEquipment CurEquipment { 
		get { return curEquipment;} 
	}

	//Here will be a list of weapon movesets. You specify the number there will be, then within that you specify the type of moveset it will be.
	#endregion
	
	#region Initializers
	protected override void OnAwake ()
	{
		user = transform.parent.GetComponent<BasePlayer> ();
		userState = transform.parent.GetComponent<PlayerStateModule> ();
		curEquipment = GetComponent<BaseEquipment>();
		anim = GetComponent<MeshRenderer> ();
		//		Debug.LogError ("EQUIPMENT SETUP");
		//currentState = EquipmentActions.Idle;
	}
//
	protected virtual void Start ()
	{
//		curEquipment = thisWeapon;
		scalingBuff.SetScaling(user);
	}
	#endregion Initializerss
	
	#region Moveset

	void Idle_EnterState ()
	{
		anim.material.color = Color.white;
		Return ();
	}

	void Idle_Update ()
	{
		//		userState
	}
	#endregion Moveset
}