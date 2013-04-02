/// <summary>
/// Stats for weapons. Contains the rigid values that dictate basic weapon effectiveness (damage and type, and what stats it benefits from)
/// </summary>
/*using UnityEngine;

[RequireComponent(typeof(BaseWeapon))]
public class WeaponStats : MonoBehaviour {
	public enum DamageType {
		Normal,
		Tearing,
		Piercing,
		Crushing,
		Spirit,
		Fire,
		Ice,
		Lightning,
		Earth
	}
	private enum PhysicalAtt {
		Normal,
		Poisoned,				//poison (build - once meter is full, causes gradual damage)
		Tearing,				//bleeding (build - once meter is full, causes burst damage)
		Piercing,				//armor piercing (passive - ignores armor)
		Crushing				//greater stun/knockback distance (passive)		
	}
	private enum SpiritualAtt {
		Spirit,
		Fire,					//ablaze (build - once meter is full, causes gradual damage for a short time that also spreads to nearby units)
		Ice,					//chilled (build - once meter is full, causes slowdown)
		Lightning,		
		Earth
	}
		
	#region Members
	public string name;
	public DamageType baseDamageType;
	public float baseDamage;
	public float scalingRatio;
	
	private DamageType _baseDamageType;
	private float _baseDamage;
	
	private float _scalingRatio;	
	private Attribute _scalingStat;
	private ModifiedStat _scalingBuff;
	
	private BaseWeapon _curWeapon;
	private Transform _user;
	private float _abilityBuff;
	#endregion
	
	#region Initializers
	public void SetValues () {
		baseDamageType = DamageType.Normal;
		baseDamage = 0;
		_scalingBuff = new ModifiedStat("default scaling buff", 1);
	}
	public void SetValues (Attribute scaleStat, float bScale) {
		baseDamageType = DamageType.Normal;
		baseDamage = 0;
		_scalingBuff = new ModifiedStat("shield buff", scaleStat, bScale);
	}
	public void SetValues (DamageType dt, float bDmg, Attribute scaleStat, float bScale) {
		baseDamageType = dt;
		baseDamage = bDmg;
		scalingRatio = bScale;
		_scalingStat = scaleStat;
		_scalingBuff = new ModifiedStat("scaling buff", scaleStat, bScale);
	}
	
	private void Awake () {
		_curWeapon = GetComponent<BaseWeapon>();
		_user = _curWeapon.User;
		SetValues();
	}
	#endregion
	
	#region Properties
	public Transform User 
	{
		get {return _user;}
		set {_user = value;}
	}
	public DamageType BaseDamageType 
	{
		get {return _baseDamageType;}
		set {_baseDamageType = value;}
	}
	public float BaseDamage
	{
		get {return _baseDamage;}
		set {_baseDamage = value;}
	}
	public ModifiedStat ScalingBuff
	{
		get {return _scalingBuff;}
		set {_scalingBuff = value;}
	}
	public float AbilityBuff
	{
		get {return _abilityBuff;}
		set {_abilityBuff = value;}
	}
	public float AdjustedDamage
	{
		get {return (baseDamage + _scalingBuff.AdjustedBaseValue) * _abilityBuff;}
	}
	#endregion
	
	
}

 */