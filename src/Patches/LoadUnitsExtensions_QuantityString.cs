using HarmonyLib;
using Model.Definition.Data;
using UnityEngine;

namespace better_ui_mod.Patches;

/// <summary>
/// no comma in gallons numbers
/// </summary>
[HarmonyPatch(typeof(LoadUnitsExtensions), nameof(LoadUnitsExtensions.QuantityString))]
public class LoadUnitsExtensions_QuantityString_Patch
{
	private static void Postfix(ref string __result, LoadUnits units, float quantity)
	{
		if (units != LoadUnits.Gallons)
		{
			return;
		}
		
		__result = Mathf.RoundToInt(quantity) + " gal";
	}
}