using UnityEngine;

/// <summary>
/// Prepares variables to be used by IntRangeDrawer.
/// </summary>
public class IntRangeAttribute : PropertyAttribute
{
	public int min;
	public int max;
	public IntRangeAttribute(int min, int max)
	{
		this.min = min;
		this.max = max;
	}
}
