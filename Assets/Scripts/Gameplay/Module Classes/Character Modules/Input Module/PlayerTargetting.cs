/*using UnityEngine;
using System.Collections;
/// <summary>
/// Player targetting class. Made up of several parts.
/// Part 1: Mouse control.
/// Part 2: 
/// </summary>
[RequireComponent (typeof(CapsuleCollider))]
public class PlayerTargetting : BaseTargetting
{	
	protected BaseCharacter _potentialTarget;
	protected Transform _reticle;
	
	//updates location of mouse cursor
	private Vector3 UpdateMouse() {										//player aim
		Plane playerPlane = new Plane(Vector3.back, _user.Coordinates.position);
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		float hitdist = 0.0f;	
		
		Vector3 targetPoint;
		
		if (playerPlane.Raycast (ray, out hitdist))
			targetPoint = ray.GetPoint(hitdist);
		else 
			targetPoint = Vector3.zero;
		
		return targetPoint;
	}	
	
	//checks for target at targetting reticle, changing reticle color and lock-on ability accordingly
	private bool CheckTarget ()
	{
		Ray ray;
		RaycastHit hit;
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hit, 100))
		{
			if (hit.transform.GetComponent<BaseEnemy>())
			{
				if (hit.transform.GetComponent<CharacterController>())
				{
					if (_potentialTarget == null)
					{
						_potentialTarget = hit.transform.GetComponent<BaseCharacter>();	
					}
					return true;
				}
			}
			else
			{
				_potentialTarget = null;
				return false;
			}
		}
		else
			_potentialTarget = null;
			return false;
	}

/*	private bool CheckTarget ()
	{
		if (Physics.Raycast(ray, out hit, 100))
		{
			if (hit.transform.tag == "Enemy")
			{
				if (_potentialTarget == null)
					_potentialTarget = hit.transform.GetComponent<BaseCharacter>();
				
				return true;	
			}
			else
			{
				_potentialTarget = null;
				return false;
			}
		}
		else
			_potentialTarget = null;
			return false;
	}*
	protected override Vector3 SetViewTarget ()
	{
		if(_hasTarget == true)																	//if locked on, will set location to target
			return _target.Coordinates.position;

		else
			return UpdateMouse();																//if not locked on, will set location to mouse
	}	
	
	public IEnumerator UpdateTargetting ()
	{
		while (true)
		{
			if (_user == null)
				Debug.LogError("User is null");
			CheckTarget();
			if (CheckTarget() == true)
			{
				renderer.material.color = Color.red;
				//if(_user.CurrentState == BaseCharacter.CharState.Target)
				//{
				//	LockOn(_potentialTarget);
				//}
			}
			else
			{
				renderer.material.color = Color.white;
			}
			_reticle.position = UpdateMouse();													//update targetting reticle location
			_location = SetViewTarget();
//			Debug.LogWarning("the targetting coordinate is : " + _location);			
			_user.LookDir = _location;
			yield return null;
		}
	}
	
	protected override void Awake ()
	{
		base.Awake ();
		_user = transform.parent.GetComponent<BaseCharacter>();		
		_potentialTarget = null;
		_reticle = GetComponent<Transform>();
//		_reticle.position = UpdateMouse();
	}
	
	/// <summary>
	/// Locks onto target if valid, and displays relevant lockon information.
	/// </summary>
	/// <param name='target'>
	/// Target.
	/// </param>
	public override void LockOn ()
	{
		Debug.LogWarning("attempting lockon");
		if (_potentialTarget == null)
		{
			_hasTarget = false;
			_target = null;
			Debug.LogWarning("Lock on failed!");

			//will remove any lock on info
		}
		else
		{
			_hasTarget = true;
			_target = _potentialTarget;
			Debug.LogWarning("Lock on succeeded!");

			//sets up lock on info to dispay (name and health)
		}
	}
}

*/