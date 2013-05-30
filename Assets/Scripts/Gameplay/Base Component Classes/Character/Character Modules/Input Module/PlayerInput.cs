using UnityEngine;
using System.Collections;
using System;

[System.Serializable] public class PlayerInput : BaseInputModule {
	//Player inputs
//	private BasePlayer _user;
	//private float x;							//Horizontal movement input.
	//private float y;							//Vertical movement input.
	//[SerializeField] protected Vector3 dir;				//Overall movement input.
	private PlayerTargetting _targetting;	
	public PlayerTargetting Targetting {
		get {return _targetting;}
	}
	
	//delegate methods called when their attached event is activate from player input
//	public ButtonInput stance;													//used for drawing weapon and locking on
//	public ButtonInput evasion;													//used for running and dodging
//	public ButtonInput mainHand;												//used for mainhand input
//	public ButtonInput offHand;													//used for offhand input
//	public ButtonInput special1;
//	public ButtonInput special2;
//	public ButtonInput special3;

	public void Awake () 
	{
		user = GetComponent<BasePlayer>();
		_targetting = new PlayerTargetting ();
		//ActivateRun += evasion;
	}

	public void Start () {
		_targetting.Setup (user);
//		user = GetComponent<BaseCharacter>();

		//_targetting.Setup(user);
	}
	
	// Update is called once per frame
	public void Update () {
		float dodgeTimer = 0;
		float targettingTimer = 0;

		Debug.Log ("the character is aiming at :"+lookDir);

		//_targetting.UpdateTargetting();
		moveDir = UpdateDirection ();
		lookDir = _targetting.UpdateMouse ();

		//Dodge and Evasion input
		if (Input.GetButtonDown("Evasion")) StartCoroutine(CheckInput(1, "Evasion", delegate {ActivateDodge();}, delegate {ActivateRun();}));

		if (Input.GetButtonDown("DrawWeapon")) StartCoroutine(CheckInput(1, "DrawWeapon", delegate {ActivateLockOn();}, delegate {ActivateSheathe();}));

		if (Input.GetButtonDown("Primary")) ActivatePrimary();

//		if (Input.GetButtonDown("Evasion"))
//		{
//			dodgeTimer = 3 + Time.time;
//		}
//		if (Input.GetButton("Evasion"))
//		{
//			if (dodgeTimer >= Time.time)
//			{
//				Debug.LogWarning("1");
//				ActivateRun();
//			}
//		}
//		if (Input.GetButtonUp("Evasion")) 
//		{
//			if (dodgeTimer >= Time.time)
//			{
//				Debug.LogWarning("2");
//				ActivateDodge();
//			}
//		}
//			//directional input
//			if (Input.GetButtonDown("Horizontal")||Input.GetButtonDown("Vertical")) ActivateWalk(); 
//
//			//dodge/run input
//			if (Input.GetButton) {
//				dodgeTimer++;
//			}
//
//			if (Input.GetButtonDown("Evasion")) {
//				dodgeTimer = Time.time;
//			}
//
//			if (Input.GetButtonUp("Evasion")) {
//				//If button is released after 1 second charge
//				if (dodgeTimer <= (Time.time+1)) ActivateDodge();
//				else ActivateRun();
//			}
//
//			if (Input.GetButtonDown("MainHand")) ActivatePrimary();
//			if (Input.GetButtonDown("OffHand")) ActivateSecondary();
//
//			//targetting/sheathe weapon input
//			if (Input.GetButtonDown("DrawWeapon")) {
//				targettingTimer = Time.time;
//			}
//			if (Input.GetButtonUp("DrawWeapon")) {
//				//If button is released after 2 second charge
//				if (targettingTimer <= (Time.time+1)) ActivateSheathe();
//				else ActivateTarget();
//			}

		//dodge
//		if (Input.GetButtonDown(evasion.name)) StartCoroutine(evasion.CheckInput(1)); // ChargeInput(evasion);
//		if (Input.GetButtonDown(stance.name)) StartCoroutine(stance.CheckInput(1));
//		
//		if (Input.GetButtonDown (mainHand.name)) {
//			if (Input.GetButton(special1.name)) StartCoroutine(special1.CheckInput(0));
//			if (Input.GetButton(special2.name)) StartCoroutine(special2.CheckInput(0));
//			if (Input.GetButton(special3.name)) StartCoroutine(special3.CheckInput(0));
//			else StartCoroutine(mainHand.CheckInput(1));
//		}
//		if (Input.GetButtonDown (offHand.name)) StartCoroutine(offHand.CheckInput(0));

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

		//			Debug.Log (name + " : buttonDown");
		//if (buttonDown != null) buttonDown();

		while (Input.GetButton(buttonName)) {
			while (_timer <= Time.time && time != 0 && _chargedToggle == false) {
				if (chargeActivate != null) {
					chargeActivate();							//input charged
					//						Debug.Log (name + " : buttonCharged");
				}
				_chargedToggle = true;
				yield return null;
				//break;	
			}
			//				Debug.Log (name + "button is held");
			yield return null;			
		}

		if (Input.GetButtonUp(buttonName) && _timer >= Time.time) {
			if (tapActivate != null) tapActivate();
			//				Debug.Log (name + " : buttonTapped");
		}
//		if (buttonUp != null) {
//			buttonUp();														//input is inactive (button up)
//			//				Debug.Log (name + " : buttonUp");			
//		}
		yield break;
	}
	
//	public class ButtonInput {
//		public string name;
//		
////		public delegate void InputActivated();
////		public delegate void TapInput();
////		public delegate void ChargeInput();
////		public delegate void InputDeactivated();
////		
////		public event InputActivated buttonDown;
////		public event TapInput buttonTapped;
////		public event ChargeInput buttonCharged;
////		public event InputDeactivated buttonUp;
//
//		public event Action buttonDown;
//		public event Action buttonTapped;
//		public event Action buttonCharged;
//		public event Action buttonUp;
//
//		public ButtonInput (string inputName) {
//			name = inputName;
//		}	
//		
//		public IEnumerator CheckInput (float time) {
//			bool _chargedToggle;
//			_chargedToggle = false;
//			float _timer;
//			if (time == 0) _timer = time;
//			else _timer = time + Time.time;
//			
//			//			Debug.Log (name + " : buttonDown");
//			if (buttonDown != null) buttonDown();
//			
//			while (Input.GetButton(name)) {
//				while (_timer <= Time.time && time != 0 && _chargedToggle == false) {
//					if (buttonCharged != null) {
//						buttonCharged();							//input charged
//						//						Debug.Log (name + " : buttonCharged");
//					}
//					_chargedToggle = true;
//					break;
//					//break;	
//				}
//				//				Debug.Log (name + "button is held");
//				yield return null;			
//			}
//			
//			if (_timer >= Time.time) {
//				if (buttonTapped != null) buttonTapped();
//				//				Debug.Log (name + " : buttonTapped");
//			}
//			if (buttonUp != null) {
//				buttonUp();														//input is inactive (button up)
//				//				Debug.Log (name + " : buttonUp");			
//			}
//			yield break;
//		}
//		
//		public IEnumerator CheckInput () {
//			//if (button.buttonHeld != null) 
//			buttonDown();
//			while (Input.GetButton(name)) {
//				yield return null;
//			}
//			buttonUp();
//		}
//	}
	
	[System.Serializable] public class PlayerTargetting {	
		protected ICharacter _user;																//character using targetting component
		
		protected Vector3 targetLocation;															//coordinate data of target (mouse or lock on for the player, player or random wandering spot for AI)
		protected BaseCharacter target;																//character being targetted by component
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
		public Vector3 UpdateMouse() {										//player aim
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
				if (hit.transform.GetComponent<BaseEnemy>()) {
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