#define DEBUG
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable] public class BaseEquipmentLoadoutModule
{
	/// <summary> 
	/// Used to determine the kinds of equipment/weapons that the character can equip. If one handing, will display a slot for
	/// a second weapon. </summary>
	public enum EquipmentLoadoutType
	{
		OneHandedWithOffHand
,
		TwoHand
,
		DualWielding
	}
	//Current Equipment
	[SerializeField] EquipmentLoadoutType loadoutType;

	[SerializeField] BaseEquipment primary;
	[SerializeField] BaseEquipment secondary;			//will be left null and hidden in the editor if loadout type isn't one handed.

	private BaseEquipmentMoveset primaryMoveset;
	private BaseEquipmentMoveset secondaryMoveset;

	public BaseEquipmentMoveset PrimaryMoveset { get { return primaryMoveset; } }
	public BaseEquipmentMoveset SecondaryMoveset { get { return secondaryMoveset; } }

	public BaseEquipment Primary { get { return primary; } }
	public BaseEquipment Secondary { get { return secondary; } }

	public void Setup ()
	{
#if DEBUG
		if (Primary == null) {
			return;
			Debug.LogError ("Primary is null");
		}
		else
			Debug.LogWarning ("Should be good to go");

		if (Primary.WeaponProperties == null)
			Debug.LogError ("Primary weapon properties is null");
		else
			Debug.LogWarning ("Should be good to go");

		if (Primary.WeaponProperties.AvailableMovesets == null)
			Debug.LogError ("Primary available movesets is null");
		else
			Debug.LogWarning ("Should be good to go");
#endif

		switch (loadoutType) {
		case EquipmentLoadoutType.OneHandedWithOffHand:
			primaryMoveset = Primary.WeaponProperties.AvailableMovesets.Find (i => i.LoadoutType == BaseEquipmentMoveset.MovesetType.OneHanded_MainHand);
			Primary.WeaponProperties.ActiveMoveset = primaryMoveset;

			Debug.Log (Primary.WeaponProperties);
//			secondaryMoveset = Secondary.WeaponProperties.AvailableMovesets.Find (i => i.LoadoutType == BaseEquipmentMoveset.MovesetType.OneHanded_OffHand);
//			Secondary.WeaponProperties.ActiveMoveset = secondaryMoveset;
			break;

		case EquipmentLoadoutType.TwoHand:
			primaryMoveset = Primary.WeaponProperties.AvailableMovesets.Find (i => i.LoadoutType == BaseEquipmentMoveset.MovesetType.TwoHanded_MainHand);
			secondaryMoveset = Primary.WeaponProperties.AvailableMovesets.Find (i => i.LoadoutType == BaseEquipmentMoveset.MovesetType.TwoHanded_OffHand);
			break;

		case EquipmentLoadoutType.DualWielding:
			primaryMoveset = Primary.WeaponProperties.AvailableMovesets.Find (i => i.LoadoutType == BaseEquipmentMoveset.MovesetType.DualWielding_MainHand);
			secondaryMoveset = Primary.WeaponProperties.AvailableMovesets.Find (i => i.LoadoutType == BaseEquipmentMoveset.MovesetType.DualWielding_OffHand);
			break;

		default:
			Debug.LogError ("Weapon loadout not set to a proper type!");
			break;
		}

		if (primaryMoveset == null)
			Debug.LogError ("Primary Equipment Loadout not properly setup, moveset could not be found!");
		if (secondaryMoveset == null)
			Debug.LogError ("Secondary Equipment Loadout not properly setup, moveset could not be found!");
//		switch (loadoutType) {
//		case EquipmentLoadoutType.OneHandedWithOffHand:
//		case EquipmentLoadoutType.DualWielding:
//		case EquipmentLoadoutType.TwoHand:
//		default:
//			break;
//		}
	}

	void ActivatePrimary () {
		primaryMoveset.ActivateMoveset ();
	}
	void ActivateSecondary () {
		secondaryMoveset.ActivateMoveset ();
	}
}

public interface IEquipmentLoadout
{

}