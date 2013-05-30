using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSet : MonoBehaviour, IMoveSet {
	[SerializeField]protected ICharacter user;

	[SerializeField] protected BaseMovementModule charMovement;
	[SerializeField] protected BaseEquipmentLoadoutModule charEquipment;
	[SerializeField] protected BaseCombatModule charCombat;
	[SerializeField] protected BaseStealthModule charStealth;

	public IMovement CharMovement {
		get {return charMovement;}
	}
	public IEquipmentLoadout CharEquipment {
		get {return charEquipment;}
	}
	public ICombat CharCombat {
		get {return charCombat;}
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
		charMovement.Setup(user);
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

	[SerializeField] IMovement CharMovement {get;}
	[SerializeField] IEquipmentLoadout CharEquipment {get;}
	[SerializeField] ICombat CharCombat {get;}
	[SerializeField] IStealth CharStealth {get;}

	//	void AttackRecoil (Vector3 hitDir);
	//	void HitStun (float hitStrength, Vector3 hitDir);
}