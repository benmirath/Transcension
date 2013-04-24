using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// Base interface for all Equipment. Acceses basic functionality. </summary>
public interface IEquippable
{
	ICharacter User {
		get;
	}
	Collider HitBox {
		get;
	}
	IEquipmentProperties Stats {
		get;
	}

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
public abstract class BaseEquipment : MonoBehaviour, IEquippable
{

	protected ICharacter user;
	protected Collider hitBox;

	[SerializeField] protected IEquipmentProperties stats;


	#region Properties
	
	//protected List<BaseEquipmentMovelist> moveSets;

	/// <summary>
	/// Acts as the available states (attacks) of this weapon </summary>

	public IEquipmentProperties Stats {
		get {return stats;}
	}
	public ICharacter User {
		get {return user;}
		set {user = value;}
	}
	public Collider HitBox {
		get {return hitBox;}
		set {hitBox = value;}
	}
//	public bool FollowupAvailable {
//		get {return _followupAvailable;}
//		set {_followupAvailable = value;}
//	}
	#endregion

	#region Initialization
	protected virtual void Awake ()
	{
		user = transform.parent.GetComponent<BaseCharacter>();
		hitBox = GetComponent<Collider>();

		//moveSets = new List<BaseEquipmentMovelist>();
	}
	
	protected virtual void Start ()	{
		//stats.Setup(this);
		
		hitBox.isTrigger = true;
		hitBox.enabled = false;	
	}
	#endregion Initialization

	#region Methods
	#endregion Methods

//	#region Abstract Weight Specializations
//	// Can be dualwielded. Prayer Seals.
//	public abstract class UltraLightEquipment : BaseEquipment {
//		public abstract BaseEquipmentMovelist.PrimaryMoveSet.PrimaryFinesseMoveSet PrimaryFinesse {get;}
//		public abstract BaseEquipmentMovelist.SecondaryMoveSet.SecondaryFinesseMoveSet SecondaryFinesse {get;}
//
//		protected override void Start () {
//			base.Start ();
//			moveSets.Add(PrimaryFinesse);
//			moveSets.Add(SecondaryFinesse);
//		}
//	}
//	/// Can be used in either hand, or dualwielded. Knife, Boltshooter.
//	public abstract class LightEquipment : BaseEquipment {
//		public abstract BaseEquipmentMovelist.PrimaryMoveSet.PrimaryBalancedMoveSet PrimaryBalanced {get;}
//		public abstract BaseEquipmentMovelist.SecondaryMoveSet.SecondaryBalancedMoveSet SecondaryBalanced {get;}
//		
//		public abstract BaseEquipmentMovelist.PrimaryMoveSet.PrimaryFinesseMoveSet PrimaryFinesse {get;}
//		public abstract BaseEquipmentMovelist.SecondaryMoveSet.SecondaryFinesseMoveSet SecondaryFinesse {get;}
//
//		protected override void Start () {
//			base.Start ();
//			moveSets.Add(PrimaryBalanced);
//			moveSets.Add(SecondaryBalanced);
//			moveSets.Add(PrimaryFinesse);
//			moveSets.Add(SecondaryFinesse);
//		}
//	}
//	/// Can be used in the main hand, twohanded, or dualwielded. Sword, Spear-Scythe, Chi Focus
//	public abstract class MediumEquipment : BaseEquipment {
//		public abstract BaseEquipmentMovelist.PrimaryMoveSet.PrimaryBalancedMoveSet PrimaryBalanced {get;}
//
//		public abstract BaseEquipmentMovelist.PrimaryMoveSet.PrimaryFinesseMoveSet PrimaryFinesse {get;}
//		public abstract BaseEquipmentMovelist.SecondaryMoveSet.SecondaryFinesseMoveSet SecondaryFinesse {get;}
//
//		public abstract BaseEquipmentMovelist.PrimaryMoveSet.PrimaryPowerMoveSet PrimaryPower {get;}
//		public abstract BaseEquipmentMovelist.SecondaryMoveSet.SecondaryPowerMoveSet SecondaryPower {get;}
//
//		protected override void Start () {
//			base.Start ();
//			moveSets.Add(PrimaryBalanced);
//			moveSets.Add(PrimaryFinesse);
//			moveSets.Add(SecondaryFinesse);
//			moveSets.Add(PrimaryPower);
//			moveSets.Add(SecondaryPower);
//		}
//	}
//	/// Can be used in the main hand, or twohanded. Mace, Orb.
//	public abstract class HeavyEquipment : BaseEquipment {
//		public abstract BaseEquipmentMovelist.PrimaryMoveSet.PrimaryBalancedMoveSet PrimaryBalanced {get;}
//
//		public abstract BaseEquipmentMovelist.PrimaryMoveSet.PrimaryPowerMoveSet PrimaryPower {get;}
//		public abstract BaseEquipmentMovelist.SecondaryMoveSet.SecondaryPowerMoveSet SecondaryPower {get;}
//
//		protected override void Start () {
//			base.Start ();
//			moveSets.Add(PrimaryBalanced);
//			moveSets.Add(PrimaryPower);
//			moveSets.Add(SecondaryPower);
//		}
//	}
//	/// Can be twohanded. Greatsword, Greatshield, Greatbow
//	public abstract class UltraHeavyEquipment : BaseEquipment {
//		
//		public abstract BaseEquipmentMovelist.PrimaryMoveSet.PrimaryPowerMoveSet PrimaryPower {get;}
//		public abstract BaseEquipmentMovelist.SecondaryMoveSet.SecondaryPowerMoveSet SecondaryPower {get;}
//
//		protected override void Start () {
//			base.Start ();
//			moveSets.Add(PrimaryPower);
//			moveSets.Add(SecondaryPower);
//		}
//	}
//	/// Can be used in the off hand. Shield-Totem, Blade-Relic, Prayer Beads
//	public abstract class OffHandEquipment : BaseEquipment {
//		public abstract BaseEquipmentMovelist.SecondaryMoveSet.SecondaryBalancedMoveSet SecondaryBalanced {get;}
//		protected override void Start () {
//			base.Start ();
//			moveSets.Add(SecondaryBalanced);
//		}}
//	#endregion


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
//
//	public interface IMoveSet {
//		//Acts as the initial startup for any weapon abilities
//		IEnumerator ActivateEquipment ();
//	}
//	[System.Serializable] public abstract class BaseEquipmentMovelist : IMoveSet {
//		protected BaseCharacter user;
//		public abstract IEnumerator ActivateEquipment ();
//
//		protected WeaponAbility.StandardAttack.ComboAttack primaryAttack;
//
//		public abstract class PrimaryMoveSet : BaseEquipmentMovelist {
//			public abstract class PrimaryBalancedMoveSet : PrimaryMoveSet {
//
//			}
//			public abstract class PrimaryPowerMoveSet : PrimaryMoveSet{
//
//			}
//			public abstract class PrimaryFinesseMoveSet : PrimaryMoveSet{
//
//			}
//		}
//
//		public abstract class SecondaryMoveSet : BaseEquipmentMovelist{
//			///<summary>
//			/// Used for any any OffHand pieces of equipment </summary>
//			public abstract class SecondaryBalancedMoveSet : SecondaryMoveSet{
//
//			}
//			///<summary>
//			/// Used for the offhand component of a power weapon</summary>
//			public abstract class SecondaryPowerMoveSet : SecondaryMoveSet{
//
//			}
//			public abstract class SecondaryFinesseMoveSet : SecondaryMoveSet{
//
//			}
//		}
//	}
}


