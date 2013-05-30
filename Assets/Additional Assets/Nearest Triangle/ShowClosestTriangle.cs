using UnityEngine;
using System.Collections;

public class ShowClosestTriangle : MonoBehaviour {
	
	public MeshFilter target;
	BaryCentricDistance distance;
	
	// Use this for initialization
	void Start () {
		distance = new BaryCentricDistance(target);
	}
	
	// Update is called once per frame
	void Update () {
		var result = distance.GetClosestTriangle(target.transform.position, transform.position);
		Debug.DrawRay(result.closestPoint, result.normal * 50, Color.red);
	}
}
