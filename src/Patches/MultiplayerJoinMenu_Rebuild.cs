using System.Linq;
using HarmonyLib;
using UI.Menu;

namespace better_ui_mod.Patches;

/// <summary>
/// Filter by name
/// </summary>
[HarmonyPatch(typeof(MultiplayerJoinMenu), nameof(MultiplayerJoinMenu.Rebuild))]
public static class MultiplayerJoinMenu_Rebuild_Patch
{
	private static void Prefix(MultiplayerJoinMenu __instance)
	{
		if (!ModEntryPoint.MySettings!.AlsoSearchLobbiesByName)
		{
			return;
		}
		
		// should we filter with Network.Steam.LobbyKeys.Name in ClientLobbyHelper_FetchLobbies_Patch?
		var filterText = __instance.filterField.text;
		__instance._lobbies = __instance._lobbies
			.Where(lobby =>
				// name
				lobby.Name.ToLower().Contains(filterText.ToLower())
				// password protected
				&& (!ModEntryPoint.MySettings.HidePasswordProtectedLobbies || !lobby.IsPasswordProtected())
				)
			.ToList();
	}

	private static void Postfix(MultiplayerJoinMenu __instance)
	{
		if (!ModEntryPoint.MySettings!.AlsoSearchLobbiesByName)
		{
			return;
		}

		__instance.statusLabel.text = __instance.statusLabel.text.Replace("with exact reporting mark", "matching");
	}
}
