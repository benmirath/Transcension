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

[CustomPropertyDrawer (typeof(PrimaryVital))]
public class PrimaryVitalDrawer : PropertyDrawer
{
	bool showContent = false;
	float foldoutWidth = 30f;

	float nameWidth = .30f;
	float barWidth = .65f;

	float testCur;
	float testMax;

	public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
	{
		if (showContent)
			return 50;
		else 
			return 20;
	}

	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
	{
		SerializedProperty name = property.FindPropertyRelative("name");

		SerializedProperty maxValue = property.FindPropertyRelative("maxValue");
		SerializedProperty curValue = property.FindPropertyRelative("curValue");

		SerializedProperty baseScalingStat = property.FindPropertyRelative("baseScaling");
		SerializedProperty regenScalingStat = property.FindPropertyRelative("regenScaling");

		Rect foldoutPos = new Rect (position.x, position.y, foldoutWidth, 15);
		Rect namePos 	 = new Rect (position.x + foldoutWidth, position.y, position.width * nameWidth - foldoutWidth, 15);
		Rect barPos	 = new Rect (position.x + (nameWidth * position.width), 
		                         position.y,
		                         position.width * barWidth,
		                         15);

		Rect baseScalingPos = new Rect (position.x + 15, position.y + 15, position.width - 15, 15);
		Rect regenScalingPos = new Rect (position.x + 15, position.y + 30, position.width - 15, 15);

		name.enumValueIndex = EditorGUI.Popup (namePos, name.enumValueIndex, name.enumNames);

		if (maxValue == null || maxValue.floatValue <= 0) //Debug.LogWarning ("CUR VALUE IS NULL");
			testMax = 1;
		else
			testMax = maxValue.floatValue;

		if (testCur == null || testCur < 0)
			testCur = 1;
		else
			testCur = curValue.floatValue;

		EditorGUI.ProgressBar (barPos, 
		                       testCur / testMax, 
		                       testCur+" / "+testMax);

		showContent = EditorGUI.Foldout (foldoutPos, showContent, "");

		if (showContent) {

			EditorGUI.PropertyField (baseScalingPos, baseScalingStat);
			EditorGUI.PropertyField (regenScalingPos, regenScalingStat);
		}
	}
}



[CustomPropertyDrawer (typeof(StatusVital))]
public class StatusVitalDrawer : PropertyDrawer
{
	bool showContent = false;
	float nameWidth = .30f;
	float barWidth = .65f;
	float foldoutWidth = 30f;

	//float regenWidth;
	float testMax;
	float testCur;

	public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
	{
		if (showContent)
			return 50;
		else 
			return 20;
	}

	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
	{
		SerializedProperty name = property.FindPropertyRelative ("name");
		SerializedProperty maxValue = property.FindPropertyRelative ("maxValue");
		SerializedProperty curValue = property.FindPropertyRelative ("curValue");

		SerializedProperty baseValue = property.FindPropertyRelative ("baseValue");
		SerializedProperty regenScalingStat = property.FindPropertyRelative("regenScaling");

		Rect foldoutPos = new Rect (position.x, position.y, foldoutWidth, 15);
		Rect namePos 	 = new Rect (position.x + foldoutWidth, position.y, position.width * nameWidth-foldoutWidth, 15);
		Rect barPos	 = new Rect (position.x + (nameWidth * position.width), 
		                         position.y,
		                         position.width * barWidth,
		                         15);
		Rect baseValuePos = new Rect (position.x + 15, position.y + 15, position.width - 15, 15);
		Rect regenScalingPos = new Rect (position.x + 15, position.y + 30, position.width - 15, 15);

		name.enumValueIndex = EditorGUI.Popup (namePos, name.enumValueIndex, name.enumNames);

		if (baseValue == null || baseValue.floatValue <= 0)
			testMax = 1;
		else 
			testMax = baseValue.floatValue;

		if (curValue == null || curValue.floatValue < 0)
			testCur = 1;
		else 
			testCur = curValue.floatValue;

		EditorGUI.ProgressBar (barPos, 
		                       testCur / testMax,
		                       testCur+" / "+testMax);

		showContent = EditorGUI.Foldout (foldoutPos, showContent, "");

		if (showContent) {
			EditorGUI.PropertyField (regenScalingPos, regenScalingStat);
			EditorGUI.PropertyField (baseValuePos, baseValue);
		}
	}
}


