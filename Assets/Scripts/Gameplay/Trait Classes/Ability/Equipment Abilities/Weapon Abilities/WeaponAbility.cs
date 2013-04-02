/// <summary>
/// Weapon ability that acts as the base of all weapon-based attack abilities, which include stamina-consuming combo
/// and finisher attacks, and energy-consuming special attacks. Adds a hitbox and various attack stats. </summary>
using UnityEngine;
using System;
using System.Collections;

public abstract class WeaponAbility : BaseAbility {	
	#region Fields
	//Inspector Fields
	[SerializeField] protected AttackStats stats;
	
	//Internal Fields
	protected BaseEquipment _curWeapon;																//Weapon that the attack derives it's core stats from
	protected BoxCollider _hitBox;
	#endregion Fields
	
	#region Properties
	new public AttackStats Stats {
		get {return stats;}
	}
	public float RealDamage	{																	//Final damage calculation, combining the weapon's base damage, the buffs received from scaling, as well as the multiplier of the specific attack's strength{
		get {return _curWeapon.Stats.AdjustedBaseDamage * stats.DamageModifier;}
	}	
	public BaseEquipment CurWeapon {
		get {return _curWeapon;}
		set {_curWeapon = value;}
	}
	public BoxCollider HitBox {
		get {return _hitBox;}
		set {_hitBox = value;}
	}
	#endregion Properties
	
	#region Initialization
	protected override void Awake ()
	{
		base.Awake ();
		Char = transform.parent.GetComponent<BaseCharacter>();
		Body = transform.parent.GetComponent<CharacterController>();
		_curWeapon = GetComponent<BaseEquipment>();
		_hitBox = GetComponent<BoxCollider>();
	}
	protected virtual void Start ()
	{
		//base.Start ();
		SetHitBox(false);
	}
	/// <summary>
	/// Sets default values for the attack. </summary>
	public override void SetValues ()
	{
		name = "default attack";
		Stats.Speed = 0;
		Stats.Cost = 0;
		Stats.StartupLength = 0;
		Stats.DurationLength = 0;
		Stats.CooldownLength = 0;
	}
/*	/// <summary>
	/// Sets the values of the ability based on the attack stat values which are determined in the Unity editor. </summary>
	/// <param name='attack'> The attack stat of the ability. </param>
	public void SetValues (AttackStat attack)
	{
		attackStats = attack;
		name = attack.Name;
		Stats.Speed = attack.Speed;
		cost = attack.Cost;
		
		startupLength = attack.StartupLength;
		durationLength = attack.DurationLength;
		cooldownLength = attack.CooldownLength;
	}*/
	#endregion Initialization

	/// <summary>
	/// Sets whether the hitbox is currently active. </summary>
	/// <param name='active'> If set to 
	/// <c>true</c> active. 
	/// <c>false</c> inactive. </param>
	protected virtual void SetHitBox (bool active)												//Used to turn the attack hitbox on or off, at the beginning and ending of the attack respectively
	{
		if (active == true)
		{
			_hitBox.enabled = true;
			_curWeapon.Stats.AbilityBuff = stats.DamageModifier;
			HitBox.size = stats.Range;
		}
		if (active == false)
		{
			_hitBox.enabled = false;			
			_curWeapon.Stats.AbilityBuff = 1;
			HitBox.size = Vector3.zero;
		}
	}	
	
	/// <summary>
	/// This is where the effect of an attack upon a hit is found. This will contain the logic of any "succesful" attack, whether landing
	/// a clean strike, or whiffing it from a block or dodge. </summary>
	/// <param name='hit'> The hit registered by the hitbox. </param>
	protected virtual void OnTriggerEnter (Collider hit)										//The effect of the attack when a potential target enters the attack hitbox
	{		
		if (hit.GetComponent<BaseEnemy>()) 
		{
			BaseEnemy target = hit.GetComponent<BaseEnemy>();
			Vector3 hitDir;
			
			if (target.IsEvading) return;														//checks to see if the attack is evaded, cancels effect of attack
			
			if (target.IsDefending)																//checks to see if the attack is blocked
			{
				if(Vector3.Dot(target.transform.up, _curWeapon.User.Coordinates.position) > 0.75f)
				{
					Debug.Log ("Attack blocked!");
					hitDir = (target.transform.position - _curWeapon.User.Coordinates.position).normalized;
					
					StopCoroutine ("Activate");
					Char.ApplyAttackRecoil(hitDir);
				}
			}
			
			hitDir = (target.transform.position - _curWeapon.User.Coordinates.position).normalized;			
			target.ApplyDamage(_curWeapon.Stats.AdjustedBaseDamage);
			Debug.LogError ("the attack strength of the attack is :" + stats.AttackStrength);
			target.ApplyHitStun(stats.AttackStrength, hitDir);
			Debug.Log ("You landed a hit, and dealt " + _curWeapon.Stats.AdjustedBaseDamage + " damage");
		}
	}
	
