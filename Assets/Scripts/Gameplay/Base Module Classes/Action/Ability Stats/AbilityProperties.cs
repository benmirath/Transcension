using System;
using System.Collections;
using UnityEngine;
using System.Linq.Expressions;

public interface IAbility
{
	Action EnterAbility { get;}
	Action ActiveAbility { get;}
	Action ExitAbility { get;}
	float EnterLength {
		get;
		set;
	}
	float ActiveLength {
		get;
		set;
	}
	float ExitLength {
		get;
		set;
	}
}
public interface IAttack
{

}
/// <summary>
/// The distinctive properties for the Ability Base. Contains variables relating to coordinating and determining its functionality. </summary>
[System.Serializable]public abstract class AbilityProperties
{
//	public enum BaseAbilityType 
//	{
//		StandardAction,				//Uses Stamina, or is free
//		SpecialAction,				//Uses Energy
//		ClassAction,				//Uses "Class Energy" (still looking into viability)
//	}

	#region Properties
	private ICharacter _user;
	private IInput _userInput;
	private IVital _userVital;
	protected Action enterAbility;
	protected Action activeAbility;
	protected Action exitAbility;
	[SerializeField] protected Vital.PrimaryVitalName vitalType;
	[SerializeField] protected float cost;
	[SerializeField] protected float enterLength;//length of time between when ability is activated, and when it takes effect.
	[SerializeField] protected float activeLength;//length of time that the ability remains in effect.
	[SerializeField] protected float exitLength;//length of time between when ability effect ends, and character can act again.
//	public string name {
//		get {
//			Expression _name = this;
//			_name
//
//
//		}
//	}
	#endregion
	
	#region Initialization
	protected virtual void Awake ()
	{
		Debug.Log ("Ability Created");
	}
	/// <summary>
	/// Sets the primary values and variables required for the ability.
	/// </summary>
	/// <param name="user">User.</param>
	public virtual void SetValue (ICharacter user)
	{
		_user = user;
		_userInput = _user.CharInput;

		if (_user == null)
			Debug.LogError ("user is null");
		if (_user.CharStats == null)
			Debug.LogError ("user stats are null");


		_userVital = _user.CharStats.CharVitals.Find (i => i.Name == vitalType.ToString());

		if (cost > 0) {
			enterAbility += delegate {
				_userVital.StopRegen = true;
			};
			exitAbility += delegate {
				_userVital.StopRegen = false;
			};
		}
	}
	#endregion

	#region Effects

	public virtual IEnumerator ActivateDurationalAbility () {
		float timer;

		if (User.CharState.CheckAbilityVital(this)) {
			_userVital.StopRegen = true;

			if (enterLength > 0) {
				timer = Time.time + enterLength;
				while (timer > Time.time) {
					enterAbility ();
					yield return null;
				}
			} else
				enterAbility ();
			if (activeLength > 0) {
				timer = Time.time + activeLength;
				while (timer > Time.time) {
					activeAbility ();
					yield return null;
				}
			} else
				activeAbility ();
			if (exitLength > 0) {
				timer = Time.time + exitLength;
				while (timer > Time.time) {
					exitAbility ();
					yield return null;
				}
			} else
				exitAbility ();
		}
		if (_userVital.StopRegen = true)
			_userVital.StopRegen = false;
		yield break;
	}
	public IEnumerator ActivateSustainedAbility (IInputAction _input) {
		float timer;

		if (User.CharState.CheckAbilityVital(this)) {
			_userVital.StopRegen = true;

			if (enterLength > 0) {
				timer = Time.time + enterLength;
				while (timer > Time.time) {
					enterAbility ();
					yield return null;
				}
			} else
				enterAbility ();

		
			while (_input.Active) {
				if (User.CharState.CheckAbilityVital (this)) {
					activeAbility ();
					yield return null;
				} 
				else 
					break;
			}

			if (exitLength > 0) {
				timer = Time.time + exitLength;
				while (timer > Time.time) {
					exitAbility ();
					yield return null;
				}
			} else
				exitAbility ();
		}
		if (_userVital.StopRegen == true)
			_userVital.StopRegen = false;
		yield break;
	}
	#endregion
	
	#region  Propertiess
//	protected string name;
//	public abstract string name { get;}
//		get {}
//	}

	public ICharacter User { get { return _user; } }

	public IInput CharInput { get { return _userInput; } }

	public IVital UserVital { get { return _userVital; } }

	public Vital.PrimaryVitalName VitalType { get { return vitalType; } }

	public float Cost {
		get { return cost;}
		set { cost = value;}
	}
	public Action EnterAbility {
		get { return enterAbility;}
	}
	public Action ActiveAbility {
		get { return activeAbility;}
	}
	public Action ExitAbility {
		get { return exitAbility;}
	}


	//How long each phase of the ability lasts. If duration is 0, then it will be an updating ability.
	public float EnterLength {
		get { return enterLength;}
		set { enterLength = value;}
	}

	public float ActiveLength {
		get { return activeLength;}
		set { activeLength = value;}
	}

	public float ExitLength {
		get { return exitLength;}
		set { exitLength = value;}
	}
	#endregion Properties
}
/// <summary> 
/// Movement properties for abilities. Many (anything integrating movement into the action) will derive from this property. </summary>
[System.Serializable]public class MovementProperties : AbilityProperties
{
	#region Properties
	public enum MovementType {
		BurstMove,
		ConstantMove,
		BurstStrafe,
		ConstantStrafe,
		FreeAim,

	}
	public enum MovementPropertyType
	{
		Aim
,
		Walk
,
		Strafe
,
		Run
,
		Dodge
,
		Stun
,
		Knockback
	}

	[SerializeField] private MovementPropertyType movementType;
	[SerializeField] protected float enterMoveSpeed;
	[SerializeField] protected float activeMoveSpeed;
	[SerializeField] protected float exitMoveSpeed;
	[SerializeField] protected float lookSpeed;
	[SerializeField] protected Vector3 direction;

//	public override string name { get { return movementType.ToString (); } }
	public Vector3 Direction {
		get { return direction;}
		set { direction = value;}
	}

	public float EnterMoveSpeed {
		get { return enterMoveSpeed;}
	}

	public float ActiveMoveSpeed {
		get { return activeMoveSpeed;}
	}

	public float ExitMoveSpeed {
		get { return exitMoveSpeed;}
	}

	public float TurnSpeed {
		get { return TurnSpeed;}
	}
	#endregion

	#region Initialization
	public override void SetValue (ICharacter user)
	{
//		Debug.Log ("AbilityProperty: Setting Values");
		base.SetValue (user);
		switch (movementType) {
		case MovementPropertyType.Aim:
			enterAbility += Aim;
			activeAbility += Aim;
			exitAbility += Aim;
			break;

		case MovementPropertyType.Walk:
			activeAbility += delegate {
				ControlledMove (activeMoveSpeed);
				Turn ();
			};
			break;

		case MovementPropertyType.Strafe:
			activeAbility += delegate {
				ControlledMove (activeMoveSpeed);
				Aim ();
			};
			break;

		//these last two will likely add some extra control logic to fine tune momentum.
		case MovementPropertyType.Run:
			enterAbility += delegate {
				//will be the code to accelerate the character from walking to running speed.
				ControlledMove (enterMoveSpeed);
				Turn ();
				UserVital.StopRegen = true;
			};
			activeAbility += delegate {
				ControlledMove (activeMoveSpeed);
				Turn ();
				//if (UserVital.CurValue < cost) user.CharState.currentState = BaseCharacterStateModule.CharacterActions.Idle;
				UserVital.CurValue -= (cost * Time.deltaTime);
			};
			exitAbility += delegate {
				ControlledMove (exitMoveSpeed);
				Turn ();
				UserVital.StopRegen = false;
			};
			break;

		case MovementPropertyType.Dodge:
			enterAbility += delegate {
				direction = CharInput.MoveDir;
//				BurstMove (enterMoveSpeed);
//				Turn ();
			};
			activeAbility += delegate {
				BurstMove (activeMoveSpeed);
				Turn ();
			};
			exitAbility += delegate {
				BurstMove (exitMoveSpeed);
				Turn ();
			};
			break;

		case MovementPropertyType.Stun:
			enterAbility += delegate {
				Debug.LogError ("STUNNED");
				UserVital.RegenScaling.ScalingRatio *= 2;
				ExternalMove (enterMoveSpeed);
			};
			activeAbility += delegate {
				ExternalMove (activeMoveSpeed);
			};
			exitAbility += delegate {
				UserVital.RegenScaling.ScalingRatio /= 2;
			};

			break;


		default:
			break;
		}
		if (User == null)
			Debug.Log ("Ability Set: User is Null");
		if (CharInput == null)
			Debug.Log ("Ability Set: Input is Null");
	}
	#endregion
	
	#region Effects
	protected void ControlledMove (float speed)
	{
		User.Controller.Move (speed * CharInput.MoveDir.normalized * Time.deltaTime);		
	}
	protected void ExternalMove (float speed) {

		User.Controller.Move (speed * User.CharActions.CharStatus.KnockDir * Time.deltaTime);		
	}

	protected void BurstMove (float speed)
	{
		User.Controller.Move (speed * direction.normalized * Time.deltaTime);		
		//will be worked on later, will be used for movements that are a quick acceleration that degrades over time
	}

	protected void Turn ()
	{
		Vector3 dir = CharInput.MoveDir;
		if (dir == Vector3.zero)
			return;
		
		float aim = Mathf.Atan2 (dir.x, dir.z) * Mathf.Rad2Deg;						//holds direction the character should be facing
		User.Coordinates.rotation = Quaternion.Euler (0, aim, 0);
	}

	protected void Aim ()
	{
		Debug.LogWarning ("aim activating");
		//User.Coordinates.rotation = Quaternion.Euler (CharInput.LookDir);

		Vector3 target = CharInput.LookDir;
		float angleX = target.x - User.Coordinates.position.x;
		float angleZ = target.z - User.Coordinates.position.z;
		float targetAngle = Mathf.Atan2 (angleZ, - angleX) * Mathf.Rad2Deg;
		
		Quaternion fromRotation = User.Coordinates.rotation;
		Quaternion finalRotation = Quaternion.AngleAxis (targetAngle - 90, Vector3.up);
		User.Coordinates.rotation = Quaternion.RotateTowards (fromRotation, finalRotation, lookSpeed);

		Debug.Log (fromRotation);
		Debug.Log (finalRotation);
	}
	#endregion
}
//will house status effects and actions related to combat operations, but seperate from equipment.
public class CombatProperties : MovementProperties
{
	#region Properties

