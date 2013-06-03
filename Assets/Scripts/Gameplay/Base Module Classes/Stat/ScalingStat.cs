using System;
using UnityEngine;

/// <summary>
/// This class acts as the value added to the relevant object's effectiveness 
/// based on the user's own stats. </summary>
[System.Serializable] 
public class ScalingStat
{	
		//Inspector Members
		[SerializeField] private CharacterAttribute.AttributeName scalingAttribute;
		[SerializeField] private float scalingRatio;
		private IAttribute _scalingAtt;

		public float BaseValue {		
				get { return _scalingAtt.AdjustedBaseValue * scalingRatio;}
		}

		public ScalingStat ()
		{
				scalingRatio = 5;
		}
		/// <summary>
		/// Sets the stat scaling on this weapon based on the user character's selected attribute. </summary>
		/// <param name='userStats'> User character's stat module. </param>
		public void SetScaling (ICharacter user)
		{
				
				switch (scalingAttribute) {
				case CharacterAttribute.AttributeName.Vitality:
						_scalingAtt = user.CharStats.Vitality;
						break;
				case CharacterAttribute.AttributeName.Endurance:
						_scalingAtt = user.CharStats.Endurance;
						break;
				case CharacterAttribute.AttributeName.Spirit:
						_scalingAtt = user.CharStats.Spirit;
						break;
				case CharacterAttribute.AttributeName.Strength:
						_scalingAtt = user.CharStats.Strength;
						break;
				case CharacterAttribute.AttributeName.Dexterity:
						_scalingAtt = user.CharStats.Dexterity;
						break;
				case CharacterAttribute.AttributeName.Mind:
						_scalingAtt = user.CharStats.Mind;
						break;
				default:
						_scalingAtt = null;
						break;
				}

				if (_scalingAtt == null)
						Debug.Log ("no scaling attribute set!"); 

		}
}