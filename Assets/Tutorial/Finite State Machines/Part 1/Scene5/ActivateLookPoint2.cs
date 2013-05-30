using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

public class ActivateLookPoint2 : MonoBehaviour {
	
	CameraController2 _controller;
	
	// Use this for initialization
	void Start () {
		_controller = Camera.main.GetComponent<CameraController2>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Q) && _controller.idle)
		{
			SendMessage("Activate");
		}
	}
}
