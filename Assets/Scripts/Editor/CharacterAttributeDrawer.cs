using System;
using UnityEngine;
using UnityEditor;


[CustomPropertyDrawer(typeof(BaseCharacterClassicStats))]
public class BaseCharacterClassicStatsDrawer : PropertyDrawer
{

//	BaseCharacterClassicStats att {get {return ((BaseCharacterClassicStats)attribute);}}

		public BaseCharacterClassicStatsDrawer ()
		{

		}

	public override void OnGUI (UnityEngine.Rect position, SerializedProperty property, UnityEngine.GUIContent label)
	{
		EditorGUI.BeginProperty(position,property,label);

		SerializedProperty attName = property.FindPropertyRelative ("name");

		Rect nameRect;



		EditorGUI.EndProperty ();
	}
}

