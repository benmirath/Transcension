using UnityEngine;
using System.Collections;

public class GentleRiseAndDestroy : MonoBehaviour {
	
	public float timeToLive = 2f;
	Transform _transform;
	
	
	// Use this for initialization
	void Start () {
		_transform = transform;
		Destroy(gameObject, timeToLive);
	}
	
	// Update is called once per frame
	void Update () {
		_transform.position += Vector3.up * Time.deltaTime;
	}
}
