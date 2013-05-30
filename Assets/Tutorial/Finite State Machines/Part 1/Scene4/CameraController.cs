using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

public class CameraController : StateMachineBaseEx {
	
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
	
	public bool idle;
	
	void Start()
	{
		currentState = CameraStates.FollowPlayer;
	}
	
	
	
	void FollowPlayer_Update()
	{
		transform.position = linkedPlayer.position - positionOffset;
		transform.rotation = linkedPlayer.rotation * rotationOffset;
		idle = timeInCurrentState > 1f;
	}
	
	IEnumerator FollowPlayer_ExitState()
	{
		idle = false;
		yield break;
	}
	
	
}
