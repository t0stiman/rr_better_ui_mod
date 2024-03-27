using HarmonyLib;
using Model;
using Model.Definition;
using RollingStock;
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
			return Stuff.EXECUTE_ORIGINAL;
		}
		
		// keep R N F on diesels
		if (TrainController.Shared.SelectedLocomotive.Definition.Archetype == CarArchetype.LocomotiveDiesel)
		{
			return Stuff.EXECUTE_ORIGINAL;
		}
		
		// abstractReverser is the value of the reverser. -1 is 100% reverse, 0 is neutral, 1 is 100% forward
		var reverserPercent = Mathf.RoundToInt(abstractReverser * 100);
		__result = reverserPercent.ToString();

		return Stuff.SKIP_ORIGINAL;
	}
}

/// <summary>
/// ShowThrottleNotchInsteadOfPercentage
/// </summary>
[HarmonyPatch(typeof(LocomotiveControlsUIAdapter))]
[HarmonyPatch(nameof(LocomotiveControlsUIAdapter.Update))]
public class LocomotiveControlsUIAdapter_Update_Patch
{
	private static bool Prefix(LocomotiveControlsUIAdapter __instance)
	{
		if (!Main.MySettings.ShowThrottleNotchInsteadOfPercentage)
		{
			return Stuff.EXECUTE_ORIGINAL;
		}

		var locomotive = LocomotiveControlsUIAdapter.Locomotive;
		__instance.controls.gameObject.SetActive(locomotive != null);
		if (locomotive == null)
		{
			return Stuff.SKIP_ORIGINAL;
		}

		__instance._updatingControls = true;
		var locomotiveControl = locomotive.locomotiveControl;
		__instance.throttleSlider.SetValueUnlessDragging(locomotiveControl.AbstractThrottle);
		__instance.reverserSlider.SetValueUnlessDragging(locomotiveControl.AbstractReverser);
		__instance.locomotiveBrakeSlider.SetValueUnlessDragging(locomotiveControl.LocomotiveBrakeSetting);
		__instance.trainBrakeSlider.SetValueUnlessDragging(locomotiveControl.TrainBrakeSetting);
		
		// ====== changed: ======

		var throttleNotches = locomotiveControl.ThrottleNotches == 0 ? 100 : locomotiveControl.ThrottleNotches;
		var selectedNotch = locomotiveControl.AbstractThrottle * throttleNotches;
		__instance.throttleSlider.handleText.SetText(Mathf.RoundToInt(selectedNotch).ToString());
		
		// ======================
		
		__instance.reverserSlider.handleText.text = LocomotiveControlsUIAdapter.ReverserText(locomotiveControl.AbstractReverser);
		__instance.reverserSlider.wholeNumbers = locomotive.Archetype != CarArchetype.LocomotiveSteam;
		__instance.simpleSlider.SetValueUnlessDragging(__instance.SimpleValue());
		__instance.directionSlider.value = __instance.DirectionValue();
		if (__instance.simpleSlider.value > 0.15000000596046448)
		{
			__instance.simpleSlider.handleText.SetText("{0}%", Mathf.RoundToInt(locomotiveControl.AbstractThrottle * 100f));
		}
		else
		{
			__instance.simpleSlider.handleText.text = __instance.simpleSlider.value >= -0.15000000596046448 ? "N" : "B";
		}

		__instance.directionSlider.handleText.text = __instance.directionSlider.value < 0.0 ? "R" : "F";
		__instance._updatingControls = false;

		return Stuff.SKIP_ORIGINAL;
	}
}