using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// Base interface for all Equipment. Acceses basic functionality. </summary>
public interface IBaseEquipment
{
	//Properties
	bool FollowupAvailable 					{get; 	set;}
	
	BaseEquipment.BaseEquipmentStats Stats 	{get;	set;}
	//	WeaponAbility.SpecialAttack Special 	{get;}
	
	//	void TransitionToActivation();
	//	IEnumerator ActivateEquipment ();
}

/// <summary>
/// Base equipment.
/// </summary>
/// <remarks> 
/// Matters to be specified for equipment functionality:
/// -Type (Melee, Ranged, Utility (shields, relic))
/// -Style (Balanced (no abilities), Power (charge attacks), Finesse (more combo followups), Universal (works in every style))
/// -Handling Types (One, Two and Half Handed)
/// </remarks>
public abstract class BaseEquipment : MonoBehaviour, IBaseEquipment
{
	[SerializeField] protected BaseEquipmentStats stats;

	protected BaseCharacter user;
	protected Collider hitBox;
	protected bool _followupAvailable;
	
	#region Properties
	
	protected List<BaseEquipmentMovelist> moveSets;

	/// <summary>
	/// Acts as the available states (attacks) of this weapon </summary>

	public BaseEquipmentStats Stats {
		get {return stats;}
		set {stats = value;}
	}
	public BaseCharacter User {
		get {return user;}
		set {user = value;}
	}
	public Collider HitBox {
		get {return hitBox;}
		set {hitBox = value;}
	}
	public bool FollowupAvailable {
		get {return _followupAvailable;}
		set {_followupAvailable = value;}
	}
	#endregion

	#region Initialization
	protected virtual void Awake ()
	{
		user = transform.parent.GetComponent<BaseCharacter>();
		hitBox = GetComponent<Collider>();

		moveSets = new List<BaseEquipmentMovelist>();
	}
	
	protected virtual void Start ()	
	{
		stats.Setup(this);
		
		hitBox.isTrigger = true;
		hitBox.enabled = false;	
	}
	#endregion Initialization

	#region Methods
	#endregion Methods

	#region Abstract Weight Specializations
	// Can be dualwielded. Prayer Seals.
	public abstract class UltraLightEquipment : BaseEquipment {
		public abstract BaseEquipmentMovelist.PrimaryMoveSet.PrimaryFinesseMoveSet PrimaryFinesse {get;}
		public abstract BaseEquipmentMovelist.SecondaryMoveSet.SecondaryFinesseMoveSet SecondaryFinesse {get;}

		protected override void Start () {
			base.Start ();
			moveSets.Add(PrimaryFinesse);
			moveSets.Add(SecondaryFinesse);
		}
	}
	/// Can be used in either hand, or dualwielded. Knife, Boltshooter.
	public abstract class LightEquipment : BaseEquipment {
		public abstract BaseEquipmentMovelist.PrimaryMoveSet.PrimaryBalancedMoveSet PrimaryBalanced {get;}
		public abstract BaseEquipmentMovelist.SecondaryMoveSet.SecondaryBalancedMoveSet SecondaryBalanced {get;}
		
		public abstract BaseEquipmentMovelist.PrimaryMoveSet.PrimaryFinesseMoveSet PrimaryFinesse {get;}
		public abstract BaseEquipmentMovelist.SecondaryMoveSet.SecondaryFinesseMoveSet SecondaryFinesse {get;}

		protected override void Start () {
			base.Start ();
			moveSets.Add(PrimaryBalanced);
			moveSets.Add(SecondaryBalanced);
			moveSets.Add(PrimaryFinesse);
			moveSets.Add(SecondaryFinesse);
		}
	}
	/// Can be used in the main hand, twohanded, or dualwielded. Sword, Spear-Scythe, Chi Focus
	public abstract class MediumEquipment : BaseEquipment {
		public abstract BaseEquipmentMovelist.PrimaryMoveSet.PrimaryBalancedMoveSet PrimaryBalanced {get;}

		public abstract BaseEquipmentMovelist.PrimaryMoveSet.PrimaryFinesseMoveSet PrimaryFinesse {get;}
		public abstract BaseEquipmentMovelist.SecondaryMoveSet.SecondaryFinesseMoveSet SecondaryFinesse {get;}

		public abstract BaseEquipmentMovelist.PrimaryMoveSet.PrimaryPowerMoveSet PrimaryPower {get;}
		public abstract BaseEquipmentMovelist.SecondaryMoveSet.SecondaryPowerMoveSet SecondaryPower {get;}

