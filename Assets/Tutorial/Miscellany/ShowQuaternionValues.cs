using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ShowQuaternionValues : MonoBehaviour {

	
	// Update is called once per frame
	void Update () {
		Debug.Log(transform.rotation.ToString());
	}
}
