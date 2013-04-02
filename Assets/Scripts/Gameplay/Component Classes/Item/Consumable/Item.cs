using UnityEngine;

public class Item {
	private string _name;
	private int _value;
	private RarityType _rarity;
	
	/// <summary>
	/// Initializes a default instance of the <see cref="Item"/> class.
	/// </summary>
	public Item () {
		_name = "Need Name";
		_value = 0;
		_rarity = RarityType.Common;
	}
	
	/// <summary>
	/// Initializes a new instance with parameters of the <see cref="Item"/> class.
	/// Used for consummables that can be found or sold.
	/// </summary>
	/// <param>
	/// name = Name of the item.
	/// valye = Value of the item.
	/// rarity = Rarity of the item.
	/// </param>
	public Item (string name, int value, RarityType rare) {
		_name = name;
		_value = value;
		_rarity = rare;
	}
	
	/// <summary>
	/// Initializes a new instance of the <see cref="Item"/> class.
	/// Used for equipment and quest items.
	/// </summary>
	/// <param name='name'>
	/// Name.
	/// </param>
	public Item (string name) {
		_name = name;
		_value = 0;
		_rarity = RarityType.Unique;
	}
	
	public string Name {
		get {return _name;}
		set {_name = value;}
	}
	
	public int Value {
		get {return _value;}
		set {_value = value;}
	}
	
	public RarityType Rarity {
		get {return _rarity;}
		set {_rarity = value;}
	}	
}

public enum RarityType {
	Common,
	Uncommon,
	Rare,
	Unique
}