using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

public class LockCursor : MonoBehaviour {

	void OnMouseDown()
	{
		Screen.lockCursor = true;
	}
	
	void Update()
	{
		Screen.lockCursor = true;
	}
}
