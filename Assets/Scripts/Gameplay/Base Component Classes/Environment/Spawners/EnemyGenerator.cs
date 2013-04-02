using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyGenerator : MonoBehaviour {
	public enum State {
		Idle,
		Initialize,
		Setup,
		Spawn
	}
	
	public GameObject[] enemyPrefabs;			//an array to hold all of the prefabs of enemies we want to spawn
	public GameObject[] spawnPoints;			//this array will hold a reference to all of the spawn in the scene
	
	public State state;							//this is our local variable that holds our current state
	
	void Awake() {
		state = EnemyGenerator.State.Initialize;	
	}
	
	// Use this for initialization
	IEnumerator Start () {
		while (true) {
			switch	(state) {
			case State.Initialize:
				Initialize ();
				break;
				
			case State.Setup:
				Setup ();
				break;
				
			case State.Spawn:
				Spawn ();
				break;
			}
			
			yield return 0;
		}
	}
	
	//make sure that everything is initialized before we go on to the next step
	private void Initialize() {
		Debug.Log("***We are in the Initialize function***");	
		
		if(!CheckForEnemyPrefabs())
			return;
		if(!CheckForSpawnPoints())
			return;
		
		state = EnemyGenerator.State.Setup;
	}
	
	//make sure that everything is set up before we continue
	private void Setup() {
		Debug.Log("***We are in the Setup function***");	
		state = EnemyGenerator.State.Spawn;
	}
	
	//spawn a mob if we have an open spawn point
	private void Spawn() {
		Debug.Log("***Spawn Enemy***");
		
		GameObject[] gos = AvailableSpawnPoints();
		
		for (int i = 0; i < gos.Length; i++) {
			GameObject go = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], 
										gos[i].transform.position, 
										Quaternion.identity) as GameObject;
			go.transform.parent = gos[i].transform;
		}
		
		state = EnemyGenerator.State.Idle;
	}
	
	//check to see that we have at least one enemy prefab to spawn
	private bool CheckForEnemyPrefabs() {
		if(enemyPrefabs.Length > 0)
			return true;	
		else 
			return false;
	}
	
	//check to see if we have at least one spawnpoint to spawn enemies at
	private bool CheckForSpawnPoints() {
		if(spawnPoints.Length > 0)
			return true;	
		else 
			return false;
	}
	
	//generate a list of available spawnpoints that do not have any enemies childed to it
	private GameObject[] AvailableSpawnPoints() {
		List<GameObject> gos = new List<GameObject>();
		
		for(int i = 0; i <spawnPoints.Length; i++) {
			if(spawnPoints[i].transform.childCount == 0) {
				Debug.Log("***Spawn Point Available***");
				gos.Add (spawnPoints[i]);
			}
		}
		
		return gos.ToArray();
	}
}
