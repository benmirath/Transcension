/// <summary>
/// VitalBar.cs
/// 
/// This class is responsible for displaying a vital for the player character or an enemy
/// </summary>

using UnityEngine;
using System.Collections;

public class VitalBar : MonoBehaviour {
	public enum VitalType
	{
		Health,
		Stamina,
		Energy
	}
	private GUITexture _display;

	private float _maxBarLength;				//this is how large the vital bar can be if the target is at 100%
	private float _curBarLength;				//this is current length of the vital bar;
	
	public VitalType vitalType;	
	public bool isPlayerBar;				//this boolean value tells us if this is the player or enemy healthbar.
	
	//setting the healthbar to the player or mob
	public bool SetPlayerBar
	{
		get {return isPlayerBar;}
		set {isPlayerBar = value;}
	}

	private void ToggleDisplay (bool show) {
		_display.enabled = show;	
	}	
	
	// Use this for initialization
	void Start () {
		_display = GetComponent<GUITexture>();
			
		ICharacter user;
		
		if (isPlayerBar)
		{
			GameObject player = GameObject.FindWithTag("Player");
			user = player.GetComponent<BasePlayer>();
			if (user == null) Debug.LogError("No user for vitality bars set");
			else Debug.LogError("VITALITY BAR SET");
			ToggleDisplay(true);
		}
		else 
		{
			user = transform.parent.GetComponent<BaseEnemy>();			
			ToggleDisplay(false);
		}
		switch (vitalType)
		{
		case VitalType.Health:
			user.CharStats.Health.VitalChanged += UpdateBar;
			Debug.LogError("HEALTH BAR SET");
			break;
			
		case VitalType.Stamina:
			user.CharStats.Stamina.VitalChanged += UpdateBar;
			Debug.LogError("STAMINA BAR SET");
			break;
			
		case VitalType.Energy:
			user.CharStats.Energy.VitalChanged += UpdateBar;
			Debug.LogError("ENERGY BAR SET");
			break;
		}

		_maxBarLength = _display.pixelInset.width;
		_curBarLength = _maxBarLength;
	}

	
/*	
	//This method is called when the gameObject is enabled
	public void OnEnable () {
		if(_isPlayerHealthBar)
	        Messenger<int, int>.AddListener("player health update", OnChangeHealthBarSize);
		else {
			ToggleDisplay(false);
	        Messenger<int, int>.AddListener("enemy health update", OnChangeHealthBarSize);
			Messenger<bool>.AddListener("show enemy vitalbars", ToggleDisplay);
		}
	}
	
	//This method is called when the gameObject is disabled
	public void OnDisable () {
		if (_isPlayerHealthBar)	
			Messenger<int, int>.RemoveListener("player health update", OnChangeHealthBarSize);
		else {
			Messenger<int, int>.RemoveListener("enemy health update", OnChangeHealthBarSize);
			Messenger<bool>.RemoveListener("show enemy vitalbars", ToggleDisplay);
		}
	}*/
	
	
	void UpdateBar(Vital v)
	{
//		Debug.LogError("UpdateBar: " + v.CurValue);
		_curBarLength = (v.CurValue / v.MaxValue) * _maxBarLength;		//this calculates the current bar length based on player's health %
		_display.pixelInset = CalculatePosition();
	}

	private Rect CalculatePosition() {
		float xPos;
		float yPos;
		//sets enemy health to right side and insure it drains properly
		//if(isPlayerBar)
		//{
			xPos = _display.pixelInset.x;//((int)_maxBarLength - (int)_curBarLength) + ((int)_maxBarLength + 50);
			yPos = _display.pixelInset.y;
//		}
//		else
//		{
//			xPos = _display.pixelInset.x;
//			yPos = _display.pixelInset.y;
//	//		return new Rect(_display.pixelInset.x, yPos, _curBarLength, _display.pixelInset.height);		
//		}
		return new Rect(xPos, yPos, _curBarLength, _display.pixelInset.height);
	}
}
