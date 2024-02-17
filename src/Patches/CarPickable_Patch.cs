using HarmonyLib;
using Model;
using Model.OpsNew;
using RollingStock;
using UnityEngine;

namespace better_ui_mod.Patches;

/// <summary>
/// percentage on tenders and diesel locomotives
/// </summary>
[HarmonyPatch(typeof(CarPickable))]
[HarmonyPatch(nameof(CarPickable.TooltipText))]
public class CarPickable_Patch
{
	private static void Postfix(ref string __result, CarPickable __instance)
	{
		if (!Main.MySettings.ShowComsumablesPercent)
		{
			return;
		}
		
		var coalLoad = CarPrototypeLibrary.instance.LoadForId(Names.COAL_ID);
		var coalInfo = __instance.car.QuantityCapacityOfLoad(coalLoad);
		var coalPercent = Mathf.RoundToInt(coalInfo.quantity / coalInfo.capacity * 100);
		
		__result = __result.Replace("T Coal, ", $"T Coal ({coalPercent}%)\n");
		
		var waterLoad = CarPrototypeLibrary.instance.LoadForId(Names.WATER_ID);
		var waterInfo = __instance.car.QuantityCapacityOfLoad(waterLoad);
		var waterPercent = Mathf.RoundToInt(waterInfo.quantity / waterInfo.capacity * 100);
		
		__result = __result.Replace("gal Water", $"gal Water ({waterPercent}%)");
	}
}