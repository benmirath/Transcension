using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

public class PositionIndicator : MonoBehaviour {

	void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawCube(transform.position, Vector3.one * 0.2f);
		Gizmos.DrawLine(transform.position, transform.position + transform.forward * 1);
		Gizmos.DrawSphere(transform.position + transform.forward * 1, 0.05f);
	}
	
}
