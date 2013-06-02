using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable] public class BaseEquipmentLoadoutModule : MonoBehaviour
{
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
	[SerializeField] BaseEquipment secondary;
//will be left null depending on the moveset
	public BaseEquipment Primary { get { return primary; } }

	public BaseEquipment Secondary { get { return secondary; } }

	void Awake ()
	{
//		switch (loadoutType) {
//		case EquipmentLoadoutType.OneHandedWithOffHand:
//		case EquipmentLoadoutType.DualWielding:
//		case EquipmentLoadoutType.TwoHand:
//		default:
//			break;
//		}
	}
}

public interface IEquipmentLoadout
{

}