using HarmonyLib;
using TMPro;
using UI.EngineControls;
using UnityEngine;

namespace better_ui_mod.Patches;

/// <summary>
/// Setting the reverser to 10% will sometimes make the control window show 'N', and sometimes 10. This tweak makes it consistently show N.
/// </summary>
[HarmonyPatch(typeof(ManualControls))]
[HarmonyPatch(nameof(ManualControls.SetReverserText))]
public class ManualControls_Patch
{
	private static void Postfix(ManualControls __instance, float abstractReverser, TMP_Text text)
	{
		if (!Main.MySettings.NeutralTweak)
		{
			return;
		}
		
		if (Mathf.Abs(abstractReverser) < 0.099)
		{
			text.text = "N";
		}
	}
}