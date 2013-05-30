using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public interface ICharacterStats 
{
	IAttribute Vitality {get;}
	IAttribute Endurance {get;}
	IAttribute Spirit{get;}
	IAttribute Strength {get;}
	IAttribute Dexterity {get;}
	IAttribute Mind {get;}
	IVital Health {get;}
	IVital Stamina {get;}
	IVital Energy {get;}
	IVital StunResistance {get;}
	
	float StunDuration {get;}
	float StunStrength {get;}
}

/// <summary>
/// Character Stat Module. Holds all relevant in-mechanics values, especially those (down the line) visible to the player. </summary>
[System.Serializable] public class BaseCharacterStats : MonoBehaviour
{
//	public interface IAttribute {
//		int BaseValue				{get; set;}
//		float BuffValue				{get; set;}
//		float AdjustedBaseValue 	{get;}
//	}
	
//	public interface IVital {
//		float MinValue 				{get;}
//		float MaxValue 				{get;}
//		float CurValue 				{get; set;}
//		float BuffValue 			{get; set;}
//		
//		bool StopRegen 				{get; set;}
//		
//		event CharacterStatsModule.Vital.VitalChangedHandler VitalChanged;
//		IEnumerator StartRegen ();
//	}
	
	protected BaseCharacter _user;
	#region Fields
	//Attributes
	//		[SerializeField] private List<Attribute> attributes;
	//		[SerializeField] private List<PrimaryVital> primaryVitals;
	//		[SerializeField] private List<StatusVital> statusVitals;
	
	[SerializeField, CharacterAttAttribute(Attribute.AttributeName.Vitality)] protected Attribute vitality;
	[SerializeField, CharacterAttAttribute(Attribute.AttributeName.Endurance)] protected Attribute endurance;
	[SerializeField, CharacterAttAttribute(Attribute.AttributeName.Spirit)] protected Attribute spirit;
	[SerializeField, CharacterAttAttribute(Attribute.AttributeName.Strength)] protected Attribute strength;
	[SerializeField, CharacterAttAttribute(Attribute.AttributeName.Dexterity)] protected Attribute dexterity;
	[SerializeField, CharacterAttAttribute(Attribute.AttributeName.Mind)] protected Attribute mind;
	
	//Primary Vitals
	[SerializeField] protected PrimaryVital health;
	[SerializeField] protected PrimaryVital stamina;
	[SerializeField] protected PrimaryVital energy;
	
	//Status Vitals
	[SerializeField] protected StatusVital stunResistance;		
	
	//Stat Lists
	//		[SerializeField] protected List<Attribute> attList;
	//		[SerializeField] protected List<Vital> vitList;
	
	//Combat Stats
	[SerializeField] private float stunDuration;
	[SerializeField] private float stunStrength;
	#endregion
	
	#region Properties
	public IAttribute Vitality 
	{
		get {return vitality;}
	}
	public IAttribute Endurance 
	{
		get {return endurance;}
	}
	public IAttribute Spirit
	{
		get {return spirit;}
	}
	public IAttribute Strength 
	{
		get {return strength;}
	}
	public IAttribute Dexterity 
	{
		get {return dexterity;}
	}
	public IAttribute Mind 
	{
		get {return mind;}
	}
	public IVital Health 
	{
		get {return health;}
	}
	public IVital Stamina 
	{
		get {return stamina;}
	}
	public IVital Energy 
	{
		get {return energy;}
	}
	public IVital StunResistance 
	{
		get {return stunResistance;}
	}
	
	public float StunDuration
	{
		get {return stunDuration;}
		set {stunDuration = value;}
	}
	public float StunStrength
	{
		get {return stunStrength;}
		set {stunStrength = value;}
	}
	#endregion
	
	
	#region Initializers
	public void Awake ()
	{
		_user = GetComponent<BaseCharacter>();
	}
	public void Start () 
	{
	
		if (_user == null)
				Debug.Log ("us er is null");
		else
				Debug.Log ("user is set");

		health.SetScaling(_user);
		stamina.SetScaling(_user);
		energy.SetScaling(_user);

		stunResistance.SetScaling(_user);
	}
	#endregion


	#region Combat Maintenance
	/// <summary>
	/// Applies damage to the character. </summary>
	/// <param name='atkStrength'> Strength of attack being received. </param>
	public void ApplyDamage(float atkStrength) 
	{
		float damage = atkStrength;
		Health.CurValue -= damage;
	}
	/// <summary>
	/// Decreases the current value of the character's stamina vital. </summary>
	/// <param name='cost'> The value to be decreased from character's vital, from an ability used or an attack received. </param>
	public void ApplyStaminaUse(float cost)
	{
		Stamina.CurValue -= cost;
	}
	/// <summary>
	/// Decreases the current value of the character's energy vital. </summary>
	/// <param name='cost'> The value to be decreased from character's vital, typically from using a special ability. </param>
	public void ApplyEnergyUse(float cost)
	{
		Energy.CurValue -= cost;
	}
	#endregion
}