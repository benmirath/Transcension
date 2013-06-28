using System;
using System.Collections;
using UnityEngine;

public interface IVital
{
	string Name 		{ get; }
	float MinValue 		{ get; }
	float MaxValue 		{ get; }
	float CurValue 		{ get; set; }
	float BuffValue 	{ get; set; }
	ScalingStat RegenScaling { get; set;}
	bool StopRegen 		{ get; set; }

	Action MinValueEffect { get; set;}
	Action MaxValueEffect { get; set;}

	event Vital.VitalChangedHandler VitalChanged;

	void SetScaling (BaseCharacter user);
	IEnumerator StartRegen ();
	IEnumerator PauseRegen ();
}

[System.Serializable] 
/// <summary>
/// The Vital Class. A shifting min-max stat, used to represent shifting statistics like health, stamina and other status conditions.
/// Will also have option delegates that called when the min or max region is used, which can be filled in by the different stats for different mechanical effects.
/// </summary>
public abstract class Vital : IVital
{
	#region Properties
	public enum PrimaryVitalName
	{
		Health
,
		Stamina
,
		Energy
,		
	}

	public enum StatusVitalName
	{
		Stun
		,
		Knockback
		,
		Poison
		,
		Bleed
		,
		Burn
		,
		Freeze
		,
		Shock
	}
		//Inspector Fields
	public abstract string Name {
		get;
	}

	[SerializeField] protected float 	minValue;
	public virtual float			MinValue { 
		get { return minValue; } 
	}
	protected Action			minValueEffect;
	public Action 				MinValueEffect {
		get { return minValueEffect;}
		set { minValueEffect = value;}
	}
	
	[SerializeField] protected float 	maxValue;
	public virtual float 			MaxValue { 
		get { return maxValue; } 
	}
	protected Action 			maxValueEffect;
	public Action 				MaxValueEffect {
		get { return maxValueEffect;}
		set { maxValueEffect = value;}
	}
	
	[SerializeField] protected float 	curValue;
	public float 				CurValue {
		get { return curValue; }
		set {
			float val = value;

			if (val > MaxValue) {
//				Debug.LogError ("Max value of "+Name+" reached!");
				if (maxValueEffect != null) {
//					Debug.LogError ("Vital "+Name+"'s max value effect has been activated");
					maxValueEffect ();
				}
//				else Debug.LogError ("no max value effect for "+Name);
				val = MaxValue;
			}

			else if (val < MinValue) {
				if (minValueEffect != null)
					minValueEffect ();
				val = MinValue;
			}

			curValue = val;

			if (VitalChanged != null)
				VitalChanged (this);
		}
	}


	[SerializeField] protected ScalingStat 	regenScaling;
	public ScalingStat 			RegenScaling {
		get { return regenScaling; }
		set {regenScaling = value;}
	}

	protected float buffValue;





	public delegate void VitalChangedHandler (Vital vital);

	public event VitalChangedHandler VitalChanged;
		//Internal Fields
	private BaseCharacter _user;
	protected float _prevValue;
	protected float _regenTimer;
	protected bool _stopRegen;

	public float BuffValue {
		get { return buffValue;}
		set {
			buffValue = value;
			CurValue = CurValue;
		}
	}

	public float RegenRate {
		get { return regenScaling.BaseValue;}
	}

	public bool StopRegen {
		get { return _stopRegen;}
		set { _stopRegen = value;}
	}
	#endregion

	#region Setup
	public virtual void SetScaling (BaseCharacter user)
	{
		_user = user;
	}
	#endregion
	
	#region Methods
	public abstract IEnumerator StartRegen ();

	public IEnumerator PauseRegen ()
	{
		if (_stopRegen == false) {
			_stopRegen = true;
			yield return new WaitForSeconds (1.5f);
			_stopRegen = false;
			yield break;
		}
		else
			yield break;
	}
	#endregion
}

[System.Serializable] public class PrimaryVital : Vital
{
	[SerializeField] protected PrimaryVitalName name;
	[SerializeField] private ScalingStat baseScaling;

	public override string Name { get { return name.ToString(); } }
	public override float MaxValue {
		get {
			maxValue = baseScaling.BaseValue + BuffValue;
			return maxValue;
		}
	}

	public override void SetScaling (BaseCharacter user)
	{
		base.SetScaling (user);
		Debug.Log ("Scaling is Set");
		baseScaling.SetScaling (user);
		regenScaling.SetScaling (user);
		user.StartCoroutine (StartRegen());
		CurValue = MaxValue;
	}


	public override IEnumerator StartRegen () {
		while (true) {
			if (_stopRegen != true) {
				CurValue += (RegenRate * Time.deltaTime);	//apply regen rate at steady time increments

			}
			yield return null;
		}
		Debug.LogError("Outside of regen loop");
		yield return null;
	}
}

[System.Serializable] public class StatusVital : Vital
{
	[SerializeField] protected StatusVitalName name;
	[SerializeField, Range(0, 100)] private float baseValue;

	public override string Name { get { return name.ToString(); } }
	public override float MaxValue {
		get { return baseValue + BuffValue;}
	}
	public override float MinValue {
		get { return 0; }
	}

	public override void SetScaling (BaseCharacter user)
	{
		//_user = user;
		base.SetScaling (user);
		regenScaling.SetScaling (user);
		user.StartCoroutine (StartRegen());
		curValue = MinValue;
	}

	public override IEnumerator StartRegen () {
		while (true) {
			if (_stopRegen == true) 
				Debug.LogError ("regenerating paused");

			else {
//				Debug.LogWarning("regenerating...");
				CurValue -= (RegenRate * Time.deltaTime);	//apply regen rate at steady time increments

			}
			yield return null;
		}
		Debug.LogError("Outside of regen loop");
		yield return null;
	}
}