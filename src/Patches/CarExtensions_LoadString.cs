using HarmonyLib;
using Model;
using Model.Definition.Data;
using Model.Ops.Definition;
using Model.Ops;
using UnityEngine;

namespace better_ui_mod.Patches;

//todo update for RR 0.6.1

/// <summary>
/// add a space between value and T
/// no comma in gallons numbers
/// </summary>
// [HarmonyPatch(typeof(CarExtensions), nameof(CarExtensions.LoadString))]
// public class CarExtensions_LoadString_Patch
// {
// 	private static bool Prefix(ref string __result, CarLoadInfo info, Load load)
// 	{
// 		switch (load.units)
// 		{
// 			case LoadUnits.Pounds:
// 				__result = $"{Stuff.PoundsToUSShortTons(info.Quantity):0.0} T {load.description}";
// 				return Stuff.SKIP_ORIGINAL;
// 			case LoadUnits.Gallons:
// 				__result = $"{Mathf.RoundToInt(info.Quantity)} gal {load.description}";
// 				return Stuff.SKIP_ORIGINAL;
// 		}
//
// 		return Stuff.EXECUTE_ORIGINAL;
// 	}
// }