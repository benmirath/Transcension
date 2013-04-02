/// <summary>
/// Base ability class, that acts as the basis of all in-game character abilities. Consists of a cost
/// value (specified in later derived classes), and three values that specify the duration of 
/// the startup, ability effect, and cooldown. </summary>
using System;
using System.Collections;
using UnityEngine;

public abstract class BaseAbility : MonoBehaviour
{
	#region Fields
	[SerializeField] private AbilityStats stats;
	private Action effect;																//empty delegate that will hold the end effect of the created ability

	private CharacterStatsModule.IVital _vitalType;

	private BaseCharacter _char;
	private CharacterController _body;													//Controller of character. Used for movement.

	#endregion Fields

	#region Properties
	public AbilityStats Stats {
		get {return stats;}
	}
	public Action Effect {
		get {return effect;}
		set {effect = value;}
	}
	public CharacterStatsModule.IVital VitalType {
		get {return _vitalType;}
		set {_vitalType = value;}
	}
	
	public BaseCharacter Char {
		get {return _char;}
		set {_char = value;}
	}
	public CharacterController Body {
		get {return _body;}
		set {_body = value;}
	}
	#endregion
	
	#region Initialization
	protected virtual void Awake ()
	{
		_char = GetComponent<BaseCharacter>();
		_body = GetComponent<CharacterController>();

		name = "default";
		stats.Cost = 0;
		stats.Speed = 0;
		stats.StartupLength = 0;
		stats.DurationLength = 0;
		stats.CooldownLength = 0;
		SetValues();
	}

	/// <summary>
	/// Will be used to specify values in derived classes to tailer functionality of abilities. </summary>
	public abstract void SetValues ();
	#endregion Initialization
	
	private bool CheckCost ()
	{
		if (_vitalType.CurValue < stats.Cost)	return false;
		if (stats.Cost != 0)																			
		{
			_vitalType.CurValue -= stats.Cost;
			_vitalType.StopRegen = true;
		}
		return true;
	}
	
	protected virtual IEnumerator ActivateAbilityStartup ()
	{
		if (stats.StartupLength > 0) yield return new WaitForSeconds(stats.StartupLength);				//checks if ability has a startup time, and pauses script for appropriate length
		yield break;
	}
	protected virtual IEnumerator ActivateAbilityDuration ()
	{
		float curDuration = Time.time + stats.DurationLength;											//Sets length of time at which ability's duration will be finished

		do {
			Effect();																		
			yield return null;
		} while (curDuration > Time.time);
		yield break;
	}
	protected virtual IEnumerator ActivateAbilityCooldown ()
	{
		if (stats.CooldownLength > 0) yield return new WaitForSeconds(stats.CooldownLength);	
		_vitalType.StopRegen = false;
		yield break;
	}

	/// <summary>
	/// Activates the ability, checking if the required vital is at a usable level. </summary>
	public virtual IEnumerator ActivateAbility () {
		if (CheckCost()) 
		{	
			if (stats.StartupLength != 0)	yield return StartCoroutine(ActivateAbilityStartup());
			if (stats.DurationLength != 0) yield return StartCoroutine(ActivateAbilityDuration());
			else Effect();
			
			if (stats.CooldownLength != 0) yield return StartCoroutine(ActivateAbilityCooldown());
			yield break;
		}
		else yield break;
	}


	[System.Serializable] public abstract class AbilityStats
	{
		#region Fields
		[SerializeField] protected string name;												//Name of the attack
		[SerializeField] protected float cost;
		[SerializeField] protected float speed;
		
		[SerializeField] protected float startupLength;										//length of time between when ability is activated, and when it takes effect.
		[SerializeField] protected float durationLength;									//length of time that the ability remains in effect.
		[SerializeField] protected float cooldownLength;									//length of time between when ability effect ends, and character can act again.
		#endregion

		#region  Properties
		public float Cost 
		{
			get {return cost;}
			set {cost = value;}
		}
		public float Speed 
		{
			get {return speed;}
			set {speed = value;}
		}
		public float StartupLength 
		{
			get {return startupLength;}
			set {startupLength = value;}
		}
		public float DurationLength 
		{
			get {return durationLength;}
			set {durationLength = value;}
		}	
		public float CooldownLength 
		{
			get {return cooldownLength;}
			set {cooldownLength = value;}
		}
		#endregion Properties
	}
}