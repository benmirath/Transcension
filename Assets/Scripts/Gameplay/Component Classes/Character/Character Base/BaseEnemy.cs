/// <summary>
/// Base enemy script. Expands on the base character script, adding in more specific functionality for the enemy characters.
/// This includes AI input (such as pathfinding), and various states to control behaviour. </summary>
using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Seeker))]
[RequireComponent (typeof(AIInput))]
public class BaseEnemy : BaseCharacter {
/*	public enum AIStates{
		Stunned,									//character is stunned, out of commision for a brief period
		Idle,										//basic state, exists in this state until becoming aware of player. will likely involve small wandering around area
		Alert,										//is cautiously aware of player. slowly approach until makes more certain contact
		Seeking,									//has now "seen" the player, begins moving faster (running) towards player
		CombatReady,								//is now close enough to the player to engage in combat. will involve slower movement, maybe strafing
		Attack,										//attack state, launches an attack and then returns back to combatready
		Defend,										//defends against an attack (must still evaluate how and when this is used, perhaps based on timer and brief tracking of players last actions?)
		Dodge										//dodge an attack, requires similar planning to defend state.
	}*/
	#region Fields
	private AIInput _input;
//	private AIStates state;
#endregion

	#region Properties
	public override BaseCharacter.IInput CharInput {
		get {return _input;}
	}

/*	public AIStates CurrentState
	{
		get {return state;}
		set {state = value;}
	}
	public new string GetState 
	{
		get {return CurrentState.ToString();}
	}*/
	#endregion Propertie
	
	#region Initialization
	protected override void Awake () {
		base.Awake();	
		_input = GetComponent<AIInput>();
	}
	protected override void Start ()
	{
		base.Start ();
	}
	#endregion Initialization
	
	#region State Transitions
	/*protected void ChangeState(AIStates nextState)
	{
		Debug.Log ("Changing AI state to : " + nextState.ToString());
		state = nextState;
	}
	protected override void TransitionToIdle () 
	{
		CurrentState = AIStates.Idle;
	}
	protected void TransitionToAlert ()
	{
		CurrentState = AIStates.Alert;
	}
	protected void TransitionToSeeking ()
	{
		CurrentState = AIStates.Seeking;
	}
	protected void TransitionToAttack ()
	{
		CurrentState = AIStates.Attack;
	}
	protected override void TransitionToDefend () 
	{
		CurrentState = AIStates.Defend;
	}
	protected override void TransitionToDodge () 
	{
		CurrentState = AIStates.Dodge;
	}
	protected override void TransitionToStunned () 
	{
		CurrentState = AIStates.Stunned;
	}*/
	
/*	protected override void StateTransition()
	{
		string methodName = state.ToString() + "State";
		Debug.Log (methodName);
		System.Reflection.MethodInfo info = GetType().GetMethod(methodName,System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
		Debug.LogWarning("activating method of name : " + methodName);
		StartCoroutine((IEnumerator)info.Invoke(this, null));
	}*/
	#endregion State Transitions
	
	#region States
	protected override IEnumerator IdleState ()
	{
		while (state == CharState.Idle)
		{	
			//Transitions
			if (_input.AIPath != null) if(_input.AIPath.vectorPath.Length <= _input.alertRange) {
				CharMovement.Turn ();
				Body.Move (CharMovement.WalkSpeed * _input.MoveDir.normalized *Time.deltaTime);
			}

			//if detects suspicious activity
			//	move towards suspicious location
			//		if finds player
			//			switch to combat ready
			//		else
			//			return to spawn/anchor point
			//else 
			//	wander spawn area (set area upon spawning, TBD)

			//Effect
			yield return null;
		}
		StateTransition();
		yield break;
	}
/*	protected IEnumerator AlertState ()											//walk towards target
	{
		float timer = Time.time + _input.disengageTimer;
		while (CurrentState == AIStates.Alert)
		{
			//Transitions
			if(Time.time > timer) ChangeState(AIStates.Idle);
			if(_input.AIPath.vectorPath.Length <= _input.seekingRange) ChangeState(AIStates.Seeking);		
			
			//Effect
			CharMovement.Turn();
			Body.Move(CharMovement.WalkSpeed * MoveDir.normalized * Time.deltaTime);
			yield return null;
		}
		Debug.LogWarning("Leaving Alert State");
		StateTransition();
		yield break;
	}
	protected IEnumerator SeekingState ()										//run towards target
	{
		while (CurrentState == AIStates.Seeking)
		{
			//Transitions
			if(_input.AIPath.vectorPath.Length >= _input.disengageRange) ChangeState(AIStates.Alert);
			if(_input.AIPath.vectorPath.Length <= _input.combatRange) ChangeState(AIStates.CombatReady);
			
			//Effect
			CharMovement.Turn();
			Body.Move(CharMovement.RunSpeed * MoveDir.normalized * Time.deltaTime);
			yield return null;
		}
		Debug.LogWarning("Leaving Seeking State");		
		StateTransition();
		yield break;
	}*/

	/// <summary>
	/// Combat ready state of the AI once it has found a viable target.
	/// </summary>
	/// <returns>
	/// The ready state.
	/// </returns>
	protected override IEnumerator CombatReadyState ()
	{
		while (state == CharState.CombatReady)
		{
			//Transitions
			CharMovement.Aim ();

			if(_input.AIPath.vectorPath.Length >= _input.combatRange) {
				Body.Move (CharMovement.WalkSpeed * _input.MoveDir.normalized * Time.deltaTime);
			}
			else if (_input.AIPath.vectorPath.Length <= _input.attackRange) {
				//Launch attack
			}
			//if(_input.AIPath.vectorPath.Length <= _input.attackRange) ChangeState(AIStates.Attack);

			//if outside of range 
			//	run towards
			//if within range
			//	if target is moving towards AI (rushing/attack)
			//		dodge/block
			//	if target is strafing/not moving for x time 
			//		start attack



			//Effect
			else Body.Move(CharMovement.WalkSpeed * _input.MoveDir.normalized * Time.deltaTime);

			yield return null;
		}
		StateTransition();
		yield break;
	}
/*	protected IEnumerator AttackState ()
	{
		//attack target
		CharAnimation.SetTintColor(Color.red);
		CharMovement.Aim();
		//yield return StartCoroutine(CharEquipment.Primary.StartComboAttack1());
		CharAnimation.SetTintColor(Color.white);
		ChangeState(AIStates.CombatReady);
		StateTransition();
		yield break;
	}*/
/*	protected override IEnumerator DodgeState ()
	{
		while (CurrentState == AIStates.Dodge)
		{
			if (MoveDir == Vector3.zero) 
			{
				TransitionToIdle();
				break;
			}
			
			if (WeaponReady)
			{
				CharMovement.Aim();
				StartCoroutine(CharMovement.Dodge());
			}
			else 
			{
				CharMovement.Turn();
				StartCoroutine(CharMovement.Dodge());
			}
			TransitionToIdle();
			break;
		}
		StateTransition();		
		yield break;
	}
	protected override IEnumerator DefendState ()
	{
		ChangeState(AIStates.CombatReady);
		StateTransition();		
		yield break;
	}*/
	#endregion States
	
	protected override IEnumerator DecideAnimation ()
	{
		yield return null;
	}
}
