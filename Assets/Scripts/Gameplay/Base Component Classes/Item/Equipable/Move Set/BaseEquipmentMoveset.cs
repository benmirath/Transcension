using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//TODO Fold entire moveset into properties class, and rename as proper moveset class

/// <summary>
/// Base equipment moveset module. Any given weapon will contain multiple objects of this
/// class, containing each of its particular actions for a specied loadout style.</summary>
[System.Serializable]
public abstract class BaseEquipmentMoveset
{
	#region Properties
	public enum MovesetType
	{
		OneHanded_MainHand,
		OneHanded_OffHand,
		TwoHanded_MainHand,
		TwoHanded_OffHand,
		DualWielding_MainHand,
		DualWielding_OffHand,
	}
	[SerializeField] protected MovesetType loadoutType;		//Used by the equipment loadout script to dynamically select appropriate moveset
	public MovesetType LoadoutType { 
		get { return loadoutType; } 
		set { loadoutType = value; }
	}

	//private members
	protected BaseEquipment _weapon;
	protected ICharacter _user;
	protected BaseCharacterStateModule _userState;
	//[SerializeField] protected List<MeleeProperties> moveset;
	#endregion

	#region Initialization & Setup
	public void Setup (BaseEquipment weapon, MovesetType type) {
		_weapon = weapon;
		_user = weapon.User;
		_userState = _user.CharState;
	}
//	void Awake ()
//	{
//		_weapon = GetComponent<BaseEquipment>();
//		_user = transform.parent.GetComponent<BaseCharacter>();
//	}
//	void SetMovesetType ()
//	{
//
//		switch (movesetType) {
//		case EquipmentMovesetType.PrimaryMelee:
//			ActivateMoveset = PrimaryMeleeActivation;			//will be customizable down the line
//			break;
//
//		case EquipmentMovesetType.PrimaryRanged:
//		case EquipmentMovesetType.SecondaryMelee:
//		case EquipmentMovesetType.SecondaryRanged:
//		case EquipmentMovesetType.SecondaryUtility:
//			break;
//		}
//
//	}
//	public void PopulateMoveList () {
//		foreach ()
//	}
	#endregion

	#region Equipment Activation
	public abstract void ActivateMoveset ();
//	private void PrimaryMeleeActivation ()
//	{
//		moveset.Find ();
//		//if idle or walking -> Start combo
//		//else if running -> Start run attack
//		//else if dodging -> Start dodge attack
//		if (_userState.currentState.ToString () == BaseCharacterStateModule.CharacterActions.Idle.ToString () || _userState.currentState.ToString () == BaseCharacterStateModule.CharacterActions.Walk.ToString ())
//			_userState.Call (BaseEquipmentStateModule.EquipmentActions.Combo1, _weapon.WeaponProperties);
//
//		else if(_userState.currentState.ToString () == BaseCharacterStateModule.CharacterActions.Run.ToString ())
//			_userState.Call (BaseEquipmentStateModule.EquipmentActions.RunAttack, _weapon.WeaponProperties);
//
//		else if (_userState.currentState.ToString () == BaseCharacterStateModule.CharacterActions.Dodge.ToString ()) 
//			_userState.Call (BaseEquipmentStateModule.EquipmentActions.DodgeAttack, _weapon.WeaponProperties);
//		else
//			_weapon.WeaponProperties.Followup = BaseEquipmentProperties.FollowupType.Primary;
//	}
//	private void PrimaryRangedActivation ()
//	{
//		//if idle or walking -> Fire shot
//	}
//	private void SecondaryMeleeActivation ()
//	{
//		//if idle or walking -> Start finisher
//		//if some combo -> Start combo finisher
//	}
	#endregion
}
[System.Serializable]
public class PrimaryMeleeThreeComboMoveset : BaseEquipmentMoveset {
	public override void ActivateMoveset ()
	{
		//		moveset.Find ();
		//if idle or walking -> Start combo
		//else if running -> Start run attack
		//else if dodging -> Start dodge attack
		if (_userState.currentState.ToString () == BaseCharacterStateModule.CharacterActions.Idle.ToString () || _userState.currentState.ToString () == BaseCharacterStateModule.CharacterActions.Walk.ToString ())
			_userState.Call (BaseEquipmentProperties.EquipmentActions.Combo1, _weapon.WeaponProperties);

		else if(_userState.currentState.ToString () == BaseCharacterStateModule.CharacterActions.Run.ToString ())
			_userState.Call (BaseEquipmentProperties.EquipmentActions.RunAttack, _weapon.WeaponProperties);

		else if (_userState.currentState.ToString () == BaseCharacterStateModule.CharacterActions.Dodge.ToString ()) 
			_userState.Call (BaseEquipmentProperties.EquipmentActions.DodgeAttack, _weapon.WeaponProperties);
		else
			_weapon.WeaponProperties.Followup = BaseEquipmentProperties.FollowupType.Primary;
	}

	[SerializeField] protected MeleeProperties combo1;
	[SerializeField] protected MeleeProperties combo2;
	[SerializeField] protected MeleeProperties combo3;
	[SerializeField] protected MeleeProperties runAttack;
	[SerializeField] protected MeleeProperties dodgeAttack;

	public MeleeProperties Combo1 { get { return combo1; } }
	public MeleeProperties Combo2 { get { return combo2; } }
	public MeleeProperties Combo3 { get { return combo3; } }
	public MeleeProperties RunAttack { get { return runAttack; } }
	public MeleeProperties DodgeAttack { get { return dodgeAttack; } }
}

[System.Serializable]
public class PrimaryMeleeFourComboMoveset : PrimaryMeleeThreeComboMoveset{
	[SerializeField] protected MeleeProperties combo4;
	public MeleeProperties Combo4 { get { return combo4;}}
}

[System.Serializable]
public class PrimaryMeleeFiveComboMoveset : PrimaryMeleeFourComboMoveset{
	[SerializeField] protected MeleeProperties combo5;

	public MeleeProperties Combo5 { get { return combo5;}}
}


[System.Serializable]
public class PrimaryRangedMoveset {

}

[System.Serializable]
public class SecondaryMeleeMoveset : BaseEquipmentMoveset {
	public override void ActivateMoveset ()
	{
		//If idle, use finisher

		//if attacking, queu to use finisher interrupt followup
	}
}

[System.Serializable]
public class SecondaryRangedMoveset {

}

[System.Serializable]
public class SecondaryUtilityMoveset {

}

public interface IEquipmentMoveset
{
	Action ActivateMoveset();
}

