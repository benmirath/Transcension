using UnityEngine;
using System.Collections;

public abstract class BaseShield : BaseEquipment
{
	//protected ShieldAbility block;
//	public float blockStrength;														//percentage of damage mitigated by blocking with shield
	public Vector3 shieldSize;														//size of collider for the shield
	
//	public float damageToStamina;													//amount of damage blocked that is dealt instead to stamina
	
	public float blockSpeed;
	//public float blockCost;
	public float blockStartup;
	//public float blockDuration;
	public float blockCooldown;
	
	
	protected override void Awake ()
	{
		base.Awake ();
		hitBox.isTrigger = true;
		
		//block = gameObject.AddComponent<ShieldAbility>();
	}
	
	protected override void Start ()
	{
		base.Start ();
//		_scalingAttribute = Char.Stats.Endurance;
//		_stats.SetValues(_scalingAttribute, scalingRatio);		
	//	block.SetValues("block", BaseCharacter.AnimState.Defending, blockSpeed, shieldSize, blockStartup, 0, blockCooldown);
	//	block.Effect = ShieldEffect;
	}
	
//	public IEnumerator StartBlock()
//	{
//		yield return block.CheckActivation();
//		yield break;
//	}
//	public override IEnumerator StartCombo ()
//	{
//		Debug.LogError("No OffHand ability found for this weapon :" + name);		
//		yield break;
//	}
//	public override IEnumerator StartOffhand ()
//	{
//	//	yield return 
//		StartCoroutine(block.Activate());
//		yield break;
//	}
//	public override IEnumerator StartSpecial ()
//	{
//		yield break;
//	}
	
}

