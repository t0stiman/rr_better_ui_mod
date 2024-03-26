using System.Linq;
using HarmonyLib;
using UI.Menu;

namespace better_ui_mod.Patches;

/// <summary>
/// Filter by name
/// </summary>
[HarmonyPatch(typeof(MultiplayerJoinMenu))]
[HarmonyPatch(nameof(MultiplayerJoinMenu.Rebuild))]
public static class MultiplayerJoinMenu_Rebuild_Patch
{
	private static void Prefix(ref MultiplayerJoinMenu __instance)
	{
		if (!Main.MySettings.AlsoSearchLobbiesByName)
		{
			return;
		}
		
		var filterText = __instance.filterField.text;
		__instance._lobbies = __instance._lobbies
			.Where(lobby => lobby.Name.Contains(filterText))
			.ToList();
	}
}
