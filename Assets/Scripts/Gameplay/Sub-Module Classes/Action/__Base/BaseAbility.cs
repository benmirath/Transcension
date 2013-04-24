/// <summary>
/// Base ability class, that acts as the basis of all in-game character abilities. Consists of a cost
/// value (specified in later derived classes), and three values that specify the duration of 
/// the startup, ability effect, and cooldown. </summary>
using System;
using System.Collections;
using UnityEngine;

public interface IAbility {
	void Activate ();

	ICharacter User {
		get;
	}
	IVital VitalType {
		get;
	}
	IPhysics UserPhysics {
		get;
	}
	AbilityProperties Stats {
		get;
	}
}

/// <summary>
/// The Base for all actions and abilities in the game. Consists of three distinct phases, a startup, duration in which 
/// the primary effect is active, and the cooldown. Each has an generic Action function for each phase. The finer points of functionality
/// are filled in by the ability's stat module.
/// </summary>
public class BaseAbility : MonoBehaviour {
	public enum AbilityType {
		//General
		Movement,		//moves(or restricts movement of) character
		Combat,			//initiates the effect of a combat result (being hit, status effect, etc.)
		Stealth,		//initiates a stealth-oriented period
		//Attacks (Combat Equipment)
		Melee,			//initiates a melee attack
		Ranged,			//initiates a ranged attack
		Defend,			//initiates a damage reducing period
		Counter,		//initiates a counter period
		Throw,
		//Player Specific
		Class,
	}
	
	#region External Fields
	[SerializeField] protected AbilityType abiltyType;				//determines the type of stat created, and the general role of the ability
	[SerializeField] protected AbilityProperties stats;

	//Action delegates that execute the ability's functionality
	public delegate void StartupEffect();
	public delegate void DurationEffect();
	public delegate void CooldownEffect();

	protected StartupEffect Start;
	protected DurationEffect Middle;
	protected CooldownEffect End;
	#endregion

	#region Internal Fields   
	private ICharacter user;
	private IVital userVital;
	private IPhysics userPhysics;
	//private CharacterController controller;													//Controller of character. Used for movement.
	#endregion

	#region Properties
	public AbilityProperties Stats {
		get {return stats;}
	}
	public ICharacter User {
		get {return user;}
	}
	public IVital VitalType {
		get {return userVital;}
	}
	public IPhysics UserPhysics {
		get {return userPhysics;}
	}
	#endregion

	#region Initialization
	protected void Setup () {
		switch (abiltyType) {
		case AbilityType.Movement:
			stats = ScriptableObject.CreateInstance<MovementProperties>();
			break;
			
		case AbilityType.Melee:
			stats = ScriptableObject.CreateInstance<MeleeProperties>();
			break;

		case AbilityType.Ranged:
			break;
			
		case AbilityType.Defend:
			
			break;
			
		case AbilityType.Counter:
			
			break;
			
		case AbilityType.Stealth:
			
			break;
		}
	}
	#endregion
	
	#region Initialization
	protected void Awake () {
		user = GetComponent<BaseCharacter>();
		//controller = GetComponent<CharacterController>();

		name = "default";
//		stats.Cost = 0;
//		//stats.Speed = 0;
//		stats.StartupLength = 0;
//		stats.DurationLength = 0;
//		stats.CooldownLength = 0;
//		SetValues();
	}

	/// <summary>
	/// Will be used to specify values in derived classes to tailer functionality of abilities. </summary>
//	public virtual void SetValues () {
//
//	}
//	public virtual void SetValues (float start, float mid, float end, IVital vital, float cost) {
//		stats=new AbilityStats(start, mid, end, cost);
//		//vitalType=vital;
//	}
//	public virtual void SetMovementValues (float start, float mid, float end, IVital vital, float cost, Action<Vector3> effect, float speed) {
//		stats=new AbilityStats(start, mid, end, cost);
//	}

	#endregion

	#region Effects
	protected virtual IEnumerator ActivateAbilityStartup () {
		float dur = Time.time + stats.StartupLength;											//Sets length of time at which ability's duration will be finished
		do {
			Start ();
			yield return null;
		} while (dur > Time.time);
		//if (stats.StartupLength > 0) yield return new WaitForSeconds(stats.StartupLength);				//checks if ability has a startup time, and pauses script for appropriate length
		yield break;
	}
	protected virtual IEnumerator ActivateAbilityDuration () {
		float dur = Time.time + stats.DurationLength;											//Sets length of time at which ability's duration will be finished

		do {
			Middle ();
			yield return null;
		} while (dur > Time.time);
		yield break;
	}
	protected virtual IEnumerator ActivateAbilityCooldown () {
		float dur = Time.time + stats.CooldownLength;
		do {
			End ();
			yield return null;
		} while (dur > Time.time);
		//if (stats.CooldownLength > 0) yield return new WaitForSeconds(stats.CooldownLength);	
		//vitalType.StopRegen = false;
		yield break;
	}

	/// <summary>
	/// Activates the ability, checking if the required vital is at a usable level. </summary>
	protected virtual IEnumerator ActivateAbility () {
		if (stats.StartupLength != 0)	yield return StartCoroutine(ActivateAbilityStartup());

		yield return StartCoroutine(ActivateAbilityDuration());

		if (stats.CooldownLength != 0) yield return StartCoroutine(ActivateAbilityCooldown());

		yield break;
	}
	
	public void Activate () {
		if (VitalType.CurValue < stats.Cost) return;
		if (stats.Cost != 0) {
			VitalType.CurValue -= stats.Cost;
			VitalType.StopRegen = true;
		}

		if (stats.DurationLength>0) StartCoroutine(ActivateAbility());
		else Middle ();
	}
	#endregion
}