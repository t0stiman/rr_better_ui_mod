using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using HeathenEngineering.SteamworksIntegration;
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
		
		// should we filter with Network.Steam.LobbyKeys.Name in ClientLobbyHelper_FetchLobbies_Patch?
		var filterText = __instance.filterField.text;
		__instance._lobbies = __instance._lobbies
			.Where(lobby => 
				lobby.Name.ToLower().Contains(filterText.ToLower())
				&& (!Main.MySettings.HidePasswordProtectedLobbies || !lobby.IsPasswordProtected())
				)
			.ToList();
	}
}
