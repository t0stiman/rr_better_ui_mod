using HarmonyLib;
using RollingStock;
using UnityEngine;

namespace better_ui_mod.Patches;

/// <summary>
/// percentage on coaling towers and diesel refueling stands
/// </summary>
[HarmonyPatch(typeof(IndustryContentHoverable))]
[HarmonyPatch(nameof(IndustryContentHoverable.TooltipText))]
public class IndustryContentHoverable_Patch
{
	private static void Postfix(ref string __result, IndustryContentHoverable __instance)
	{
		if (!Main.MySettings.ShowComsumablesPercent)
		{
			return;
		}
		
		var capacities = __instance.industry.GetCapacities();

		var storage = __instance.industry.Storage;
		foreach (var load in storage.Loads())
		{
			var unit = Stuff.UnitToText(load.units);
			
			var gotCapacity = capacities.TryGetValue(load.id, out float capacity);
			if (!gotCapacity)
			{
				Main.Error($"{nameof(IndustryContentHoverable_Patch)}: can't get capacity for id {load.id}");
				continue;
			}
			
			var capacityText = Stuff.GetCapacityText(load.units, capacity);
			var quantity = storage.QuantityInStorage(load);
			var fillPercentage = Mathf.RoundToInt(quantity / capacity * 100);
			
			// max capacity is shown when the thing isn't empty or full
			var thingy = fillPercentage != 0 && fillPercentage != 100 ? $" / {capacityText}" : "";
			
			__result = __result.Replace($" {unit} {load.description}", $"{thingy} {unit} {load.description} ({fillPercentage}%)");
		}
	}
}
