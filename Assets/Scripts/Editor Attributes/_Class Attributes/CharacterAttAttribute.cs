using UnityEngine;
using UnityEditor;
using System.Collections;

public class CharacterAttAttribute : PropertyAttribute {
	public CharacterAttribute.AttributeName name;
	//public string name;						//Name of Attribute
	public float value;						//Base Value of Attribute

	public CharacterAttAttribute (CharacterAttribute.AttributeName n) {
		name = n;
	//	value = att.BaseValue
	}
}
