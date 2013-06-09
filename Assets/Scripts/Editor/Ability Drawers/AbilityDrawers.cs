using System;
using UnityEngine;
using UnityEditor;

//[CustomPropertyDrawer(typeof(MovementProperties))]
public class BaseAbilityPropertyDrawer : PropertyDrawer
{
	float foldoutWidth = 30f;
	float abilityTypeWidth = .20f;
	float vitalTypeWidth = .20f;
	float costWidth = .5f;

	protected bool showContent = false;
	protected bool showTimer = false;
	protected SerializedProperty abilityType;
	protected Rect abilityTypePos;

	protected float GetTimerHeight
	{
		get {
			if (showTimer)
				return 60;
			else
				return 15;
		}
	}
	public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
	{
		return GetTimerHeight;
	}
	
	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUIUtility.LookLikeControls();
		#region Base Properties
		SerializedProperty vitalType = property.FindPropertyRelative ("vitalType");
		SerializedProperty cost = property.FindPropertyRelative ("cost");
		SerializedProperty enterLength = property.FindPropertyRelative ("enterLength");
		SerializedProperty activeLength = property.FindPropertyRelative ("activeLength");
		SerializedProperty exitLength = property.FindPropertyRelative ("exitLength");
		#endregion

		#region Coordinates
		Rect foldoutPos = new Rect (position.x, 
		                            position.y, 
		                            foldoutWidth, 
		                            15);

		abilityTypePos = new Rect (position.x + foldoutWidth, 
		                                 position.y, 
		                                 position.width*abilityTypeWidth, 
		                                 15);
		Rect vitalTypePos = new Rect (position.x + (position.width * abilityTypeWidth) + foldoutWidth, 
		                              position.y, 
		                              position.width*vitalTypeWidth, 
		                              15);
		Rect costPos = new Rect (position.x + (position.width * costWidth),
		                         position.y, 
		                         position.width*costWidth, 
		                         15);


		Rect showTimerPos = new Rect (position.x,
		                              position.y + 15,
		                              position.width,
		                              15);
		Rect enterLengthPos = new Rect (position.x, 
		                                position.y + GetTimerHeight - 30, 
		                                position.width, 
		                                15);
		Rect activeLengthPos = new Rect (position.x,
		                                position.y + GetTimerHeight - 15,
		                                position.width,
		                                15);
		Rect exitLengthPos = new Rect (position.x,
		                              position.y + GetTimerHeight,
		                              position.width,
		                              15);
		#endregion

		#region Inspector Elements
		EditorGUI.indentLevel = 1;
		showContent = EditorGUI.Foldout (foldoutPos, showContent, "");
		if (abilityType != null) abilityType.enumValueIndex = EditorGUI.Popup (abilityTypePos, abilityType.enumValueIndex, abilityType.enumNames);
		vitalType.enumValueIndex = EditorGUI.Popup (vitalTypePos, vitalType.enumValueIndex, vitalType.enumNames);
		cost.floatValue = EditorGUI.Slider (costPos, cost.floatValue, 0, 100);

		if (showContent) {
			EditorGUI.indentLevel = 2;
			//Duration Timers
			showTimer = EditorGUI.Foldout (showTimerPos, showTimer, "Duration Values");
			if (showTimer) {
				EditorGUI.indentLevel = 4;
				enterLength.floatValue = EditorGUI.Slider (enterLengthPos,  "Startup Duration", enterLength.floatValue, 0, 10);
				activeLength.floatValue = EditorGUI.Slider (activeLengthPos, "Active Duration", activeLength.floatValue, 0, 10);
				exitLength.floatValue = EditorGUI.Slider (exitLengthPos, "Cooldown Duration", exitLength.floatValue, 0, 10);
			}
		}
		#endregion
	}
}

