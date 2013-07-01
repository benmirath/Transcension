using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using RAIN;
using RAIN.Core;
using RAIN.Belief;
using RAIN.Action;
using RAIN.Sensors;

public class UpdateCharacterStats : RAIN.Action.Action
{
	CharacterStats stats;

	public UpdateCharacterStats ()
	{

	}
	public override ActionResult Start(Agent agent, float deltaTime)
	{

		stats = agent.Avatar.GetComponent<CharacterStats> ();
		return ActionResult.SUCCESS;
	}

	public override ActionResult Execute(Agent agent, float deltaTime)
	{
		var health = stats.Health;
		var stamina = stats.Stamina;
		var energy = stats.Energy;
		actionContext.SetContextItem<float> ("maxHealth", health.MaxValue);
		actionContext.SetContextItem<float> ("curHealth", health.CurValue);
		actionContext.SetContextItem<float> ("maxStamina", stamina.MaxValue);
		actionContext.SetContextItem<float> ("curStamina", stamina.CurValue);
		actionContext.SetContextItem<float> ("maxEnergy", energy.MaxValue);
		actionContext.SetContextItem<float> ("curEnergy", energy.CurValue);

		Debug.LogError ("The character's state is " + actionContext.GetContextItem<string> ("currentState"));

		var target = actionContext.GetContextItem<GameObject> ("playerPos");
		if (target != null) Debug.LogError ("The target's location is " + target);

		var inRange = actionContext.GetContextItem<bool> ("inRange");
		if (inRange != null) Debug.LogError ("is the target in range? " + inRange);

		var meleeRange = actionContext.GetContextItem<GameObject> ("meleeRange");
		if (meleeRange != null) Debug.LogError ("The target's melee range is " + meleeRange);


		return ActionResult.SUCCESS;
	}

	public override ActionResult Stop(Agent agent, float deltaTime)
	{
		return ActionResult.SUCCESS;
	}
}


