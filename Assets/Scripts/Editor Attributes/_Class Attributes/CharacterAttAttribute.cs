using UnityEngine;
using System.Collections;

public class CharacterAttAttribute : PropertyAttribute {
	public Attribute.AttributeName name;
	//public string name;						//Name of Attribute
	public float value;						//Base Value of Attribute

	public CharacterAttAttribute (Attribute.AttributeName n) {
		name = n;
	//	value = att.BaseValue
	}
}
