using UnityEngine;
using System.Collections;

public class MoodIndicator : MonoBehaviour {
	
	public Texture2D[] moodIndicators;
	public Color happyColor = Color.green;
	public Color angryColor = Color.red;
	public float fadeTime = 4f;
	public EnemyMood enemy;
	
	Transform _transform;
	Transform _cameraTransform;
	Material _material;
	Color _color;
	Quaternion _pointUpAtForward;
	
	void Start () {
		
		_pointUpAtForward = Quaternion.FromToRotation(Vector3.up, Vector3.forward);
		
		_material = renderer.material;
		
		//Set the graphic
		_material.mainTexture = moodIndicators[
			Mathf.Clamp(
				Mathf.RoundToInt( enemy.mood/(100/moodIndicators.Length)),
				0,
				moodIndicators.Length-1)
			];
		
		//Calculate the color
		var moodRatio = enemy.mood/100;
		_material.color = _color = new Color(
			angryColor.r * (1 - moodRatio) + happyColor.r * moodRatio,
			angryColor.g * (1 - moodRatio) + happyColor.g * moodRatio,
			angryColor.b * (1 - moodRatio) + happyColor.b * moodRatio
		);
		
		Update();
	}
	
	void Awake()
	{
		_transform = transform;
		_cameraTransform = Camera.main.transform;
	}
	
	public void Update () {
		
		//Point a plane at the camera
		_transform.rotation = Quaternion.LookRotation(-_cameraTransform.forward, Vector3.up)
			* _pointUpAtForward;
		
		//Fade out the graphic
		_color.a -= Time.deltaTime/fadeTime;
		_material.color = _color;
	}
}
