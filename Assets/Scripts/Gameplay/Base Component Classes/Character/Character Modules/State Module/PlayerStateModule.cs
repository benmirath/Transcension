using System;
using UnityEngine;

public class PlayerStateModule : BaseCharacterStateModule
{
		protected override void OnAwake ()
		{
				base.OnAwake ();
		}

		public override void Start ()
		{
				base.Start ();
				input = GetComponent<PlayerInput> ();

				Debug.Log ("Setting up Player input signals");
				CharInput.walkSignal += TransitionToWalk;
				CharInput.runSignal += TransitionToRun;
				CharInput.dodgeSignal += TransitionToDodge;
				CharInput.primarySignal += TransitionToPrimary;
				CharInput.sheatheSignal += TransitionToSheatheWeapon;
		}
}


