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
		//Primary Actions
		Combo1
		,
		Combo2
		,
		Combo3
		,
		Combo4
		,
		Combo5
		,
		AltPrimary
		,	
		RunAttack
		,
		DodgeAttack
		,
		RangeAttack
		,
		//Secondary Actions
		Defend
		,
		Counter
		,
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
	protected CharacterStateMachine userState;
	protected BaseEquipment curEquipment;
	public MeshRenderer anim;					//Will be turned into the animation element of this class once we have that more in place

	//Inspector Fields
	[SerializeField] protected string equipmentName;

	[SerializeField] protected float baseDamage;			//Base offensive efficacy of equipment, modified by stats and abilities.
	[SerializeField] protected float baseImpact;			//Base kinetic force of an attack, used when calculating stun and knockback.
	[SerializeField] protected ScalingStat scalingBuff;

	protected List<BaseEquipmentMoveset> availableMovesets;		//movesets on offer
	protected BaseEquipmentMoveset activeMoveset;			//moveset that will be looked at by inherited property classes for filling in proper stats

	protected List<AttackProperties> availableActions;		//actions available in the currently active moveset

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

	public float BaseImpact {
		get { return baseImpact; }
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
		user = transform.root.GetComponent<BaseCharacter> ();
		userState = transform.root.GetComponent<CharacterStateMachine> ();
		curEquipment = GetComponent<BaseEquipment>();
		anim = GetComponent<MeshRenderer> ();
	}

	protected virtual void Start ()
	{
		scalingBuff.SetScaling(user);

		availableActions = activeMoveset.Moveset;
	}
	#endregion Initializerss
	
	#region Moveset

	protected void Idle_EnterState ()
	{
		anim.material.color = Color.white;
		Return ();
	}

	protected void Idle_Update ()
	{
		//		userState
	}
	#endregion Moveset
}