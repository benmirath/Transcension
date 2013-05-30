using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;
using System.Reflection;
using System.Diagnostics;
using System.Linq.Expressions;

/// <summary>
/// Base class for state machines
/// </summary>
public abstract class StateMachineBehaviour : MonoBehaviour {
	
	/// <summary>
	/// Should this object show GUI always, not only if it has
	/// state based GUI methods.  Set true if you write your
	/// own OnGUI function
	/// </summary>
	public bool useGUI;
	
	/// <summary>
	/// A coroutine executor that can be interrupted
	/// </summary>
	public class InterruptableCoroutine
	{
		IEnumerator enumerator;
		MonoBehaviour _behaviour;
		
		/// <summary>
		/// Coroutine info for running YieldInstructions as a separate coroutine
		/// </summary>
		private class CoroutineInfo
		{
			/// <summary>
			/// The instruction to execute
			/// </summary>
			public YieldInstruction instruction;
			/// <summary>
			/// Whether the coroutine is complete
			/// </summary>
			public bool done;
		}
		
		/// <summary>
		/// A coroutine that runs a single yield instruction
		/// </summary>
		/// <returns>
		/// The instruction coroutine.
		/// </returns>
		/// <param name='info'>
		/// The info packet for the coroutine to run
		/// </param>
		IEnumerator YieldInstructionCoroutine(CoroutineInfo info)
		{
			info.done = false;
			yield return info.instruction;
			info.done = true;
		}
		
		/// <summary>
		/// Waits for a yield instruction
		/// </summary>
		/// <returns>
		/// The coroutine to execute
		/// </returns>
		/// <param name='instruction'>
		/// The instruction to run
		/// </param>
		IEnumerator WaitForCoroutine(YieldInstruction instruction)
		{
			var ci = new CoroutineInfo {instruction = instruction, done = false };
			_behaviour.StartCoroutine(YieldInstructionCoroutine(ci));
			while(!ci.done)
				yield return null;
		}
		
		IEnumerator Run()
		{
			//Loop forever
			while(true)
			{
				//Check if we have a current coroutine
				if(enumerator != null)
				{
					//Make a copy of the enumerator in case it changes
					var enm = enumerator;
					//Execute the next step of the coroutine
					var valid = enumerator.MoveNext();
					//See if the enumerator has changed
					if(enm == enumerator)
					{
						//If this is the same enumerator
						if(enumerator != null)
						{
							//Get the result of the yield
							var result = enumerator.Current;
							//Check if it is a coroutine
							if(result is IEnumerator)
							{
								//Push the current coroutine and execute the new one
								_stack.Push(enumerator);
								enumerator = result as IEnumerator;
								yield return null;
							}
							//Check if it is a yield instruction
							else if(result is YieldInstruction)
							{
								//To be able to interrupt yield instructions
								//we need to run them as a separate coroutine
								//and wait for them
								_stack.Push(enumerator);
								//Create the coroutine to wait for the yieldinstruction
								enumerator = WaitForCoroutine(result as YieldInstruction);
								yield return null;
							}
							else
							{
								//Otherwise return the value
								yield return enumerator.Current;
							}
						}
						else
						{
							//If the enumerator was set to null then we
							//need to mark this as invalid
							valid = false;
							yield return null;
						}
						//Check if we are in a valid state
						if(!valid)
						{
							//If not then see if there are any stacked coroutines
							if(_stack.Count >= 1)
							{
								//Get the stacked coroutine back
								enumerator = _stack.Pop();
							}
							else
							{
								//Ensure we don't use this enumerator again
								enumerator = null;
							}
						}
					}
					else
					{
						//If the enumerator changed then just yield
						yield return null;
					}
				}
				else
				{
					//If the enumerator was null then just yield
					yield return null;
				}
			}
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="StateMachineBehaviour.InterruptableCoroutine"/> class.
		/// </summary>
		/// <param name='behaviour'>
		/// The behaviour on which the coroutines should run
		/// </param>
		public InterruptableCoroutine(MonoBehaviour behaviour)
		{
			_behaviour = behaviour;
			_behaviour.StartCoroutine(Run());
		}
		
		/// <summary>
		/// Stack of executing coroutines
		/// </summary>
		Stack<IEnumerator> _stack = new Stack<IEnumerator>();
		
		/// <summary>
		/// Call the specified coroutine
		/// </summary>
		/// <param name='enm'>
		/// The coroutine to call
		/// </param>
		public void Call(IEnumerator enm)
		{
			_stack.Push(enumerator);
			enumerator = enm;
		}
		
		/// <summary>
		/// Run the specified coroutine with an optional stack
		/// </summary>
		/// <param name='enm'>
		/// The coroutine to run
		/// </param>
		/// <param name='stack'>
		/// The stack that should be used for this coroutine
		/// </param>
		public void Run(IEnumerator enm, Stack<IEnumerator> stack = null)
		{
			enumerator = enm;
			if(stack != null)
			{
				_stack = stack;
			}
			else
			{
				_stack.Clear();
			}
			
		}
		
		/// <summary>
		/// Creates a new stack for executing coroutines
		/// </summary>
		/// <returns>
		/// The stack.
		/// </returns>
		public Stack<IEnumerator> CreateStack()
		{
			var current = _stack;
			_stack = new Stack<IEnumerator>();
			return current;
		}
		
		/// <summary>
		/// Cancel the current coroutine
		/// </summary>
		public void Cancel()
		{
			enumerator = null;
			_stack.Clear();
		}
		
		
	}
	
