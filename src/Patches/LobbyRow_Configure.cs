using HarmonyLib;
using UI.Menu;

namespace better_ui_mod.Patches;

/*

/// <summary>
/// TODO Show max amount of players in the server browser 
/// </summary>
[HarmonyPatch(typeof(LobbyRow))]
[HarmonyPatch(nameof(LobbyRow.Configure), typeof(int), typeof(object))]
public static class LobbyRow_Configure_Patch
{
	// todo always 32?
	private static bool Prefix(ref LobbyRow __instance, int listIndex, object obj)
	{
		if (!ModEntryPoint.MySettings.ShowMaxPlayers)
		{
			return Stuff.EXECUTE_ORIGINAL;
		}
		
		__instance.ListIndex = listIndex;
		__instance._info = (LobbyRow.Info) obj;

		var lobby = __instance._info.LobbyData;
		var countText = $"{lobby.Members.Length}/{lobby.MaxMembers}";
		
		__instance.Configure(
			__instance._info.LobbyName, 
			__instance._info.Version, 
			countText, 
			__instance._info.AllowNewPlayers, 
			__instance._info.HasPassword);

		return Stuff.SKIP_ORIGINAL;
	}
}

*/