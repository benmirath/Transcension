using System;
using System.Collections;
using UnityEngine;

public interface IVital
{
		float MinValue 		{ get; }

		float MaxValue 		{ get; }

		float CurValue 		{ get; set; }

		float BuffValue 	{ get; set; }

		bool StopRegen 		{ get; set; }

		event Vital.VitalChangedHandler VitalChanged;

		IEnumerator StartRegen ();

//		IEnumerator Regen ();
}

[System.Serializable] public abstract class Vital : IVital
{
		public enum VitalName
		{
				Health
,
				Stamina
,
				Energy
,		
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
		#region Fields
		//Editor Fields
		protected VitalName name;
		protected BaseCharacter _user;

		[SerializeField] protected float maxValue;
		[SerializeField] protected float curValue;
		[SerializeField, ScalingStatAttribute(ScalingStatAttribute.ScalingType.Regen)] protected ScalingStat regenScalingStat;
		protected float buffValue;
		
		//Events - in place to constantly update relevant vital of any change
		public delegate void VitalChangedHandler (Vital vital);

		public event VitalChangedHandler VitalChanged;			//Coordinaton Fields
		protected float _prevValue;								//internal value from previous frame 
		
		protected float _regenTimer;
		protected bool 	stopRegen;
		#endregion
		
		#region Properties
		public float MinValue {
				get { 	return 0;}
		}

		public abstract float MaxValue { get; }

		public float BuffValue {
				get { 	return buffValue;}
				set {
						buffValue = value;
						CurValue = CurValue;
				}
		}
	//			public ScalingStat BaseScalingStat {
	//				get {return baseScalingStat;}
	//			}
	
		public float CurValue {
				get { return curValue;}
				set {
						float val = value;
						if (val > MaxValue)
								val = MaxValue;
						else if (val < MinValue)
								val = MinValue;

						if (val < curValue)
								_user.StartCoroutine (PauseRegen());
			
						curValue = val;
						if (VitalChanged != null)
								VitalChanged (this);
				}
		}

		public float RegenRate {
				get { return regenScalingStat.BaseValue;}
		}

		public bool StopRegen {
				get { return stopRegen;}
				set { stopRegen = value;}
		}
	#endregion
	
	#region Initialization
		public Vital ()
		{
				maxValue = 100;
				curValue = 100;
				buffValue = 0;
		}
	#endregion
	
	#region Setup
		public abstract void SetScaling (BaseCharacter user);
	#endregion
	
	#region Methods
		public abstract IEnumerator StartRegen ();

		private IEnumerator PauseRegen ()
		{
				stopRegen = true;
				yield return new WaitForSeconds (1);
				stopRegen = false;
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
		[SerializeField, ScalingStatAttribute(ScalingStatAttribute.ScalingType.Base)] private ScalingStat baseScalingStat;

		public override float MaxValue {
				get { return baseScalingStat.BaseValue + BuffValue;}
		}

		public override void SetScaling (BaseCharacter user)
		{
				Debug.Log ("Scaling is Set");
				_user = user;
				baseScalingStat.SetScaling (user);
				regenScalingStat.SetScaling (user);
				user.StartCoroutine (StartRegen());
				CurValue = MaxValue;
		}

	/// <summary>
	/// Starts the regen effect for this primar vital. </summary>
		public override IEnumerator StartRegen ()
		{
				while (!StopRegen) {
						float _regenTimer = Time.time;
			
						if (_prevValue <= CurValue) {
								if (_regenTimer == 0) {
										_regenTimer = Time.time + 0.75f;
										yield return null;
								}
				
								if (_regenTimer <= Time.time) {
										CurValue += (RegenRate * Time.deltaTime);
										_prevValue = CurValue;
										yield return null;
								} else
										yield return null;
						} else {
								_regenTimer = 0;
								_prevValue = CurValue;
								yield return null;
						}
				}
				yield return null;			
		}
}

[System.Serializable] public class StatusVital : Vital
{
		[SerializeField, Range(0, 100)] private float baseValue;

		public override float MaxValue {
				get { return baseValue + BuffValue;}
		}
	
		public override void SetScaling (BaseCharacter user)
		{
				_user = user;
				regenScalingStat.SetScaling (user);
				user.StartCoroutine (StartRegen());
				curValue = MinValue;
		}

		public override IEnumerator StartRegen ()
		{
				while (!StopRegen) {
						CurValue -= (RegenRate * Time.deltaTime);
						yield return null;
				}
				yield return null;
		}

		public IEnumerator ActivateEffect (float dur)
		{
				float t = Time.time + dur;
				while (Time.time < t) {
						Effect ();
						yield return null;
				}
				yield break;
		}

		private Action Effect;
}