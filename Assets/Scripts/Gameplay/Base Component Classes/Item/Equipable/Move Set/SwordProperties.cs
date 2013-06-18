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

//		StartCoroutine (AddAbilities());

	}
//	private IEnumerator AddAbilities () {
//		foreach (AttackProperties attack in activeMoveset) {
////			if (attack == null)
////				break;
////			else  {	
//				if ()
//				availableActions.Add (attack);
//				Debug.LogWarning("Adding attack "+attack);
//				yield return null;
////			}
//
//
//		}
//	}
//

	#region Moveset States
	protected IEnumerator Combo1_EnterState ()
	{
		anim.material.color = Color.red;
		if (activeMoveset == oneHandedPrimary)
			Debug.Log ("ONEHANDED MOVESET");
		if (activeMoveset == twoHandedPrimary) 
			Debug.Log ("TWOHANDED MOVESET");
//		float timer = Time.time + activeMoveset


		yield return new WaitForSeconds (.5f);

		Debug.Log ("Checking followup stats");
		switch (followup) {
		case FollowupType.Primary:
			currentState = SwordActions.Combo2;
			break;

		case FollowupType.Dodge:
			currentState = SwordActions.Idle;
			Return ();
			currentState = BaseCharacterStateModule.CharacterActions.Dodge;
			break;

		case FollowupType.None:
			//break;
		default:
			currentState = SwordActions.Idle;
			break;
		}
		yield break;
	}
	//will check for whether a followup was signaled, and branch the combo into the next branch, and play the appropriate animation (swing followthrough or a second strike)
	protected void Combo1_ExitState ()
	{
		followup = FollowupType.None;
	}

	protected IEnumerator Combo2_EnterState ()
	{
		anim.material.color = Color.magenta;
		yield return new WaitForSeconds (.5f);

		switch (followup) {
		case FollowupType.Primary:
			currentState = SwordActions.Combo3;
			break;

		case FollowupType.Dodge:
			currentState = SwordActions.Idle;
			Return ();
			currentState = BaseCharacterStateModule.CharacterActions.Dodge;
			break;

		case FollowupType.None:
			//break;
		default:
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
		anim.material.color = Color.blue;
		yield return new WaitForSeconds (.5f);
		switch (followup) {
		case FollowupType.Primary:
			currentState = SwordActions.Combo4;
			break;

		case FollowupType.Dodge:
			currentState = SwordActions.Idle;
			Return ();
			currentState = BaseCharacterStateModule.CharacterActions.Dodge;
			break;

		case FollowupType.None:
			//break;
		default:
			currentState = SwordActions.Idle;
			break;
		}
		yield break;
	}

	protected void Combo3_ExitState ()
	{
		followup = FollowupType.None;
	}

	protected IEnumerator Combo4_EnterState ()
	{
		anim.material.color = Color.blue;
		yield return new WaitForSeconds (.5f);
		switch (followup) {
		case FollowupType.Dodge:
			currentState = SwordActions.Idle;
			Return ();
			currentState = BaseCharacterStateModule.CharacterActions.Dodge;
			break;

		case FollowupType.Primary:
		case FollowupType.None:
		default:
			currentState = SwordActions.Idle;
			break;
		}
		yield break;
	}

	protected void Combo4_ExitState ()
	{
		followup = FollowupType.None;
	}

	protected IEnumerator RunAttack_EnterState ()
	{
		anim.material.color = Color.yellow;
		yield return new WaitForSeconds (.5f);
		switch (followup) {
		case FollowupType.Dodge:
			currentState = SwordActions.Idle;
			Return ();
			currentState = BaseCharacterStateModule.CharacterActions.Dodge;
			break;

		case FollowupType.None:
		case FollowupType.Primary:
		default:
			currentState = SwordActions.Idle;
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
		anim.material.color = Color.grey;

		yield return new WaitForSeconds (.5f);
		switch (followup) {
		case FollowupType.Dodge:
			currentState = SwordActions.Idle;
			Return ();
			currentState = BaseCharacterStateModule.CharacterActions.Dodge;
			break;

		case FollowupType.None:
		case FollowupType.Primary:
		default:
			currentState = SwordActions.Idle;
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


