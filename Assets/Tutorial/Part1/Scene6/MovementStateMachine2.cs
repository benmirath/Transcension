using UnityEngine;
using System.Collections;

public class MovementStateMachine2 : MonoBehaviour {
	
	public bool sleeping = true;
	public Transform sleepingPrefab;
	
	// Use this for initialization
	IEnumerator Start () {
		var t = transform;
		while(true)
		{
			yield return new WaitForSeconds(Random.value * 6f + 3);
			if(sleeping)	
			{
				var newPrefab = Instantiate(sleepingPrefab, t.position + Vector3.up * 3f, Quaternion.identity) as Transform;
				newPrefab.forward = Camera.main.transform.forward;
			}
		}
		
	}
	
	
}
