using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable] public class PlayerInput : BaseInputModule {
	protected IInputAction key_tab;
	protected IInputAction key_shift;
	protected IInputAction key_primary;
	protected IInputAction key_secondary;

	protected List<IInputAction> inputActions = new List<IInputAction>();
	public override List<IInputAction> InputActions { get { return inputActions; } }

	private PlayerTargetting _targetting;	
	public PlayerTargetting Targetting {
		get {return _targetting;}
	}

	public override void Awake () 
	{
		base.Awake();
		key_tab = new ButtonInput ("DrawWeapon");
		key_shift = new ButtonInput ("Evasion");
		key_primary = new ButtonInput ("Primary");
		key_secondary = new ButtonInput ("Secondary");
		inputActions.AddRange (new List <IInputAction> {key_tab, key_shift, key_primary, key_secondary});
	}

	public void Start () {
		_targetting = new PlayerTargetting ();
		_targetting.Setup (user);
	}
	
	public virtual void Update () {
		moveDir = UpdateDirection ();

		if (user.CharState.Armed)
			lookDir = _targetting.UpdateMouse ();
		else if (moveDir != Vector3.zero)
			lookDir = moveDir;
		else
			lookDir = transform.forward;

		if (Input.GetButtonDown("Evasion")) StartCoroutine(key_shift.CheckInput(1, delegate {ActivateDodge();}, delegate {ActivateRun();}));

		if (Input.GetButtonDown("DrawWeapon")) StartCoroutine(key_tab.CheckInput(1, delegate {ActivateLockOn();}, delegate {ActivateSheathe();}));

		if (Input.GetButtonDown("Primary")) ActivatePrimary();
	}

	//
	public void FixedUpdate () {
		base.FixedUpdate ();
		if (animator == null) Debug.LogError ("animator null");
//		animator.SetFloat ("V Move Input", moveDir.z);
//		animator.SetFloat ("H Move Input", moveDir.x);

		if (moveDir != Vector3.zero && animator.GetBool ("Walking") == false)	//checks the character has move input, but isn't walking
			animator.SetBool ("Walking", true);
		else if (moveDir == Vector3.zero && animator.GetBool ("Walking") == true)				//if the c
			animator.SetBool ("Walking", false);

		if (user.CharState.Running == true && animator.GetBool ("Running") == false)
			animator.SetBool ("Running", true);
		else if (user.CharState.Running == false && animator.GetBool ("Running") == true)
			animator.SetBool ("Running", false);

		if (user.CharState.Evading == true && animator.GetBool ("Dodging") == false)
			animator.SetBool ("Dodging", true);
		else if (user.CharState.Evading == false && animator.GetBool ("Dodging") == true)
			animator.SetBool ("Dodging", false);

		if (user.CharState.Attacking == true && animator.GetBool ("Attacking") == false)
			animator.SetBool ("Attacking", true);
		else if (user.CharState.Attacking == false && animator.GetBool ("Attacking") == true)
			animator.SetBool ("Attacking", false);
	}
	
	private Vector3 UpdateDirection () {
		Vector3 d;
		
		if(Input.GetButton("Vertical") || Input.GetButton("Horizontal")) {
			if (Input.GetAxis("Vertical") != 0) d.z = Input.GetAxis("Vertical");
			else d.z = 0;
			
			if (Input.GetAxis("Horizontal") != 0) d.x = Input.GetAxis("Horizontal");
			else d.x = 0;
			
			d = new Vector3(d.x, 0, d.z);
		}
		else d = Vector3.zero;
		
		return d;
	}

//	private IEnumerator CheckInput (float time, string buttonName, Action tapActivate, Action chargeActivate) {
//		bool _chargedToggle;
//		_chargedToggle = false;
//		float _timer;
//		if (time == 0) _timer = time;
//		else _timer = time + Time.time;
//
//		while (Input.GetButton(buttonName)) {
//			while (_timer <= Time.time && time != 0 && _chargedToggle == false) {
//				if (chargeActivate != null) {
//					chargeActivate();		//input charged
//				}
//				_chargedToggle = true;
//				yield return null;
//			}
//			yield return null;			
//		}
//
//		if (Input.GetButtonUp(buttonName) && _timer >= Time.time) {
//			if (tapActivate != null) tapActivate();
//		}
//		yield break;
//	}

	public class ButtonInput : IInputAction {
		string name;
		public string Name { get { return name; } }
		bool active;
		public bool Active { get { return active; } }

		public ButtonInput (string _name) {
			name = _name;
		}


		public IEnumerator CheckInput (float time, Action tapActivate, Action chargeActivate) {
			bool _chargedToggle;
			_chargedToggle = false;
			active = true;
			float _timer;
			if (time == 0) _timer = time;
			else _timer = time + Time.time;

			if (active = true) {
				while (Input.GetButton(name)) {
					while (_timer <= Time.time && time != 0 && _chargedToggle == false) {
						if (chargeActivate != null) {
							chargeActivate ();		//input charged
						}
						_chargedToggle = true;
						yield return null;
					}
					yield return null;			
				}
				
				if (Input.GetButtonUp (name) && _timer >= Time.time) {
					if (tapActivate != null)
						tapActivate ();
				}
			}
			active = false;
			yield break;
		}

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
			Plane playerPlane = new Plane(Vector3.up, _user.Coordinates.position);
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