/// <summary>
/// Base player script. Expands on the base character script, adding in more specific functionality for the player character.
/// This includes input (currently just from keyboard), targetting (may later get rolled into input script), and states. </summary>
using UnityEngine;
using System;
using System.Collections;

public class BasePlayer : BaseCharacter
{
	//[SerializeField] protected new CharState state;

	//protected PlayerTargetting _targetting;
	[SerializeField] protected PlayerInput charInput;
	
	#region Properties
	public override IInput CharInput {
		get {return charInput;}
	}

/*	public CharState CurrentState
	{
		get {return state;}
		set {state = value;}
	}
	public override string GetState 
	{
		get {return CurrentState.ToString();}
	}*/
	#endregion Properties
	
	#region Initialization
	protected override void Initialization ()
	{
		base.Initialization ();
		//charInput = new PlayerInput (this);
	}


	/// <summary>
	/// This is called once all components are initialized. This method then sets the values necessary for functioning. Currently
	/// used primarily for connecting inputs to their relevant events. </summary>
	protected override void Setup ()
	{
		base.Setup ();
		if (charInput == null) Debug.LogError("Input Is Null");

		charInput.Setup (this);

//		charInput.stance.buttonTapped += charInput.Targetting.LockOn;				//tab - tap - lock on
//		
//		charInput.stance.buttonCharged += SwitchCombatStances;						//tab - press - draw weapon
//
//		charInput.evasion.buttonTapped += UseDodge;				//command - tap = dodge
//		charInput.evasion.buttonCharged += StartRunning;//TransitionToRun;
//		charInput.evasion.buttonUp += StopRunning;//TransitionToWalk;
//		
//		charInput.mainHand.buttonDown += UsePrimary;	
//		charInput.offHand.buttonDown += UseSecondary;
//		
//		charInput.special1.buttonDown += UseSpecial1;
	}
	#endregion Initialization
	
	protected void Update ()
	{
//		Target = Target;
	}
	
	#region GUI
	private void ActivateLockOn()
	{
//		if (WeaponReady) charInput.Targetting.LockOn();
//		else return;
	}
/*	private void DrawWeapon()
	{
		if (CurrentState == CharState.Idle) 
		{
			if (WeaponReady == true) WeaponReady = false;
			else WeaponReady = true;
		}
		else return;
	}*/
	#endregion GUI
	
	#region State Transitions	
//	protected override void StateTransition()
//	{
//		string methodName = state.ToString () + "State";
//		Debug.Log (methodName);
//		System.Reflection.MethodInfo info = GetType().GetMethod(methodName,System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
//		Debug.LogWarning("activating method of name : " + methodName);
//		StartCoroutine((IEnumerator)info.Invoke(this, null));
//	}
//	protected void TransitionToStunned()
//	{
//		state = CharState.Stunned;
//	}
//
//	protected void TransitionToAttack () {
//		if (state == CharState.CombatReady) {
//			state = CharState.Attacking;
//		}
//	}

