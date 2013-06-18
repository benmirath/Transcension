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
	private IInput _charInput;
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
		_charInput = _user.CharInput;
	}
	#endregion

	#region Effects



	//public Action ActivateAbility;
	#endregion
	
	#region  Propertiess
	public ICharacter User { get { return _user; } }

	public IInput CharInput { get { return _charInput; } }

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
		Debug.Log ("AbilityProperty: Setting Values");
		base.SetValue (user);
		switch (movementType) {
		case MovementPropertyType.Aim:
			activeAbility = delegate {
				Aim ();
			};
			break;

		case MovementPropertyType.Walk:
			activeAbility = delegate {
				ConstantMove (activeMoveSpeed);
				Turn ();
			};
			break;

		case MovementPropertyType.Strafe:
			activeAbility = delegate {
				ConstantMove (activeMoveSpeed);
				Aim ();
			};
			break;

		//these last two will likely add some extra control logic to fine tune momentum.
		case MovementPropertyType.Run:
			enterAbility = delegate {
				//will be the code to accelerate the character from walking to running speed.
				ConstantMove (enterMoveSpeed);
				Turn ();
			};
			activeAbility = delegate {
				ConstantMove (activeMoveSpeed);
				Turn ();
			};
			break;

		case MovementPropertyType.Dodge:
			enterAbility = delegate {
				BurstMove (enterMoveSpeed);
				Turn ();
			
			};
			activeAbility = delegate {
				BurstMove (activeMoveSpeed);
				Turn ();
			
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
	protected void ConstantMove (float speed)
	{
		User.Controller.Move (speed * CharInput.MoveDir.normalized * Time.deltaTime);		
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
		
		float aim = Mathf.Atan2 (-dir.x, dir.y) * Mathf.Rad2Deg;						//holds direction the character should be facing
		User.Coordinates.rotation = Quaternion.Euler (0, 0, aim);
	}

	protected void Aim ()
	{
		Debug.Log ("aim activating");
		//User.Coordinates.rotation = Quaternion.Euler (CharInput.LookDir);

		Vector3 target = CharInput.LookDir;
		float angleX = target.x - User.Coordinates.position.x;
		float angleY = target.y - User.Coordinates.position.y;
		float targetAngle = Mathf.Atan2 (angleY, angleX) * Mathf.Rad2Deg;
		
		Quaternion fromRotation = User.Coordinates.rotation;
		Quaternion finalRotation = Quaternion.AngleAxis (targetAngle - 90, Vector3.forward);
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
//		StandardMelee
//,
//		ChargeMelee
//,
//		MultiMelee
//,
//		StandardRanged
//,
//		ChargeRanged
//,
//		MultiRanged
//,
//
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
	private BaseEquipment _weapon;
	private BoxCollider _hitbox;

	[SerializeField] protected AttackPropertyType attackType;

	[SerializeField] protected DamageEffectType primaryDamageType;
	[SerializeField] protected DamageEffectType secondaryDamageType;
	[SerializeField] protected float damageModifier;		//amount that this attack modifies the base weapon's damage range. 
	[SerializeField] protected float impactModifier;		//raw force behind attack, used for determining effectiveness of blocks and calculating hit stun

	public BaseEquipment Weapon { get { return _weapon;}}
	public BoxCollider Hitbox { get { return _hitbox;}}

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
	#endregion Properties
	
	#region Initialization
	public virtual void SetValue (BaseEquipment weapon)
	{
		_weapon = weapon;
		base.SetValue (_weapon.User);
	}

	protected override void Awake ()
	{
		base.Awake ();
		primaryDamageType = DamageEffectType.Standard;
		damageModifier = 1f;
		impactModifier = 1f;
	}
	#endregion Initialization

	#region Effects
	//normal attack, single activation.
	protected abstract void StandardAttack ();

	//has features of normal attack, but it's startup continues while activation button is held. Effect amplifies while held as well
	protected abstract void ChargeAttack ();

	//releases multiple distinct attacks (hitboxes) in succession
//	protected abstract void MultiAttack ();

//	protected abstract void CounterAttack ();
	#endregion
}

[System.Serializable]public class MeleeProperties : AttackProperties
{
	#region Effects 
	void OnTriggerEnter (Collider hit) {
		Debug.Log ("Hit Registered!");
		BaseCharacter target;
		if (hit.transform.GetComponent<BaseCharacter>()) {
			target = hit.transform.GetComponent<BaseCharacter>();
			target.CharStats.ApplyDamage(AdjustedDamageValue);
		}
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

public class RangedProperties : AttackProperties
{
	#region Properties
	protected float range;
	#endregion

	#region Effects


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

public class UtilityProperties : MovementProperties
{

}