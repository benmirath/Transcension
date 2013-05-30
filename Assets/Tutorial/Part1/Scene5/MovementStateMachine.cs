using UnityEngine;
using System.Collections;

public class MovementStateMachine : MonoBehaviour {
	
	public bool sleeping = true;
	public Transform sleepingPrefab;
	
	// Use this for initialization
	IEnumerator Start () {
		var t = transform;
		while(true)
		{
			yield return new WaitForSeconds(Random.value * 3f + 2);
			if(sleeping)	
			{
				Instantiate(sleepingPrefab, t.position + Vector3.up * 3f, t.rotation);
			}
		}
	}
}
