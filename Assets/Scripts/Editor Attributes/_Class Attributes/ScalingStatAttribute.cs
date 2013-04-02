using UnityEngine;
using System;

public class ScalingStatAttribute : PropertyAttribute
{
	public enum ScalingType {
		Base,
		Regen
	}

	public string scaling;

	public ScalingStatAttribute (ScalingType type) {
		scaling = type.ToString();
	}
}


