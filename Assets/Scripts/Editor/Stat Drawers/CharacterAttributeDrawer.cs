using System;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(CharacterAttribute))]
public class CharacterAttributeDrawer : PropertyDrawer
{
	float nameWidth = .3f;
	float sliderWidth = .5f;
//	BaseCharacterClassicStats att {get {return ((BaseCharacterClassicStats)attribute);}}
	
	public override void OnGUI (UnityEngine.Rect position, SerializedProperty property, UnityEngine.GUIContent label)
	{
		//create instances of the attribute's name and value
		SerializedProperty attName = property.FindPropertyRelative ("name");
		SerializedProperty attValue = property.FindPropertyRelative ("baseValue");

		//Set dimensions for drawing the values in the inspector
		Rect namePosition = new Rect (position.x, position.y, position.width * nameWidth, position.height);
		Rect sliderPosition = new Rect (position.width-(position.width*sliderWidth), position.y, position.width * .5f, position.height);

		//Draw Inspector objects
		attName.enumValueIndex = EditorGUI.Popup (namePosition, attName.enumValueIndex, attName.enumNames);
		attValue.intValue = EditorGUI.IntSlider (sliderPosition, attValue.intValue, 1, 100);
	}
}

