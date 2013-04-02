using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Character Stat Module. Holds all relevant in-mechanics values, especially those (down the line) visible to the player. </summary>
[System.Serializable] public class CharacterStatsModule {
	public interface IAttribute {
		int BaseValue				{get; set;}
		float BuffValue				{get; set;}
		float AdjustedBaseValue 	{get;}
	}
	
	public interface IVital {
		float MinValue 				{get;}
		float MaxValue 				{get;}
		float CurValue 				{get; set;}
		float BuffValue 			{get; set;}
		
		bool StopRegen 				{get; set;}
		
		event CharacterStatsModule.Vital.VitalChangedHandler VitalChanged;
		IEnumerator StartRegen ();
	}
	
	protected BaseCharacter _user;
	#region Fields
	//Attributes
	//		[SerializeField] private List<Attribute> attributes;
	//		[SerializeField] private List<PrimaryVital> primaryVitals;
	//		[SerializeField] private List<StatusVital> statusVitals;
	
	[SerializeField, CharacterAttAttribute(AttributeName.Vitality)] protected Attribute vitality;
	[SerializeField, CharacterAttAttribute(AttributeName.Endurance)] protected Attribute endurance;
	[SerializeField, CharacterAttAttribute(AttributeName.Spirit)] protected Attribute spirit;
	[SerializeField, CharacterAttAttribute(AttributeName.Strength)] protected Attribute strength;
	[SerializeField, CharacterAttAttribute(AttributeName.Dexterity)] protected Attribute dexterity;
	[SerializeField, CharacterAttAttribute(AttributeName.Mind)] protected Attribute mind;
	
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
	public void Setup (BaseCharacter user) {
		_user = user;
		health.SetScaling(_user);
		stamina.SetScaling(_user);
		energy.SetScaling(_user);
		stunResistance.SetScaling(_user);
		
		//			attributes.Add (vitality);
		//			attributes.Add (endurance);
		//			attributes.Add (spirit);
		//			attributes.Add (strength);
		//			attributes.Add (dexterity);
		//			attributes.Add (mind);
	}
	
	/*		public void SetLists () {
			attList.Add(vitality);
			attList.Add(endurance);
			attList.Add(spirit);
			attList.Add(strength);
			attList.Add(dexterity);
			attList.Add(mind);

			vitList.Add(health);
			vitList.Add(stamina);
			vitList.Add(energy);
			vitList.Add(stunResistance);
		}*/
	
	/// <summary>
	/// Selects the appropriate stat to determine scaling for other stats such as vitals. </summary>
	/// <returns> The relevant character attribute. </returns>
	/// <param name='name'> Name of the stat to be used. </param>
	/*		public Attribute SelectStatScaling(AttributeName name)
		{
			Attribute att;
			switch (name)
			{
			case BaseCharacter.CharacterStatsModule.AttributeName.Vitality:
				att = Vitality;
				break;
			case BaseCharacter.CharacterStatsModule.AttributeName.Endurance:
				att = Endurance;
				break;
			case BaseCharacter.CharacterStatsModule.AttributeName.Spirit:
				att = Spirit;
				break;
			case BaseCharacter.CharacterStatsModule.AttributeName.Strength:
				att = Strength;
				break;
			case BaseCharacter.CharacterStatsModule.AttributeName.Dexterity:
				att = Dexterity;
				break;
			case BaseCharacter.CharacterStatsModule.AttributeName.Mind:
				att = Mind;
				break;
			default:
				att = null;
				break;
			}
			return att;
		}*/
	#endregion
	
	public enum AttributeName 
	{
		None,						//Empty value used when dealing with stat scaling equipment
		Vitality,					//influences max health
		Endurance,					//influences max stamina
		Spirit,						//influences max energy 
		Strength,					//influences melee attack damage and health regen
		Dexterity,					//influences stamina regen and...something else
		Mind,						//influences spirit attack damage and energy consumption
	}
	
	/// <summary>
	/// This is the class for all of the character attributes in-game. These values are more 
	/// rigid than those that follow, and primarily modify the other character stats like Vitals. </summary>
	[System.Serializable] public class Attribute : IAttribute
	{
		#region Fields
		//Inspector Fields
		[SerializeField] private AttributeName name;
		[SerializeField] private int baseValue = 10;						//The basic value of this stat. May change based on levelling/status effect, but is otherwise constant.
		//Internal Fields
		private float _buffValue;										//The amount of the buff to this stat. (value will be filled based on equipment, abilities, etc.)
		#endregion
		
		#region Properties
		public string Name {
			get {return name.ToString();}
		}
		public int BaseValue {
			get {return baseValue;}
			set {baseValue = value;}
		}
		public float BuffValue {
			get {return _buffValue;}
			set {_buffValue = value;}
		}
		public float AdjustedBaseValue {						//The tallied total value of this stat.
			get {return BaseValue + BuffValue;}	
		}
		#endregion
		
		#region Initialization
		public Attribute () {
			baseValue = 10;
		}
		#endregion
	}
	
	public enum VitalName 
	{
		Health,
		Stamina,
		Energy,
		
		Stun,
		Knockback,
		Poison,
		Bleed,
		Burn,
		Freeze,
		Shock
	}
	[System.Serializable] public abstract class Vital : IVital {
		//			private enum VitalType {
		//				Primary,
		//				Secondary
		//			}
		#region Fields
		//Editor Fields
		//[SerializeField] private VitalType type;
		protected VitalName name;
		
		[SerializeField] protected float curValue;
		
		//			[SerializeField] private ScalingStat baseScalingStat;
		[SerializeField, ScalingStatAttribute(ScalingStatAttribute.ScalingType.Regen)] protected ScalingStat regenScalingStat;
		
		protected float buffValue;
		
		//Events - in place to constantly update relevant vital of any change
		public delegate void VitalChangedHandler(Vital vital);		
		public event VitalChangedHandler VitalChanged;
		
		//Coordinaton Fields
		protected float _prevValue;								//internal value from previous frame 
		protected float _regenTimer;
		protected bool _stopRegen;
		#endregion
		
		#region Properties
		public float MinValue {
			get {return 0;}
		}
		public abstract float MaxValue {get;}
		
		public float BuffValue {
			get {return buffValue;}
			set {
				buffValue = value;
				CurValue = CurValue;
			}
		}
		
		//			public ScalingStat BaseScalingStat {
		//				get {return baseScalingStat;}
		//			}
		
		public float CurValue {
			get {return curValue;}
			set {
				float val = value;
				if (val > MaxValue) val = MaxValue;
				else if (val < MinValue) val = MinValue;
				
				curValue = val;
				if (VitalChanged != null) VitalChanged(this);
			}
		}
		
		public float RegenRate {
			get {return regenScalingStat.BaseValue;}
		}
		
		public bool StopRegen
		{
			get {return _stopRegen;}
			set {_stopRegen = value;}
		}
		#endregion
		
		#region Initialization
		public Vital () {
			
		}
		#endregion
		
		#region Setup
		public abstract void SetScaling (BaseCharacter user);
		#endregion
		
		#region Methods
		public abstract IEnumerator StartRegen ();
		#endregion
	}
	[System.Serializable] public class PrimaryVital : Vital {
		[SerializeField, ScalingStatAttribute(ScalingStatAttribute.ScalingType.Base)] private ScalingStat baseScalingStat;
		
		public override float MaxValue {
			get {return baseScalingStat.BaseValue + BuffValue;}
		}
		
		public override void SetScaling (BaseCharacter user) {
			baseScalingStat.SetScaling (user);
			CurValue = MaxValue;
			regenScalingStat.SetScaling (user);
			user.StartCoroutine(StartRegen());
		}
		
		/// <summary>
		/// Starts the regen effect for this primar vital. </summary>
		public override IEnumerator StartRegen ()
		{
			while (!StopRegen) {
				float _regenTimer = Time.time;
				
				if (_prevValue <= CurValue) {
					if (_regenTimer == 0) {
						_regenTimer = Time.time + 0.75f;
						yield return null;
					}
					
					if (_regenTimer <= Time.time) {
						CurValue += (RegenRate * Time.deltaTime);
						_prevValue = CurValue;
						yield return null;
					} 
					
					else yield return null;
				} 
				else {
					_regenTimer = 0;
					_prevValue = CurValue;
					yield return null;
				}
			}
			yield return null;			
		}
	}
	[System.Serializable] public class StatusVital : Vital {
		[SerializeField, Range(0, 100)] private float baseValue;
		
		public override float MaxValue {
			get {return baseValue + BuffValue;}
		}
		
		public override void SetScaling (BaseCharacter user) {
			curValue = MinValue;
			regenScalingStat.SetScaling (user);
			user.StartCoroutine(StartRegen());
		}
		
		public override IEnumerator StartRegen () {
			while (!StopRegen ) {
				CurValue -= (RegenRate * Time.deltaTime) ;
				yield return null;
			}
			yield return null;
		}
	}
	
	/// <summary>
	/// Derived from Attribute, this class acts as the value added to the relevant object's effectiveness 
	/// based on the user's own stats. </summary>
	[System.Serializable] public class ScalingStat {	
		//Inspector Members
		[SerializeField] private CharacterStatsModule.AttributeName scalingAttribute;
		[SerializeField] private float scalingRatio = 5;
		private CharacterStatsModule.IAttribute _scalingAtt;
		
		public float BaseValue {
			get {return _scalingAtt.AdjustedBaseValue * scalingRatio;}
		}
		
		public ScalingStat () {
			
		}
		
		/// <summary>
		/// Sets the stat scaling on this weapon based on the user character's selected attribute. </summary>
		/// <param name='userStats'> User character's stat module. </param>
		public void SetScaling (BaseCharacter user)
		{
			switch (scalingAttribute)
			{
			case CharacterStatsModule.AttributeName.Vitality:
				_scalingAtt = user.CharStats.Vitality;
				break;
			case CharacterStatsModule.AttributeName.Endurance:
				_scalingAtt = user.CharStats.Endurance;
				break;
			case CharacterStatsModule.AttributeName.Spirit:
				_scalingAtt = user.CharStats.Spirit;
				break;
			case CharacterStatsModule.AttributeName.Strength:
				_scalingAtt = user.CharStats.Strength;
				break;
			case CharacterStatsModule.AttributeName.Dexterity:
				_scalingAtt = user.CharStats.Dexterity;
				break;
			case CharacterStatsModule.AttributeName.Mind:
				_scalingAtt = user.CharStats.Mind;
				break;
			default:
				_scalingAtt = null;
				break;
			}
		}
	}
}