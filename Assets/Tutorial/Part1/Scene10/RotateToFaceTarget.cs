using UnityEngine;
using System.Collections;

public class RotateToFaceTarget : MonoBehaviour {

public float maximumRotateSpeed = 40;
	public float minimumTimeToReachTarget = 0.5f;
	Transform _transform;
	float _velocity;
	MovementStateMachine3 _movement;
	
	void Start()
	{
		_transform = transform;
		_movement = GetComponent<MovementStateMachine3>();
		
	}
	
	// Update is called once per frame
	void Update () {
		if(_movement.target)
		{
			var newRotation = Quaternion.LookRotation(_movement.target.position - _transform.position).eulerAngles;
			var angles = _transform.rotation.eulerAngles;
			_transform.rotation = Quaternion.Euler(angles.x, Mathf.SmoothDampAngle(angles.y, newRotation.y, ref _velocity, minimumTimeToReachTarget, maximumRotateSpeed),
				angles.z);
		}
	}
}
