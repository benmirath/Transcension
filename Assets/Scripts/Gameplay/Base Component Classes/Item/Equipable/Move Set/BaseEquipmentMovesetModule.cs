using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base equipment moveset module. Any given weapon will contain multiple objects of this
/// class, containing each of its particular actions for a specied loadout style.</summary>
[System.Serializable]
public class BaseEquipmentMovesetModule : MonoBehaviour
{
	#region Properties
	public enum EquipmentMovesetType
	{
		PrimaryMelee,					//main hand melee moveset (sword, mace)
		PrimaryRanged,					//main hand ranged moveset (great bow, sidearm)
		SecondaryMelee,					//secondary melee moveset (2handed sword, chi focus)
		SecondaryRanged,				//secondary ranged moveset (chi focus, 2handed sidearm)
		SecondaryUtility,				//secondary assistance moveset (shield, claw's counter)
	}
	//public members
	[SerializeField] protected EquipmentMovesetType movesetType;

	//private members
	BaseEquipment _weapon;
	ICharacter _user;
	BaseCharacterStateModule _userState;
	[SerializeField] protected List<MeleeProperties> moveset;

	#endregion

	#region Initialization & Setup
	void Awake ()
	{
		_weapon = GetComponent<BaseEquipment>();
		_user = transform.parent.GetComponent<BaseCharacter>();
	}
	void Start ()
	{
		_userState = _user.CharState;

		ActivateMoveset = PrimaryMeleeActivation;			//will be customizable down the line
	}
	#endregion

	#region Equipment Activation
	public Action ActivateMoveset;
	private void PrimaryMeleeActivation ()
	{
		//if idle or walking -> Start combo
		//else if running -> Start run attack
		//else if dodging -> Start dodge attack
		if (_userState.currentState.ToString () == BaseCharacterStateModule.CharacterActions.Idle.ToString () || _userState.currentState.ToString () == BaseCharacterStateModule.CharacterActions.Walk.ToString ())
			_userState.Call (BaseEquipmentStateModule.EquipmentActions.StartPrimary, _weapon.WeaponState);

		else if(_userState.currentState.ToString () == BaseCharacterStateModule.CharacterActions.Run.ToString ())
			_userState.Call (BaseEquipmentStateModule.EquipmentActions.RunAttack, _weapon.WeaponState);

		else if (_userState.currentState.ToString () == BaseCharacterStateModule.CharacterActions.Dodge.ToString ()) 
			_userState.Call (BaseEquipmentStateModule.EquipmentActions.DodgeAttack, _weapon.WeaponState);
		else
			_weapon.WeaponState.Followup = BaseEquipmentStateModule.FollowupType.Primary;
	}
	private void PrimaryRangedActivation ()
	{
		//if idle or walking -> Fire shot
	}
	private void SecondaryMeleeActivation ()
	{
		//if idle or walking -> Start finisher
		//if some combo -> Start combo finisher
	}
	#endregion

	#region Equipment Actions

	#endregion

}

public interface IEquipmentMoveset
{
	Action ActivateMoveset();
}

