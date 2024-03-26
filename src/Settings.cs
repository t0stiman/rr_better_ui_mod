using UnityEngine;
using UnityModManagerNet;

namespace better_ui_mod
{
	public class Settings : UnityModManager.ModSettings
	{
		public bool ShowReverserPercentage = true;
		public bool ShowComsumablesPercent = true;
		public bool ShowSteamProfileButton = true;
		public bool AlsoSearchLobbiesByName = true;

		public void Setup(){}
		
		public void Draw(UnityModManager.ModEntry modEntry)
		{
			ShowReverserPercentage = GUILayout.Toggle(ShowReverserPercentage, "Show reverser percentage instead of R/N/F");
			ShowComsumablesPercent = GUILayout.Toggle(ShowComsumablesPercent, "Show fill percentage on cargo wagons, tenders, diesel locomotives, coaling towers and diesel fuel stands");
			ShowSteamProfileButton = GUILayout.Toggle(ShowSteamProfileButton,
				"Show a button in the 'Employees' menu that takes you to the steam profile of the selected player");
			AlsoSearchLobbiesByName = GUILayout.Toggle(AlsoSearchLobbiesByName, "In the multiplayer join menu, search by server name as well as reporting mark");
		}

		public override void Save(UnityModManager.ModEntry modEntry)
		{
			Save(this, modEntry);
		}
	}
}
