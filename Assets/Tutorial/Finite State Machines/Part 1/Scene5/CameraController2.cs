using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

public class CameraController2 : StateMachineBehaviour {
	
	public Transform linkedPlayer;
	
	Vector3 positionOffset;
	Quaternion rotationOffset;
	
	public enum CameraStates
	{
		FollowPlayer = 0
	}
	
	protected override void OnAwake()
	{
		positionOffset = linkedPlayer.position - transform.position;
		rotationOffset = Quaternion.identity;//Quaternion.Euler(linkedPlayer.rotation.eulerAngles - transform.rotation.eulerAngles);
	}
	
	public bool idle
	{
		get
		{
			return timeInCurrentState > 1 && currentState.Equals(CameraStates.FollowPlayer);
		}
	}
	
	void Start()
	{
		currentState = CameraStates.FollowPlayer;
	}
	
	
	
	void FollowPlayer_Update()
	{
		transform.position = linkedPlayer.position - positionOffset;
		transform.rotation = linkedPlayer.rotation * rotationOffset;
	}
	
	
}
