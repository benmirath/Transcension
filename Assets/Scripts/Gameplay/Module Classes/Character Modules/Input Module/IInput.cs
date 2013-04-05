using UnityEngine;
using System;

public interface IInput {
	//protected Vector3 _moveDir;
	Vector3 MoveDir {get;}
	
	//protected Vector3 _lookDir;
	Vector3 LookDir {get;}
}