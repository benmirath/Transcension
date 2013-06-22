using UnityEngine;
using System.Collections;
using System;

public class CharacterGenerator : MonoBehaviour {
//	private BasePlayer _player;
	private const int STARTING_POINTS = 350;
	private const int MIN_STARTING_ATTRIBUTE_VALUE = 10;
	private const int STARTING_VALUE = 50;
	private int pointsLeft;
	
	private const int OFFSET = 5;
	private const int LINE_HEIGHT = 20;
	
	private const int STAT_LABEL_WIDTH = 100;
	private const int BASEVALUE_LABEL_WIDTH = 30;
	private const int BUTTON_WIDTH = 20;
	private const int BUTTON_HEIGHT = 20;
	
	private int statStartingPos = 40;
	
	public GameObject playerPrefab;
	
	// Use this for initialization
	void Start () {
		GameObject pc = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity) as GameObject;
		pc.name = "pc";
		
		//_player = new PlayerCharacter();
		//_player.Awake();
//		_player = pc.GetComponent<BasePlayer>();
				
		pointsLeft = STARTING_POINTS;
		
/*		for(int i = 0; i < Enum.GetValues(typeof(AttributeName)).Length; i++) {
//			_player.GetPrimaryAttribute(i).BaseValue = STARTING_VALUE;
			pointsLeft -= (STARTING_VALUE - MIN_STARTING_ATTRIBUTE_VALUE);
		} 
//		_player.StatUpdate(); */
	}		 
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI() {
		DisplayName();
		DisplayPointsLeft();
		
//		DisplayAttributes();
//		DisplayVitals();
//		DisplaySkills();
		
////		if(_player.name == "" || pointsLeft > 0)
//			DisplayCreateLabel();
//		else
//			DisplayCreateButton();
	}

	private void DisplayName () {
		GUI.Label(new Rect(10, 10, 50, 25), "Name:");
//		_player.name = GUI.TextField (new Rect(65, 10, 100, 25), _player.name);
	}
	
/*	private void DisplayAttributes () {
		for(int i = 0; i < Enum.GetValues(typeof(AttributeName)).Length; i++) {
			//Attribute Names
			GUI.Label (new Rect 		(	OFFSET, 															//x
											statStartingPos + (i * LINE_HEIGHT),			 					//y
											STAT_LABEL_WIDTH, 													//width
											LINE_HEIGHT															//height
										), ((AttributeName)i).ToString() );
			
			//Attribute Values
			GUI.Label (new Rect 		(	STAT_LABEL_WIDTH + OFFSET, 											//x
											statStartingPos + (i * LINE_HEIGHT), 								//y
											BASEVALUE_LABEL_WIDTH, 												//width
											LINE_HEIGHT															//height
										), _player.GetPrimaryAttribute(i).AdjustedBaseValue.ToString());
			
			//Attribute modifier buttons
			if (GUI.Button (new Rect 	(	OFFSET + STAT_LABEL_WIDTH + BASEVALUE_LABEL_WIDTH,  				//x
											statStartingPos + (i * BUTTON_HEIGHT), 								//y	
											BUTTON_WIDTH, 														//width
											BUTTON_HEIGHT														//height
										), "-")) {
				if(_player.GetPrimaryAttribute(i).BaseValue > MIN_STARTING_ATTRIBUTE_VALUE) {
					_player.GetPrimaryAttribute (i).BaseValue--;
					pointsLeft++;
					_player.StatUpdate();
				}
			}
			
			if (GUI.Button (new Rect 	(	OFFSET + STAT_LABEL_WIDTH + BASEVALUE_LABEL_WIDTH  + BUTTON_WIDTH,  //x
											statStartingPos + (i * BUTTON_HEIGHT), 								//y
											BUTTON_WIDTH, 														//width
											BUTTON_HEIGHT														//height
										), "+")) {
				if(pointsLeft > 0) {
					_player.GetPrimaryAttribute (i).BaseValue++;
					pointsLeft--;
					_player.StatUpdate();
				}
			}			
		}
	}*/
	
/*	private void DisplayVitals () {
		for(int i = 0; i < Enum.GetValues(typeof(VitalName)).Length; i++) {
			GUI.Label (new Rect 	(	OFFSET, 
										statStartingPos + ((i + 4) * LINE_HEIGHT), 
										STAT_LABEL_WIDTH, 
										LINE_HEIGHT
									), ((VitalName)i).ToString() );
			
			
			GUI.Label (new Rect 	(	OFFSET + STAT_LABEL_WIDTH, 
										statStartingPos + ((i + 4) * LINE_HEIGHT), 
										BASEVALUE_LABEL_WIDTH, 
										LINE_HEIGHT
									), _player.GetVital(i).AdjustedBaseValue.ToString());
		}
	}*/
	
/*	private void DisplaySkills () {
		for(int i = 0; i < Enum.GetValues(typeof(SkillName)).Length; i++) {
			GUI.Label (new Rect 	(	OFFSET * 3 + STAT_LABEL_WIDTH + BASEVALUE_LABEL_WIDTH + BUTTON_WIDTH * 2, 
										statStartingPos + (i * LINE_HEIGHT), 
										STAT_LABEL_WIDTH, 
										LINE_HEIGHT
									), ((SkillName)i).ToString() );
			
			GUI.Label (new Rect 	(	OFFSET * 3 + STAT_LABEL_WIDTH * 2 + BASEVALUE_LABEL_WIDTH + BUTTON_WIDTH * 2, 
										statStartingPos + (i * LINE_HEIGHT), 
										BASEVALUE_LABEL_WIDTH, 
										LINE_HEIGHT
									), _player.GetSkill(i).AdjustedBaseValue.ToString());
		}
	}*/
	
	private void DisplayPointsLeft () {
		GUI.Label(new Rect(250, 10, 100, 25), "Points Left: " + pointsLeft.ToString());
	}
	
	private void DisplayCreateLabel () {
		GUI.Label(new Rect			(	Screen.width/2 - 50, 
										statStartingPos + (10 * LINE_HEIGHT), 
										STAT_LABEL_WIDTH,
										LINE_HEIGHT
									), "Creating...", "Button"); 
	}
	
	private void DisplayCreateButton () {
		if(GUI.Button(new Rect			(	Screen.width/2 - 50, 
										statStartingPos + (10 * LINE_HEIGHT), 
										STAT_LABEL_WIDTH, 
										LINE_HEIGHT
									), "Create")) {
			//GameSettings gsScript = GameObject.Find("__GameSettings").GetComponent<GameSettings>();
			
			//change the cur value of the vitals to the max modified value of that vital
			UpdateCurVitalValues();
			
			//gsScript.SaveCharacterData();
			Application.LoadLevel("Level1");
		}
	}
	
	private void UpdateCurVitalValues() {
//		for(int i = 0; i < Enum.GetValues(typeof(VitalName)).Length; i++) {
//			_player.GetVital(i).CurValue = _player.GetVital(i).AdjustedBaseValue;	
//		}
	}
}
