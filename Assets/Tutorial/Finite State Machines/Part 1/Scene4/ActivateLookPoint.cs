using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

public class ActivateLookPoint : MonoBehaviour {
	
	CameraController _controller;
	
	// Use this for initialization
	void Start () {
		_controller = Camera.main.GetComponent<CameraController>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Q) && _controller.idle)
		{
			SendMessage("Activate");
		}
	}
}
