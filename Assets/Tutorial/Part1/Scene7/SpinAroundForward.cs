using UnityEngine;
using System.Collections;

public class SpinAroundForward : MonoBehaviour {

	Transform _transform;
	float _rotatingFactor;
	
	void Start()
	{
		_transform = transform;
		_rotatingFactor = 0.2f + (Random.value * 0.8f);
	}
	
	// Update is called once per frame
	void Update () {
		_transform.rotation = _transform.rotation * Quaternion.AngleAxis(Time.deltaTime * 20 * _rotatingFactor, Vector3.forward); 
	}
}
