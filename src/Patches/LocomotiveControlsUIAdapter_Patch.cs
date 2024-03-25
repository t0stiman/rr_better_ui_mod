using HarmonyLib;
using Model.Definition;
using UI;
using UnityEngine;

namespace better_ui_mod.Patches;

/// <summary>
/// reverser number instead of R N F on steam locomotives
/// </summary>
[HarmonyPatch(typeof(LocomotiveControlsUIAdapter))]
[HarmonyPatch(nameof(LocomotiveControlsUIAdapter.ReverserText))]
public class LocomotiveControlsUIAdapter_ReverserText_Patch
{
	private static bool Prefix(LocomotiveControlsUIAdapter __instance, float abstractReverser, ref string __result)
	{
		if (!Main.MySettings.ShowReverserPercentage)
		{
			return true; //execute original function
		}
		
		// keep R N F on diesels
		if (TrainController.Shared.SelectedLocomotive.Definition.Archetype == CarArchetype.LocomotiveDiesel)
		{
			return true;
		}
		
		// abstractReverser is the value of the reverser. -1 is 100% reverse, 0 is neutral, 1 is 100% forward
		var reverserPercent = Mathf.RoundToInt(abstractReverser * 100);
		__result = reverserPercent.ToString();
		
		return false;
	}
}