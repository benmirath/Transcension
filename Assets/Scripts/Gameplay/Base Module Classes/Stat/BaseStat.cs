/// <summary>
/// Base stat.cs
/// 
/// This is the base class for all stats in game
/// </summary>

public class BaseStat {
	//public const int STARTING_EXP_COST = 100;	//publicly accessible value for all stats to start at

	private string _name;						//this is the name of the stat
	private float _baseValue;					//the base value of this stat 
	private float _buffValue;					//the amount of the buff to this stat. (value will be filled based on equipment, abilities, etc.)
	//private int _expToLevel;					//the total amount of exp needed to raise this skill
	//private float _levelModifier;				//the modifier applied to the exp needed to raise the skill
	
	//Constructor for BaseStat
	public BaseStat () {
//		UnityEngine.Debug.Log("Base Stat Created");
		_name = "";
		_baseValue = 0;
		_buffValue = 0;
		//_levelModifier = 1.1f;
		//_expToLevel = STARTING_EXP_COST;
	}
	public BaseStat (string name, float baseVal)
	{
		_name = name;
		_baseValue = baseVal;
		_buffValue = 0;
		//_levelModifier = 1.1f;
		//_expToLevel = STARTING_EXP_COST;
	}

	#region Properties
	/// <summary>
	/// Gets or sets the name.
	/// </summary>
	/// <value>
	/// The name.
	/// </value>
	public string Name {
		get {return _name;}
		set {_name = value;}
	}
	
	/// <summary>
	/// Gets or sets the _baseValue.
	/// </summary>
	/// <value>
	/// The _baseValue.
	/// </value>
	public virtual float BaseValue {
		get {return _baseValue;}
		set {_baseValue = value;}
	}
	
	/// <summary>
	/// Gets or sets the _buffValue.
	/// </summary>
	/// <value>
	/// The _buffValue.
	/// </value>
	public virtual float BuffValue {
		get {return _buffValue;}
		set {_buffValue = value;}
	}

	/// <summary>
	/// Recalculate the adjusted base value and return it.
	/// </summary>
	/// <value>
	/// The adjusted base value.
	/// </value>
	public virtual float AdjustedBaseValue {
		get {return _baseValue + _buffValue;}	
	}
	#endregion
}