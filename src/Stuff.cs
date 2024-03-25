using Model.Definition.Data;
using UnityEngine;

namespace better_ui_mod;

public static class Stuff
{
	public const bool EXECUTE_ORIGINAL = true;
	public const bool SKIP_ORIGINAL = false;
	
	public static float PoundsToUSShortTons(float pounds)
	{
		return pounds / 2000;
	}

	public static string UnitToText(LoadUnits loadUnits)
	{
		switch (loadUnits)
		{
			case LoadUnits.Pounds:
				return "T"; //US short ton
			case LoadUnits.Gallons:
				return "gal";
			case LoadUnits.Quantity:
				return "";
			default:
				Main.Warning($"{nameof(UnitToText)}: unit not implemented: {loadUnits.ToString()}");
				return "";
		}
	}
	
	public static string GetCapacityText(LoadUnits units, float capacity)
	{
		string capacityText = default;
		switch (units)
		{
			case LoadUnits.Pounds:
				capacityText = Stuff.PoundsToUSShortTons(capacity).ToString("0.0");
				break;
			case LoadUnits.Gallons:
			case LoadUnits.Quantity:
				capacityText = Mathf.RoundToInt(capacity).ToString();
				break;
			default:
				Main.Error($"{nameof(GetCapacityText)}: unreachable code reached");
				break;
		}

		return capacityText;
	}
}