using System;
using UnityEngine;
using UnityEditor;

//[CustomPropertyDrawer (typeof(Vital))]
//public class VitalDrawer : PropertyDrawer
//{
//	float nameWidth = .20f;
//	float minValueWidth = .20f;
//	float maxValueWidth = .20f;
//	float curValueWidth = .20f;
//	//float regenWidth;
//
//	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
//	{
//		SerializedProperty name = property.FindPropertyRelative("name");
//		SerializedProperty MinValue = property.FindPropertyRelative("MinValue");
//		SerializedProperty MaxValue = property.FindPropertyRelative("MaxValue");
//		SerializedProperty curValue = property.FindPropertyRelative("curValue");
//		SerializedProperty regenScalingStat = property.FindPropertyRelative("regenScalingStat");
//
//		Rect namePos = new Rect (position.x, position.y, position.width * nameWidth, position.height);
//		Rect minValuePos = new Rect (position.x + nameWidth, position.y, position.width * minValueWidth, position.height);
//		Rect maxValuePos = new Rect (position.x + nameWidth + minValueWidth, position.y, position.width * maxValueWidth, position.height);
//
//		EditorGUI.Popup (namePos, name.enumValueIndex, name.enumNames);
//		//EditorGUI.LabelField (namePos, name.stringValue);
//
//	}
//}

[CustomPropertyDrawer (typeof(StatusVital))]
public class StatusVitalDrawer : PropertyDrawer
{
	float nameWidth = .30f;
	float minValueWidth = .23f;
	float maxValueWidth = .23f;
	float curValueWidth = .23f;
	//float regenWidth;

	public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
	{
		return 60;
	}

	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
	{
		SerializedProperty name = property.FindPropertyRelative ("name");
		SerializedProperty minValue = property.FindPropertyRelative ("minValue");
		SerializedProperty maxValue = property.FindPropertyRelative ("maxValue");
		SerializedProperty curValue = property.FindPropertyRelative ("curValue");
		SerializedProperty baseValue = property.FindPropertyRelative ("baseValue");
		SerializedProperty regenScalingStat = property.FindPropertyRelative("regenScalingStat");

		Rect namePos 	 = new Rect (position.x, position.y, position.width * nameWidth, 15);
		Rect minValuePos = new Rect (position.x + (nameWidth * position.width), 
		                             position.y, 
		                             position.width * minValueWidth, 
		                             15);

		Rect maxValuePos = new Rect (position.x + (nameWidth * position.width) + (minValueWidth * position.width), 
		                             position.y, 
		                             position.width * maxValueWidth, 
		                             15);

		Rect curValuePos = new Rect (position.x + (nameWidth * position.width) + (minValueWidth * position.width) + (maxValueWidth * position.width), 
		                             position.y, 
		                             position.width * curValueWidth, 
		                             15);
		Rect baseValuePos = new Rect (position.x + 15, position.y + 20, position.width - 15, 15);
		Rect regenScalingPos = new Rect (position.x + 15, position.y + 35, position.width - 15, 15);

		name.enumValueIndex = EditorGUI.Popup (namePos, name.enumValueIndex, name.enumNames);
		EditorGUI.LabelField (minValuePos, "min val : " + minValue.floatValue);
		EditorGUI.LabelField (maxValuePos, "max val : " + maxValue.floatValue);
		EditorGUI.LabelField (curValuePos, "cur val : " + curValue.floatValue);

		EditorGUI.PropertyField (regenScalingPos, regenScalingStat);
		EditorGUI.PropertyField (baseValuePos, baseValue);
	}
}

[CustomPropertyDrawer (typeof(PrimaryVital))]
public class PrimaryVitalDrawer : PropertyDrawer
{
	float nameWidth = .30f;
	float minValueWidth = .23f;
	float maxValueWidth = .23f;
	float curValueWidth = .23f;

	public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
	{
		return 60;
	}

	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
	{
		SerializedProperty name = property.FindPropertyRelative("name");
		SerializedProperty minValue = property.FindPropertyRelative("minValue");
		SerializedProperty maxValue = property.FindPropertyRelative("maxValue");
		SerializedProperty curValue = property.FindPropertyRelative("curValue");
		SerializedProperty baseScalingStat = property.FindPropertyRelative("baseScalingStat");
		SerializedProperty regenScalingStat = property.FindPropertyRelative("regenScalingStat");

		Rect namePos 	 = new Rect (position.x, position.y, position.width * nameWidth, 15);
		Rect minValuePos = new Rect (position.x + (nameWidth * position.width), 
		                             position.y, 
		                             position.width * minValueWidth, 
		                             15);
		Rect maxValuePos = new Rect (position.x + (nameWidth * position.width) + (minValueWidth * position.width), 
		                             position.y, 
		                             position.width * maxValueWidth, 
		                             15);
		Rect curValuePos = new Rect (position.x + (nameWidth * position.width) + (minValueWidth * position.width) + (maxValueWidth * position.width), 
		                             position.y, 
		                             position.width * curValueWidth, 
		                             15);
		Rect baseScalingPos = new Rect (position.x + 15, position.y + 20, position.width - 15, 15);
		Rect regenScalingPos = new Rect (position.x + 15, position.y + 35, position.width - 15, 15);

		name.enumValueIndex = EditorGUI.Popup (namePos, name.enumValueIndex, name.enumNames);
		EditorGUI.LabelField (minValuePos, "min val : " + minValue.floatValue);
		EditorGUI.LabelField (maxValuePos, "max val : " + maxValue.floatValue);
		EditorGUI.LabelField (curValuePos, "cur val : " + curValue.floatValue);

		EditorGUI.PropertyField (baseScalingPos, baseScalingStat);
		EditorGUI.PropertyField (regenScalingPos, regenScalingStat);
	}
}


