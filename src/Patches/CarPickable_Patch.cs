using HarmonyLib;
using Model;
using Model.Definition.Data;
using Model.OpsNew;
using RollingStock;
using UnityEngine;

namespace better_ui_mod.Patches;

/// <summary>
/// Show fill percentage and capacity on tenders, freight wagons and diesel locomotives
/// Contains contributions by Kakashi Hatake and Percival Binglebottom
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

		for (var loadSlotNr = 0; loadSlotNr < __instance.car.Definition.LoadSlots.Count; loadSlotNr++)
		{
			var loadSlot = __instance.car.Definition.LoadSlots[loadSlotNr];
			var loadInfo = __instance.car.GetLoadInfo(loadSlotNr);
			if (!loadInfo.HasValue)
			{
				continue;
			}

			var load = CarPrototypeLibrary.instance.LoadForId(loadInfo.Value.LoadId);
			var unit = Stuff.UnitToText(load.units);
			
			var capacityText = Stuff.GetCapacityText(load.units, loadSlot.MaximumCapacity);
			var quantity = loadInfo.Value.Quantity;
			var fillPercentage = Mathf.RoundToInt(quantity / loadSlot.MaximumCapacity * 100);
			
			// max capacity is shown when car isn't empty or full
			var thingy = fillPercentage != 0 && fillPercentage != 100 ? $" / {capacityText}" : "";

			__result = __result.Replace($" {unit} {load.description}", $"{thingy} {unit} {load.description} ({fillPercentage}%)");
		}

		// each unit on a new line
		__result = __result.Replace(" - ", "\n");
		__result = __result.Replace(", ", "\n");
	}
}
