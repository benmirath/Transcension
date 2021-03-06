using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class AINavigation
{
	#region Properties
	protected BaseCharacter _user;
	protected Transform _tr;
	protected Transform target;

	public bool canMove = true;						//Enables pathfinding

	protected Vector3 _targetPoint;						//Coordinate point tht AI is heading to at any one time
	protected Vector3 _targetDir;						//Relative direction of where the AI is heading
	public Vector3 TargetDir { get { return _targetDir; } }

	protected Seeker _seeker;
	protected Path path;							//Current path which is followed

	protected bool canSearch = true;					//Enables ability to look for paths
	protected bool canSearchAgain = true;					//controls if AI can re-search it's path.
	protected int currentWaypointIndex = 0;					//Current index in the path which is current target

	private bool waitingForRepath = false;				
	protected float repathRate = 0.5F;					//speed at which AI recalculates new paths. Set to lower number for faster targets
	private float lastRepath = -9999;					//Time when the last path request was sent

	protected float slowdownDistance = 0.6F;				//Distance to next waypoint (final OR intermediate) before slowing down. Make sure #forwardLook and #pickNextWaypointDist have higher values
	protected float pickNextWaypointDist = 2;				//range AI will switch to target the next waypoint in the path 
	protected float forwardLook = 1;					//Target point is Interpolated on the current segment in the path so that it has a distance of #forwardLook from the AI.
	protected float endReachedDistance = 0.2F;				//Distance to final waypoint. Once reached movement stops until target changes.
	protected bool targetReached = false;					//Holds if the end-of-path is reached
	protected bool closestOnPathCheck = true;

	protected float minMoveScale = 0.05F;
	/** Do a closest point on path check when receiving path callback.
	 * Usually the AI has moved a bit between requesting the path, and getting it back, and there is usually a small gap between the AI
	 * and the closest node.
	 * If this option is enabled, it will simulate, when the path callback is received, movement between the closest node and the current
	 * AI position. This helps to reduce the moments when the AI just get a new path back, and thinks it ought to move backwards to the start of the new path
	 * even though it really should just proceed forward.
	 */

	public Path AIPath { get { return path; } }
	public Vector3 TargetPoint { get { return _targetPoint; } }
	#endregion

	#region Initialization
	public AINavigation (AIInput input) {
		_user = input.User;
		_tr = _user.transform;
//		_seeker = _tr.gameObject.AddComponent<Seeker> ();
//		_seeker.pathCallback += OnPathComplete;

		GameObject go = GameObject.FindGameObjectWithTag("Player");	

		if(go == null)
			Debug.LogError("Could not find the player");
		target = go.transform;

//		_user.StartCoroutine (RepeatTrySearchPath ());


	}
	#endregion

	#region Methods
	public virtual Vector3 GetFeetPosition () {
		//if (_controller != null)
		//	return _tr.position - Vector3.down*_controller.height*0.5F;

		return _tr.position;
	}

	public IEnumerator RepeatTrySearchPath () {
		while (true) {
			TrySearchPath ();
			yield return new WaitForSeconds (repathRate);
		}
	}

	public void TrySearchPath () {

		if (Time.time - lastRepath >= repathRate && canSearchAgain) {
			SearchPath ();
		} else {
			_user.StartCoroutine (WaitForRepath ());
		}
	}

	protected IEnumerator WaitForRepath () {
		if (waitingForRepath) yield break; //A coroutine is already running

		waitingForRepath = true;
		//Wait until it is predicted that the AI should search for a path again
		yield return new WaitForSeconds (repathRate - (Time.time-lastRepath));

		waitingForRepath = false;
		//Try to search for a path again
		TrySearchPath ();
	}

	public void SearchPath () {

		if (target == null) { Debug.LogError ("Target is null, aborting all search"); return; }

		lastRepath = Time.time;
		//This is where we should search to
		Vector3 targetPoint = target.position;

		canSearchAgain = false;

		//We should search from the current position
		//		Path p = new Path(GetFeetPosition(), targetPoint, null);
		//		_seeker.StartPath (p);
		//seeker.StartPath (_tr.position, targetPoint);
		_seeker.StartPath (GetFeetPosition(), targetPoint);
	}

	public virtual void OnTargetReached () {
		//End of path has been reached
		//If you want custom logic for when the AI has reached it's destination
		//add it here
		//You can also create a new script which inherits from this one
		//and override the function in that script
	}

	//Returns if the end-of-path has been reached
	public bool TargetReached {
		get {
			return targetReached;
		}
	}

	// Called when a requested path has finished calculation.
	public void OnPathComplete (Path _p) {
		ABPath p = _p as ABPath;
		currentWaypointIndex = 0;
		targetReached = false;
		canSearchAgain = true;

		if (closestOnPathCheck) {
			Vector3 p1 = p.startPoint;
			Vector3 p2 = GetFeetPosition();
			float magn = Vector3.Distance (p1,p2);
			Vector3 dir = p2-p1;
			dir /= magn;
			int steps = (int)(magn/pickNextWaypointDist);
			for (int i=0;i<steps;i++) {
				CalculateVelocity (p1);
				p1 += dir;
			}
		}

		TrySearchPath ();
	}

	protected float XZSqrMagnitude (Vector3 a, Vector3 b) {
		float dx = b.x-a.x;
		float dz = b.z-a.z;
		return dx*dx + dz*dz;
	}	

	/// <summary>
	/// Calculates and returns the target point from the current line segment. 
	/// The returned point will like somewhere on the line segment.
	/// 
	/// todo This function uses .magnitude quite a lot, can it be optimized?
	/// </summary>
	/// <parameters>
	/// P = Current position
	/// A = Line segment start
	/// B = Line segment end
	/// </parameters>
	protected Vector3 CalculateTargetPoint (Vector3 p, Vector3 a, Vector3 b) {
		a.z = p.z;
		b.z = p.z;

		float magn = (a-b).magnitude;
		if (magn == 0) return a;

		float closest = Mathfx.Clamp01 (Mathfx.NearestPointFactor (a, b, p));
		Vector3 point = (b-a)*closest + a;
		float distance = (point-p).magnitude;

		float lookAhead = Mathf.Clamp (forwardLook - distance, 0.0F, forwardLook);

		float offset = lookAhead / magn;
		offset = Mathf.Clamp (offset+closest,0.0F,1.0F);
		return (b-a)*offset + a;
	}

	public Vector3 CalculateVelocity (Vector3 currentPosition) {
		if (path == null || path.vectorPath == null || path.vectorPath.Count == 0) return Vector3.zero; 

		List<Vector3> vPath = path.vectorPath;
		//Vector3 currentPosition = GetFeetPosition();

		if (vPath.Count == 1) {
			vPath.Insert (0,currentPosition);
		}

		if (currentWaypointIndex >= vPath.Count) { currentWaypointIndex = vPath.Count-1; }

		if (currentWaypointIndex <= 1) currentWaypointIndex = 1;

		while (true) {
			if (currentWaypointIndex < vPath.Count-1) {
				//There is a "next path segment"
				float dist = XZSqrMagnitude (vPath[currentWaypointIndex], currentPosition);
				//Mathfx.DistancePointSegmentStrict (vPath[currentWaypointIndex+1],vPath[currentWaypointIndex+2],currentPosition);
				if (dist < pickNextWaypointDist*pickNextWaypointDist) {
					currentWaypointIndex++;
				} else {
					break;
				}
			} else {
				break;
			} 
		}

		Vector3 moveDir = vPath[currentWaypointIndex] - vPath[currentWaypointIndex-1];
		Vector3 targetPoint = CalculateTargetPoint (currentPosition, vPath[currentWaypointIndex-1], vPath[currentWaypointIndex]);
		//vPath[currentWaypointIndex] + Vector3.ClampMagnitude (dir,forwardLook);

		moveDir = targetPoint - currentPosition;
		moveDir.z = 0;
		float targetDist = moveDir.magnitude;

		float slowdown = Mathf.Clamp01 (targetDist / slowdownDistance);

		this._targetDir = moveDir;
		this._targetPoint = targetPoint;


		//activated once final waypoint (destination) is reached.
		if (currentWaypointIndex == vPath.Count-1 && targetDist <= endReachedDistance) {
			if (!targetReached) { targetReached = true; OnTargetReached (); }

			//Send a move request, this ensures gravity is applied
			return Vector3.zero;
		}

		Vector3 forward = _tr.up;
		float dot = Vector3.Dot (moveDir.normalized, forward);

		float sp = Mathf.Max (dot,minMoveScale) * slowdown;

		if (Time.fixedDeltaTime	> 0) {
			sp = Mathf.Clamp (sp,0,targetDist/(Time.fixedDeltaTime*2));
		}

		return moveDir;
		//return forward * sp;
	}
	#endregion

}


