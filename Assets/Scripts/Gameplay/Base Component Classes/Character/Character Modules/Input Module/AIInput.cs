using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

//[RequireComponent (typeof(Movement))]
//[RequireComponent (typeof(SphereCollider))]
public class AIInput : BaseInputModule {
	//TODO expand the aiinput section, work in some basic state interactions
	#region Fields
	enum AIState {idle, seeking, attacking, falling, dead}	
	
	public float alertRange;
	public float seekingRange;
	public float combatRange;
	public float attackRange;
	public float disengageRange;
	
	public float disengageTimer;

	AINavigation pathfinder;

	//Cached components - improves efficiency
//	protected Transform _tr;
//	protected BaseCharacter _char;
//	protected Seeker _seeker;
//	protected CharacterController _controller;
	private CharacterController _body;
	protected Rigidbody _rigid;	
	
	private Transform target;						//Target that AI will pursue
	#endregion
	
	#region Properties
	public override List<IInputAction> InputActions { get { return null; } }
	public override Vector3 MoveDir {
		get {if (pathfinder != null)
				return pathfinder.CalculateVelocity (pathfinder.GetFeetPosition());
			else
				return Vector3.zero;}
	}
	public override Vector3 LookDir {
		get {return pathfinder.TargetPoint;}
	}

	public Transform Target
	{
		get {return target;}
		set {target = value;}
	}

//	public Seeker Seeker { get { return _seeker; } }

	#endregion
	// Use this for initialization
	void Awake () {

//		_seeker = gameObject.AddComponent <Seeker>();
		user = GetComponent<BaseCharacter>();
		pathfinder = new AINavigation (this);
//		_tr = transform;
//		_seeker.pathCallback += pathfinder.OnPathComplete;
		
//		GameObject go = GameObject.FindGameObjectWithTag("Player");	
//		
//		if(go == null)
//			Debug.LogError("Could not find the player");
//		target = go.transform;
		
//		canMove = false;
		//navController = GetComponent<NavmeshController>();
		//rigid = rigidbody;		

		user.CharState.currentState = CharacterStateMachine.CharacterActions.Idle;


			
		
	}
	private void Start ()
	{
	}

	private void BeginSeeking() {
		pathfinder.canMove = true;	
	}
	
	private void EndSeeking() {
		pathfinder.canMove = false;	
	}
	

}
