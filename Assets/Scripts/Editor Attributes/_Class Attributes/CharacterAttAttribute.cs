using UnityEngine;
using System.Collections;

public class CharacterAttAttribute : PropertyAttribute {
	public string name;
	//public string name;						//Name of Attribute
	public float value;						//Base Value of Attribute

	public CharacterAttAttribute (CharacterStatsModule.AttributeName n) {
		name = n.ToString();
	//	value = att.BaseValue
	}
}
