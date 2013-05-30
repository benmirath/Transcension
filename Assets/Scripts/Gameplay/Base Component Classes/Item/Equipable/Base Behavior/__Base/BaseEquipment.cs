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

		BaseEquipmentProperties Stats {
				get;
		}

		BaseEquipmentMovesetModule Moveset {
				get;
		}

		BaseEquipmentStateModule WeaponState {
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
public class BaseEquipment : MonoBehaviour, IEquippable
{

		protected ICharacter user;
		protected Collider hitBox;
		[SerializeField] protected BaseEquipmentProperties stats;				//Holds the raw stats for the weapon
		[SerializeField] protected BaseEquipmentMovesetModule moveset;		//Holds the various moves a given weapon can utilize
		[SerializeField] protected BaseEquipmentStateModule weaponState;	//Coordinates use of moveset by controlling character


	#region Properties
		public ICharacter User {
				get { return user;}
				set { user = value;}
		}

		public Collider HitBox {
				get { return hitBox;}
				set { hitBox = value;}
		}

		public BaseEquipmentProperties Stats {
				get { return stats;}
		}

		public BaseEquipmentMovesetModule Moveset {
				get { return moveset;}
		}

		public BaseEquipmentStateModule WeaponState {
				get { return weaponState;}
		}
	#endregion

	#region Initialization
		protected virtual void Awake ()
		{
				user = transform.parent.GetComponent<BaseCharacter> ();
				hitBox = GetComponent<Collider> ();
	
				stats = GetComponent<BaseEquipmentProperties> ();
				moveset = GetComponent<BaseEquipmentMovesetModule> ();
				weaponState = GetComponent <BaseEquipmentStateModule> ();

		}

		protected virtual void Start ()
		{
				//stats.Setup(this);
		
				hitBox.isTrigger = true;
				hitBox.enabled = false;	
		}
	#endregion Initialization

	#region Methods
	#endregion Methods	
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
}


