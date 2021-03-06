using System;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ScalingStat))]
public class ScalingStatDrawer : PropertyDrawer
{
	float labelWidth = .25f;
	float attWidth = .20f;
	float ratioWidth = .525f;

	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
	{	
		EditorGUIUtility.LookLikeControls();

		string statName = property.name;
		SerializedProperty scalingAttribute = property.FindPropertyRelative ("scalingAttribute");
		SerializedProperty scalingRatio = property.FindPropertyRelative ("scalingRatio");

		Rect labelPosition = new Rect (position.x, position.y, position.width * labelWidth, position.height);
		Rect attPosition = new Rect (position.x + (position.width*labelWidth), position.y, position.width * attWidth, position.height);
		Rect ratioPosition = new Rect (position.x + (position.width-(position.width*ratioWidth)), position.y, position.width * ratioWidth, position.height);

		EditorGUI.LabelField (labelPosition, statName);
		scalingAttribute.enumValueIndex = EditorGUI.Popup (attPosition, scalingAttribute.enumValueIndex, scalingAttribute.enumNames);
		scalingRatio.floatValue = EditorGUI.Slider (ratioPosition, scalingRatio.floatValue, 0, 5);
	}
}


