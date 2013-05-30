using UnityEngine;
using System.Collections;

public class WalkingAnimation : MonoBehaviour {

	Transform _transform;
	Vector3 _lastPosition;
	AnimationState _walk;

	
	public float minimumDistance = 0.01f;
	
	// Use this for initialization
	void Start () {
		_transform = transform;
		_lastPosition = _transform.position;
		_walk = animation["walk"];
		_walk.layer = 2;
	}
	
	// Update is called once per frame
	void Update () {
		var moved = (_transform.position - _lastPosition).magnitude;
		_lastPosition = _transform.position;
		if(moved < minimumDistance || float.IsNaN(moved) || Time.deltaTime == 0)		
		{
			_walk.weight = 0;
		}
		else
		{
			_walk.weight = moved * 100;
			_walk.enabled = true;
			_walk.speed = moved / Time.deltaTime;
		}
	}
}
