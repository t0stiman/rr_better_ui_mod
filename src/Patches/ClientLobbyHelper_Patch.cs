using System;
using System.Linq;
using System.Threading.Tasks;
using HarmonyLib;
using HeathenEngineering.SteamworksIntegration;
using HeathenEngineering.SteamworksIntegration.API;
using Network.Steam;
using Steamworks;
using UnityEngine;

namespace better_ui_mod.Patches;

/// <summary>
/// Only difference with the base method is we don't filter any lobbies (we do that in MultiplayerJoinMenu_Rebuild_Patch)
/// </summary>
[HarmonyPatch(typeof(ClientLobbyHelper))]
[HarmonyPatch(nameof(ClientLobbyHelper.FetchLobbies))]
public static class ClientLobbyHelper_FetchLobbies_Patch
{
	private static bool Prefix(ref ClientLobbyHelper __instance, ref Task<Lobby[]> __result, string reportingMark)
	{
		if (!Main.MySettings.AlsoSearchLobbiesByName)
		{
			return Stuff.EXECUTE_ORIGINAL;
		}

		var tcs = new TaskCompletionSource<Lobby[]>();
		Matchmaking.Client.AddRequestLobbyListStringFilter("ver", Application.version,
			ELobbyComparison.k_ELobbyComparisonEqual);
		Matchmaking.Client.AddRequestLobbyListStringFilter("status", "open", ELobbyComparison.k_ELobbyComparisonEqual);
		Matchmaking.Client.AddRequestLobbyListDistanceFilter(ELobbyDistanceFilter.k_ELobbyDistanceFilterWorldwide);
		// if (!string.IsNullOrEmpty(reportingMark))
		// 	Matchmaking.Client.AddRequestLobbyListStringFilter("rpmk", ServerLobbyHelper.SanitizeReportingMark(reportingMark), ELobbyComparison.k_ELobbyComparisonEqual);

		var aaa = new idfk(__instance, tcs);
		Matchmaking.Client.RequestLobbyList(aaa.Callback);

		__result = aaa.tcs.Task;

		return Stuff.SKIP_ORIGINAL;
	}

	private class idfk
	{
		public idfk(ClientLobbyHelper clientLobbyHelper, TaskCompletionSource<Lobby[]> tcs)
		{
			_clientLobbyHelper = clientLobbyHelper;
			this.tcs = tcs;
		}

		private ClientLobbyHelper _clientLobbyHelper;
		public TaskCompletionSource<Lobby[]> tcs;

		public void Callback(Lobby[] lobbies, bool error)
		{
			if (error)
			{
				_clientLobbyHelper._lobbies = Array.Empty<Lobby>();
				Debug.LogError("Error fetching lobbies");
				tcs.SetException(new Exception("Error fetching lobbies"));
			}
			else
			{
				Debug.Log($"Received {lobbies.Length} lobbies: {string.Join(", ", lobbies.Select(l => l.Name))}");
				_clientLobbyHelper._lobbies = lobbies;
				tcs.SetResult(lobbies);
			}
		}
	}
}