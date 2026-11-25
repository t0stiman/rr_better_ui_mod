using HarmonyLib;
using Model.Definition.Data;
using Model.Ops;
using Model.Ops.Definition;
using RollingStock;
using UnityEngine;

namespace better_ui_mod.Patches;

/// <summary>
/// Show fill percentage and capacity on tenders, freight wagons and diesel locomotives
/// Contains contributions by Kakashi Hatake and Percival Binglebottom
/// </summary>
[HarmonyPatch(typeof(CarPickable), nameof(CarPickable.TooltipText))]
public class CarPickable_TooltipText_Patch
{
	private static void Postfix(ref string __result, CarPickable __instance)
	{
		if (!ModEntryPoint.MySettings!.ShowComsumablesPercent)
		{
			return;
		}

		for (var loadSlotNr = 0; loadSlotNr < __instance.car.Definition.LoadSlots.Count; loadSlotNr++)
		{
			LoadSlot loadSlot = __instance.car.Definition.LoadSlots[loadSlotNr];
			var loadInfo = __instance.car.GetLoadInfo(loadSlotNr);
			if (!loadInfo.HasValue)
			{
				continue;
			}
			
			Load load = CarLoadLibrary.GetLoad(loadInfo.Value.LoadId);
			string unitText = Stuff.UnitToText(load.units);
			
			var capacityText = Stuff.GetCapacityText(load.units, loadSlot.MaximumCapacity);
			var quantity = loadInfo.Value.Quantity;
			var fillPercentage = Mathf.RoundToInt(quantity / loadSlot.MaximumCapacity * 100);
			
			// max capacity is shown when car isn't empty or full
			var maxCapacityText = fillPercentage != 0 && fillPercentage != 100 ? $" / {capacityText}" : "";
			
			__result = __result.Replace($" {unitText} {load.description}", $"{maxCapacityText} {unitText} {load.description} ({fillPercentage}%)");
		}
	}
}
