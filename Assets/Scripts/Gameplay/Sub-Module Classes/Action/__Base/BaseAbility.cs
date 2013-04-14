/// <summary>
/// Base ability class, that acts as the basis of all in-game character abilities. Consists of a cost
/// value (specified in later derived classes), and three values that specify the duration of 
/// the startup, ability effect, and cooldown. </summary>
using System;
using System.Collections;
using UnityEngine;

public interface IAction {
	void Activate ();
}

public class BaseAction : MonoBehaviour {
	#region Fields
	[SerializeField] private AbilityStats stats;
	private Action effect;																//empty delegate that will hold the end effect of the created ability
	private Action<Vector3> movementEffect;


	private IVital _vitalType;

	private ICharacter _char;
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
	public Action<Vector3> MovementEffect {
		get {return movementEffect;}
	}
	public IVital VitalType {
		get {return _vitalType;}
		set {_vitalType = value;}
	}
	
	public ICharacter Char {
		get {return _char;}
		set {_char = value;}
	}
	public CharacterController Controller {
		get {return _body;}
		set {_body = value;}
	}
	#endregion
	
	#region Initialization
	protected virtual void Awake () {
		_char = GetComponent<BaseCharacter>();
		_body = GetComponent<CharacterController>();

		name = "default";
		stats.Cost = 0;
		//stats.Speed = 0;
		stats.StartupLength = 0;
		stats.DurationLength = 0;
		stats.CooldownLength = 0;
		SetValues();
	}

	/// <summary>
	/// Will be used to specify values in derived classes to tailer functionality of abilities. </summary>
	public virtual void SetValues () {

	}
	public virtual void SetValues (float start, float mid, float end, IVital vital, float cost) {
		stats=new AbilityStats(start, mid, end, cost);
		_vitalType=vital;
	}
	public virtual void SetMovementValues (float start, float mid, float end, IVital vital, float cost, Action<Vector3> effect, float speed) {
		stats=new AbilityStats(start, mid, end, cost);
	}

	#endregion Initialization
	protected virtual IEnumerator ActivateAbilityStartup () {
		if (stats.StartupLength > 0) yield return new WaitForSeconds(stats.StartupLength);				//checks if ability has a startup time, and pauses script for appropriate length
		yield break;
	}
	protected virtual IEnumerator ActivateAbilityDuration () {
		float curDuration = Time.time + stats.DurationLength;											//Sets length of time at which ability's duration will be finished

		do {
			Effect();																		
			yield return null;
		} while (curDuration > Time.time);
		yield break;
	}
	protected virtual IEnumerator ActivateAbilityCooldown () {
		if (stats.CooldownLength > 0) yield return new WaitForSeconds(stats.CooldownLength);	
		_vitalType.StopRegen = false;
		yield break;
	}

	/// <summary>
	/// Activates the ability, checking if the required vital is at a usable level. </summary>
	protected virtual IEnumerator ActivateAbility () {
		if (stats.StartupLength != 0)	yield return StartCoroutine(ActivateAbilityStartup());

		if (stats.DurationLength != 0) yield return StartCoroutine(ActivateAbilityDuration());
		else Effect();
		
		if (stats.CooldownLength != 0) yield return StartCoroutine(ActivateAbilityCooldown());
		yield break;
	}
	
	public void Activate () {
		if (_vitalType.CurValue < stats.Cost) return;
		if (stats.Cost != 0) {
			_vitalType.CurValue -= stats.Cost;
			_vitalType.StopRegen = true;
		}
		StartCoroutine(ActivateAbility());
	}
}