		protected override void Start () {
			base.Start ();
			moveSets.Add(PrimaryBalanced);
			moveSets.Add(PrimaryFinesse);
			moveSets.Add(SecondaryFinesse);
			moveSets.Add(PrimaryPower);
			moveSets.Add(SecondaryPower);
		}
	}
	/// Can be used in the main hand, or twohanded. Mace, Orb.
	public abstract class HeavyEquipment : BaseEquipment {
		public abstract BaseEquipmentMovelist.PrimaryMoveSet.PrimaryBalancedMoveSet PrimaryBalanced {get;}

		public abstract BaseEquipmentMovelist.PrimaryMoveSet.PrimaryPowerMoveSet PrimaryPower {get;}
		public abstract BaseEquipmentMovelist.SecondaryMoveSet.SecondaryPowerMoveSet SecondaryPower {get;}

		protected override void Start () {
			base.Start ();
			moveSets.Add(PrimaryBalanced);
			moveSets.Add(PrimaryPower);
			moveSets.Add(SecondaryPower);
		}
	}
	/// Can be twohanded. Greatsword, Greatshield, Greatbow
	public abstract class UltraHeavyEquipment : BaseEquipment {
		
		public abstract BaseEquipmentMovelist.PrimaryMoveSet.PrimaryPowerMoveSet PrimaryPower {get;}
		public abstract BaseEquipmentMovelist.SecondaryMoveSet.SecondaryPowerMoveSet SecondaryPower {get;}

		protected override void Start () {
			base.Start ();
			moveSets.Add(PrimaryPower);
			moveSets.Add(SecondaryPower);
		}
	}
	/// Can be used in the off hand. Shield-Totem, Blade-Relic, Prayer Beads
	public abstract class OffHandEquipment : BaseEquipment {
		public abstract BaseEquipmentMovelist.SecondaryMoveSet.SecondaryBalancedMoveSet SecondaryBalanced {get;}
		protected override void Start () {
			base.Start ();
			moveSets.Add(SecondaryBalanced);
		}}
	#endregion


	/*Base Character (Equipment Module)
	 * Player Class
	 * -Wanderer
	 * --Balanced (sword, mace, spear-scythe, shield, boltshooter)
	 * --Power (sword, mace, spear-scythe)
	 * 
	 * -Shade
	 * --Balanced (sword, blade-relic, prayer beads)
	 * --Finesse (sword, chi focus)
	 * 
	 * -Warden
	 * --Balanced (knife, boltshooter)
	 * --Power (greatbow)
	 * --Finesse (boltshooter)
	 * 
	 * -Stalker
	 * --Balanced (knife, prayer beads)
	 * --Finesse (knife, prayer seals)
	 * 
	 * -Elementalist
	 * --Balanced (mace, orb, shield-totem, prayer beads)
	 * --Power (mace, orb)
	 * 
	 * -Zoologist
	 * 
	 * -Judgement
	 * --Wrath - Power (greatsword, chi focus)
	 * --Mercy - Power (greatshield, chi focus)
	 * 
	 * Special Weapon Inheritance (class specific weapons and the specials they inherit upon upgrading)
	 * -Spear-Scythe (sword)
	 * -Blade-Relic (prayer beads)
	 * -Greatbow (bolt shooter)
	 * -Prayer Seals (knife)
	 * -Orb (chi focus)
	 * -Greatsword (mace)
	 * -Greatshield (shield)
	 * 
	 * Equipment Module (should only be able to accept a specific array of weapon types and configs)
	 * -Derived characters will override this module to specify their own loadouts.
	 * --Specify what kind of loadout style it is (Balanced, power, finesse)
	 * ---if balanced
	 * ----two equipment slots available (mainhand and offhand)
	 * ---if power of finesse
	 * ----one equipment slot available (two handed and dualwielded)
	 * 
	 * 
	 * Base Weapon
	 * -Movelist (holds all weapon moves behind single method interface. overridden by each derived class)
	 * --multiple Movelists per weapon, for each playstyle associated with them (Maces have 2: mainhand and two handed)
	 * -Stats (holds in-game statistics)
	 */