	//This will be abstract later on, with this version used only in derived classes that implement melee weapons
	protected void UsePrimary()
	{
		//if (!WeaponReady) DrawWeapon();
		//if (CurrentState == CharState.Idle || CurrentState == CharState.Walk) CurrentState = CharState.ComboAttack1;
		//if (GetState == BaseCharacter.CharState.CombatReady || this.IsEvading == true || this.IsRunning == true)
		//	state = 
		
/*		else if (CharEquipment.Primary.FollowupAvailable == true)										//Followup Attacks
		{
			//Activate FollowUp
			if (CurrentState == CharState.ComboAttack1)	CurrentState = CharState.ComboAttack2;			//Combo Attack 2
			else if (CurrentState == CharState.ComboAttack2) CurrentState = CharState.ComboAttack3;		//Combo Attack 3 (Finisher)
			else if (CurrentState == CharState.Run) CurrentState = CharState.RunAttack;					//Running Attack
			else if (CurrentState == CharState.Dodge) CurrentState = CharState.DodgeAttack;				//Dodging Attack
		}*/
		//else return;
	}
	protected void UseSecondary ()
	{

	}
	protected void UseSpecial1 () {
	
	}
	/*private void TransitionToOffHand()
	{
		if (state == CharState.Idle || state == CharState.Walk) state = CharState.Defend;
		else return;
	}
	private void TransitionToSpecial1()
	{
		if (state == CharState.Idle || state == CharState.Walk) state = CharState.Special1;
//		else if (CharEquipment.Primary.FollowupAvailable) CurrentState = CharState.Special1;
		
		else return;
	}
	protected void TransitionToDefend()
	{
		if (!WeaponReady) return;
		if (state == CharState.Idle || state == CharState.Walk) state = CharState.Defend;
		else return;
	}*/
	#endregion State Transition

	
	#region Internal State Setters
//	public void StartRunning () {
//		IsRunning = true;
//	}
//	public void StopRunning () {
//		IsRunning = false;
//	}
//	public void UseDodge () {
//		if (CharMovement.DodgeCost > CharStats.Stamina.CurValue) return;
//		else if (IsEvading == false) IsEvading = true;
//		else IsEvading = false;
//	}
////	public void UseDodge () {
////		if (CharMovement.DodgeCost < CharStats.Stamina.CurValue && (GetState == CharState.Idle || GetState == CharState.CombatReady)) StartCoroutine (CharMovement.Dodge());
////	}
//	public void BeginDefending () {
//		if (IsDefending == false) IsDefending = true;
//		else IsDefending = false;
//	}
	#endregion

