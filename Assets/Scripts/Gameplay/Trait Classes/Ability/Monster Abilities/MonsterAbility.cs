using UnityEngine;
using System.Collections;

public class MonsterAbility : WeaponAbility
{
	public enum MonsterAttackType
	{
		Tackle,
		Swipe
	}
	
	protected override void Awake ()
	{
		base.Awake ();
		Char = transform.parent.GetComponent<BaseEnemy>();
		VitalType = Char.CharStats.Stamina;
		
		Effect = TackleEffect;
	}
	
	private void TackleEffect ()
	{
		Char.Body.Move (Char.Coordinates.up * Stats.Speed * Time.deltaTime);
	}
	private void SwipeEffect ()
	{
		
	}
}

