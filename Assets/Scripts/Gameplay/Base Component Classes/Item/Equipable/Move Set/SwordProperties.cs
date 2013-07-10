using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//These interfaces give the signatures for the standard abilities involved with a sword
public interface ISwordPrimaryMoveset {
	MeleeProperties combo1 { get;}
	MeleeProperties combo2 { get;}
	MeleeProperties combo3 { get;}
	MeleeProperties runAttack { get;}
	MeleeProperties dodgeAttack { get;}
}
public interface ISwordSecondaryMoveset {

}

public class SwordProperties : BaseEquipmentProperties
{
	#region Properties
	private enum SwordMoveset
	{
		OneHandedMainHand,
		TwoHandedMainHand,
		TwoHandedOffHand,
	}
	private enum SwordActions
	{
		//Mainhand
		Idle,
		Combo1,
		Combo2,
		Combo3,
		Combo4,
		RunAttack,
		DodgeAttack,
		//Offhand
		Finisher,
		ComboFinisher,
	}
	//Available Movesets
	//SwordMoveset currentMoveset;
	[SerializeField] PrimaryMeleeFourComboMoveset oneHandedPrimary;
	[SerializeField] PrimaryMeleeFourComboMoveset twoHandedPrimary;
	[SerializeField] SecondaryMeleeMoveset twoHandedSecondary;
//
//	ISwordPrimaryMoveset currentPrimary;
//	ISwordSecondaryMoveset currentSecondary;
//
//	public override BaseEquipmentMoveset CurrentMoveset {
//		get {
//			int moveset = (int)currentMoveset;
//			return availableMovesets[moveset];
//		}
//	}



	#endregion Properties

	protected override void OnAwake ()
	{
		base.OnAwake ();
		availableMovesets = new List<BaseEquipmentMoveset> ();

		availableMovesets.Add (oneHandedPrimary);
		availableMovesets.Add (twoHandedPrimary);
		availableMovesets.Add (twoHandedSecondary);
	}

	protected override void Start () {
		base.Start ();
		oneHandedPrimary.Setup (curEquipment, BaseEquipmentMoveset.MovesetType.OneHanded_MainHand);
		twoHandedPrimary.Setup (curEquipment, BaseEquipmentMoveset.MovesetType.TwoHanded_MainHand);
		twoHandedSecondary.Setup (curEquipment, BaseEquipmentMoveset.MovesetType.TwoHanded_OffHand);
	}

	#region Moveset States
	protected IEnumerator Combo1_EnterState ()
	{
		//Attack Initialization
//		float _timer;
		userState.Attacking = true;
		var attack = availableActions.Find (i => i.AttackName == EquipmentActions.Combo1);
//		if (attack == null) Debug.LogError ("Attack is null");
		curEquipment.ActiveAttack = attack;
//		if (userState.CheckAbilityVital (attack))
		yield return StartCoroutine( attack.ActivateDurationalAbility ());


		//Attack Startup
//		_timer = Time.time + attack.EnterLength;
//		do {
//			//attack info
//			anim.material.color = Color.grey;
//			attack.Aim ();
//			yield return null;
//		} while (Time.time < _timer);
//
//		//Attack Activte
//		_timer = Time.time + attack.ActiveLength;
//		do {
//			//attack info
//			attack.ActiveAbility();
//			anim.material.color = Color.red;
//			yield return null;
//		} while (Time.time < _timer);
//
//		//Attack Cooldown
//		anim.material.color = Color.grey;
//		yield return new WaitForSeconds (attack.ExitLength);
//
		switch (followup) {
		case FollowupType.Primary:
			currentState = BaseEquipmentProperties.EquipmentActions.Combo2;
			break;

		case FollowupType.Dodge:
			userState.Attacking = false;
			currentState = BaseEquipmentProperties.EquipmentActions.Idle;
			Return ();
			currentState = CharacterStateMachine.CharacterActions.Dodge;
			break;

		case FollowupType.None:
		default:
			userState.Attacking = false;
			currentState = BaseEquipmentProperties.EquipmentActions.Idle;
			break;
		}
		yield break;
	}
//	protected void Combo1_OnTriggerEnter (Collider hit ) {
//		Debug.LogError ("!!!HIIIIIIT!!!!");
//		var attack = availableActions.Find (i => i.AttackName == EquipmentActions.Combo1);
//		attack.OnTriggerEnter (hit);
//
//	}
	//will check for whether a followup was signaled, and branch the combo into the next branch, and play the appropriate animation (swing followthrough or a second strike)
	protected void Combo1_ExitState ()
	{
		followup = FollowupType.None;
	}