[CustomPropertyDrawer(typeof(MovementProperties))]
public class MovementPropertyDrawer : BaseAbilityPropertyDrawer
{
	protected bool showSpeed = false;
	protected float GetSpeedHeight {
		get {
			if (showSpeed)
				return 75;
			else 
				return 15;
		}
	}
	public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
	{
		if (showContent)
			return GetTimerHeight + GetSpeedHeight + 20;
		else 
			return 20;
	}

	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
	{
		base.OnGUI (position, property, label);
		#region Movement Properties
		abilityType = property.FindPropertyRelative ("movementType");
		SerializedProperty enterMoveSpeed = property.FindPropertyRelative ("enterMoveSpeed");
		SerializedProperty activeMoveSpeed = property.FindPropertyRelative ("activeMoveSpeed");
		SerializedProperty exitMoveSpeed = property.FindPropertyRelative ("exitMoveSpeed");
		SerializedProperty lookSpeed = property.FindPropertyRelative ("lookSpeed");
		#endregion

		#region Coordinates
		Rect showSpeedPos = new Rect (position.x,
		                              position.y + GetTimerHeight + 15,
		                              position.width,
		                              15);
		Rect enterMoveSpeedPos = new Rect (position.x,
		                                   position.y + GetTimerHeight + GetSpeedHeight - 45,
		                              position.width,
		                              15);
		Rect activeMoveSpeedPos = new Rect (position.x,
		                                    position.y + GetTimerHeight + GetSpeedHeight - 30,
		                                   position.width,
		                                   15);
		Rect exitMoveSpeedPos = new Rect (position.x,
		                                  position.y + GetTimerHeight + GetSpeedHeight - 15,
		                                   position.width,
		                                   15);
		Rect lookSpeedPos = new Rect (position.x,
		                              position.y + GetTimerHeight + GetSpeedHeight,
		                              position.width,
		                              15);
		#endregion

		#region Inspector Elements
		EditorGUI.indentLevel = 1;
//		DrawAbilityType ();
		if (showContent) {
			EditorGUI.indentLevel = 2;
			//Speed Values
			showSpeed = EditorGUI.Foldout (showSpeedPos, showSpeed, "Speed Values");
			if (showSpeed) {
				EditorGUI.indentLevel = 4;
				//EditorGUI.LabelField (new Rect (position.x, position.y + 40, position.width * speedLabelWidth, 15), "Move Speed");
				//moveSpeed.floatValue = EditorGUI.FloatField (moveSpeedPos, moveSpeed.floatValue);
				//EditorGUI.LabelField (new Rect (position.width - (position.width * (speedLabelWidth + speedValueWidth)), position.y + 40, position.width * speedLabelWidth, 15), "Look Speed");
				//lookSpeed.floatValue = EditorGUI.FloatField (lookSpeedPos, lookSpeed.floatValue);
				enterMoveSpeed.floatValue = EditorGUI.Slider (enterMoveSpeedPos, "Startup Speed", enterMoveSpeed.floatValue, 0, 50);
				activeMoveSpeed.floatValue = EditorGUI.Slider (activeMoveSpeedPos, "Active Speed", activeMoveSpeed.floatValue, 0 , 50);
				exitMoveSpeed.floatValue = EditorGUI.Slider (exitMoveSpeedPos, "Cooldown Speed", exitMoveSpeed.floatValue, 0, 50);
				lookSpeed.floatValue = EditorGUI.Slider (lookSpeedPos, "Look Speed", lookSpeed.floatValue, 0, 50);
			}
		}
		#endregion
	}
}



[CustomPropertyDrawer(typeof(MeleeProperties))]
public class AttackPropertyDrawer : MovementPropertyDrawer
{
	bool showAttackStats = false;
	public float GetAttackStatsHeight {
		get {
			if (showAttackStats)
				return 60;
			else
				return 15;
		}
	}
	public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
	{
		if (showContent)
			return GetTimerHeight + GetSpeedHeight + GetAttackStatsHeight + 20;
		else 
			return 20;
	}

	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
	{
		base.OnGUI (position, property, label);
		#region Attack Properties
		abilityType = property.FindPropertyRelative ("attackType");
		SerializedProperty primaryDamageType = property.FindPropertyRelative ("primaryDamageType");
		SerializedProperty secondaryDamageType = property.FindPropertyRelative ("secondaryDamageType");
		SerializedProperty damageModifier = property.FindPropertyRelative ("damageModifier");
		SerializedProperty impactModifer = property.FindPropertyRelative ("impactModifier");
		#endregion

		#region Coordinates
		Rect showAttackStatsPos =  new Rect (position.x,
		                                       position.y + GetTimerHeight + GetSpeedHeight + 15,
		                                       position.width,
		                                       15);
		Rect primaryDamageTypePos = new Rect (position.x,
		                                      position.y + GetTimerHeight + GetSpeedHeight + GetAttackStatsHeight - 30,
		                                      position.width / 2,
		                                      15);
		Rect secondaryDamageTypePos = new Rect (position.x + (position.width / 2),
		                                        position.y + GetTimerHeight + GetSpeedHeight + GetAttackStatsHeight - 30,
		                                        position.width / 2,
		                                        15);
		Rect damageModifierPos = new Rect (position.x,
		                                   position.y + GetTimerHeight + GetSpeedHeight + GetAttackStatsHeight - 15,
		                                   position.width,
		                                   15);
		Rect impactModifierPos = new Rect (position.x,
		                                   position.y + GetTimerHeight + GetSpeedHeight + GetAttackStatsHeight,
		                                   position.width,
		                                   15);
		#endregion

		#region Inspector Elements
		EditorGUI.indentLevel = 1;
		if (showContent) {
			EditorGUI.indentLevel = 2;
			showAttackStats = EditorGUI.Foldout (showAttackStatsPos, showAttackStats, "Attack Values");

			if (showAttackStats) {
				EditorGUI.indentLevel = 4;
				primaryDamageType.enumValueIndex = EditorGUI.Popup (primaryDamageTypePos, primaryDamageType.enumValueIndex, primaryDamageType.enumNames);
				secondaryDamageType.enumValueIndex = EditorGUI.Popup (secondaryDamageTypePos, secondaryDamageType.enumValueIndex, secondaryDamageType.enumNames);
				damageModifier.floatValue = EditorGUI.Slider (damageModifierPos, "Damage Modifier", damageModifier.floatValue, 0, 5);
				impactModifer.floatValue = EditorGUI.Slider (impactModifierPos, "Impact Modifier", impactModifer.floatValue, 0, 5);
			}
		}
		#endregion
	}
}

[CustomPropertyDrawer (typeof (MeleeProperties))]
public class MeleePropertyDrawer : AttackPropertyDrawer {

}

[CustomPropertyDrawer(typeof(RangedProperties))]
public class RangedPropertyDrawer : PropertyDrawer
{


	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
	{

	}

}


