using UnityEngine;
using System.Collections;

public class FiniteStateMachine1 : MonoBehaviour {

	public bool sleeping = true;
	public Transform sleepingPrefab;
	public float attackDistance = 5;
	public float sleepDistance = 30;
	public float speed = 2;
	public float health = 20;
	public float maximumAttackEffectRange = 1f;
	
	Transform _transform;
	Transform _player;
	public Transform target;
	
	public EnemyMood _mood;
	
	CharacterController _controller;
	AnimationState _attack;
	AnimationState _die;
	AnimationState _hit;
	Animation _animation;
	
	float _attackDistanceSquared;
	float _sleepDistanceSquared;
	float _attackRotation;
	float _maximumAttackEffectRangeSquared;
	float _angleToTarget;
	
	public enum EnemyStates
	{
		sleeping = 0,
		following = 1,
		attacking = 2,
		beingHit = 3,
		dying = 4
	}
	
	public EnemyStates currentState = EnemyStates.sleeping;
	
	
	bool _busy;
	
	// Use this for initialization
	void Start () {
		_transform = transform;
		_player = Camera.main.transform;
		
		_mood = GetComponent<EnemyMood>();
		
		_attackDistanceSquared = attackDistance * attackDistance;
		_sleepDistanceSquared = sleepDistance * sleepDistance;
		_maximumAttackEffectRangeSquared = maximumAttackEffectRange * maximumAttackEffectRange;
		
		_controller = GetComponent<CharacterController>();
		
		_animation = animation;
		_attack = _animation["attack"];
		_hit = _animation["gothit"];
		_die = _animation["die"];
		
		_attack.layer = 5;
		_hit.layer = 5;
		_die.layer = 5;
		
		_controller.Move(new Vector3(0,-20,0));
		
	}
		
	float timeOfNextZZZZ;
	bool hasStruckTarget;
	
	void Update()
	{
		
		switch(currentState)
		{
		case EnemyStates.sleeping:
			
			if(timeOfNextZZZZ < Time.time)
			{
				var newPrefab = Instantiate(sleepingPrefab, _transform.position + Vector3.up * 3f, Quaternion.identity) as Transform;
				newPrefab.forward = Camera.main.transform.forward;
				timeOfNextZZZZ = Time.time + 2 + (6 * Random.value);
			}
			
			if((_transform.position - _player.position).sqrMagnitude < _attackDistanceSquared)
			{
				currentState = EnemyStates.following;
				target = _player;
				//Where this enemy wants to stand to attack
				_attackRotation = Random.Range(60,310);
			}
			break;
			
		case EnemyStates.following:
			if(!target)
			{
				currentState = EnemyStates.sleeping;
				return;
			}
			
			var difference = (target.position - _transform.position);
			difference.y /= 6;
			var distanceSquared = difference.sqrMagnitude;
			
			//Too far away to care?
			if( distanceSquared > _sleepDistanceSquared)
			{
				currentState = EnemyStates.sleeping;
				return;
			}
			
			//Close enough to attach
			if( distanceSquared < _maximumAttackEffectRangeSquared && _angleToTarget < 60f)
			{
				currentState = EnemyStates.attacking;
				_attack.enabled = true;
				_attack.time = 0;
				_attack.wrapMode = WrapMode.ClampForever;
				_attack.weight = 1;
				hasStruckTarget = false;
				
				return;
			}
			
			//Move towards the target
			
			//First decide target position
			var targetPosition = target.position + (Quaternion.AngleAxis(_attackRotation, Vector3.up) * target.forward * maximumAttackEffectRange * 0.8f);
			var basicMovement = (targetPosition - _transform.position).normalized * speed * Time.deltaTime;
			basicMovement.y = 0;
			
			//Only move when facing
			_angleToTarget = Vector3.Angle(basicMovement, _transform.forward);
			if( _angleToTarget < 70f)
			{
				basicMovement.y = -20 * Time.deltaTime;
				_controller.Move(basicMovement);
			}
			
			break;
		case EnemyStates.attacking:
			if(!hasStruckTarget && _attack.normalizedTime > 0.5f)
			{
				hasStruckTarget = true;
				//Check if still in range
				if(target && (target.position - _transform.position).sqrMagnitude < _maximumAttackEffectRangeSquared)
				{
					//Apply the damage
					target.SendMessage("TakeDamage", 1 + Random.value * 5, SendMessageOptions.DontRequireReceiver);
				}
				
			}
			if(_attack.normalizedTime >= 1 - float.Epsilon)
			{
				currentState = target ? EnemyStates.following : EnemyStates.sleeping;
				_attack.weight = 0;
			}
			break;
			
		case EnemyStates.beingHit:
			if(_hit.normalizedTime >= 1 - float.Epsilon)
			{
				currentState = target ? EnemyStates.following : EnemyStates.sleeping;
			}
			break;
			
		case EnemyStates.dying:
			if(_die.normalizedTime >= 1 - float.Epsilon)
			{
				Destroy(gameObject);
			}
			break;
		}
		
		
			
	}
	
	
	void OnTriggerEnter(Collider hit) 
	{
		if(hit.transform == _transform)
			return;
		
		switch(currentState)
		{
		case EnemyStates.sleeping:
		case EnemyStates.following:
			if(hit.transform == _player)
			{
				target = _player;
				currentState = EnemyStates.following;
			}
			else
			{
				var rival = hit.transform.GetComponent<EnemyMood>();
				if(rival)
				{
					if(Random.value > _mood.mood/100)
					{
						target = hit.transform;
						currentState = EnemyStates.following;
					}
				}
			}
			break;
		}
		
		
	}
	
	
	void TakeDamage(float amount)
	{
		
		health -= amount;
		if(health > 0)
		{
			_hit.time = 0;
			_hit.weight = 1;
			_hit.wrapMode = WrapMode.ClampForever;
			_hit.speed = 1;
			currentState = EnemyStates.beingHit;
		}
		else
		{
			_die.time = 0;
			_die.weight = 1;
			_die.wrapMode = WrapMode.ClampForever;
			_die.speed = 1;
			currentState = EnemyStates.dying;
		}
	}
	
	
	
}
