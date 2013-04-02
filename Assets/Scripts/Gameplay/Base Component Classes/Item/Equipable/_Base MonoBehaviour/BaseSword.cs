using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class BaseSword : BaseEquipment
{
//	new public enum MovelistSelection {
//		Combo1,
//		Combo2,
//		Combo3,
//		Combo4,
//		RunAttack,
//		DodgeAttack,
//		Finisher,
//		Special
//	}
	//private MovelistSelection _currentMove;

	[SerializeField] protected WeaponAbility.StandardAttack.ComboAttack comboAttack2;
	protected WeaponAbility.StandardAttack.ComboAttack comboAttack3;
	protected WeaponAbility.StandardAttack.FinisherAttack comboAttack4;

	//public  MovelistSelection CurrentMove {get {return _currentMove;}}

	protected override void Awake ()
	{
		base.Awake ();

	}
//	public override void TransitionToActivation () {
//		if (user.GetState == BaseCharacter.CharState.CombatReady) {
//
//			//if (user.IsRunning)
//		}
//
//		if (user.GetState == BaseCharacter.CharState.CombatReady && user.IsRunning == true) runAttack.ActivateAbility();
//		else if (user.GetState == BaseCharacter.CharState.CombatReady && user.IsEvading == true) dodgeAttack.ActivateAbility();
//		else if (user.GetState == BaseCharacter.CharState.CombatReady) comboAttack1.ActivateAbility();
//		else if (user.GetState == BaseCharacter.CharState.ComboAttack1 && FollowupAvailable == true) comboAttack2.ActivateAbility();
//		else if (user.GetState == BaseCharacter.CharState.ComboAttack2 && FollowupAvailable == true) comboAttack3.ActivateAbility();
//		else if (user.GetState == BaseCharacter.CharState.ComboAttack3 && FollowupAvailable == true) comboAttack4.ActivateAbility();
//		else return;
//	}

//	public override IEnumerator ActivateEquipment() {
//		while (User.GetState == BaseCharacter.CharState.Attacking) {
//
//
//		}
//		yield break;
//	}
//
//
//
//	public override void ActivateFollowup ()
//	{
//
//	}


	#region Properties
	/*public FinisherAbility Combo3
	{
		get {return combo3;}
		set {combo3 =  value;}
	}*/
	#endregion
//	public class SwordMoveset_Balanced : BaseEquipment.BaseEquipmentMovelist.MeleeMovelist.SingleSlotMovelist.MainHandMovelist {
//
//	}
//	public class SwordMoveset_Power : BaseEquipment.BaseEquipmentMovelist.MeleeMovelist.DoubleSlotMovelist.TwoHandedMovelist {
//		
//	}
//	public class SwordMoveset_Finesse : BaseEquipment.BaseEquipmentMovelist.MeleeMovelist.DoubleSlotMovelist.DualWieldedMovelist {
//
//	}
	public class SwordStats : BaseEquipment.BaseEquipmentStats
	{

	}
}

