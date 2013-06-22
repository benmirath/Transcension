using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovesetModule : MonoBehaviour, IMoveSet {
	[SerializeField]protected BaseCharacter user;

	[SerializeField] protected BaseMovementModule charMovement;
	[SerializeField] protected BaseEquipmentLoadoutModule charEquipment;
	[SerializeField] protected CharacterStatusModule charStatus;
	[SerializeField] protected BaseStealthModule charStealth;

	public BaseMovementModule CharMovement {
		get {return charMovement;}
	}
	public BaseEquipmentLoadoutModule CharEquipment {
		get {return charEquipment;}
	}
	public IStatus CharStatus {
		get {return charStatus;}
	}
	public IStealth CharStealth {
		get {return charStealth;}
	}

	public void Awake() 
	{
		user = GetComponent<BaseCharacter>();
		//construct ability classes
	}
	public void Start()
	{
		Debug.Log("MoveSet: Setting Values");
		if (user == null)
			Debug.Log ("Moveset user is currently null");
		charMovement.Setup (user);
		charStatus.Setup (user);
		charEquipment.Setup ();
	}


//	public void AttackRecoil (Vector3 hitDir) {
//		user.CharBase.StartCoroutine (StunnedState());
//		TransitionToStunned();
//	}
	
	/// <summary>
	/// Applies hit stun to the character, and activates the character's stunned state if the max value is exceeded.
	/// </summary>
	/// <param name='hitStrength'> Strength (physical, not statistical) of the attack. </param>
	/// <param name='hitDir'> Direction in which the attack pushes the character. </param>
//	public void HitStun(float hitStrength, Vector3 hitDir) {
//		CharStats.StunResistance.CurValue += hitStrength;
//		
//		if (CharStats.StunResistance.CurValue >= CharStats.StunResistance.MaxValue)
//		{
//			Debug.Log ("Beginning stunned");
//			CharStats.StunResistance.CurValue = CharStats.StunResistance.MinValue;
//			_knockDir = hitDir * Time.deltaTime * CharStats.StunStrength;				//new Vector3(hitDir.x, hitDir.y, hitDir);
//			
//			StartCoroutine (StunnedState());
//			TransitionToStunned();
//		}
//	}

}

public interface IMoveSet {
	//IAbility CurrentAction {get;}

	[SerializeField] BaseMovementModule CharMovement {get;}
	[SerializeField] BaseEquipmentLoadoutModule CharEquipment {get;}
	[SerializeField] IStatus CharStatus {get;}
	[SerializeField] IStealth CharStealth {get;}

	//	void AttackRecoil (Vector3 hitDir);
	//	void HitStun (float hitStrength, Vector3 hitDir);
}