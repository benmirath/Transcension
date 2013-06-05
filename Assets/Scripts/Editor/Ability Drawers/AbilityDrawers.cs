using System;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(MovementProperties))]
public class MovementPropertyDrawer : PropertyDrawer
{
	bool showContent = true;
	float foldoutWidth = 30f;
	float abilityTypeWidth = .20f;
	float vitalTypeWidth = .20f;
	float costWidth = .5f;

	float durationLabelWidth = .140f;
	float durationValueWidth = .169f;

	float speedLabelWidth = .25f;
	float speedValueWidth = .25f;

	float enterMoveSpeedWidth = .25f;
	float activeMoveSpeedWidth = .25f;
	float exitMoveSpeedWidth = .25f;
	float lookSpeedWidth = .25f;

	public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
	{
		if (showContent)
			return 60;
		else
			return 15;
	}

	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
	{
		//EditorGUIUtility.LookLikeControls();
		EditorGUI.indentLevel = 1;
		//Base Ability Properties
		SerializedProperty vitalType = property.FindPropertyRelative ("vitalType");
		SerializedProperty cost = property.FindPropertyRelative ("cost");
		SerializedProperty enterLength = property.FindPropertyRelative ("enterLength");
		SerializedProperty activeLength = property.FindPropertyRelative ("activeLength");
		SerializedProperty exitLength = property.FindPropertyRelative ("exitLength");

		//Derived Ability Properties
		SerializedProperty movementType = property.FindPropertyRelative ("movementType");
//		SerializedProperty enterMoveSpeed = property.FindPropertyRelative ("enterMoveSpeed");
//		SerializedProperty activeMoveSpeed = property.FindPropertyRelative ("activeMoveSpeed");
//		SerializedProperty exitMoveSpeed = property.FindPropertyRelative ("exitMoveSpeed");
		SerializedProperty moveSpeed = property.FindPropertyRelative ("moveSpeed");
		SerializedProperty lookSpeed = property.FindPropertyRelative ("lookSpeed");

		#region Coordinates
		Rect foldoutPos = new Rect (position.x, 
		                            position.y, 
		                            foldoutWidth, 
		                            15);

		Rect movementTypePos = new Rect (position.x + foldoutWidth, 
		                                 position.y, 
		                                 position.width*abilityTypeWidth, 
		                                 15);
		Rect vitalTypePos = new Rect (position.x + (position.width * abilityTypeWidth) + foldoutWidth, 
		                              position.y, 
		                              position.width*vitalTypeWidth, 
		                              15);
		Rect costPos = new Rect (position.x + (position.width * costWidth) - 15, 
		                         position.y, 
		                         position.width*costWidth, 
		                         15);

		Rect enterLengthPos = new Rect (position.x + (position.width * durationLabelWidth), 
		                                position.y + 20, 
		                                position.width * durationValueWidth, 
		                                15);
		Rect activeLenthPos = new Rect (position.x + (position.width * ((durationLabelWidth + (durationValueWidth * 2))) + 20),
		                                position.y + 20,
		                                position.width * durationValueWidth,
		                                15);
		Rect exitLenthPos = new Rect (position.width - (position.width * durationValueWidth) - 15,
		                              position.y + 20,
		                              position.width * durationValueWidth,
		                              15);

		Rect moveSpeedPos = new Rect (position.x + (position.width * speedLabelWidth),
		                              position.y + 40,
		                              position.width * speedValueWidth,
		                              15);
		Rect lookSpeedPos = new Rect (position.width - (position.width * speedValueWidth) - 15,
		                              position.y + 40,
		                              position.width * speedValueWidth,
		                              15);
//		Rect enterMovePos = new Rect (position.x);
		#endregion



		showContent = EditorGUI.Foldout (foldoutPos, showContent, "");
		movementType.enumValueIndex = EditorGUI.Popup (movementTypePos, movementType.enumValueIndex, movementType.enumNames);
		vitalType.enumValueIndex = EditorGUI.Popup (vitalTypePos, vitalType.enumValueIndex, vitalType.enumNames);
		cost.floatValue = EditorGUI.Slider (costPos, cost.floatValue, 0, 100);



		if (showContent) {
			EditorGUI.indentLevel = 2;
			//Duration Timers
			EditorGUI.LabelField (new Rect (position.x, position.y + 20, position.width * durationLabelWidth, 15), "Enter");
			enterLength.floatValue = EditorGUI.FloatField (enterLengthPos, enterLength.floatValue);
			EditorGUI.LabelField (new Rect (position.x + (position.width * (durationLabelWidth + durationValueWidth) + 10), position.y+20, position.width * durationLabelWidth, 15), "Active");
			activeLength.floatValue = EditorGUI.FloatField (activeLenthPos, activeLength.floatValue);
			EditorGUI.LabelField (new Rect (position.width - (position.width * (durationLabelWidth + durationValueWidth)), position.y+20, position.width * durationValueWidth, 15), "Exit");
			exitLength.floatValue = EditorGUI.FloatField (exitLenthPos, exitLength.floatValue);

			//Speed Values
			EditorGUI.LabelField (new Rect (position.x, position.y + 40, position.width * speedLabelWidth, 15), "Move Speed");
			moveSpeed.floatValue = EditorGUI.FloatField (moveSpeedPos, moveSpeed.floatValue);
			EditorGUI.LabelField (new Rect (position.width - (position.width * (speedLabelWidth + speedValueWidth)), position.y + 40, position.width * speedLabelWidth, 15), "Look Speed");
			lookSpeed.floatValue = EditorGUI.FloatField (lookSpeedPos, lookSpeed.floatValue);

		}

	}


}

[CustomPropertyDrawer(typeof(MeleeProperties))]
public class MeleePropertyDrawer : PropertyDrawer
{


	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
	{
		SerializedProperty enterLength = property.FindPropertyRelative ("enterLength");
		SerializedProperty durationLength = property.FindPropertyRelative ("durationLength");
		SerializedProperty exitLength = property.FindPropertyRelative ("exitLength");
		SerializedProperty vitalType = property.FindPropertyRelative ("vitalType");
		SerializedProperty cost = property.FindPropertyRelative ("cost");

	}

}

[CustomPropertyDrawer(typeof(RangedProperties))]
public class RangedPropertyDrawer : PropertyDrawer
{


	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
	{
		SerializedProperty enterLength = property.FindPropertyRelative ("enterLength");
		SerializedProperty durationLength = property.FindPropertyRelative ("durationLength");
		SerializedProperty exitLength = property.FindPropertyRelative ("exitLength");

		SerializedProperty vitalType = property.FindPropertyRelative ("vitalType");
		SerializedProperty cost = property.FindPropertyRelative ("cost");

	}

}


