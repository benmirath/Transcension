using UnityEngine;
using System.Collections;

[System.Serializable] public class PlayerInput : BaseCharacter.IInput {
	//Player inputs
	private BasePlayer _user;
	[SerializeField] private PlayerTargetting _targetting;
	
	private float x;							//Horizontal movement input.
	private float y;							//Vertical movement input.
	[SerializeField] protected Vector3 dir;				//Overall movement input.
	
	public PlayerTargetting Targetting {
		get {return _targetting;}
	}
	public Vector3 MoveDir {
		get {return dir;}
	}
	public Vector3 LookDir {
		get {return _targetting.UpdateTargetting();}
	}
	
	//delegate methods called when their attached event is activate from player input
	public ButtonInput stance;													//used for drawing weapon and locking on
	public ButtonInput evasion;												//used for running and dodging
	public ButtonInput mainHand;												//used for mainhand input
	public ButtonInput offHand;												//used for offhand input
	public ButtonInput special1;
	public ButtonInput special2;
	public ButtonInput special3;
	
	public PlayerInput () {
		//_user = user;
		stance = new ButtonInput("DrawWeapon");
		evasion = new ButtonInput("Evasion");
		mainHand = new ButtonInput("MainHand");
		offHand = new ButtonInput("OffHand");
		special1 = new ButtonInput("Special1");
		special2 = new ButtonInput("Special2");
		special3 = new ButtonInput("Special3");
	}
	
	public void Setup (BasePlayer user) {
		_user = user;
		_targetting.Setup(_user);
		_user.StartCoroutine (UpdateInputs());
	}
	
	// Update is called once per frame
	private IEnumerator UpdateInputs () {
		while (true) {
			_targetting.UpdateTargetting();
			dir = UpdateDirection();
			
			//dodge
			if (Input.GetButtonDown(evasion.name)) _user.StartCoroutine(evasion.CheckInput(1)); // ChargeInput(evasion);
			if (Input.GetButtonDown(stance.name)) _user.StartCoroutine(stance.CheckInput(1));
			
			if (Input.GetButtonDown (mainHand.name)) {
				if (Input.GetButton(special1.name)) _user.StartCoroutine(special1.CheckInput(0));
				if (Input.GetButton(special2.name)) _user.StartCoroutine(special2.CheckInput(0));
				if (Input.GetButton(special3.name)) _user.StartCoroutine(special3.CheckInput(0));
				else _user.StartCoroutine(mainHand.CheckInput(1));
			}
			if (Input.GetButtonDown (offHand.name)) _user.StartCoroutine(offHand.CheckInput(0));
			
			yield return null;
		}
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
	
	public class ButtonInput {
		public string name;
		
		public delegate void InputActivated();
		public delegate void TapInput();
		public delegate void ChargeInput();
		public delegate void InputDeactivated();
		
		public event InputActivated buttonDown;
		public event TapInput buttonTapped;
		public event ChargeInput buttonCharged;
		public event InputDeactivated buttonUp;
		
		public ButtonInput (string inputName) {
			name = inputName;
		}	
		
		public IEnumerator CheckInput (float time) {
			bool _chargedToggle;
			_chargedToggle = false;
			float _timer;
			if (time == 0) _timer = time;
			else _timer = time + Time.time;
			
			//			Debug.Log (name + " : buttonDown");
			if (buttonDown != null) buttonDown();
			
			while (Input.GetButton(name)) {
				while (_timer <= Time.time && time != 0 && _chargedToggle == false) {
					if (buttonCharged != null) {
						buttonCharged();							//input charged
						//						Debug.Log (name + " : buttonCharged");
					}
					_chargedToggle = true;
					break;
					//break;	
				}
				//				Debug.Log (name + "button is held");
				yield return null;			
			}
			
			if (_timer >= Time.time) {
				if (buttonTapped != null) buttonTapped();
				//				Debug.Log (name + " : buttonTapped");
			}
			if (buttonUp != null) {
				buttonUp();														//input is inactive (button up)
				//				Debug.Log (name + " : buttonUp");			
			}
			yield break;
		}
		
		public IEnumerator CheckInput () {
			//if (button.buttonHeld != null) 
			buttonDown();
			while (Input.GetButton(name)) {
				yield return null;
			}
			buttonUp();
		}
	}
	
	[System.Serializable] public class PlayerTargetting {	
		protected BaseCharacter _user;																//character using targetting component
		
		protected Vector3 targetLocation;															//coordinate data of target (mouse or lock on for the player, player or random wandering spot for AI)
		protected BaseCharacter target;																//character being targetted by component
		protected BaseCharacter _potentialTarget;
		
		protected bool _hasTarget;
		
		[SerializeField] protected Transform reticle;						
		
		public Vector3 Location {
			get {return targetLocation;}
			set {targetLocation = value;}
		}
		
		public void Setup (BasePlayer user) {
			_user = user;
			_hasTarget = false;
			_potentialTarget = null;
		}
		
		//updates location of mouse cursor
		private Vector3 UpdateMouse() {										//player aim
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