using System;
using UnityEngine;

public interface IAttribute {
	int BaseValue				{get; set;}
	float BuffValue				{get; set;}
	float AdjustedBaseValue 	{get;}
}

/// <summary>
/// This is the class for all of the character attributes in-game. These values are more 
/// rigid than those that follow, and primarily modify the other character stats like Vitals. </summary>
[System.Serializable] 
public class Attribute : IAttribute {
	public enum AttributeName {
		None,						//Empty value used when dealing with stat scaling equipment
		Vitality,					//influences max health
		Endurance,					//influences max stamina
		Spirit,						//influences max energy 
		Strength,					//influences strength (power) scaling weapons and health regen
		Dexterity,					//influences destirty (finesse) scaling weapons and stamina regen
		Mind						//influences mind (insight) scaling weapons and energy consumption
	}

	#region Fields
	//Inspector Fields
	[SerializeField] private AttributeName name;
	[SerializeField] private int baseValue = 10;						//The basic value of this stat. May change based on levelling/status effect, but is otherwise constant.
	//Internal Fields
	private float _buffValue;										//The amount of the buff to this stat. (value will be filled based on equipment, abilities, etc.)
	#endregion
	
	#region Properties
	public string Name {
		get {return name.ToString();}
	}
	public int BaseValue {
		get {return baseValue;}
		set {baseValue = value;}
	}
	public float BuffValue {
		get {return _buffValue;}
		set {_buffValue = value;}
	}
	public float AdjustedBaseValue {						//The tallied total value of this stat.
		get {return BaseValue + BuffValue;}	
	}
	#endregion
	
	#region Initialization
	public Attribute () {
		baseValue = 10;
	}
	#endregion
}
