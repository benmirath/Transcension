//using System;
//using UnityEngine;
//
////[System.Serializable] 
//public abstract class BaseActionStats {
//	#region Fields
//	[SerializeField] protected string name;												//Name of the attack
//	protected IVital vitalType;
//	[SerializeField] protected float cost;
//	[SerializeField] protected float speed;
//	
//	[SerializeField] protected float startupLength;										//length of time between when ability is activated, and when it takes effect.
//	[SerializeField] protected float durationLength;									//length of time that the ability remains in effect.
//	[SerializeField] protected float cooldownLength;									//length of time between when ability effect ends, and character can act again.
//	#endregion
//	
//	#region Initialization
//	public BaseActionStats (float start, float mid, float end, float c) {
//		startupLength=start;
//		durationLength=mid;
//		cooldownLength=end;
//		cost=c;
//	}
//	#endregion
//	
//	#region  Properties
//	public float Cost {
//		get {return cost;}
//		set {cost = value;}
//	}
//	public IVital VitalType {
//		get {return vitalType;}
//	}
//	public float Speed {
//		get {return speed;}
//		set {speed = value;}
//	}
//	public float StartupLength {
//		get {return startupLength;}
//		set {startupLength = value;}
//	}
//	public float DurationLength {
//		get {return durationLength;}
//		set {durationLength = value;}
//	}	
//	public float CooldownLength {
//		get {return cooldownLength;}
//		set {cooldownLength = value;}
//	}
//	#endregion Properties
//}