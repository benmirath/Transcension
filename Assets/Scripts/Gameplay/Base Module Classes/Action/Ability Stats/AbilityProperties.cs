using System;
using System.Collections;
using UnityEngine;

public interface IAbilityProperties
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
		//private bool _followupAvailable;
	protected Vital.VitalName vitalType;
	public Action enterAbility;
	public Action durationAbility;
	public Action exitAbility;
	[SerializeField] protected float cost;
	[SerializeField] protected float enterLength;
//length of time between when ability is activated, and when it takes effect.
	[SerializeField] protected float durationLength;
//length of time that the ability remains in effect.
	[SerializeField] protected float exitLength;
//length of time between when ability effect ends, and character can act again.

	protected bool isSpecial;
//determines whether ability will use stamina or energy
	#endregion
	
	#region Initialization
	protected virtual void Awake ()
	{
		Debug.Log ("Ability Created");
		isSpecial = false;
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
	//public IPhysics CharPhysics {get { return _charPhysics; }}
	public IInput CharInput { get { return _charInput; } }
		//public bool FollowupAvailable { get { return _followupAvailable; } }

	public bool IsSpecial { get { return isSpecial; } }

	public Vital.VitalName VitalType { get { return vitalType; } }

	public float Cost {
		get { return cost;}
		set { cost = value;}
	}
	//How long each phase of the ability lasts. If duration is 0, then it will be an updating ability.
	public float EnterLength {
		get { return enterLength;}
		set { enterLength = value;}
	}

	public float DurationLength {
		get { return durationLength;}
		set { durationLength = value;}
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
//	protected Action enterMoveEffect;				//when move is starting up
//	protected Action activeMoveEffect;				//when move is active
//	protected Action exitMoveEffect;
//	protected Action lookEffect;

	[SerializeField] protected MovementPropertyType movementType;
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
		isSpecial = false;
		base.SetValue (user);
		switch (movementType) {
		case MovementPropertyType.Aim:
//			activeMoveEffect = null;
//			lookEffect = Aim;
			durationAbility = delegate {
				Aim ();
			};
			break;

		case MovementPropertyType.Walk:
//			activeMoveEffect = ConstantMove;
//			lookEffect = Turn;
			durationAbility = delegate {
				ConstantMove (activeMoveSpeed);
				Turn ();
			};
			break;

		case MovementPropertyType.Strafe:
//			activeMoveEffect = ConstantMove;
//			lookEffect = Aim;
			durationAbility = delegate {
				ConstantMove (activeMoveSpeed);
				Aim ();
			};
			break;

		//these last two will likely add some extra control logic to fine tune momentum.
		case MovementPropertyType.Run:
//			activeMoveEffect = ConstantMove;
//			lookEffect = Turn;

			enterAbility = delegate {
				//will be the code to accelerate the character from walking to running speed.
				ConstantMove (enterMoveSpeed);
				Turn ();
			};
			durationAbility = delegate {
				ConstantMove (activeMoveSpeed);
				Turn ();
			};
			break;

		case MovementPropertyType.Dodge:
			//if (CharInput == null)
			
						

			enterAbility = delegate {
//				ConstantMove (enterMoveSpeed);
				BurstMove (enterMoveSpeed);
				Turn ();
			
			};
			durationAbility = delegate {
//				ConstantMove (activeMoveSpeed);
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
		//durationAbility = activeMoveEffect + lookEffect;
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

		if (User == null)
			Debug.LogWarning ("user is null");
		if (User.Coordinates == null)
			Debug.LogWarning ("coordinates are null");
		if (target == null)
			Debug.LogWarning ("target is null");

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
		StandardMelee,
		ChargeMelee,
		MultiMelee,
		StandardRanged,
		ChargeRanged,
		MultiRanged,

//		Standard
//,
//		Charge
//,
//		Multi
//,
//		Counter
	}

	public enum DamageType
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
	//[SerializeField] protected Vector3 range;										//vector3 that determines the proprotion of the attack
	[SerializeField] protected AttackPropertyType attackType;

	[SerializeField] protected float dmgModifier;									//amount that this attack modifies the base weapon's damage range. 
	[SerializeField] protected DamageType dmgType;
	[SerializeField] protected float attackStrength;								//raw force behind attack, used for determining effectiveness of blocks and calculating hit stun
	//[SerializeField] protected bool comboable;
	//[SerializeField] protected float comboWindow;									//the window of oppotunity to activate a followup attack (combo)

//	public Vector3 Range {
//		get {return range;}
//	}
	public float DamageModifier {
		get { return dmgModifier;}
	}
	public float AttackStrength {
		get { return attackStrength;}
	}

//	public bool Comboable {
//		get { return comboable;}
//	}
//	public float ComboWindow {
//		get {return comboWindow;}
//	}
	#endregion Properties
	
	#region Initialization

	#endregion Initialization

	#region Effects
	protected abstract void StandardAttack ();

	protected abstract void ChargeAttack ();

	protected abstract void MultiAttack ();

	protected abstract void CounterAttack ();
	#endregion
}

public class MeleeProperties : AttackProperties
{
	#region Effects 
	protected override void StandardAttack ()
	{

	}

	protected override void ChargeAttack ()
	{

	}

	protected override void MultiAttack ()
	{

	}

	protected override void CounterAttack ()
	{

	}
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

	protected override void MultiAttack ()
	{
	
	}

	protected override void CounterAttack ()
	{
		
	}
	#endregion
}

public class UtilityProperties : MovementProperties
{

}