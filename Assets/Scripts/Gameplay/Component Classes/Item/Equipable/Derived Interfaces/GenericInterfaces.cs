using System;
using System.Collections.Generic;

//Generic Equipment Interface
/// <summary>
/// Generic interface for equippable items.
/// Available moveset and potential use is customized by specifying four values at initialization. </summary>
/// <param name="T1"> Equipment Range </para>
/// <param name="T2"> Equipment Weight </para>
/// <param name="T3"> Equipment Handling Methods </para>
/// <param name="T4"> Equipment Styles </para>
/*public interface IEquippable<out T1, out T2, out T3, out T4> : IBaseEquipment //, IEquipmentType<T1>, IEquipmentComboNum<T2>, IEquipmentWieldingMethod<T3>, IEquipmentStyle<T4>
	where T1 : IEquipmentType
	where T2 : IEquipmentComboNum
	where T3 : IEquipmentWieldingMethod
	where T4 : IEquipmentStyle
{
	//T1 GetWeapon
	//{get;}
}


//Range of weapon
public interface IEquipmentType<out T> where T : IEquipmentType
{
	//T RangeClass
	//{get;}
}

//Weight class of weapon, reperesentative of hit strength and dps.
public interface IEquipmentComboNum<out T> where T : IEquipmentComboNum
{
	//T WeightClass
	//{get;}
}

//Wielding method
public interface IEquipmentWieldingMethod<out T> where T : IEquipmentWieldingMethod
{}

//Style class
public interface IEquipmentStyle<out T> where T : IEquipmentStyle
{}


//2 Handed Weapons (Speed)
public interface IPrayerSeal : IEquippable<IMeleeWeapon, IEquipment5HitCombo, IDoubleHandedWeapon, IFinesseWeapon> {}
//1.5 Handed Weapons (Balanced, Speed)
public interface IKnife : IEquippable<IMeleeWeapon, IEquipment5HitCombo, IHalfHandedWeapon, IFinesseWeapon> {}
public interface IBoltShooter : IEquippable<IRangedWeapon, IEquipment4HitCombo, IHalfHandedWeapon, IFinesseWeapon> {}	
//Medium Weapons

//2 Handed Weapons (Balanced, Power Speed)
public interface IChiFocus : IEquippable <IMeleeWeapon, IEquipment4HitCombo, IDoubleHandedWeapon, IFinesseWeapon> {}

//1.5 Handed Weapons (Balanced, Power, Speed)
/// <summary> Sword. 
/// The most balanced of weapons, capable of being used in a variety of styles. </summary>
/// <remarks>
/// Universal
/// -Combo1
/// -Combo2
/// -Combo3
/// -RunAttack
/// -DodgeAttack
/// 
/// TwoHanded
/// -AltFinisher
/// Power (TwoHanding)
/// -ChargeAttack
/// Speed (DualWield)
/// -RunAttackFollowup
/// -DodgeAttackFollowup
/// </remarks>	
public interface ISword : IEquippable<IMeleeWeapon, IEquipment4HitCombo, IHalfHandedWeapon, IUniversalWeapon> {}
/// <summary>
/// Spear-scythe. Can switch between a spear and a scythe mode on the fly. </summary>
/// <remarks> 
/// Spear - a Power weapon, with fewer, but more concentrated hits. Is well suited for armor penetrating attacks.
/// Scythe - a Finesse weapon, with more varied and speedy combo options, though each hit individually hits for less. 
/// Causes bleed to buildup, making the weapon well suited for agressive hit and runs. </remarks>
public interface ISpearScythe : IEquippable<IMeleeWeapon, IEquipment4HitCombo, IHalfHandedWeapon, IUniversalWeapon> {}

//1.5 Handed Weapons (Balanced, Power)
public interface IMace : IEquippable <IMeleeWeapon, IEquipment4HitCombo, IHalfHandedWeapon, IPowerWeapon> {}
public interface IOrb : IEquippable <IRangedWeapon, IEquipment4HitCombo, IHalfHandedWeapon, IPowerWeapon> {}

//2 Handed Weapons (Power)
public interface IGreatSword : IEquippable <IMeleeWeapon, IEquipment3HitCombo, IDoubleHandedWeapon, IPowerWeapon> {}
public interface IGreatShield : IEquippable <IMeleeWeapon, IEquipment2HitCombo, IDoubleHandedWeapon, IPowerWeapon> {}
public interface IGreatBow : IEquippable <IRangedWeapon, IEquipment2HitCombo, IDoubleHandedWeapon, IPowerWeapon> {}

//OffHand (Balanced)
public interface IShieldTotem : IEquippable <IUtilityWeapon, IEquipmentAuxiliary, IOneHandedWeapon, IBalanceWeapon> {}
public interface IBladeRelic : IEquippable <IUtilityWeapon, IEquipmentAuxiliary, IOneHandedWeapon, IBalanceWeapon> {}
public interface IPrayerBeads : IEquippable <IRangedWeapon, IEquipment3HitCombo, IOneHandedWeapon, IBalanceWeapon> {}
*/