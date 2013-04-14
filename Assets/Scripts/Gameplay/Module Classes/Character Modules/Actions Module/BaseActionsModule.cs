using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActions {
	IMovement CharMovement {get;}
	IEquipmentLoadout CharEquipment {get;}
	ICombat CharCombat {get;}
	IStealth CharStealth {get;}

//	void AttackRecoil (Vector3 hitDir);
//	void HitStun (float hitStrength, Vector3 hitDir);
}
public class ActionsModule : IActions {
	protected ICharacter user;

	protected IMovement charMovement;
	protected IEquipmentLoadout charEquipment;
	protected ICombat charCombat;
	protected IStealth charStealth;

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
