using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(PositionIndicator))]
public class CameraLook2 : StateMachineBehaviour {
	
	public float timeToActivate = 3;
	
	
#if UNITY_EDITOR
	[MenuItem("Camera/Place Look Position")]
	static void PlaceLookPosition()
	{
		var go = new GameObject("Camera Look");
		go.AddComponent<CameraLook>();
	}
	
	[MenuItem("Camera/Place Look Position @ Scene Camera %p")]
    public static void Place()
    {
       var camera = SceneView.lastActiveSceneView.camera;
       if( camera == null)
         return;

       if(Selection.activeGameObject == null || Selection.activeGameObject.GetComponent<CameraLook2>()==null)
       {

         var go = new GameObject();
         go.name = "LookPosition";
         go.AddComponent<CameraLook2>();
         if(Selection.activeGameObject)
         {
          go.transform.parent = Selection.activeGameObject.transform;
         }
         go.transform.position = Camera.current.transform.position;
         go.transform.rotation = Camera.current.transform.rotation;
       } else if(Selection.activeGameObject != null)
       {
         Selection.activeGameObject.transform.position = camera.transform.position;
         Selection.activeGameObject.transform.rotation = camera.transform.rotation;
       }
    }

    [MenuItem("Camera/Goto Look Position _l")]
    public static void Goto()
    {
       var camera = SceneView.lastActiveSceneView.camera;
       if( camera == null)
         return;

       if(Selection.activeGameObject != null && Selection.activeGameObject.GetComponentInChildren<CameraLook2>()!=null)
       {
         Selection.activeGameObject = Selection.activeGameObject.GetComponentInChildren<CameraLook2>().GetComponent<Transform>().gameObject;

         SceneView.lastActiveSceneView.pivot = Selection.activeGameObject.transform.position;
         SceneView.lastActiveSceneView.rotation = Selection.activeGameObject.transform.rotation;
         SceneView.lastActiveSceneView.size = 1;
         SceneView.RepaintAll();

       }
    }

	
#endif
	
	string _message;
	
	void Activate(string message)
	{
		_message = message ?? "";
		Camera.main.GetStateMachine().Call(CameraModes.MoveToTarget, this);
	}
		
	#region MoveToTarget
		
	Vector3 _originalPosition;
	Quaternion _originalRotation;
	float _delta;
	float _oldTimeScale;
	float _t;
	
	void MoveToTarget_EnterState()
	{
		Debug.Log("MoveToTarget");
		_t = 0;
		GunBehaviour.Gun.Call(GunBehaviour.GunStates.Disabled);
		_originalPosition = transform.position;
		_originalRotation = transform.rotation;
		_delta = Time.deltaTime	;
		_oldTimeScale = Time.timeScale;
		Time.timeScale = 0.05f;
		
	}
	
	void MoveToTarget_Update()
	{
		transform.position = Vector3.Lerp(_originalPosition, localTransform.position, _t);
		transform.rotation = Quaternion.Slerp(_originalRotation, localTransform.rotation, _t);
		_t += _delta/timeToActivate;
		if(_t>=1)
			currentState = CameraModes.WaitForKeypress;
	}
	
	
	#endregion
	
	#region WaitForKeypress
	
	int _numberOfCharacters = 0;
	
	void WaitForKeypress_Update()
	{
		_numberOfCharacters++;
		if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return))
		{
			currentState = CameraModes.ReturnToOrigin;
		}
	}
	
	void WaitForKeypress_OnGUI()
	{
		GUI.color = Color.red;
		GUILayout.BeginArea(new Rect(0,0,Screen.width, Screen.height));
		GUILayout.BeginVertical();
		GUILayout.FlexibleSpace();
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.Label(_message.Substring(0, Mathf.Min(_numberOfCharacters, _message.Length)));
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.EndVertical();
		GUILayout.EndArea();
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
			t += _delta/timeToActivate;
			yield return null;
		}
		Time.timeScale = _oldTimeScale;
		GunBehaviour.Gun.Return();
		
		Return();
	}
	
	#endregion
	
	public enum CameraModes
	{
		MoveToTarget = 0,
		WaitForKeypress = 1,
		ReturnToOrigin = 2
	}
	
}
