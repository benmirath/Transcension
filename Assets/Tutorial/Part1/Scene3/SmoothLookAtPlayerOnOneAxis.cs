using UnityEngine;
using System.Collections;

public class SmoothLookAtPlayerOnOneAxis : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		var newRotation = Quaternion.LookRotation(Camera.main.transform.position - transform.position).eulerAngles;
		newRotation.x = 0;
		newRotation.z = 0;
		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(newRotation), Time.deltaTime);
	}
}
