using UnityEngine;
using System.Collections;

public class FiniteStateMachine3 : StateMachineBaseEx {

	public bool sleeping = true;
	public Transform sleepingPrefab;
	public float attackDistance = 5;
	public float sleepDistance = 30;
	public float speed = 2;
	public float health = 20;
	public float maximumAttackEffectRange = 1f;
	
	public Transform target;
	
	public EnemyMood _mood;
	
	AnimationState _attack;
	AnimationState _die;
	AnimationState _hit;
	
	Transform _player;
	
	float _attackDistanceSquared;
	float _sleepDistanceSquared;
	float _attackRotation;
	float _maximumAttackEffectRangeSquared;
	float _angleToTarget;
	
	public enum EnemyStates
	{
		Sleeping = 0,
		Following = 1,
		Attacking = 2,
		BeingHit = 3,
		Dying = 4
	}
	
	
	bool _busy;
	
	// Use this for initialization
	void Start () {
		
		_player = Camera.main.transform;
		
		_mood = GetComponent<EnemyMood>();
		
		_attackDistanceSquared = attackDistance * attackDistance;
		_sleepDistanceSquared = sleepDistance * sleepDistance;
		_maximumAttackEffectRangeSquared = maximumAttackEffectRange * maximumAttackEffectRange;
		
		
		_attack = animation["attack"];
		_hit = animation["gothit"];
		_die = animation["die"];
		
		_attack.layer = 5;
		_hit.layer = 5;
		_die.layer = 5;
		
		controller.Move(new Vector3(0,-20,0));
		currentState = EnemyStates.Sleeping;
		
	}
		
	float timeOfNextZZZZ;
	bool hasStruckTarget;
	
	#region Sleeping
	
	IEnumerator Sleeping_EnterState()
	{
		while(true)
		{
			yield return new WaitForSeconds(Time.time + 2 + (6 * Random.value));
			var newPrefab = Instantiate(sleepingPrefab, transform.position + Vector3.up * 3f, Quaternion.identity) as Transform;
				newPrefab.forward = Camera.main.transform.forward;
				
		}
	}
	
	void Sleeping_Update()
	{
		if((transform.position - _player.position).sqrMagnitude < _attackDistanceSquared)
		{
			target = _player;
			//Where this enemy wants to stand to attack
			_attackRotation = Random.Range(60,310);
			
			currentState = EnemyStates.Following;
			
		}
	}
	
	void Sleeping_OnTriggerEnter(Collider hit)
	{
		Following_OnTriggerEnter(hit);
	}
	
	#endregion
	
	#region Following
	
	void Following_Update()
	{
		if(!target)
		{
			currentState = EnemyStates.Sleeping;
			return;
		}
		
		var difference = (target.position - transform.position);
		difference.y /= 6;
		var distanceSquared = difference.sqrMagnitude;
		
		//Too far away to care?
		if( distanceSquared > _sleepDistanceSquared)
		{
			currentState = EnemyStates.Sleeping;
			return;
		}
		
		//Close enough to attach
		if( distanceSquared < _maximumAttackEffectRangeSquared)
		{
			currentState = EnemyStates.Attacking;
			return;
		}
		
		//Move towards the target
		
		//First decide target position
		var targetPosition = target.position + (Quaternion.AngleAxis(_attackRotation, Vector3.up) * target.forward * maximumAttackEffectRange * 0.8f);
		var basicMovement = (targetPosition - transform.position).normalized * speed * Time.deltaTime;
		basicMovement.y = 0;
		
		//Only move when facing
		_angleToTarget = Vector3.Angle(basicMovement, transform.forward);
		if( _angleToTarget < 70f)
		{
			basicMovement.y = -20 * Time.deltaTime;
			controller.Move(basicMovement);
		}
			
	}
	
	void Following_OnTriggerEnter(Collider hit)
	{
		if(hit.transform == transform)
			return;
		
		if(hit.transform == _player)
		{
			target = _player;
			currentState = EnemyStates.Following;
		}
		else
		{
			var rival = hit.transform.GetComponent<EnemyMood>();
			if(rival)
			{
				if(Random.value > _mood.mood/100)
				{
					target = hit.transform;
					currentState = EnemyStates.Following;
				}
			}
		}

	}
	
	#endregion
	
	#region Attacking
	
	IEnumerator Attacking_EnterState()
	{
		_attack.enabled = true;
		_attack.time = 0;
		_attack.weight = 1;
		//Wait for half way through the animation
		yield return StartCoroutine(WaitForAnimation(_attack, 0.5f));
		//Check if still in range
		if(target && (target.position - transform.position).sqrMagnitude < _maximumAttackEffectRangeSquared)
		{
			//Apply the damage
			target.SendMessage("TakeDamage", 1 + Random.value * 5, SendMessageOptions.DontRequireReceiver);
		}
		//Wait for the end of the animation
		yield return StartCoroutine(WaitForAnimation(_attack, 1f));
		_attack.weight = 0;
		currentState = target ? EnemyStates.Following : EnemyStates.Sleeping;
	}
	
	#endregion
	
	#region BeingHit
	
	IEnumerator BeingHit_EnterState()
	{
		yield return PlayAnimation(_hit);
		currentState = target ? EnemyStates.Following : EnemyStates.Sleeping;
	}
	
	#endregion
	
	#region Dying
	
	IEnumerator Dying_EnterState()
	{
		yield return PlayAnimation(_die);
	}
	
	#endregion
	
	void TakeDamage(float amount)
	{
		
		health -= amount;
		if(health > 0)
		{
			currentState = EnemyStates.BeingHit;
		}
		else
		{
			currentState = EnemyStates.Dying;
		}
	}
	
	
	
}
