using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using RAIN;
using RAIN.Core;
using RAIN.Belief;
using RAIN.Action;
using RAIN.Sensors;

public class UpdateCharacterCoordinates : RAIN.Action.Action
{
	BaseInputModule input;

//	private void UpdateTargetting (out float speed, out float bearing) {
////		bearing = Vector3.Dot ()
//	}

	public override ActionResult Start(Agent agent, float deltaTime)
	{

		input = agent.Avatar.GetComponent<BaseInputModule> ();
		return ActionResult.SUCCESS;
	}

	public override ActionResult Execute(Agent agent, float deltaTime)
	{
		var tr = input.transform;
		var target = actionContext.GetContextItem<GameObject> ("PlayerPos");

		var forward = input.transform.localPosition - tr.forward;
		var right = input.transform.localPosition - tr.right;
		var left = input.transform.localPosition - (-tr.right);
	
//		float speed;
//		float bearing;
//		if (target != null)  {
//			bearing = Vector3.Dot (forward, target.transform.position);
//			Debug.LogError ("the target discrepancy is " + bearing);
//		}

//		left.x = -right.x;
//		left.z = -left.z;
		Debug.LogError(right);
		Debug.LogError(left);

		actionContext.SetContextItem<Vector3> ("forward", forward);
		actionContext.SetContextItem<Vector3> ("right", right);
		actionContext.SetContextItem<Vector3> ("left", left);
		return ActionResult.SUCCESS;
	}

	public override ActionResult Stop(Agent agent, float deltaTime)
	{
		return ActionResult.SUCCESS;
	}
}

