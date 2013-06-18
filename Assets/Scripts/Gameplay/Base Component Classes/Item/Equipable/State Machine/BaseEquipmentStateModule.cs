using System;
using System.Collections;
using UnityEngine;

[System.Serializable]
public class BaseEquipmentStateModule : StateMachineBehaviourEx
{
	BaseCharacter user;
	PlayerStateModule userState;
	MeshRenderer anim;
	//State Flags

	//Dictates what the intended followup after the attack will be. 
	public enum FollowupType
	{
		None
,
		Primary
,
		Secondary
,
		Dodge
,
	}

	[SerializeField] FollowupType followup = FollowupType.None;

	public FollowupType Followup {
		get { return followup;}
		set { followup = value;}
	}
//	bool attackFollowup = false;
//	bool dodgeFollowup = false;
//	bool counterFollowup = false;
//	bool guardFollowup = false;

	public enum EquipmentActions
	{
		Idle
,
//		ActivateWeapon
//,
//		//Primary Actions
		//Basic startup attack for weapon. Might start a combo, fire a projectile, etc.
		Combo1
,		//The alt attack for the primary moveset. 			
		AltPrimary
,					
		Combo2
,
		Combo3
,
		Combo4
,
		Combo5
,
		RunAttack
,
		DodgeAttack
,
		RangeAttack
,
		//Secondary Actions
		StartSecondary
,					//Basic secondary moveset attack. Might activate a shield, use a finisher, or start a counter.
		AltSecondary
,
		InterruptSecondary
	}

	protected override void OnAwake ()
	{
		user = transform.parent.GetComponent<BasePlayer> ();
		userState = transform.parent.GetComponent<PlayerStateModule> ();
		anim = GetComponent<MeshRenderer> ();
//		Debug.LogError ("EQUIPMENT SETUP");
		currentState = EquipmentActions.Idle;
	}

	void Start ()
	{
		StartCoroutine (MonitorStatus());
	}
//	protected void OnUpdate ()
//	{
//		Debug.Log (followup);
//	}
	protected IEnumerator MonitorStatus ()
	{
		while (true) {
			Debug.Log ("The followup status was: "+followup);
			yield return null;   
		}
	}

	void Idle_EnterState ()
	{
		anim.material.color = Color.white;
		Return ();
	}

	void Idle_Update ()
	{
//		userState
	}
	//This will be turned into a delegate, with functionality specified from the current moveset (found in the moveset module)
//	IEnumerator ActivateWeapon_EnterState ()
//	{
//		anim.material.color = Color.green;
//		Debug.Log ("The current state is:"+currentState);
//		Debug.Log ("The user's current state is:"+userState.currentState);
//		Debug.Log ("The last state is:"+lastState);
//		Debug.Log ("The users last state is:"+userState.lastState);
//		switch (userState.lastState.ToString ()) {
//		//Character is idle or walking
//		case "Idle":
//		case "Walk":
//			currentState = EquipmentActions.StartPrimary;			
//			break;
//		
//		//Character is running
//		case "Run":
//			currentState = EquipmentActions.RunAttack;
//			break;
//
//		//Character is dodging
//		case "Dodge":
//			currentState = EquipmentActions.DodgeAttack;
//			break;
//
//		case "StartPrimary":
//		case "Combo2":
//		case "Combo3":
//			followup = FollowupType.Primary;
//			Return ();
//			break;
//
//		default:
//			currentState = EquipmentActions.Idle;
//			break;
//
//
//		//Character is in the middle of combo1
//		//Character is in the middle of combo2
//		//Character is in the middle of combo3
//				
//		}
//		yield return new WaitForSeconds (1);
//		Debug.LogWarning ("Preparing to return from weapon call");
////		Return ();				
//	}
//
//	public void ActivateWeapon ()
//	{
//		anim.material.color = Color.green;
//		Debug.Log ("The current state is:"+currentState);
//		Debug.Log ("The user's current state is:"+userState.currentState);
//		Debug.Log ("The last state is:"+lastState);
//		Debug.Log ("The users last state is:"+userState.lastState);
//		switch (userState.lastState.ToString ()) {
//		//Character is idle or walking
//		case "Idle":
//		case "Walk":
//			currentState = EquipmentActions.StartPrimary;			
//			break;
//
//		//Character is running
//		case "Run":
//			currentState = EquipmentActions.RunAttack;
//			break;
//
//		//Character is dodging
//		case "Dodge":
//			currentState = EquipmentActions.DodgeAttack;
//			break;
//
//		case "StartPrimary":
//		case "Combo2":
//		case "Combo3":
//			followup = FollowupType.Primary;
//			break;
//
//		default:
//			currentState = EquipmentActions.Idle;
//			break;
//
//
//		//Character is in the middle of combo1
//		//Character is in the middle of combo2
//		//Character is in the middle of combo3
//
//		}
////		yield return new WaitForSeconds (1);
//		Debug.LogWarning ("Preparing to return from weapon call");
//		//		Return ();				
//	}

