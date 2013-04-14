using System;
using UnityEngine;	

public interface IMoveSet {
	MoveSet.MoveSetType type {
		get;
	}
	void Activate();
}

public class MoveSet {
	public enum MoveSetType {
		BalancedPrimary,
		BalancedSecondary,
		FocusedPrimary,
		FocusedSecondary
	}

	#region External Fields
	IEquippable weapon;
	BaseStateMachineModule.CharState userState;
	BaseStateMachineModule.CharSubState userSubState;
	#endregion

	#region Internal Fields
	MoveSetType type;
	#endregion

	public MoveSet (IEquippable _weapon) {

	}

	public void Activate () {
		if (type == MoveSetType.BalancedPrimary || type == MoveSetType.FocusedPrimary) {
			if (userState == BaseStateMachineModule.CharState.CombatReady) {
				if (userSubState == BaseStateMachineModule.CharSubState.Running) {
					//launch running attack
				} else if (userSubState == BaseStateMachineModule.CharSubState.Dodging) {
					//launch dodge attack
				//} else if (userSubState == BaseStateMachineModule.CharSubState.AltStance) {
					//launch alt attack
				}
				// launch basic combat attack
			}



		}
		else if (type == MoveSetType.BalancedSecondary || type == MoveSetType.FocusedSecondary) {

		}
		else {
			//DEBUG
		}
	}
}

