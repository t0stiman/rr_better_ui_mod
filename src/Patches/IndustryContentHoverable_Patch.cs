using HarmonyLib;
using Model.OpsNew;
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

		float coalCapacity = 0;
		float dieselCapacity = 0;
		var contractMultiplier = __instance.industry.GetContractMultiplier();
		
		foreach (var unloader in __instance.industry.GetComponentsInChildren<IndustryUnloader>())
		{
			switch (unloader.load.id)
			{
				case Names.COAL_ID:
					coalCapacity = unloader.maxStorage * contractMultiplier;
					break;
				case Names.DIESEL_ID:
					dieselCapacity = unloader.maxStorage * contractMultiplier;
					break; 
			}
		}
		
		var storage = __instance.industry.Storage;
		
		foreach (var load in storage.Loads())
		{
			if (load.id == Names.COAL_ID && coalCapacity > 0.1)
			{
				var coalQuantity = storage.QuantityInStorage(load);
				var coalPercent = Mathf.RoundToInt(coalQuantity / coalCapacity * 100);

				// the symbol of tonne is a small t, not capitol T
				__result = __result.Replace("T Coal", $"t coal ({coalPercent}%)");
			}
			else if (load.id == Names.DIESEL_ID && dieselCapacity > 0.1)
			{
				var dieselQuantity = storage.QuantityInStorage(load);
				var dieselPercent = Mathf.RoundToInt(dieselQuantity / dieselCapacity * 100);

				__result = __result.Replace("gal Diesel Fuel", $"gal diesel ({dieselPercent}%)");
			}
		}
	}
}