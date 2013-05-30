using UnityEngine;
using System.Collections;

public class EnemyMood : MonoBehaviour {

	public MoodIndicator moodIndicatorPrefab;
	public float mood;
	
	Transform _transform;
	MoodIndicator _currentIndicator;
	
	void Start () {
		_transform = transform;
		mood = Random.Range(30,99);
	}
	
	void Update () {
		mood -= Time.deltaTime/2;
	}

	
	void LookedAt()
	{
		if(_currentIndicator)
			return;
		
		_currentIndicator = Instantiate(moodIndicatorPrefab, _transform.position + Vector3.up * 3.5f,
			Quaternion.identity) as MoodIndicator;
		
		_currentIndicator.enemy = this;
	}
	
}
