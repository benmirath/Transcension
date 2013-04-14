/// <summary>
/// Base ability class, that acts as the basis of all in-game character abilities. Consists of a cost
/// value (specified in later derived classes), and three values that specify the duration of 
/// the startup, ability effect, and cooldown. </summary>
using System;
using System.Collections;
using UnityEngine;

public interface IAbility {
	void Activate ();
}

/// <summary>
/// The Base for all actions and abilities in the game. Consists of three distinct phases, a startup, duration in which 
/// the primary effect is active, and the cooldown. Each has an generic Action function for each phase. The finer points of functionality
/// are filled in by the ability's stat module.
/// </summary>
public class BaseAbility : MonoBehaviour {
	#region External Fields
	[SerializeField] protected AbilityStats stats;
	//private IVital vitalType;

	protected Action startupEffect;
	protected Action durationEffect;
	protected Action cooldownEffect;
	#endregion

	#region Internal Fields    //Action delegates that dictate the nature 
//	private Action effect;															//empty delegate that will hold the end effect of the created ability
//	private Action<Vector3> movementEffect;



	private ICharacter user;
	private CharacterController controller;													//Controller of character. Used for movement.
	#endregion

	#region Properties
	public AbilityStats Stats {
		get {return stats;}
	}
//	public virtual Vector3 Target {				//will indicate the desired direction of the ability, plugging in either user's move or look direction (movement or attacks respectively) 
//		get;
//	}


//	public Action Effect {
//		get {return effect;}
//		set {effect = value;}
//	}
//	public Action<Vector3> MovementEffect {
//		get {return movementEffect;}
//	}
//	public IVital VitalType {
//		get {return vitalType;}
//		set {vitalType = value;}
//	}
//	
	public ICharacter User {
		get {return user;}
		set {user = value;}
	}
	public CharacterController Controller {
		get {return controller;}
		set {controller = value;}
	}
	#endregion
	
	#region Initialization
	protected virtual void Awake () {
		user = GetComponent<BaseCharacter>();
		controller = GetComponent<CharacterController>();

		name = "default";
		stats.Cost = 0;
		//stats.Speed = 0;
		stats.StartupLength = 0;
		stats.DurationLength = 0;
		stats.CooldownLength = 0;
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

	#endregion Initialization
	protected virtual IEnumerator ActivateAbilityStartup () {
		float dur = Time.time + stats.StartupLength;											//Sets length of time at which ability's duration will be finished
		do {
			startupEffect();
			yield return null;
		} while (dur > Time.time);
		//if (stats.StartupLength > 0) yield return new WaitForSeconds(stats.StartupLength);				//checks if ability has a startup time, and pauses script for appropriate length
		yield break;
	}
	protected virtual IEnumerator ActivateAbilityDuration () {
		float dur = Time.time + stats.DurationLength;											//Sets length of time at which ability's duration will be finished

		do {
			durationEffect();
			yield return null;
		} while (dur > Time.time);
		yield break;
	}
	protected virtual IEnumerator ActivateAbilityCooldown () {
		float dur = Time.time + stats.CooldownLength;
		do {
			cooldownEffect();
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

		if (stats.DurationLength != 0) yield return StartCoroutine(ActivateAbilityDuration());
		else durationEffect();
		
		if (stats.CooldownLength != 0) yield return StartCoroutine(ActivateAbilityCooldown());

		yield break;
	}
	
	public void Activate () {
		if (stats.VitalType.CurValue < stats.Cost) return;
		if (stats.Cost != 0) {
			stats.VitalType.CurValue -= stats.Cost;
			stats.VitalType.StopRegen = true;
		}
		StartCoroutine(ActivateAbility());
	}
}