	#endregion

	#region Initialization

	#endregion

	#region Effects

	#endregion
}

public class StealthProperties : MovementProperties
{
	
}
/// <summary>
/// Holds all the statistical information for this attack, both for identification and functioning. Consists mostly
/// of template type information that will be overriden and specified in inhertied classes.</summary>
[System.Serializable] public abstract class AttackProperties : MovementProperties
{
	#region Properties
	public enum AttackPropertyType
	{
		Standard
,
		Charge
,
		Multi
,
		Counter
	}

	public enum DamageEffectType
	{
		//General Traits
		Standard
,
		Concussive
,				
		//Physical Damage Types
		Piercing
,
		Tearing
,
		Crushing
,
		Poisoned
,
		//Spirit Damage Types
		Burning
,
		Freezing
,
		Shocking
,
	}
	protected BaseEquipment _weapon;
	protected BoxCollider _hitbox;

	[SerializeField] protected BaseEquipmentProperties.EquipmentActions attackName;
	[SerializeField] protected AttackPropertyType attackType;

	[SerializeField] protected DamageEffectType primaryDamageType;
	[SerializeField] protected DamageEffectType secondaryDamageType;
	[SerializeField] protected float damageModifier;		//amount that this attack modifies the base weapon's damage range. 
	[SerializeField] protected float impactModifier;		//raw force behind attack, used for determining effectiveness of blocks and calculating hit stun

	public BaseEquipment Weapon { get { return _weapon;}}
	public BoxCollider Hitbox { get { return _hitbox;}}

	public BaseEquipmentProperties.EquipmentActions AttackName { get { return attackName; } }

	/// <summary>
	/// Gets the type of the attack. Dicates general nature of the attack, both input and output. </summary>
	/// <value>The type of the attack.</value>
	public AttackPropertyType AttackType { get {return attackType;}}

	/// <summary>
	/// Gets the damage type of the attack.</summary>
	/// <value>The type of the damage.</value>
	public DamageEffectType PrimaryDamageType { get { return primaryDamageType;}}
	public DamageEffectType SecondaryDamageType { get { return secondaryDamageType;}}
	public float DamageModifier { get { return damageModifier;}}
	public float ImpactModifier { get { return impactModifier;}}

	public float AdjustedDamageValue {
		get { return _weapon.WeaponProperties.AdjustedBaseDamage * damageModifier;}
	}
	public float AdjustedImpactValue {
		get { return _weapon.WeaponProperties.BaseImpact * impactModifier;}
	}
	#endregion Properties
	
	#region Initialization
	protected override void Awake ()
	{
		base.Awake ();
		_hitbox = _weapon.GetComponent<BoxCollider> ();
		primaryDamageType = DamageEffectType.Standard;
		damageModifier = 1f;
		impactModifier = 1f;
	}

	public virtual void SetValue (BaseEquipment weapon, BaseEquipmentProperties.EquipmentActions name)
	{
		base.SetValue (weapon.User);
		_weapon = weapon;
		_hitbox = weapon.HitBox;
		attackName = name;
	}
	#endregion Initialization

	#region Effects
	public abstract void OnTriggerEnter (Collider hit);
	//normal attack, single activation.
	protected abstract void StandardAttack ();

	//has features of normal attack, but it's startup continues while activation button is held. Effect amplifies while held as well
	protected abstract void ChargeAttack ();
	#endregion
}

