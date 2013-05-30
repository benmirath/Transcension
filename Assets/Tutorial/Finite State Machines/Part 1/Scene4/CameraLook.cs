using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(PositionIndicator))]
public class CameraLook : StateMachineBaseEx {
	
	public float timeToActivate = 3;
	
	
#if UNITY_EDITOR
	[MenuItem("Camera/Place Look Position")]
	static void PlaceLookPosition()
	{
		var go = new GameObject("Camera Look");
		go.AddComponent<CameraLook>();
	}
#endif
	
	
	void Activate()
	{
		Camera.main.GetComponent<StateMachineBaseEx>().SetState(CameraModes.MoveToTarget, this);
	}
		
	#region MoveToTarget
		
	Vector3 _originalPosition;
	Quaternion _originalRotation;
	Enum _originalState;
	StateMachineBaseEx _originalMachine;
	
	IEnumerator MoveToTarget_EnterState()
	{
		_originalPosition = transform.position;
		_originalRotation = transform.rotation;
		_originalState = stateMachine.lastState;
		_originalMachine = stateMachine.lastStateMachineBehaviour;
		var t = 0f;
		while(t < 1)
		{
			transform.position = Vector3.Lerp(_originalPosition, localTransform.position, t);
			transform.rotation = Quaternion.Slerp(_originalRotation, localTransform.rotation, t);
			t += Time.deltaTime/timeToActivate;
			yield return null;
		}
		currentState = CameraModes.WaitForKeypress;
	}
	
	
	#endregion
	
	#region WaitForKeypress
	
	void WaitForKeypress_Update()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			currentState = CameraModes.ReturnToOrigin;
		}
	}
	
	#endregion
	
	#region ReturnToOrigin
	
	IEnumerator ReturnToOrigin_EnterState()
	{
		var t = 0f;
		while(t < 1)
		{
			transform.position = Vector3.Lerp(localTransform.position, _originalPosition, t);
			transform.rotation = Quaternion.Slerp(localTransform.rotation, _originalRotation, t);
			t += Time.deltaTime/timeToActivate;
			yield return null;
		}
		stateMachine.SetState(_originalState, _originalMachine);
	}
	
	#endregion
	
	public enum CameraModes
	{
		MoveToTarget = 0,
		WaitForKeypress = 1,
		ReturnToOrigin = 2
	}
	
}