	#region States
/*	public new enum CharState {
		Stunned,													//will disrupt character's ability to react when activated
		Idle,
		CombatReady,
		Walk,														
		Run,
		Dodge,														//will encapsulate any damage-mitigating movement state. 
		Defend,														//will encapsulate any damage-mitigating equipment state.
		Counter,													//will encapsulate any damage-mitigating counter-attack state.
		ComboAttack1,
		ComboAttack2,
		ComboAttack3,
		ComboAttack4,
		ComboAttack5,
		Finisher,
		RunAttack,
		DodgeAttack,
		Special1,
		Special2,
		Special3
	}*/
	
//	protected override IEnumerator IdleState () {					//Starting state, and where other states eventually return to. 
//		while (state == CharState.Idle) {
//			if (CharInput.MoveDir != Vector3.zero) {
//				if (IsEvading == true && CharMovement.DodgeCooldown < CharStats.Stamina.CurValue) yield return StartCoroutine (CharMovement.Dodge ());
//				else if (IsRunning == true && CharMovement.RunCost < CharStats.Stamina.CurValue) CharMovement.Run ();
//				else CharMovement.Walk ();
//			}
//
////			Debug.LogWarning("Velocity: " + Body.velocity);
////			Debug.LogWarning("WalkSpeed: " + CharMovement.WalkSpeed);
//			//Debug.LogWarning();
//			yield return null;
//		}
//		StateTransition ();
//		yield break;
//	}

//	protected override IEnumerator CombatReadyState () {
//		while (state == CharState.CombatReady) {
//
//			//if () {}						//if initiating an attack
//			if (CharInput.MoveDir != Vector3.zero) {
//				if (IsEvading == true) StartCoroutine (CharMovement.Dodge ());
//				else if (IsRunning == true && CharMovement.RunCost < CharStats.Stamina.CurValue) CharMovement.Run ();
//				else CharMovement.Strafe ();
//			}
//			else CharMovement.Aim();
//			yield return null;
//		}
//		StateTransition ();
//		yield break;
//	}

//	protected IEnumerator AttackingState () {
//		while (state == CharState.Attacking) {
//			//yield return StartCoroutine ();
//		}
//		yield break;
//	}
//
//	protected IEnumerator ComboAttack1State ()
//	{
//		yield break;
//	}
//	protected IEnumerator ComboAttack2State ()
//	{
//		yield break;
//	}
//	protected IEnumerator ComboAttack3State ()
//	{
//		yield break;
//	}
//	protected IEnumerator ComboAttack4State ()
//	{
//		yield break;
//	}
//	protected IEnumerator ComboAttack5State ()
//	{
//		yield break;
//	}
//	protected IEnumerator FinisherState ()
//	{
//		yield break;
//	}
//	protected IEnumerator RunAttackState ()
//	{
//		yield break;
//	}
//	protected IEnumerator DodgeAttackState ()
//	{
//		yield break;
//	}
//	protected IEnumerator Special1 ()
//	{
//		yield break;
//	}
//	protected IEnumerator Special2 ()
//	{
//		yield break;
//	}
//	protected IEnumerator Special3 ()
//	{
//		yield break;
//	}
	#endregion
	
//	protected override IEnumerator DecideAnimation () {
//		while (true) 
//		{
//			switch (state)
//			{
//			case CharState.Idle:
//				if (CharInput.MoveDir != Vector3.zero) {
//					Debug.LogWarning("Movement animation active");
////					CharAnimation3D.Play("Run", PlayMode.StopAll);
//				}
//				else {
//					Debug.LogWarning("idle animation active");
////					CharAnimation3D.Play("Idle", PlayMode.StopAll);
//				//CharacterAnimation.PlayNamedAnimation("Idle", false);				
//				}
//				break;
//				
///*			case CharState.Walk:
//				CharacterAnimation.PlayNamedAnimation("Walk", false);				
//				break;
//				
//			case CharState.Run:
//				CharacterAnimation.PlayNamedAnimation("Walk", false);								
//				break; 
//				
//			case CharState.Dodge:
//				//CharacterAnimation.PlayNamedAnimation("Walk", false);
//				break;
//				
//			case CharState.ComboAttack1:
//				//CharacterAnimation.PlayNamedAnimation("ComboAttack1", false);
//				break;
//				
//			case CharState.ComboAttack2:
//				//CharacterAnimation.PlayNamedAnimation("ComboAttack2", false);
//				break;
//				
//			case CharState.ComboAttack3:
//				//CharacterAnimation.PlayNamedAnimation("ComboAttack3", false);
//				break;
//				
//			case CharState.RunAttack:
//				break;
//				
//			case CharState.DodgeAttack:
//				break;
//				
//			case CharState.Special1:
//				//CharacterAnimation.PlayNamedAnimation("ComboAttack3", false);				
//				break;
//				
//			case CharState.Special2:
//				break;
//				
//			case CharState.Special3:
//				break;
//				
//			case CharState.Finisher:
//				break;
//				
//			case CharState.Defend:
//				//if (CharInput.MoveDir == Vector3.zero) CharacterAnimation.PlayNamedAnimation("Block", false);
//				//else CharacterAnimation.PlayNamedAnimation("WalkingBlock", false);
//				break;
//				
//			case CharState.Stunned:
//				break;
//			}
//			yield return null;
//		}
//	}

