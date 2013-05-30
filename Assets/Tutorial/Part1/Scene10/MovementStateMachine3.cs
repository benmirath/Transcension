using UnityEngine;
using System.Collections;

public class MovementStateMachine3 : MonoBehaviour {

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
	
	
	bool _busy;
	
	// Use this for initialization
	IEnumerator Start () {
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
		
		while(true)
		{
			yield return new WaitForSeconds(Random.value * 6f + 3);
			if(sleeping)	
			{
				var newPrefab = Instantiate(sleepingPrefab, _transform.position + Vector3.up * 3f, Quaternion.identity) as Transform;
				newPrefab.forward = Camera.main.transform.forward;
			}
		}
		
	}
	
	void Update()
	{
		//Check if something else is in control
		if(_busy)
			return;
		
		if(sleeping)
		{
			if((_transform.position - _player.position).sqrMagnitude < _attackDistanceSquared)
			{
				sleeping = false;
				target = _player;
				//Where this enemy wants to stand to attack
				_attackRotation = Random.Range(60,310);
			}
		}
		else
		{
			//If the target is dead then go back to sleep
			if(!target)
			{
				sleeping = true;
				return;
			}
			
			var difference = (target.position - _transform.position);
			difference.y /= 6;
			var distanceSquared = difference.sqrMagnitude;
			
			//Too far away to care?
			if( distanceSquared > _sleepDistanceSquared)
			{
				sleeping = true;
			}
			//Close enough for an attack?
			else if( distanceSquared < _maximumAttackEffectRangeSquared && _angleToTarget < 40f)
			{
				StartCoroutine(Attack(target));
			}
			//Otherwise time to move
			else
			{
				
				
				//Decide target position
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
			}
		}
	}
	
	
	void OnTriggerEnter(Collider hit) 
	{
		if(hit.transform == _transform)
			return;
		
		if(hit.transform == _player)
		{
			StartCoroutine(Attack(_player));
		}
		else
		{
			var rival = hit.transform.GetComponent<EnemyMood>();
			if(rival)
			{
				if(Random.value > _mood.mood/100)
				{
					StartCoroutine("Attack",rival.transform);
				}
			}
		}
	}
	
	IEnumerator Attack(Transform victim)
	{
		sleeping = false;
		_busy = true;
		target = victim;
		_attack.enabled = true;
		_attack.time = 0;
		_attack.weight = 1;
		//Wait for half way through the animation
		yield return StartCoroutine(StateMachineBase.WaitForAnimation(_attack, 0.5f));
		//Check if still in range
		if(victim && (victim.position - _transform.position).sqrMagnitude < _maximumAttackEffectRangeSquared)
		{
			//Apply the damage
			victim.SendMessage("TakeDamage", 1 + Random.value * 5, SendMessageOptions.DontRequireReceiver);
		}
		//Wait for the end of the animation
		yield return StartCoroutine(StateMachineBase.WaitForAnimation(_attack, 1f));
		_attack.weight = 0;
		_busy = false;
	}
	
	void TakeDamage(float amount)
	{
		StopCoroutine("Attack");
		health -= amount;
		if(health < 0)
			StartCoroutine(Die());
		else
			StartCoroutine(Hit());
	}
	
	IEnumerator Die()
	{
		_busy = true;
		_animation.Stop();
		yield return StartCoroutine(PlayAnimation(_die));
		Destroy(gameObject);
	}
	
	IEnumerator Hit()
	{
		_busy = true;
		_animation.Stop();
		yield return StartCoroutine(PlayAnimation(_hit));
		_busy = false;
	}

		
	public static IEnumerator WaitForAnimation(AnimationState state, float ratio)
	{
		state.wrapMode = WrapMode.ClampForever;
		state.enabled = true;
		state.speed = state.speed == 0 ? 1 : state.speed;
		while(state.normalizedTime < ratio-float.Epsilon)
		{
			yield return null;
		}
	}
	
	
	public static IEnumerator PlayAnimation(AnimationState state)
	{
		state.time = 0;
		state.weight = 1;
		state.speed = 1;
		state.enabled = true;
		var wait = WaitForAnimation(state, 1f);
		while(wait.MoveNext())
			yield return null;
		state.weight = 0;
		
	}
	

	
	
}
