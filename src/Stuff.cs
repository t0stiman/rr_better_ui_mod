using Model.Definition.Data;

namespace better_ui_mod;

public static class Stuff
{
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
}