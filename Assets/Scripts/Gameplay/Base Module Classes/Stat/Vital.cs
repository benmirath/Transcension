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
	bool StopRegen 		{ get; set; }

	Action MinValueEffect { get; set;}
	Action MaxValueEffect { get; set;}

	event Vital.VitalChangedHandler VitalChanged;

	void SetScaling (BaseCharacter user);
	IEnumerator StartRegen ();
	IEnumerator PauseRegen ();
//		IEnumerator Regen ();
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

	[SerializeField] protected float 	minValue;
	public virtual float	MinValue { get { return minValue; } }
	protected Action		minValueEffect;

	public Action MinValueEffect {
		get { return minValueEffect;}
		set { minValueEffect = value;}
	}
	
	[SerializeField] protected float 	maxValue;
	public virtual float 	MaxValue { get { return maxValue; } }
	protected Action 		maxValueEffect;

	public Action MaxValueEffect {
		get { return maxValueEffect;}
		set { maxValueEffect = value;}
	}

	[SerializeField] protected float curValue;

	public abstract string Name {
		get;
	}

	public float CurValue {
		get { return curValue; }
		set {
			float val = value;
				
			if (val > MaxValue) {
				val = MaxValue;
				if (maxValueEffect != null)
					maxValueEffect ();
			}

			else if (val < MinValue) {
				val = MinValue;
				if (minValueEffect != null)
					minValueEffect ();
			}

			curValue = val;

			if (VitalChanged != null)
				VitalChanged (this);
		}
	}

	[SerializeField] protected ScalingStat regenScaling;
	protected float buffValue;
		//Events - in place to constantly update relevant vital of any change
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
	//			public ScalingStat BaseScalingStat {
	//				get {return baseScalingStat;}
	//			}

	public float RegenRate {
		get { return regenScaling.BaseValue;}
	}

	public bool StopRegen {
		get { return _stopRegen;}
		set { _stopRegen = value;}
	}
	#endregion
//	
//	#region Initialization
//	public Vital ()
//	{
//		curValue = 100;
//		buffValue = 0;
//	}
//	#endregion
//	
	#region Setup
	public virtual void SetScaling (BaseCharacter user)
	{
		_user = user;
	}
	#endregion
	
	#region Methods
//	public abstract IEnumerator StartRegen ();

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
//		public IEnumerator Regen ()
//		{
//				float timer;
//				float prevValue;
//				
//				//begins the regen process
//				while (true) {
//						
//						//checks if the 
//						if (prevValue == CurValue) {
//								timer = Time.time;
//								if (timer) {
//										
//								}
//						}
//				}
//		}
	#endregion
}

[System.Serializable] public class PrimaryVital : Vital
{
	//[SerializeField, ScalingStatAttribute(ScalingStatAttribute.ScalingType.Base)] 
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
			if (_stopRegen == true) 
				Debug.LogError ("regenerating paused");

			else {
				Debug.LogWarning("regenerating...");
				CurValue += (RegenRate * Time.deltaTime);	//apply regen rate at steady time increments

			}
			yield return null;
		}
		Debug.LogError("Outside of regen loop");
		yield return null;
	}

	/// <summary>
	/// Starts the regen effect for this primar vital. </summary>
//	public override IEnumerator StartRegen ()
//	{
//		while (!StopRegen) {
//			float _regenTimer = Time.time;
//			
//			if (_prevValue <= CurValue) {
//				if (_regenTimer == 0) {
//					_regenTimer = Time.time + 0.75f;
//					yield return null;
//				}
//				
//				if (_regenTimer <= Time.time) {
//					CurValue += (RegenRate * Time.deltaTime);
//					_prevValue = CurValue;
//					yield return null;
//				} else
//					yield return null;
//			} else {
//				_regenTimer = 0;
//				_prevValue = CurValue;
//				yield return null;
//			}
//		}
//		yield return null;			
//	}
}

[System.Serializable] public class StatusVital : Vital
{
	[SerializeField] protected StatusVitalName name;
	[SerializeField, Range(0, 100)] private float baseValue;

	public override string Name { get { return name.ToString(); } }
	public override float MaxValue {
		get { return baseValue + BuffValue;}
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
				Debug.LogWarning("regenerating...");
				CurValue -= (RegenRate * Time.deltaTime);	//apply regen rate at steady time increments

			}
			yield return null;
		}
		Debug.LogError("Outside of regen loop");
		yield return null;
	}


//	public override IEnumerator StartRegen ()
//	{
//		while (!StopRegen) {
//			CurValue -= (RegenRate * Time.deltaTime);
//			yield return null;
//		}
//		yield return null;
//	}
//
//	public IEnumerator ActivateEffect (float dur)
//	{
//		float t = Time.time + dur;
//		while (Time.time < t) {
//			Effect ();
//			yield return null;
//		}
//		yield break;
//	}
//
//	private Action Effect;
}