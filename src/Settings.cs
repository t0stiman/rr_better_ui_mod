using UnityEngine;
using UnityModManagerNet;

namespace better_ui_mod
{
	public class Settings : UnityModManager.ModSettings
	{
		public bool ShowReverserPercentage = true;
		public bool ShowComsumablesPercent = true;
		public bool ShowSteamProfileButton = true;

		public void Setup(){}
		
		public void Draw(UnityModManager.ModEntry modEntry)
		{
			ShowReverserPercentage = GUILayout.Toggle(ShowReverserPercentage, "Show reverser percentage instead of R/N/F");
			ShowComsumablesPercent = GUILayout.Toggle(ShowComsumablesPercent, "Show fill percentage of coal, water and diesel on tenders, diesel locomotives, coaling towers and diesel refuel stands");
			ShowSteamProfileButton = GUILayout.Toggle(ShowSteamProfileButton,
				"Show a button in the 'Employees' menu that takes you to the steam profile of the selected player");
		}

		private void DrawFloatInput(ref string text, ref float number)
		{
			text = GUILayout.TextField(text);
			if (float.TryParse(text, out float parsed))
			{
				number = parsed;
			}
			else
			{
				GUILayout.Label($"not a valid number");
			}
		}

		public override void Save(UnityModManager.ModEntry modEntry)
		{
			Save(this, modEntry);
		}
	}
}