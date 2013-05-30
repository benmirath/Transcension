using UnityEngine;
using System.Collections;

public class ReversedCameraDirection : MonoBehaviour {
	
	Transform _transform;
	Transform _cameraTransform;
	
	// Use this for initialization
	void Start () {
		_transform = transform;
		_cameraTransform = Camera.main.transform;
	}
	
	// Update is called once per frame
	void Update () {
		_transform.forward = _cameraTransform.forward;
	}
}
