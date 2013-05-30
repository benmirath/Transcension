using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

public class Reload : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	void OnMouseDown()
	{
		Screen.lockCursor = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space))
			Application.LoadLevel("Scene1");
	}
}
