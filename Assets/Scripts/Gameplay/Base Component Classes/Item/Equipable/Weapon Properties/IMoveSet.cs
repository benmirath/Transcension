using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public interface IMoveSet
//{
//	MeleeProperties combo1;
//	MeleeProperties combo2;
//	MeleeProperties combo3;
//	MeleeProperties combo4;
//
//}
///<summary>
/// Elements required for (all) equipment stats.
/// Lists (specified for each weapon)
/// -Equipment Type (sword vs spear)
/// -Attribute Type (physical vs fire)
/// -Wielding Type (onehanded vs dualwielded)
/// -Style Type (Power vs Balanced)
/// 
/// Base Stats (standard, and does not normally change during the course of play)
/// -Name
/// -Scaling Stat
/// -Scaling Ratio (percentage of scaling stat that gets added to weapon's base damage)
/// -Base Damage
/// -Base Defense
/// 
/// Attack Stats
/// -Stat instance of all attacks available to weapon.
/// 
/// Implemented Stats
/// -Scaling Buff (determined by calculating the value of scaling stat and ratio)
/// -Ability Buff (determined by current attack being used, modifying the base damage (i.e. a finisher attack hitting for 1.75 the base damage value))
/// -Weapon Buff (determined based on any abilities/items/effects that boost the weapons damage)
/// -Real Damage (((base damage * scaling buff) * ability buff) + Weapon Buff)
/// </summary>	  	
