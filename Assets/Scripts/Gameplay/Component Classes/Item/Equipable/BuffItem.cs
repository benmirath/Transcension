using UnityEngine;
using System.Collections;
using System;

public class BuffItem : Item {
	private Hashtable buffs;	
	
	public BuffItem(string name):base(name) {
		buffs = new Hashtable();
	}
	
	public BuffItem(Hashtable ht) {
		buffs = ht;	
	}
	
	public void AddBuff(Attribute stat, int mod) {
		try {
			buffs.Add (stat, mod);	
		}
		catch(Exception e) {
			Debug.LogWarning(e.ToString());	
		}
	}
	
	public void RemoveBuff(BaseStat stat) {
		buffs.Remove(stat.Name);
	}
	
	public int BuffCount() {
		return buffs.Count;	
	}
	
	public Hashtable GetBuffs() {
		return buffs;	
	}
}
