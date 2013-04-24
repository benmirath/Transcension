using System;
using System.Collections;
using UnityEngine;

public interface IAbilityProperties {

}

/// <summary>
/// The distinctive properties for the Ability Base. Contains variables relating to coordinating and determining its functionality. </summary>
public abstract class AbilityProperties : ScriptableObject {

	#region Properties
	//[SerializeField] protected string name;							//Name of the attack
	protected IAbility ability;											//originating ability to which stats belong
	protected ICharacter user;
	protected IPhysics charPhysics;
	protected IInput charInput;
	protected ICharacterStateMachine charState;
	
	//protected IVital userVital;
	[SerializeField] protected float cost;
	[SerializeField] protected bool followupAvailable;

	[SerializeField] protected float startupLength;										//length of time between when ability is activated, and when it takes effect.
	[SerializeField] protected float durationLength;									//length of time that the ability remains in effect.
	[SerializeField] protected float cooldownLength;									//length of time between when ability effect ends, and character can act again.

	protected bool isSpecial;											//determines whether ability will use stamina or energy
	#endregion
	
	#region Initialization
	protected virtual void OnEnable () {
		Debug.Log("Ability Created");
		isSpecial = false;
	}
	public virtual void SetValue (IAbility _ability, BaseAbility.StartupEffect _start, BaseAbility.DurationEffect _middle, BaseAbility.CooldownEffect _end) {
		ability = _ability;
		user = ability.User;
		charPhysics = user.CharPhysics;
		charInput = user.CharInput;
		charState = user.CharState;
	}
	#endregion
	
	#region  Propertiess
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

/// <summary>
/// Movement properties for abilities. Many (anything integrating movement into the action) will derive from this property. </summary>
public class MovementProperties : AbilityProperties {
	#region Properties
	public enum MovementPropertyType {
		Walk,
		Strafe,
		Run,
		Dodge,
	}
	
	protected BaseAbility.DurationEffect moveEffect;
	protected BaseAbility.DurationEffect lookEffect;

	[SerializeField] protected MovementPropertyType movementType;
	[SerializeField] protected float moveSpeed;
	[SerializeField] protected float lookSpeed;

	public float MoveSpeed {
		get {return moveSpeed;}
	}
	public float TurnSpeed {
		get {return TurnSpeed;}
	}
	#endregion

	#region Initialization
	public override void SetValue (IAbility _ability, BaseAbility.StartupEffect _start, BaseAbility.DurationEffect _middle, BaseAbility.CooldownEffect _end) {
		base.SetValue (_ability, _start, _middle, _end);
	
		switch (movementType) {
		case MovementPropertyType.Walk:
			moveEffect = Move;
			lookEffect = Turn;
			break;

		case MovementPropertyType.Strafe:
			moveEffect = Move;
			lookEffect = Aim;
			break;

		//these last two will likely add some extra control logic to fine tune momentum.
		case MovementPropertyType.Run:
			moveEffect = Move;
			lookEffect = Turn;
			break;

		case MovementPropertyType.Dodge:
			moveEffect = Move;
			lookEffect = Turn;
			break;
		}
		_middle = moveEffect+lookEffect;
	}
	#endregion

	#region Effects
	protected void Move () {
		charPhysics.Controller.Move (moveSpeed * charInput.MoveDir.normalized * Time.deltaTime);		
	}
	protected void Turn () {
		Vector3 dir = charInput.MoveDir;
		if (dir == Vector3.zero) return;
		
		float aim = Mathf.Atan2(-dir.x, dir.y) * Mathf.Rad2Deg;						//holds direction the character should be facing
		charPhysics.Coordinates.rotation = Quaternion.Euler(0, 0, aim);
	}
	protected void Aim () {
		Vector3 target = user.CharInput.LookDir;
		float angleX = target.x - user.CharPhysics.Coordinates.position.x;
		float angleY = target.y - user.CharPhysics.Coordinates.position.y;
		float targetAngle = Mathf.Atan2 (angleY, angleX) * Mathf.Rad2Deg;
		
		Quaternion fromRotation = user.CharPhysics.Coordinates.rotation;
		Quaternion finalRotation = Quaternion.AngleAxis(targetAngle - 90, Vector3.forward);
		charPhysics.Coordinates.rotation = Quaternion.RotateTowards(fromRotation, finalRotation, lookSpeed);
	}
	#endregion
}

//will house status effects and actions related to combat operations, but seperate from equipment.
public class CombatProperties : MovementProperties {
	#region Properties

	#endregion

	#region Initialization

	#endregion

	#region Effects

	#endregion
}

public class StealthProperties {
	
}

/// <summary>
/// Holds all the statistical information for this attack, both for identification and functioning. Consists mostly
/// of template type information that will be overriden and specified in inhertied classes.</summary>
[System.Serializable] public abstract class AttackProperties : MovementProperties {
	#region Properties
	public enum AttackTypes {
		Standard,
		Charge,
		Multi,
		Counter
	}
	//[SerializeField] protected Vector3 range;										//vector3 that determines the proprotion of the attack
	[SerializeField] protected float damageModifier;								//amount that this attack modifies the base weapon's damage range. 
	[SerializeField] protected float attackStrength;								//raw force behind attack, used for determining effectiveness of blocks and calculating hit stun
	[SerializeField] protected bool comboable;
	//[SerializeField] protected float comboWindow;									//the window of oppotunity to activate a followup attack (combo)

//	public Vector3 Range {
//		get {return range;}
//	}
	public float DamageModifier {
		get {return damageModifier;}
	}
	public float AttackStrength {
		get {return attackStrength;}
	}
	public bool Comboable {
		get {return comboable;}
	}
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
public class MeleeProperties : AttackProperties {
	#region Effects 
	protected override void StandardAttack () {

	}
	protected override void ChargeAttack () {

	}
	protected override void MultiAttack () {

	}
	protected override void CounterAttack() {

	}
	#endregion

}
public class RangedProperties : AttackProperties {
	#region Properties
	protected float range;
	#endregion

	#region Effects
	protected override void StandardAttack () {
	
	}
	protected override void ChargeAttack () {
		
	}
	protected override void MultiAttack () {
	
	}
	protected override void CounterAttack () {
		
	}
	#endregion
}
public class DefendProperties : MovementProperties {

}