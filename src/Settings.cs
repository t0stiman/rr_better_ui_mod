using UnityEngine;
using UnityModManagerNet;

namespace better_ui_mod
{
	public class Settings : UnityModManager.ModSettings
	{
		public bool ShowComsumablesPercent = true;
		public bool ShowSteamProfileButton = true;
		public bool AlsoSearchLobbiesByName = true;
		public bool HidePasswordProtectedLobbies = false;
		// public bool ShowMaxPlayers = true;

		public void Setup(){}
		
		public void Draw(UnityModManager.ModEntry modEntry)
		{
			GUILayout.Label("Multiplayer join menu");
			GUILayout.Space(10);
			
			AlsoSearchLobbiesByName = GUILayout.Toggle(AlsoSearchLobbiesByName, "Search by lobby name as well as reporting mark");
			HidePasswordProtectedLobbies = GUILayout.Toggle(HidePasswordProtectedLobbies,
				"Hide password-protected lobbies");
			// ShowMaxPlayers = GUILayout.Toggle(ShowMaxPlayers, "Show maximum amount of players"); //TODO
			
			GUILayout.Space(10);
			GUILayout.Label("Misc.");
			GUILayout.Space(10);
			
			ShowComsumablesPercent = GUILayout.Toggle(ShowComsumablesPercent, "Show fill percentage on cargo wagons, tenders, diesel locomotives, coaling towers and diesel fuel stands");
			ShowSteamProfileButton = GUILayout.Toggle(ShowSteamProfileButton,
				"Show a button in the 'Employees' menu that takes you to the steam profile of the selected player");
		}

		public override void Save(UnityModManager.ModEntry modEntry)
		{
			Save(this, modEntry);
		}
	}
}