	protected override IEnumerator ActivateAbilityDuration ()
	{
		SetHitBox(true);
		float curDuration = Time.time + Stats.DurationLength;												
		do {
			Effect();																		
			yield return null;
		} while (curDuration > Time.time);
		SetHitBox(false);
		yield break;
	}
	
	public abstract class SpecialAttack : WeaponAbility
	{
		protected override void Start ()
		{
			base.Start ();
			VitalType = Char.CharStats.Energy;
		}
		protected abstract void SpecialEffect ();	
	}

	public abstract class StandardAttack : WeaponAbility
	{
		protected override void Start ()
		{
			base.Start ();
			VitalType = Char.CharStats.Stamina;		
		}

		protected abstract void AttackEffect ();

		//Set attack type effects in the inherited classes
		public class FinisherAttack : StandardAttack
		{
			protected override void AttackEffect ()
			{
				Char.Body.Move(Char.Coordinates.up * Stats.Speed * Time.deltaTime);
			}
			//turn off followup
		}
		public class ComboAttack : FinisherAttack
		{
			protected WeaponAbility followupAttack;

			protected override IEnumerator ActivateAbilityDuration ()
			{
				CurWeapon.FollowupAvailable = false;	
				float curDuration = Time.time + Stats.DurationLength;												
				SetHitBox(true);
				
				do {
					Effect();					
					if ((curDuration - Time.time) <= stats.ComboWindow && CurWeapon.FollowupAvailable == false) CurWeapon.FollowupAvailable = true;
					yield return null;
				} while (curDuration > Time.time);
			
				CurWeapon.FollowupAvailable = false;
				SetHitBox(false);	
			}

			public class ComboAttackStats : AttackStats
			{

			}
		}
		public class RangedAttack : StandardAttack
		{
			protected override void AttackEffect ()
			{
				//ranged attack logic will go here
			}
		}
	}

	/// <summary>
	/// Holds all the statistical information for this attack, both for identification and functioning. </summary>
	[System.Serializable] public class AttackStats : BaseAbility.AbilityStats
	{
		#region Fields
		[SerializeField] protected Vector3 range;										//vector3 that determines the proprotion of the attack
		[SerializeField] protected float damageModifier;								//amount that this attack modifies the base weapon's damage range. 
		[SerializeField] protected float attackStrength;								//raw force behind attack, used for determining effectiveness of blocks and calculating hit stun
		[SerializeField] protected float comboWindow;									//the window of oppotunity to activate a followup attack (combo)
//		[SerializeField] private float speed;										//if the attack moves the character, this is by how much
//		[SerializeField] private float cost;										//the cost of the attack in its relevant resource
//		[SerializeField] private float startupLength;								//the length of time for the startup to complete (The pull back)
//		[SerializeField] private float durationLength;								//the length of time for the duration to complete (the actual swing)
//		[SerializeField] private float cooldownLength;								//the length of time for the cooldown to complete (the follow through)
		#endregion Fields
		
		#region Properties
/*		public string Name
		{
			get {return name;}
		}*/
		public Vector3 Range
		{
			get {return range;}
			set {range = value;}
		}
		public float DamageModifier
		{
			get {return damageModifier;}
			set {damageModifier = value;}
		}
		public float AttackStrength
		{
			get {return attackStrength;}
			set {attackStrength = value;}
		}
/*		public float Speed
		{
			get {return speed;}
			set {speed = value;}
		}
		public float Cost
		{
			get {return cost;}
			set {cost = value;}
		}*/
		public float ComboWindow
		{
			get {return comboWindow;}
			set {comboWindow = value;}
		}
/*		public float StartupLength
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
		}*/
		#endregion Properties
		
		#region Initialization
		public AttackStats () {}
		#endregion Initialization
	}
}