	protected IEnumerator Combo2_EnterState ()
	{
		//Attack Initialization
		userState.Attacking = true;
		var attack = availableActions.Find (i => i.AttackName == EquipmentActions.Combo2);
		curEquipment.ActiveAttack = attack;
//		if (userState.CheckAbilityVital (attack))
			yield return StartCoroutine (attack.ActivateDurationalAbility ());

//		//Attack Startup
//		anim.material.color = Color.grey;
//		yield return new WaitForSeconds (attack.EnterLength);
//
//		//Attack Activte
//		float _timer = Time.time + attack.ActiveLength;
//		do {
//			//attack info
//			attack.ActiveAbility();
//			anim.material.color = Color.magenta;
//			yield return null;
//		} while (Time.time < _timer);
//
//		//Attack Cooldown
//		anim.material.color = Color.grey;
//		yield return new WaitForSeconds (attack.ExitLength);

		switch (followup) {
		case FollowupType.Primary:
			currentState = BaseEquipmentProperties.EquipmentActions.Combo3;
			break;

		case FollowupType.Dodge:
			userState.Attacking = false;
			currentState = BaseEquipmentProperties.EquipmentActions.Idle;
			Return ();
			currentState = CharacterStateMachine.CharacterActions.Dodge;
			break;

		case FollowupType.None:
			//break;
		default:
			userState.Attacking = false;
			currentState = SwordActions.Idle;
			break;
		}
		yield break;
	}

	protected void Combo2_ExitState ()
	{
	 	followup = FollowupType.None;
	}

	protected IEnumerator Combo3_EnterState ()
	{
		//Attack Initialization
		userState.Attacking = true;
		var attack = availableActions.Find (i => i.AttackName == EquipmentActions.Combo3);
		curEquipment.ActiveAttack = attack;
//		if (userState.CheckAbilityVital (attack))
		yield return StartCoroutine (attack.ActivateDurationalAbility ());

		switch (followup) {
			case FollowupType.Dodge:
				userState.Attacking = false;
				currentState = BaseEquipmentProperties.EquipmentActions.Idle;
				Return ();
				currentState = CharacterStateMachine.CharacterActions.Dodge;
				break;

			case FollowupType.Primary:
			case FollowupType.None:
			default:
				userState.Attacking = false;
				currentState = BaseEquipmentProperties.EquipmentActions.Idle;
				break;
			}
		yield break;
	}

	protected void Combo3_ExitState ()
	{
		followup = FollowupType.None;
	}

//	protected IEnumerator Combo4_EnterState ()
//	{
//		//Attack Initialization
//		userState.Attacking = true;
//		var attack = availableActions.Find (i => i.AttackName == EquipmentActions.Combo3);
//		curEquipment.ActiveAttack = attack;
////		if (userState.CheckAbilityVital (attack))
//		yield return StartCoroutine (attack.ActivateDurationalAbility ());
//
//
//		switch (followup) {
//		case FollowupType.Dodge:
//			currentState = BaseEquipmentProperties.EquipmentActions.Idle;
//			Return ();
//			currentState = CharacterStateMachine.CharacterActions.Dodge;
//			break;
//
//		case FollowupType.Primary:
//		case FollowupType.None:
//		default:
//			currentState = BaseEquipmentProperties.EquipmentActions.Idle;
//			break;
//		}
//		yield break;
//	}

	protected void Combo4_ExitState ()
	{
		followup = FollowupType.None;
	}

	protected IEnumerator RunAttack_EnterState ()
	{	
		userState.Attacking = true;
		var attack = availableActions.Find (i => i.AttackName == EquipmentActions.RunAttack);
		curEquipment.ActiveAttack = attack;

		switch (followup) {
		case FollowupType.Dodge:
			currentState = BaseEquipmentProperties.EquipmentActions.Idle;
			Return ();
			currentState = CharacterStateMachine.CharacterActions.Dodge;
			break;

		case FollowupType.None:
		case FollowupType.Primary:
		default:
			currentState = BaseEquipmentProperties.EquipmentActions.Idle;
			break;
		}
		yield break;
	}

	protected void RunAttack_ExitState ()
	{
		followup = FollowupType.None;
	}

	protected IEnumerator DodgeAttack_EnterState ()
	{
		userState.Attacking = true;
		var attack = availableActions.Find (i => i.AttackName == EquipmentActions.DodgeAttack);
		curEquipment.ActiveAttack = attack;

		switch (followup) {
		case FollowupType.Dodge:
			currentState = BaseEquipmentProperties.EquipmentActions.Idle;
			Return ();
			currentState = CharacterStateMachine.CharacterActions.Dodge;
			break;

		case FollowupType.None:
		case FollowupType.Primary:
		default:
			currentState = BaseEquipmentProperties.EquipmentActions.Idle;
			break;
		}
		yield break;
	}

	protected void DodgeAttack_ExitState ()
	{
		followup = FollowupType.None;
	}

	#endregion
}


