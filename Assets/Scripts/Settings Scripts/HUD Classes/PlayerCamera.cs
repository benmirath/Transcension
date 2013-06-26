using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour {
	protected Transform camCoordinates;
	[SerializeField] protected BaseCharacter target;
	[SerializeField] protected float cameraHeight;

	//protected Transform camCoordinates;
	protected float camSize;

	protected float trackingX;
	protected float trackingY;
	protected float trackingSpeed;
	protected float minX;
	protected float maxX;
	protected float minZ;
	protected float maxZ;

//	protected Vector2 CameraCoordinates {
//		get {return Camera.main.WorldToScreenPoint(target.Coordinates.position)}
//	}

	/*protected Vector2 GetTargetCoordinates () {
		float c = Camera.mainCamera.WorldToScreenPoint(target.Coordinates.position);
		return c;
	}*/

	public void Awake () {	
		camSize = Camera.mainCamera.orthographicSize;
	}

	public void LateUpdate () {
		LockedCameraUpdate();
	}

	private void LockedCameraUpdate () {
		transform.position = new Vector3(target.Coordinates.position.x, target.Coordinates.position.y + cameraHeight, target.Coordinates.position.z);
		transform.LookAt (target.transform);
	}

	private void LooseCameraUpdate () {
		Vector3 temp = new Vector3();
//		temp = transform.position - target.Coordinates.position;
		
		if (temp.x > maxX) temp.x = maxX;
		if (temp.x < Mathf.Abs(minX)) temp.x = minX;
		
		if (temp.y > maxZ) temp.z = maxZ;
		if (temp.y < Mathf.Abs(minZ)) temp.z = minZ;
	}
}


/*
 *	Every turn
 *	-figure out player's current world coordinates
 *	-translate these to screen coordinates (c)
 *	--if diff of transform.x is greater than absolute value of maxX
 *	---move camera to maxX
 *	--if diff of transform.x is greater than absolute value of trackingX
 *	---move camera trackingSpeed towards (trackingX-transform.x normalized)
 *	--Same for Y
 *
 */