	protected IEnumerator StartPrimary_EnterState ()
	{
		anim.material.color = Color.red;
		yield return new WaitForSeconds (.5f);

		Debug.Log ("Checking followup stats");
		switch (followup) {
		case FollowupType.Primary:
			currentState = EquipmentActions.Combo2;
			break;

		case FollowupType.Dodge:
			currentState = EquipmentActions.Idle;
			Return ();
			currentState = BaseCharacterStateModule.CharacterActions.Dodge;
			break;

		case FollowupType.None:
			//break;
		default:
			currentState = EquipmentActions.Idle;
			break;
		}
		yield break;
	}
	//will check for whether a followup was signaled, and branch the combo into the next branch, and play the appropriate animation (swing followthrough or a second strike)
	protected void StartPrimary_ExitState ()
	{
		followup = FollowupType.None;
	}

	protected IEnumerator Combo2_EnterState ()
	{
		anim.material.color = Color.magenta;
		yield return new WaitForSeconds (.5f);

		switch (followup) {
		case FollowupType.Primary:
			currentState = EquipmentActions.Combo3;
			break;

		case FollowupType.Dodge:
			currentState = EquipmentActions.Idle;
			Return ();
			currentState = BaseCharacterStateModule.CharacterActions.Dodge;
			break;

		case FollowupType.None:
			//break;
		default:
			currentState = EquipmentActions.Idle;
			break;
		}
		yield break;
	}

	protected IEnumerator Combo2_ExitState ()
	{
		followup = FollowupType.None;
		yield break;
	}

	protected IEnumerator Combo3_EnterState ()
	{
		anim.material.color = Color.blue;
		yield return new WaitForSeconds (.5f);
		switch (followup) {
		case FollowupType.Primary:
			currentState = EquipmentActions.Combo4;
			break;

		case FollowupType.Dodge:
			currentState = EquipmentActions.Idle;
			Return ();
			currentState = BaseCharacterStateModule.CharacterActions.Dodge;
			break;

		case FollowupType.None:
			//break;
		default:
			currentState = EquipmentActions.Idle;
			break;
		}
		yield break;
	}

	protected IEnumerator Combo3_ExitState ()
	{
		followup = FollowupType.None;
		yield break;
	}

	protected IEnumerator Combo4_EnterState ()
	{
		anim.material.color = Color.blue;
		yield return new WaitForSeconds (.5f);
		switch (followup) {
		case FollowupType.Dodge:
			currentState = EquipmentActions.Idle;
			Return ();
			currentState = BaseCharacterStateModule.CharacterActions.Dodge;
			break;

		case FollowupType.Primary:
		case FollowupType.None:
		default:
			currentState = EquipmentActions.Idle;
			break;
		}
		yield break;
	}

	protected IEnumerator Combo4_ExitState ()
	{
		followup = FollowupType.None;
		yield break;
	}

	protected IEnumerator RunAttack_EnterState ()
	{
		anim.material.color = Color.yellow;
		yield return new WaitForSeconds (.5f);
		switch (followup) {
		case FollowupType.Dodge:
			currentState = EquipmentActions.Idle;
			Return ();
			currentState = BaseCharacterStateModule.CharacterActions.Dodge;
			break;

		case FollowupType.None:
		case FollowupType.Primary:
		default:
			currentState = EquipmentActions.Idle;
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
			currentState = EquipmentActions.Idle;
			Return ();
			currentState = BaseCharacterStateModule.CharacterActions.Dodge;
			break;

		case FollowupType.None:
		case FollowupType.Primary:
		default:
			currentState = EquipmentActions.Idle;
			break;
		}
		yield break;
	}

	protected void DodgeAttack_ExitState ()
	{
		followup = FollowupType.None;
	}
}

public interface IEquipmentStateModule
{
	BaseCharacter User { get; }

	PlayerStateModule UserState { get; }

	MeshRenderer Anim { get; }

	BaseEquipmentStateModule.EquipmentActions Followup { get; }

	void ActivateWeapon ();
}