	public interface IMoveSet {
		//Acts as the initial startup for any weapon abilities
		IEnumerator ActivateEquipment ();
	}
	[System.Serializable] public abstract class BaseEquipmentMovelist : IMoveSet {
		protected BaseCharacter user;
		public abstract IEnumerator ActivateEquipment ();

//		protected WeaponAbility.StandardAttack.ComboAttack primaryAttack;

		public abstract class PrimaryMoveSet : BaseEquipmentMovelist {
			public abstract class PrimaryBalancedMoveSet : PrimaryMoveSet {

			}
			public abstract class PrimaryPowerMoveSet : PrimaryMoveSet{

			}
			public abstract class PrimaryFinesseMoveSet : PrimaryMoveSet{

			}
		}

		public abstract class SecondaryMoveSet : BaseEquipmentMovelist{
			///<summary>
			/// Used for any any OffHand pieces of equipment </summary>
			public abstract class SecondaryBalancedMoveSet : SecondaryMoveSet{

			}
			///<summary>
			/// Used for the offhand component of a power weapon</summary>
			public abstract class SecondaryPowerMoveSet : SecondaryMoveSet{

			}
			public abstract class SecondaryFinesseMoveSet : SecondaryMoveSet{

			}
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
		public enum EquipmentType {
			//DW only
			PrayerSeal,
			//1 Hand (balanced, finesse)
			Knife,
			BoltShooter,
			//Universal (balanced, power, finesse)
			ChiFocus,
			Sword,
			SpearScythe,
			//1.5 Hand (balanced, power)
			Mace,
			CrystalOrb,
			//2 Hand only
			GreatShield,
			GreatSword,
			GreatBow,

			//Off Hand
			PrayerBeads,
			TotemShield,
			BladeRelic
		}
		public enum AttributeType {
			Normal,
			Poisoned,													//poison (build - once meter is full, causes gradual damage)
			Tearing,													//bleeding (build - once meter is full, causes burst damage)
			Piercing,													//armor piercing (passive - ignores armor)
			Crushing,													//greater stun/knockback distance (passive)		
			Spirit,
			Fire,														//ablaze (build - once meter is full, causes gradual damage for a short time that also spreads to nearby units)
			Ice,														//chilled (build - once meter is full, causes slowdown)
			Lightning,
			Earth
		}
		public enum EquipmentSlotType {
			OffHanded,							//Prayer Beads, Shield-Totem, Blade-Relic
			OneHanded,							//Knives, Seals, BoltShooter, Orb,
			OneHalfHanded,						//Sword, Mace, SpearScyhe, Chakra
			DualWielded,						//Chi Focus, Prayer Seals
			TwoHanded							//GreatShield, GreatSword, GreatBow
		}
		public enum EquipmentStyle {
			Balanced,													//
			Power,
			Finesse,
			Universal
		}

		/// <summary>
		/// This is the braodest category for equipment, delineating the manners in which a character may equip it, and 
		/// implicitly what combat styles are available. </summary>	
		/// <remarks>
		/// OneHanded = Can be used in either hand (Balanced, Speed (dualwielding))
		/// OneHalfHanded = Can be used with one hand or two hands (Balanced, Power (twohanded))
		/// TwoHanded = Can only be used in two hands (Power (twohanded))
		/// OffHanded = Can only be used in the secondary hand (Balanced)
		/// </remarks>

		
		#region Fields
		//Inspector Fields
		[SerializeField] protected string name;
		[SerializeField] protected float baseDamage;					//Base offensive efficacy of equipment, modified by stats and abilities
		[SerializeField] protected float baseDefense;					//Base defensive efficacy of equipment, modified by stats

		[SerializeField] protected CharacterStatsModule.ScalingStat scalingBuff;

		//[SerializeField] protected Attribute scalingstat;				//Buff to weapon's efficacy based on user's selected stat and its scaling ratio

		//Internal Fields
		private float abilityBuff;										//percentage of attack adjustment from ability being used.
		protected BaseEquipment curEquipment;
		#endregion
		
		#region Initializers
		public BaseEquipmentStats ()
		{
			abilityBuff = 0;
		}
		public void Setup (BaseEquipment thisWeapon)
		{
			curEquipment = thisWeapon;
			//scalingBuff.SetScaling(curEquipment.user.CharStats);
		}
		#endregion Initializers
		
		#region Properties
		public float BaseDamage {
			get {return baseDamage;}
			set {baseDamage = value;}
		}
		public float AdjustedBaseDamage
		{
			get {return baseDamage * abilityBuff;}
		}
		public float BaseDefense {
			get {return baseDefense;}
			set {baseDefense = value;}
		}
		public CharacterStatsModule.ScalingStat ScalingBuff {
			get {return scalingBuff;}
		}
		public float AbilityBuff {
			get {return abilityBuff;}
			set {abilityBuff = value;}
		}
		public BaseEquipment CurEquipment {
			get {return curEquipment;}
			set {curEquipment = value;}
		}
		#endregion Properties
	}
}


