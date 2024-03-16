using HarmonyLib;
using Model.Definition.Data;
using UnityEngine;

namespace better_ui_mod.Patches;

/// <summary>
/// no comma in gallons numbers
/// </summary>
[HarmonyPatch(typeof(LoadUnitsExtensions))]
[HarmonyPatch(nameof(LoadUnitsExtensions.QuantityString))]
public class LoadUnitsExtensions_Patch
{
	private static bool Prefix(ref string __result, LoadUnits units, float quantity)
	{
		switch (units)
		{
			case LoadUnits.Gallons:
				__result = Mathf.RoundToInt(quantity) + " gal";
				return false; //skip original function
		}
		
		return true; //execute original function 
	}
}