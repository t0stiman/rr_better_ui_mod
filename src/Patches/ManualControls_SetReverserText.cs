using HarmonyLib;
using TMPro;
using UI.EngineControls;
using UnityEngine;

namespace better_ui_mod.Patches;

/// <summary>
/// Setting the reverser to -10% will sometimes make the control window show 'N', and sometimes -10. This tweak makes it consistently show -10.
/// </summary>
[HarmonyPatch(typeof(ManualControls), nameof(ManualControls.SetReverserText))]
public class ManualControls_SetReverserText_Patch
{
	private static void Postfix(ManualControls __instance, float abstractReverser, TMP_Text text)
	{
		if (!ModEntryPoint.MySettings!.NeutralTweak || __instance._isDiesel)
		{
			return;
		}

		var reverserAbs = Mathf.Abs(abstractReverser);
		if (reverserAbs > 0.99 && reverserAbs < 0.10000000149011612)
		{
			int num = Mathf.RoundToInt(abstractReverser * 100f);
			if (num < 0)
			{
				if (num == -100)
					text.SetText("-1<mspace=0.6em>00</mspace>");
				else
					text.SetText("-<mspace=0.6em>{0}</mspace>", (float) -num);
			}
			else if (num == 100)
				text.SetText("1<mspace=0.6em>00</mspace>");
			else
				text.SetText("<mspace=0.6em>{0}</mspace>", (float) num);
		}
	}
}