/*using UnityEngine;
using System.Collections;

public class ComboAbility : WeaponAbility
{	
	public WeaponAbility _followupAbility;
	
	private bool _followupUsed;
	
	protected void BaseComboEffect ()
	{
//		Debug.Log("COMBO ABILITY ACTIVATED");
		Char.Body.Move(Char.Coordinates.up * speed * Time.deltaTime);
	}
	
	
	protected override void Start ()
	{
		base.Start ();
		VitalType = Char.CharStats.Stamina;		
		Effect = BaseComboEffect;
		
		if (Char.CharStats == null )
			Debug.LogError ("character stats are not set yet");
		if (Effect == null)
			Debug.LogError ("no ability effect set yet");
	}
	
	public void SetValues (AttackStat stats, WeaponAbility followup)
	{
		base.SetValues(stats);
		attackStats = stats;
		_followupAbility = followup;
	}
	
	protected override IEnumerator ActivateAbilityDuration ()
	{
		_followupUsed = false;	
		float curDuration = Time.time + durationLength;												
		SetHitBox(true);

		do {
			//Empty function that will hold the ability effect. Will be filled in at ability's initialization.
			Effect();
			
			if ((curDuration - Time.time) <= attackStats.ComboWindow && CurWeapon.FollowupAvailable == false) 
				CurWeapon.FollowupAvailable = true;

			yield return null;
		} while (curDuration > Time.time);
		CurWeapon.FollowupAvailable = false;
		SetHitBox(false);	
	}
		
	public void ActivateFollowup ()
	{
		if (Char.CharStats.Stamina.CurrentValue >= _followupAbility.Cost)
		{
			_followupUsed = true;
		}
	}
	
}

*/