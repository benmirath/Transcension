/*using UnityEngine;
using System.Collections;

public class FinisherAbility : WeaponAbility
{
	protected SpecialAbility _followupAbility;
	protected void BaseFinisherEffect ()
	{
		Char.Body.Move(Char.Coordinates.up * speed * Time.deltaTime);
	}
	
	protected override void Start ()
	{
		base.Start ();
		VitalType = Char.CharStats.Stamina;		
		Effect = BaseFinisherEffect;
		
		if (Char.CharStats == null )
			Debug.LogError ("character stats are not set yet");
		if (Effect == null)
			Debug.LogError ("no ability effect set yet");
	}
	
	

	public void SetValues (AttackStat attack, SpecialAbility followup)
	{
		base.SetValues(attack);
		_followupAbility = followup;
	}
	
	public IEnumerator ActivateFollowup ()
	{
		if (_followupAbility == null)
			Debug.LogWarning("followup attack is null");
		Debug.LogWarning("Activating followup attack!");	
		if (Char.CharStats.Stamina.CurrentValue >= _followupAbility.Cost)
		{
			yield return StartCoroutine(_followupAbility.ActivateAbility());
			yield break;
		}
		else 
			yield return null;
	}
	
	protected override IEnumerator ActivateAbilityDuration ()
	{
		SetHitBox(true);
		float curDuration = Time.time + durationLength;												
		do {
			//Empty function that will hold the ability effect. Will be filled in at ability's initialization.
			Effect();
			
			//This loop determines during what time span a followup attack can be launched
			if ((curDuration - Time.time) <= attackStats.ComboWindow)
			{
				_curWeapon.FollowupAvailable = true;
			}

			yield return null;
		} while (curDuration > Time.time);
		_curWeapon.FollowupAvailable = false;
		SetHitBox(false);	
	}
}

*/