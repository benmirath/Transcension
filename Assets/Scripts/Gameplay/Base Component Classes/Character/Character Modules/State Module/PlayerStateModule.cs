using System;
using UnityEngine;

public class PlayerStateMachine : BaseCharacterStateModule
{
		protected override void OnAwake ()
		{
				base.OnAwake ();
				user = GetComponent<BasePlayer> ();
				input = GetComponent<PlayerInput> ();
		}

		public override void Start ()
		{
				base.Start ();
				Debug.Log ("Setting up Player input signals");
				CharInput.walkSignal += TransitionToWalk;
				CharInput.runSignal += TransitionToRun;
				CharInput.dodgeSignal += TransitionToDodge;
				CharInput.primarySignal += TransitionToPrimary;
				CharInput.sheatheSignal += TransitionToSheatheWeapon;
		}
}