[System.Serializable]public class MeleeProperties : AttackProperties
{
	#region Effects 
	public override void OnTriggerEnter (Collider hit) {
		Debug.Log ("Hit Registered!");
		BaseCharacter target;

		if (hit.transform.GetComponent<BaseCharacter>().CharType != User.CharType) {
			Debug.LogError ("LANDED A HIT!!!");
			target = hit.transform.GetComponent<BaseCharacter>();
			target.CharActions.CharStatus.ApplyAttack (this, User.Coordinates);
		}
	}

	public virtual void SetValue (BaseEquipment weapon, BaseEquipmentProperties.EquipmentActions name)
	{
		base.SetValue (weapon, name);
//		Debug.LogError ("Setting weapon values...");
		switch (attackType) {
		case AttackPropertyType.Standard:
			enterAbility = delegate {
				direction = (CharInput.LookDir - User.Coordinates.position);
				if (_weapon.ActiveAttack != this) 
					_weapon.ActiveAttack = this;

				_weapon.WeaponProperties.anim.material.color = Color.grey;
			};
			activeAbility = delegate {
				_hitbox.enabled = true;
				Aim ();
				BurstMove (activeMoveSpeed);
				_weapon.WeaponProperties.anim.material.color = Color.red;
			};
			exitAbility = delegate {
				_hitbox.enabled = false;
				_weapon.WeaponProperties.anim.material.color = Color.grey;
			};
			break;

		case AttackPropertyType.Charge:
		case AttackPropertyType.Multi:
		case AttackPropertyType.Counter:
		default:
			Debug.LogError ("Attack Type was not set properly!");
			break;
		}

	}

	protected override void StandardAttack ()
	{
		Aim ();
	}

	protected override void ChargeAttack ()
	{
		Aim ();
	}

//	protected override void MultiAttack ()
//	{
//
//	}
//
//	protected override void CounterAttack ()
//	{
//
//	}
	#endregion

}

public class RangedProperties : AttackProperties
{
	#region Properties
	protected float range;
	#endregion

	#region Effects

	public override void OnTriggerEnter (Collider hit)
	{

	}

	protected override void StandardAttack ()
	{

	}

	protected override void ChargeAttack ()
	{
		
	}

//	protected override void MultiAttack ()
//	{
//	
//	}
//
//	protected override void CounterAttack ()
//	{
//		
//	}
	#endregion
}

//TODO flesh out utility abilities, and create additional moveset options
public class DefendProperties : MovementProperties 
{
	float damageReduction;
}

public class UtilityProperties : MovementProperties
{
	public enum UtilityPropertyTypes {
		/// <summary>
		/// Defend ability. Will either reduce or totally deflect an attack. </summary>
		Defend,
		/// <summary>
		/// Counter ability. Will activate an attack sequence if attacked while active. </summary>
		Counter,
		Evade,
	}


	public override void SetValue (ICharacter user)
	{
		base.SetValue (user);
//		switch ()
	}
}