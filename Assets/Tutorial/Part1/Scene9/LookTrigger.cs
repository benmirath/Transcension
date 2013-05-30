using UnityEngine;
using System.Collections;

public class LookTrigger : MonoBehaviour {
	
	Transform _transform;
	
	// Use this for initialization
	void Start () {
		_transform = transform;
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit hit;
		if(Physics.SphereCast(_transform.position + _transform.forward * 0.5f, 3f, _transform.forward, out hit, 400))
		{
			hit.collider.SendMessage("LookedAt", SendMessageOptions.DontRequireReceiver);
		}
	}
}
