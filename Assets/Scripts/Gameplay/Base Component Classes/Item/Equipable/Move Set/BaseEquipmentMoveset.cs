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
	protected CharacterStateMachine _userState;
	[SerializeField] protected List<AttackProperties> moveset;

	public List<AttackProperties> Moveset { get { return moveset; } }
	#endregion

	#region Initialization & Setup
	public virtual void Setup (BaseEquipment weapon, MovesetType type) {
		_weapon = weapon;
		_user = weapon.User;
		_userState = _user.CharState;
	}
	#endregion

	#region Equipment Activation
	public abstract void ActivateMoveset ();
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
		if (_userState.currentState.ToString () == CharacterStateMachine.CharacterActions.Idle.ToString () || _userState.currentState.ToString () == CharacterStateMachine.CharacterActions.Walk.ToString ())
			_userState.Call (BaseEquipmentProperties.EquipmentActions.Combo1, _weapon.WeaponProperties);

		else if(_userState.currentState.ToString () == CharacterStateMachine.CharacterActions.Run.ToString ())
			_userState.Call (BaseEquipmentProperties.EquipmentActions.RunAttack, _weapon.WeaponProperties);

		else if (_userState.currentState.ToString () == CharacterStateMachine.CharacterActions.Dodge.ToString ()) 
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

	public override void Setup (BaseEquipment weapon, MovesetType type)
	{
		base.Setup (weapon, type);

		combo1.SetValue (weapon, BaseEquipmentProperties.EquipmentActions.Combo1);
		combo2.SetValue (weapon, BaseEquipmentProperties.EquipmentActions.Combo2);
		combo3.SetValue (weapon, BaseEquipmentProperties.EquipmentActions.Combo3);
		runAttack.SetValue (weapon, BaseEquipmentProperties.EquipmentActions.RunAttack);
		dodgeAttack.SetValue (weapon, BaseEquipmentProperties.EquipmentActions.DodgeAttack);

		moveset.Add (combo1);
		moveset.Add (combo2);
		moveset.Add (combo3);
		moveset.Add (runAttack);
		moveset.Add (dodgeAttack);


	}
}

[System.Serializable]
public class PrimaryMeleeFourComboMoveset : PrimaryMeleeThreeComboMoveset{
	[SerializeField] protected MeleeProperties combo4;
	public MeleeProperties Combo4 { get { return combo4;}}

	public override void Setup (BaseEquipment weapon, MovesetType type)
	{
		base.Setup (weapon, type);
		combo4.SetValue (weapon, BaseEquipmentProperties.EquipmentActions.Combo4);
		moveset.Add (combo4);

	}
}

[System.Serializable]
public class PrimaryMeleeFiveComboMoveset : PrimaryMeleeFourComboMoveset{
	[SerializeField] protected MeleeProperties combo5;

	public MeleeProperties Combo5 { get { return combo5;}}

	public override void Setup (BaseEquipment weapon, MovesetType type)
	{
		base.Setup (weapon, type);
		combo5.SetValue (weapon, BaseEquipmentProperties.EquipmentActions.Combo5);
		moveset.Add (combo5);
	}
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