	/// <summary>
	/// Sends the message that called the function to the current state
	/// </summary>
	/// <param name='param'>
	/// Any parameter passed to the current handler that should be passed on
	/// </param>
	protected void SendStateMessage(params object[] param)
	{
		var message = currentState.ToString() + "_" + (new StackFrame(1)).GetMethod().Name;
		SendMessageEx(message, param);
	}
	
	static Dictionary<Type, Dictionary<string, MethodInfo>> _messages = new Dictionary<Type, Dictionary<string, MethodInfo>>();
	Dictionary<string, Action> _actions = new Dictionary<string, Action>();
	
	
	void SendMessageEx(string message, object[] param)
	{
		Action a = null;
		bool actionSpecified=false;
		if(_actions.TryGetValue(message, out a))
		{
			actionSpecified = true;
			if(a!=null)
			{
				a();
				return;
			}
		}
		
		
		MethodInfo d = null;
		Dictionary<string, MethodInfo> lookup = null;
		if(!_messages.TryGetValue(GetType(), out lookup))
		{
			lookup = new Dictionary<string, MethodInfo>();
			_messages[GetType()] = lookup;
		}
		
		if(!lookup.TryGetValue(message, out d))
		{
			d=GetType().GetMethod(message, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			lookup[message] = d;
		}
		
		if(d != null)
		{
			if(!actionSpecified)
			{
				if(d.GetParameters().Length == 0 && d.ReturnType == typeof(void))
				{
					var action = (Action)Delegate.CreateDelegate(typeof(Action), d, this);
					_actions[message] = action;
					action();
				}
				else
				{
					_actions[message]= null;
				}
			}
			else
				d.Invoke(this, param);

		}
		
	}
	
	/// <summary>
	/// The enter state coroutine.
	/// </summary>
	[HideInInspector]
	protected InterruptableCoroutine enterStateCoroutine;
	/// <summary>
	/// The exit state coroutine.
	/// </summary>
	[HideInInspector]
	protected InterruptableCoroutine exitStateCoroutine;
	
	/// <summary>
	/// The local transform - not the transform of the controlled object
	/// </summary>
	[HideInInspector]
	public Transform localTransform;
	/// <summary>
	/// The transform of the controlled object
	/// </summary>
	[HideInInspector]
	public new Transform transform;
	/// <summary>
	/// The rigidbody of the controlled object
	/// </summary>
	[HideInInspector]
	public new Rigidbody rigidbody;
	/// <summary>
	/// The animations of the controlled object
	/// </summary>
	[HideInInspector]
	public new Animation animation;
	/// <summary>
	/// The character controller of the controlled object
	/// </summary>
	[HideInInspector]
	public CharacterController controller;
	/// <summary>
	/// The network view of the controlled object
	/// </summary>
	[HideInInspector]
	public new NetworkView networkView;
	/// <summary>
	/// The collider of the controlled object
	/// </summary>
	[HideInInspector]
	public new Collider collider;
	/// <summary>
	/// The game object that is being controlled
	/// </summary>
	[HideInInspector]
	public new GameObject gameObject;
	/// <summary>
	/// The state machine of the controlled object
	/// </summary>
	[HideInInspector]
	public StateMachineBehaviour stateMachine;
		
	/// <summary>
	/// The time that the current state was entered
	/// </summary>
	private float _timeEnteredState;
	
	/// <summary>
	/// Gets the amount of time spent in the current state
	/// </summary>
	/// <value>
	/// The number of seconds in the current state
	/// </value>
	public float timeInCurrentState
	{
		get
		{
			return Time.time - _timeEnteredState;
		}
	}
	
	/// <summary>
	/// Gets the insistence level for the specified action
	/// </summary>
	/// <returns>
	/// The insistence floating point, 0 for no ability
	/// </returns>
	/// <param name='action'>
	/// The action to perform
	/// </param>
	public virtual float GetInsistence(string action)
	{
		return 0f;
	}
	
	/// <summary>
	/// Perform the specified action if possible.
	/// </summary>
	/// <param name='action'>
	/// The action to perform
	/// </param>
	public virtual bool OnPerform(string action)
	{
		return false;
	}
	
	/// <summary>
	/// Perform a specified action.
	/// </summary>
	/// <param name='action'>
	/// The action that should be performed
	/// </param>
	public bool Perform(string action)
	{
		return GetComponents<StateMachineBehaviour>().OrderByDescending(b=>b.GetInsistence(action)).First().OnPerform(action);
	}

	void Awake()
	{
		//Create the interruptable coroytines
		enterStateCoroutine = new InterruptableCoroutine(this);
		exitStateCoroutine = new InterruptableCoroutine(this);
		//Configure the initial state of the state machine
		state.executingStateMachine = stateMachine = this;
		//Cache the local objects transform
		localTransform = base.transform;
		//Represent the local object
		Represent(base.gameObject);
		//Perform addition awake behaviours as specified in sub classes
		OnAwake();
	}
	
	/// <summary>
	/// Represent the specified gameObject.  This objects core
	/// properties will relate to the specified gameObject after this
	/// call (e.g. transform, rigidbody etc)
	/// </summary>
	/// <param name='gameObjectToRepresent'>
	/// Game object to represent.
	/// </param>
	void Represent(GameObject gameObjectToRepresent)
	{
		if(transform != null && gameObject == gameObjectToRepresent)
			return;
		
		gameObject = gameObjectToRepresent;
		collider = gameObject.collider;
		transform = gameObject.transform;
		animation = gameObject.animation;
		rigidbody = gameObject.rigidbody;
		networkView = gameObject.networkView;
		controller = gameObject.GetComponent<CharacterController>();
		OnRepresent();
	}
	
	/// <summary>
	/// Recache the properties of the controlled instance
	/// in the case that they have changed
	/// </summary>
	public void Refresh()
	{
		var existing = gameObject;
		gameObject = null;
		Represent(existing);
	}
	
	/// <summary>
	/// Override to provide additional Awake actions
	/// </summary>
	protected virtual void OnAwake()
	{
	}
	
	/// <summary>
	/// Override to be notified when this state machine
	/// represents a gameobject
	/// </summary>
	protected virtual void OnRepresent()
	{
	}
	
	
	#region Default Implementations Of Delegates
	
	static IEnumerator DoNothingCoroutine()
	{
		yield break;
	}
	
	static void DoNothing()
	{
	}
	
	static void DoNothingCollider(Collider other)
	{
	}
	
	static void DoNothingCollision(Collision other)
	{
	}
	
	#endregion
	
	
	/// <summary>
	/// Class that represents the settings for a particular state
	/// </summary>
	public class State
	{
	
		public Action DoUpdate = DoNothing;
		public Action DoLateUpdate = DoNothing;
		public Action DoFixedUpdate = DoNothing;
		public Action<Collider> DoOnTriggerEnter = DoNothingCollider;
		public Action<Collider> DoOnTriggerStay = DoNothingCollider;
		public Action<Collider> DoOnTriggerExit = DoNothingCollider;
		public Action<Collision> DoOnCollisionEnter = DoNothingCollision;
		public Action<Collision> DoOnCollisionStay = DoNothingCollision;
		public Action<Collision> DoOnCollisionExit = DoNothingCollision;
		public Action DoOnMouseEnter = DoNothing;
		public Action DoOnMouseUp = DoNothing;
		public Action DoOnMouseDown = DoNothing;
		public Action DoOnMouseOver = DoNothing;
		public Action DoOnMouseExit = DoNothing;
		public Action DoOnMouseDrag = DoNothing;
		public Action DoOnGUI = DoNothing;
		public Func<IEnumerator> enterState = DoNothingCoroutine;
		public Func<IEnumerator> exitState = DoNothingCoroutine;
		public IEnumerator enterStateEnumerator = null;
		public IEnumerator exitStateEnumerator = null;
		
		public Enum currentState;
		//Stack of the enter state enumerators
		public Stack<IEnumerator> enterStack;
		//Stack of the exit state enumerators
		public Stack<IEnumerator> exitStack;
		//The amount of time that was spend in this state
		//when pushed to the stack
		public float time;
		
		public StateMachineBehaviour executingStateMachine;
	}
		
	/// <summary>
	/// The state of the current statemachine
	/// </summary>
	[HideInInspector]
	public State state = new State();
	
	/// <summary>
	/// Gets or sets the current state
	/// </summary>
	/// <value>
	/// The state to use
	/// </value>
	public Enum currentState
	{
		get
		{
			return state.currentState;
		}
		set
		{
			if(stateMachine != this)
			{
				stateMachine.currentState = value;
			}
			else
			{
			
				if(state.currentState == value)
					return;
				
				ChangingState();
				state.currentState = value;
				state.executingStateMachine.state.currentState = value;
				ConfigureCurrentState();				
			}
		}
	}
	
	[HideInInspector]
	/// <summary>
	/// The last state.
	/// </summary>
	public Enum lastState;
	[HideInInspector]
	/// <summary>
	/// The last state machine behaviour.
	/// </summary>
	public StateMachineBehaviour lastStateMachineBehaviour;
	
	/// <summary>
	/// Sets the state providing an injected statemachine behaviour
	/// </summary>
	/// <param name='stateToActivate'>
	/// State to activate.
	/// </param>
	/// <param name='useStateMachine'>
	/// The state machine behaviour to use for executing the state
	/// </param>
	public void SetState(Enum stateToActivate, StateMachineBehaviour useStateMachine)
	{
		if(state.executingStateMachine == useStateMachine && stateToActivate == state.currentState)
			return;
		
	
		ChangingState();
		state.currentState = stateToActivate;
		state.executingStateMachine = useStateMachine;
		
		if(useStateMachine != this)
		{
			useStateMachine.stateMachine = this;
		} 
		state.executingStateMachine.state.currentState = stateToActivate;
		useStateMachine.Represent(gameObject);
		ConfigureCurrentState();
	}
	
	//Stack of the previous running states
	private Stack<State> _stack = new Stack<State>();
	
	/// <summary>
	/// Call the specified state - activates the new state without deactivating the 
	/// current state.  Called states need to execute Return() when they are finished
	/// </summary>
	/// <param name='stateToActivate'>
	/// State to activate.
	/// </param>
	public void Call(Enum stateToActivate)
	{
		Call(stateToActivate, null);
	}
	
	/// <summary>
	/// Call the specified state - activates the new state without deactivating the 
	/// current state.  Called states need to execute Return() when they are finished.
	/// This version enables the injection of a called state from another state machine
	/// </summary>
	/// <param name='stateToActivate'>
	/// State to activate.
	/// </param>
	/// <param name='useStateMachine'>
	/// The state machine to use
	/// </param>
	public void Call(Enum stateToActivate, StateMachineBehaviour useStateMachine)
	{
		useStateMachine = useStateMachine ?? stateMachine;
		state.time = timeInCurrentState;
		state.enterStack = enterStateCoroutine.CreateStack();
		state.exitStack = exitStateCoroutine.CreateStack();
		ChangingState();
		
		_stack.Push(state);
		state = new State();
		state.currentState = stateToActivate;
		state.executingStateMachine = useStateMachine;
		
		if(useStateMachine != this)
		{
			useStateMachine.stateMachine = this;
		} 
		state.executingStateMachine.state.currentState = stateToActivate;
		useStateMachine.Represent(gameObject);
		ConfigureCurrentStateForCall();
	}
	
	//Configures the state machine when the new state has been called
	void ConfigureCurrentStateForCall()
	{
		GetStateMethods();
		if(state.enterState != null)
		{
			state.enterStateEnumerator = state.enterState();
			enterStateCoroutine.Run(state.enterStateEnumerator);
		}
	}
	
	/// <summary>
	/// Return this state from a call
	/// </summary>
	public void Return()
	{
		if(stateMachine != this)
		{
			stateMachine.Return();
			return;
		}
		if(state.exitState != null)
		{
			state.exitStateEnumerator = state.exitState();
			exitStateCoroutine.Run(state.exitStateEnumerator);
		}
		if(_stack.Count > 0)
		{
			state = _stack.Pop();
			enterStateCoroutine.Run(state.enterStateEnumerator, state.enterStack);
			_timeEnteredState = Time.time - state.time;
		}
	}
	
	/// <summary>
	/// Return the state from a call with a specified state to 
	/// enter if this state wasn't called
	/// </summary>
	/// <param name='baseState'>
	/// The state to use if there is no waiting calling state
	/// </param>
	public void Return(Enum baseState)
	{
		if(stateMachine != this)
		{
			stateMachine.Return(baseState);
			return;
		}
		if(state.exitState != null)
		{
			state.exitStateEnumerator = state.exitState();
			exitStateCoroutine.Run(state.exitStateEnumerator);

		}
		if(_stack.Count > 0)
		{
			state = _stack.Pop();
			enterStateCoroutine.Run(state.enterStateEnumerator, state.enterStack);
		
		}
		else
		{
			currentState = baseState;
		}
		_timeEnteredState = Time.time - state.time;
	}

	
	/// <summary>
	/// Caches previous states
	/// </summary>
	void ChangingState()
	{
		lastState = state.currentState;
		lastStateMachineBehaviour = state.executingStateMachine;
		_timeEnteredState = Time.time;
	}
	
	/// <summary>
	/// Configures the state machine for the current state
	/// </summary>
	void ConfigureCurrentState()
	{

		if(state.exitState != null)
		{
			exitStateCoroutine.Run(state.exitState());
		}
		
		GetStateMethods();
				
		if(state.enterState != null)
		{
			enterStateCoroutine.Run(state.enterState());
		}
		
		
	}
	
	//Retrieves all of the methods for the current state
	void GetStateMethods()
	{
		//Now we need to configure all of the methods
		state.DoUpdate = state.executingStateMachine.ConfigureDelegate<Action>("Update", DoNothing);
		state.DoOnGUI = state.executingStateMachine.ConfigureDelegate<Action>("OnGUI", DoNothing);
		state.DoLateUpdate = state.executingStateMachine.ConfigureDelegate<Action>("LateUpdate", DoNothing);
		state.DoFixedUpdate = state.executingStateMachine.ConfigureDelegate<Action>("FixedUpdate", DoNothing);
		state.DoOnMouseUp = state.executingStateMachine.ConfigureDelegate<Action>("OnMouseUp", DoNothing);
		state.DoOnMouseDown = state.executingStateMachine.ConfigureDelegate<Action>("OnMouseDown", DoNothing);
		state.DoOnMouseEnter = state.executingStateMachine.ConfigureDelegate<Action>("OnMouseEnter", DoNothing);
		state.DoOnMouseExit = state.executingStateMachine.ConfigureDelegate<Action>("OnMouseExit", DoNothing);
		state.DoOnMouseDrag = state.executingStateMachine.ConfigureDelegate<Action>("OnMouseDrag", DoNothing);
		state.DoOnMouseOver = state.executingStateMachine.ConfigureDelegate<Action>("OnMouseOver", DoNothing);
		state.DoOnTriggerEnter = state.executingStateMachine.ConfigureDelegate<Action<Collider>>("OnTriggerEnter", DoNothingCollider);
		state.DoOnTriggerExit = state.executingStateMachine.ConfigureDelegate<Action<Collider>>("OnTriggerExit", DoNothingCollider);
		state.DoOnTriggerStay = state.executingStateMachine.ConfigureDelegate<Action<Collider>>("OnTriggerEnter", DoNothingCollider);
		state.DoOnCollisionEnter = state.executingStateMachine.ConfigureDelegate<Action<Collision>>("OnCollisionEnter", DoNothingCollision);
		state.DoOnCollisionExit = state.executingStateMachine.ConfigureDelegate<Action<Collision>>("OnCollisionExit", DoNothingCollision);
		state.DoOnCollisionStay = state.executingStateMachine.ConfigureDelegate<Action<Collision>>("OnCollisionStay", DoNothingCollision);
		state.enterState = state.executingStateMachine.ConfigureDelegate<Func<IEnumerator>>("EnterState", DoNothingCoroutine);
		state.exitState = state.executingStateMachine.ConfigureDelegate<Func<IEnumerator>>("ExitState", DoNothingCoroutine);
		EnableGUI();
	}
	
	/// <summary>
	/// A cache of the delegates for a particular state and method
	/// </summary>
	Dictionary<Enum, Dictionary<string, Delegate>> _cache = new Dictionary<Enum, Dictionary<string, Delegate>>();
	
	//Creates a delegate for a particular method on the current state machine
	//if a suitable method is not found then the default is used instead
	T ConfigureDelegate<T>(string methodRoot, T Default) where T : class
	{
		
		Dictionary<string, Delegate> lookup;
		if(!_cache.TryGetValue(state.currentState, out lookup))
		{
			_cache[state.currentState] = lookup = new Dictionary<string, Delegate>();
		}
		Delegate returnValue;
		if(!lookup.TryGetValue(methodRoot, out returnValue))
		{
		
			var mtd = GetType().GetMethod(state.currentState.ToString() + "_" + methodRoot, System.Reflection.BindingFlags.Instance 
				| System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.InvokeMethod);
			
			if(mtd != null)
			{
				if(typeof(T) == typeof(Func<IEnumerator>) && mtd.ReturnType != typeof(IEnumerator))
				{
					Action a = Delegate.CreateDelegate(typeof(Action), this, mtd) as Action;
					Func<IEnumerator> func = () => { a(); return null; };
					returnValue = func;
				}
				else
					returnValue = Delegate.CreateDelegate(typeof(T), this, mtd);
			}
			else
			{
				returnValue = Default as Delegate;
			}
			lookup[methodRoot] = returnValue;
		}
		return returnValue as T;
		
	}
	
	
	/// <summary>
	/// Enables the GUI
	/// </summary>
	protected void EnableGUI()
	{
		useGUILayout = useGUI || state.DoOnGUI != DoNothing;
	}
	
	#region Pass On Methods
	
	void Update () 
	{
		state.DoUpdate();
	}
	
	void LateUpdate() 
	{
		state.DoLateUpdate();
	}
	
	void OnMouseEnter()
	{
		state.DoOnMouseEnter();
	}
	
	void OnMouseUp()
	{
		state.DoOnMouseUp();
	}
	
	void OnMouseDown()
	{
		state.DoOnMouseDown();
	}
	
	void OnMouseExit()
	{
		state.DoOnMouseExit();
	}
	
	void OnMouseDrag()
	{
		state.DoOnMouseDrag();
	}
	
	void FixedUpdate()
	{
		state.DoFixedUpdate();
	}
	void OnTriggerEnter(Collider other)
	{
		state.DoOnTriggerEnter(other);
	}
	void OnTriggerExit(Collider other)
	{
		state.DoOnTriggerExit(other);
	}
	void OnTriggerStay(Collider other)
	{
		state.DoOnTriggerStay(other);
	}
	void OnCollisionEnter(Collision other)
	{
		state.DoOnCollisionEnter(other);
	}
	void OnCollisionExit(Collision other)
	{
		state.DoOnCollisionExit(other);
	}
	void OnCollisionStay(Collision other)
	{
		state.DoOnCollisionStay(other);
	}
	void OnGUI()
	{
		state.DoOnGUI();
	}
	
	#endregion
	
	/// <summary>
	/// Waits for an animation.
	/// </summary>
	/// <returns>
	/// A coroutine that waits for the animation
	/// </returns>
	/// <param name='name'>
	/// The name of the animation
	/// </param>
	/// <param name='ratio'>
	/// The 0..1 ratio to wait for
	/// </param>
	public IEnumerator WaitForAnimation(string name, float ratio)
	{
		var state = animation[name];
		return WaitForAnimation(state, ratio);
	}
	
	/// <summary>
	/// Waits for an animation.
	/// </summary>
	/// <returns>
	/// A coroutine that waits for the animation
	/// </returns>
	/// <param name='state'>
	/// The animation state to wait for
	/// </param>
	/// <param name='ratio'>
	/// The 0..1 ratio to wait for
	/// </param>
	public static IEnumerator WaitForAnimation(AnimationState state, float ratio)
	{
		state.wrapMode = WrapMode.ClampForever;
		state.enabled = true;
		state.speed = state.speed == 0 ? 1 : state.speed;
		var t = state.time;
		while((t/state.length) + float.Epsilon < ratio)
		{
			t += Time.deltaTime * state.speed;
			yield return null;
		}
	}
	
	/// <summary>
	/// Plays the named animation and waits for completion
	/// </summary>
	/// <returns>
	/// A coroutine that waits for the animation
	/// </returns>
	/// <param name='name'>
	/// The name of the animation to play
	/// </param>
	public IEnumerator PlayAnimation(string name)
	{
		var state = animation[name];
		return PlayAnimation(state);
	}
	
	/// <summary>
	/// Plays the named animation and waits for completion
	/// </summary>
	/// <returns>
	/// A coroutine that waits for the animation
	/// </returns>
	/// <param name='state'>
	/// The animation state to play
	/// </param>
	public static IEnumerator PlayAnimation(AnimationState state)
	{
		state.time = 0;
		state.weight = 1;
		state.speed = 1;
		state.enabled = true;
		var wait = WaitForAnimation(state, 1f);
		while(wait.MoveNext())
			yield return null;
		state.weight = 0;
		
	}
	
	/// <summary>
	/// Waits for the attached object to reach within 1 world unit of a position.
	/// </summary>
	/// <returns>
	/// A coroutine to wait for the position
	/// </returns>
	/// <param name='position'>
	/// The position to wait for
	/// </param>
	public IEnumerator WaitForPosition(Vector3 position)
	{
		return WaitForPosition(position, 1f);
	}
	
	/// <summary>
	/// Waits for the attached object to reach a position.
	/// </summary>
	/// <returns>
	/// A coroutine to wait for the object
	/// </returns>
	/// <param name='position'>
	/// The position to attain
	/// </param>
	/// <param name='accuracy'>
	/// The accuracy with which the object must reach the position
	/// </param>
	public IEnumerator WaitForPosition(Vector3 position, float accuracy)
	{
		var accuracySq = accuracy * accuracy;
		while((transform.position - position).sqrMagnitude > accuracySq)
			yield return null;
	}
	
	/// <summary>
	/// Waits for the object to achieve a particular rotation
	/// </summary>
	/// <returns>
	/// A coroutine to wait for the object
	/// </returns>
	/// <param name='rotation'>
	/// The rotation to achieve
	/// </param>
	public IEnumerator WaitForRotation(Vector3 rotation)
	{
		return WaitForRotation(rotation, Vector3.one);
	}
	
	/// <summary>
	/// Waits for the object to achieve a particular rotation within 1 degree of the target
	/// </summary>
	/// <returns>
	/// A coroutine to wait for the object
	/// </returns>
	/// <param name='rotation'>
	/// The rotation to achieve
	/// </param>
	/// <param name='mask'>
	/// A Vector mask to indicate which axes are important (can also be fractional)
	/// </param>
	public IEnumerator WaitForRotation(Vector3 rotation, Vector3 mask)
	{
		return WaitForRotation(rotation, mask, 1f);
	}
	
	/// <summary>
	/// Waits for the object to achieve a particular rotation
	/// </summary>
	/// <returns>
	/// A coroutine to wait for the object
	/// </returns>
	/// <param name='rotation'>
	/// The rotation to achieve
	/// </param>
	/// <param name='mask'>
	/// A Vector mask to indicate which axes are important (can also be fractional)
	/// </param>
	/// <param name='accuracy'>
	/// The value to which the rotation must be within
	/// </param>
	public IEnumerator WaitForRotation(Vector3 rotation, Vector3 mask, float accuracy)
	{
		var accuracySq = accuracy * accuracy	;
		if(accuracySq == 0)
			accuracySq = float.Epsilon;
		rotation.Scale(mask);
		
		while(true)
		{
			var currentAngles = transform.rotation.eulerAngles;
			currentAngles.Scale(mask);
			if((currentAngles - rotation).sqrMagnitude <= accuracySq)
				yield break;
			yield return null;
		}
	}
	
	/// <summary>
	/// Moves an object over a period of time
	/// </summary>
	/// <returns>
	/// A coroutine that moves the object
	/// </returns>
	/// <param name='objectToMove'>
	/// The Transform of the object to move.
	/// </param>
	/// <param name='position'>
	/// The destination position
	/// </param>
	/// <param name='time'>
	/// The number of seconds that the move should take
	/// </param>
	public IEnumerator MoveObject(Transform objectToMove, Vector3 position, float time)
	{
		return MoveObject(objectToMove, position, time, EasingType.Quadratic);
	}
	
	/// <summary>
	/// Moves an object over a period of time
	/// </summary>
	/// <returns>
	/// A coroutine that moves the object
	/// </returns>
	/// <param name='objectToMove'>
	/// The Transform of the object to move.
	/// </param>
	/// <param name='position'>
	/// The destination position
	/// </param>
	/// <param name='time'>
	/// The number of seconds that the move should take
	/// </param>
	/// <param name='ease'>
	/// The easing function to use when moving the object
	/// </param>
	public IEnumerator MoveObject(Transform objectToMove, Vector3 position, float time, EasingType ease)
	{
		var t = 0f;
		var pos = objectToMove.position;
		while(t < 1f)
		{
			objectToMove.position = Vector3.Lerp(pos, position, Easing.EaseInOut(t, ease));
			t += Time.deltaTime/time;
			yield return null;
		}
		objectToMove.position = position;
	}
	
}


/// <summary>
/// State machine extension methods to retrieve state machines from objects
/// </summary>
public static class StateMachineExtensions
{
	public static StateMachineBehaviour GetStateMachine(this Component component)
	{
		return component.GetComponent<StateMachineBehaviour>();
	}
	
	public static StateMachineBehaviour GetStateMachine(this GameObject gameObject)
	{
		return gameObject.GetComponent<StateMachineBehaviour>();
	}
}