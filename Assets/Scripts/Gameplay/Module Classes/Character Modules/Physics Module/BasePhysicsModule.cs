using System;
using UnityEngine;

public interface IPhysics {
	Transform Coordinates {get;}
	CharacterController Controller {get;}
	Rigidbody Body {get;}

	Vector3 KnockDir {get;}
}

public class BasePhysicsModule : IPhysics {
	private Transform _coordinates;										//The primary identifier for Unity game objects
	private CharacterController _controller;							//Used for rudimentary collider and physics. Mainly used for movement and detecting collisions.
	private Rigidbody _body;											//Used for more complex physics and collisions. Mainly used for combat.
	
	private Vector3 _knockDir;											//internal knockback coordinates
	
	public Transform Coordinates {
		get {return _coordinates;}
		set {_coordinates = value;}
	}
	public CharacterController Controller {
		get {return _controller;}
		set {_controller = value;}
	}
	public Rigidbody Body {
		get {return _body;}
		set {_body = value;}
	}
	
	public Vector3 KnockDir {
		get {return _knockDir;}
		set {_knockDir = value;}
	}
}