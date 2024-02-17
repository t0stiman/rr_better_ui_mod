using HarmonyLib;
using UI;
using UnityEngine;

namespace better_ui_mod.Patches;

/// <summary>
/// reverser
/// </summary>
[HarmonyPatch(typeof(LocomotiveControlsUIAdapter))]
[HarmonyPatch(nameof(LocomotiveControlsUIAdapter.ReverserText))]
public class LocomotiveControlsUIAdapter_ReverserText_Patch
{
	private static bool Prefix(float abstractReverser, ref string __result)
	{
		if (!Main.MySettings.ShowReverserPercentage)
		{
			return true; //execute original function
		}
		
		// abstractReverser is the value of the reverser. -1 is 100% reverse, 0 is neutral, 1 is 100% forward
		var reverserPercent = Mathf.RoundToInt(abstractReverser * 100);
		__result = reverserPercent.ToString();
		
		return false;
	}
}