	private enum LoadoutType
	{
		OneHanded,										//Using a 1 handed weapon, and an offhand piece of equipment (shield, boltshooter)
		TwoHanded,										//Using a 2 handed weapon
		DualWielding									//Using a 1 handed weapon in both hands
	}

//	[System.Serializable] public class PlayerInput : IInput {
//		//Player inputs
//		private BasePlayer _user;
//		[SerializeField] private PlayerTargetting _targetting;
//
//		float x;							//Horizontal movement input.
//		float y;							//Vertical movement input.
//		[SerializeField] protected Vector3 dir;				//Overall movement input.
//
//		public PlayerTargetting Targetting {
//			get {return _targetting;}
//		}
//		public Vector3 MoveDir {
//			get {return dir;}
//		}
//		public Vector3 LookDir {
//			get {return _targetting.UpdateTargetting();}
//		}
//
//		//delegate methods called when their attached event is activate from player input
//		public ButtonInput stance;													//used for drawing weapon and locking on
//		public ButtonInput evasion;												//used for running and dodging
//		public ButtonInput mainHand;												//used for mainhand input
//		public ButtonInput offHand;												//used for offhand input
//		public ButtonInput special1;
//		public ButtonInput special2;
//		public ButtonInput special3;
//
//
//		// Use this for initialization
////		void Awake () {
////			_user = GetComponent<BasePlayer>();
////			//_targetting = GetComponent<PlayerTargetting>();
////
////		}
//
//		public PlayerInput () {
//			//_user = user;
//			stance = new ButtonInput("DrawWeapon");
//			evasion = new ButtonInput("Evasion");
//			mainHand = new ButtonInput("MainHand");
//			offHand = new ButtonInput("OffHand");
//			special1 = new ButtonInput("Special1");
//			special2 = new ButtonInput("Special2");
//			special3 = new ButtonInput("Special3");
//		}
//
//		public void Setup (BasePlayer user) {
//			_user = user;
//			_targetting.Setup(_user);
//			_user.StartCoroutine (UpdateInputs());
//		}
//	
//		// Update is called once per frame
//		private IEnumerator UpdateInputs () {
//			while (true) {
//				_targetting.UpdateTargetting();
//				dir = UpdateDirection();
//				
//				//dodge
//				if (Input.GetButtonDown(evasion.name)) _user.StartCoroutine(evasion.CheckInput(1)); // ChargeInput(evasion);
//				if (Input.GetButtonDown(stance.name)) _user.StartCoroutine(stance.CheckInput(1));
//				
//				if (Input.GetButtonDown (mainHand.name)) {
//					if (Input.GetButton(special1.name)) _user.StartCoroutine(special1.CheckInput(0));
//					if (Input.GetButton(special2.name)) _user.StartCoroutine(special2.CheckInput(0));
//					if (Input.GetButton(special3.name)) _user.StartCoroutine(special3.CheckInput(0));
//					else _user.StartCoroutine(mainHand.CheckInput(1));
//				}
//				if (Input.GetButtonDown (offHand.name)) _user.StartCoroutine(offHand.CheckInput(0));
//
//				yield return null;
//			}
//		}
//		
//		private Vector3 UpdateDirection ()
//		{
//			Vector3 d;
//			
//			if(Input.GetButton("Vertical") || Input.GetButton("Horizontal")) {
//				if (Input.GetAxis("Vertical") != 0) d.y = Input.GetAxis("Vertical");
//				else d.y = 0;
//
//				if (Input.GetAxis("Horizontal") != 0) d.x = Input.GetAxis("Horizontal");
//				else d.x = 0;
//				
//				d = new Vector3(d.x, d.y, 0);
//			}
//			else d = Vector3.zero;
//			
//			return d;
//		}
//		
//		public class ButtonInput
//		{
//			public string name;
//			
//			public delegate void InputActivated();
//			public delegate void TapInput();
//			public delegate void ChargeInput();
//			public delegate void InputDeactivated();
//			
//			public event InputActivated buttonDown;
//			public event TapInput buttonTapped;
//			public event ChargeInput buttonCharged;
//			public event InputDeactivated buttonUp;
//			
//			public ButtonInput (string inputName)
//			{
//				name = inputName;
//			}	
//			
//			public IEnumerator CheckInput (float time)
//			{
//				bool _chargedToggle;
//				_chargedToggle = false;
//				float _timer;
//				if (time == 0) _timer = time;
//				else _timer = time + Time.time;
//				
//				//			Debug.Log (name + " : buttonDown");
//				if (buttonDown != null) buttonDown();
//				
//				while (Input.GetButton(name))
//				{
//					while (_timer <= Time.time && time != 0 && _chargedToggle == false)
//					{
//						if (buttonCharged != null) 
//						{
//							buttonCharged();							//input charged
//							//						Debug.Log (name + " : buttonCharged");
//						}
//						_chargedToggle = true;
//						break;
//						//break;	
//					}
//					//				Debug.Log (name + "button is held");
//					yield return null;			
//				}
//				
//				if (_timer >= Time.time) 
//				{
//					if (buttonTapped != null) buttonTapped();
//					//				Debug.Log (name + " : buttonTapped");
//				}
//				if (buttonUp != null) 
//				{
//					buttonUp();														//input is inactive (button up)
//					//				Debug.Log (name + " : buttonUp");			
//				}
//				yield break;
//			}
//			
//			public IEnumerator CheckInput ()
//			{
//				//if (button.buttonHeld != null) 
//				buttonDown();
//				while (Input.GetButton(name)) {
//					yield return null;
//				}
//				buttonUp();
//			}
//		}
//
//		[System.Serializable] public class PlayerTargetting
//		{	
//			protected BaseCharacter _user;																//character using targetting component
//
//			protected Vector3 targetLocation;															//coordinate data of target (mouse or lock on for the player, player or random wandering spot for AI)
//			protected BaseCharacter target;																//character being targetted by component
//			protected BaseCharacter _potentialTarget;
//
//			protected bool _hasTarget;
//
//			[SerializeField] protected Transform reticle;						
//
//			public Vector3 Location {
//				get {return targetLocation;}
//				set {targetLocation = value;}
//			}
//
//			public void Setup (BasePlayer user) {
//				_user = user;
//				_hasTarget = false;
//				_potentialTarget = null;
//			}
//
//			//updates location of mouse cursor
//			private Vector3 UpdateMouse() {										//player aim
//				Plane playerPlane = new Plane(Vector3.back, _user.CharPhysics.Coordinates.position);
//				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
//				float hitdist = 0.0f;	
//				
//				Vector3 targetPoint;
//				
//				if (playerPlane.Raycast (ray, out hitdist)) targetPoint = ray.GetPoint(hitdist);
//				else targetPoint = Vector3.zero;
//				
//				return targetPoint;
//			}	
//			
//			//checks for target at targetting reticle, changing reticle color and lock-on ability accordingly
//			private bool CheckTarget () {
//				Ray ray;
//				RaycastHit hit;
//				ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//				if (Physics.Raycast(ray, out hit, 100)) {
//					if (hit.transform.GetComponent<BaseEnemy>()) {
//						if (hit.transform.GetComponent<CharacterController>()) {
//							if (_potentialTarget == null) _potentialTarget = hit.transform.GetComponent<BaseCharacter>();	
//							return true;
//						}
//					}
//					else _potentialTarget = null;
//					return false;
//				}
//				else _potentialTarget = null;
//				return false;
//			}
//			
//			/// <summary>
//			/// Locks onto target if CheckTarget returns true, and displays relevant lockon information.
//			/// </summary>
//			/// <param name='target'>
//			/// Target.
//			/// </param>
//			public void LockOn () {
//				Debug.LogWarning("attempting lockon");
//				if (_potentialTarget == null) {
//					_hasTarget = false;
//					target = null;
//					Debug.LogWarning("Lock on failed!");
//					//will remove any lock on info
//				}
//				else {
//					_hasTarget = true;
//					target = _potentialTarget;
//					Debug.LogWarning("Lock on succeeded!");
//					//sets up lock on info to dispay (name and health)
//				}
//			}
//
//			/// <summary>
//			/// Sets the current target of character
//			/// </summary>
//			/// <returns>The view target.</returns>
//			protected Vector3 SetViewTarget () {
//				if(_hasTarget == true) return target.CharPhysics.Coordinates.position;								//if locked on, will set location to target
//				else return UpdateMouse();																//if not locked on, will set location to mouse
//			}	
//
//			public Vector3 UpdateTargetting () {
//				CheckTarget();
//				if (CheckTarget() == true) reticle.renderer.material.color = Color.red;
//				else reticle.renderer.material.color = Color.white;
//
//				reticle.position = UpdateMouse();													//update targetting reticle location
//				targetLocation = SetViewTarget();
//				return targetLocation;
//			}
//		}
//	}
}
