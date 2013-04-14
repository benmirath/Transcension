using System;
using System.Collections;
using UnityEngine;

public class AbilityStats {
	#region Fields
	[SerializeField] protected string name;												//Name of the attack
	[SerializeField] protected float cost;

	[SerializeField] protected float startupLength;										//length of time between when ability is activated, and when it takes effect.
	[SerializeField] protected float durationLength;									//length of time that the ability remains in effect.
	[SerializeField] protected float cooldownLength;									//length of time between when ability effect ends, and character can act again.
	#endregion
	
	#region Initialization
	public AbilityStats (float start, float mid, float end, float c) {
		startupLength=start;
		durationLength=mid;
		cooldownLength=end;
		cost=c;
	}
	#endregion
	
	#region  Properties
	public float Cost {
		get {return cost;}
		set {cost = value;}
	}
	public float StartupLength {
		get {return startupLength;}
		set {startupLength = value;}
	}
	public float DurationLength {
		get {return durationLength;}
		set {durationLength = value;}
	}	
	public float CooldownLength {
		get {return cooldownLength;}
		set {cooldownLength = value;}
	}
	#endregion Properties
}


public class MovementAbilityStats : AbilityStats {
	[SerializeField] protected float speed;
	public float Speed {
		get {return speed;}
		set {speed = value;}
	}

	public MovementAbilityStats (float start, float mid, float end, float c, float sp) : base (start, mid, end, c) {
		speed=sp;
	}
}


/// <summary>
/// Holds all the statistical information for this attack, both for identification and functioning. </summary>
[System.Serializable] public class AttackStats : MovementAbilityStats {
	#region Fields
	[SerializeField] protected Vector3 range;										//vector3 that determines the proprotion of the attack
	[SerializeField] protected float damageModifier;								//amount that this attack modifies the base weapon's damage range. 
	[SerializeField] protected float attackStrength;								//raw force behind attack, used for determining effectiveness of blocks and calculating hit stun
	[SerializeField] protected float comboWindow;									//the window of oppotunity to activate a followup attack (combo)
	#endregion Fields
	
	#region Properties
	public Vector3 Range {
		get {return range;}
		set {range = value;}
	}
	public float DamageModifier {
		get {return damageModifier;}
		set {damageModifier = value;}
	}
	public float AttackStrength {
		get {return attackStrength;}
		set {attackStrength = value;}
	}
	public float ComboWindow {
		get {return comboWindow;}
		set {comboWindow = value;}
	}
	#endregion Properties
	
	#region Initialization
	public AttackStats (float start, float mid, float end, float c, float sp, Vector3 rn, float dMod, float atkStr, float combo) : base (start, mid, end, c, sp) {
		range=rn;
		damageModifier=dMod;
		attackStrength=atkStr;
		comboWindow=combo;
	}
	public AttackStats () {}
	#endregion Initialization
}