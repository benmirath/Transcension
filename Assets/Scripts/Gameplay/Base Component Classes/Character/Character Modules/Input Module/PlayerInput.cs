using UnityEngine;
using System.Collections;
using System;

[System.Serializable] public class PlayerInput : BaseInputModule {
	private PlayerTargetting _targetting;	
	public PlayerTargetting Targetting {
		get {return _targetting;}
	}

	public void Awake () 
	{
		user = GetComponent<BaseCharacter>();
<<<<<<< HEAD
=======
		//ActivateRun += evasion;
>>>>>>> 4dc69985bd335f0692f84f530e82348a9e76a8b3
	}

	public void Start () {
		_targetting = new PlayerTargetting ();
		_targetting.Setup (user);
	}
	
	public void Update () {
		moveDir = UpdateDirection ();
		lookDir = _targetting.UpdateMouse ();

		if (Input.GetButtonDown("Evasion")) StartCoroutine(CheckInput(1, "Evasion", delegate {ActivateDodge();}, delegate {ActivateRun();}));

		if (Input.GetButtonDown("DrawWeapon")) StartCoroutine(CheckInput(1, "DrawWeapon", delegate {ActivateLockOn();}, delegate {ActivateSheathe();}));

		if (Input.GetButtonDown("Primary")) ActivatePrimary();
	}
	
	private Vector3 UpdateDirection () {
		Vector3 d;
		
		if(Input.GetButton("Vertical") || Input.GetButton("Horizontal")) {
			if (Input.GetAxis("Vertical") != 0) d.y = Input.GetAxis("Vertical");
			else d.y = 0;
			
			if (Input.GetAxis("Horizontal") != 0) d.x = Input.GetAxis("Horizontal");
			else d.x = 0;
			
			d = new Vector3(d.x, d.y, 0);
		}
		else d = Vector3.zero;
		
		return d;
	}

	private IEnumerator CheckInput (float time, string buttonName, Action tapActivate, Action chargeActivate) {
		bool _chargedToggle;
		_chargedToggle = false;
		float _timer;
		if (time == 0) _timer = time;
		else _timer = time + Time.time;

		while (Input.GetButton(buttonName)) {
			while (_timer <= Time.time && time != 0 && _chargedToggle == false) {
				if (chargeActivate != null) {
					chargeActivate();		//input charged
				}
				_chargedToggle = true;
				yield return null;
			}
			yield return null;			
		}

		if (Input.GetButtonUp(buttonName) && _timer >= Time.time) {
			if (tapActivate != null) tapActivate();
		}
		yield break;
	}
	

	[System.Serializable] public class PlayerTargetting {	
		protected ICharacter _user;						//character using targetting component
		
		protected Vector3 targetLocation;					//coordinate data of target (mouse or lock on for the player, player or random wandering spot for AI)
		protected BaseCharacter target;						//character being targetted by component
		protected BaseCharacter _potentialTarget;
		
		protected bool _hasTarget;
		
		[SerializeField] protected Transform reticle;						
		
		public Vector3 Location {
			get {return targetLocation;}
			set {targetLocation = value;}
		}
		
		public void Setup (ICharacter user) {
			_user = user;
			_hasTarget = false;
			_potentialTarget = null;
		}
		
		//updates location of mouse cursor
		public Vector3 UpdateMouse() {						//player aim
			Plane playerPlane = new Plane(Vector3.back, _user.Coordinates.position);
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			float hitdist = 0.0f;	
			
			Vector3 targetPoint;
			
			if (playerPlane.Raycast (ray, out hitdist)) targetPoint = ray.GetPoint(hitdist);
			else targetPoint = Vector3.zero;
			
			return targetPoint;
		}	
		
		//checks for target at targetting reticle, changing reticle color and lock-on ability accordingly
		private bool CheckTarget () {
			Ray ray;
			RaycastHit hit;
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit, 100)) {
				if (hit.transform.GetComponent<BaseCharacter>().CharType == BaseCharacter.CharacterType.Enemy) {
					if (hit.transform.GetComponent<CharacterController>()) {
						if (_potentialTarget == null) _potentialTarget = hit.transform.GetComponent<BaseCharacter>();	
						return true;
					}
				}
				else _potentialTarget = null;
				return false;
			}
			else _potentialTarget = null;
			return false;
		}
		
		/// <summary>
		/// Locks onto target if CheckTarget returns true, and displays relevant lockon information.
		/// </summary>
		/// <param name='target'>
		/// Target.
		/// </param>
		public void LockOn () {
			Debug.LogWarning("attempting lockon");
			if (_potentialTarget == null) {
				_hasTarget = false;
				target = null;
				Debug.LogWarning("Lock on failed!");
				//will remove any lock on info
			}
			else {
				_hasTarget = true;
				target = _potentialTarget;
				Debug.LogWarning("Lock on succeeded!");
				//sets up lock on info to dispay (name and health)
			}
		}
		
		/// <summary>
		/// Sets the current target of character
		/// </summary>
		/// <returns>The view target.</returns>
		protected Vector3 SetViewTarget () {
			if(_hasTarget == true) return target.Coordinates.position;								//if locked on, will set location to target
			else return UpdateMouse();																//if not locked on, will set location to mouse
		}	
		
		public Vector3 UpdateTargetting () {
			CheckTarget();
			if (CheckTarget() == true) reticle.renderer.material.color = Color.red;
			else reticle.renderer.material.color = Color.white;
			
			reticle.position = UpdateMouse();													//update targetting reticle location
			targetLocation = SetViewTarget();
			return targetLocation;
		}
	}
}