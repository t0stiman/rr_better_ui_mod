using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Game;
using Game.AccessControl;
using Game.Messages;
using Game.Persistence;
using Game.State;
using HarmonyLib;
using Steamworks;
using UI.Builder;
using UI.CompanyWindow;
using UnityEngine;

namespace better_ui_mod.Patches;

//TODO make this a transpiler patch
[HarmonyPatch(typeof(EmployeesPanelBuilder))]
[HarmonyPatch(nameof(EmployeesPanelBuilder.BuildRecordsPanel))]
public class EmployeesPanelBuilder_Patch
{
  private static bool Prefix (
    UIPanelBuilder builder,
    PlayersManager playersManager,
    PlayerRecordsClientManager recordsManager,
    UIState<string> selectedPlayerId
    )
  {
    if (!Main.MySettings.ShowSteamProfileButton)
    {
      return Stuff.EXECUTE_ORIGINAL;
    }
    
    var playerId1 = PlayersManager.PlayerId;
    var first = new List<UIPanelBuilder.ListItem<EmployeesPanelBuilder.RecordsPanelItem>>();
    var second1 = new List<UIPanelBuilder.ListItem<EmployeesPanelBuilder.RecordsPanelItem>>();
    var second2 = new List<UIPanelBuilder.ListItem<EmployeesPanelBuilder.RecordsPanelItem>>();
    var dictionary = playersManager.AllPlayers.ToDictionary(p => p.PlayerId, p => p);
    foreach (var playerRecord1 in recordsManager.PlayerRecords)
    {
      var key = playerRecord1.Key;
      var record = playerRecord1.Value;
      var str = key.String;
      if (dictionary.TryGetValue(key, out var player))
      {
        var recordsPanelItem = new EmployeesPanelBuilder.RecordsPanelItem(str, player, record);
        first.Add(new UIPanelBuilder.ListItem<EmployeesPanelBuilder.RecordsPanelItem>(str, recordsPanelItem, "Online", player.Name));
      }
      else if (record.AccessLevel == AccessLevel.Banned)
      {
        var recordsPanelItem = new EmployeesPanelBuilder.RecordsPanelItem(str, null, record);
        second2.Add(new UIPanelBuilder.ListItem<EmployeesPanelBuilder.RecordsPanelItem>(str, recordsPanelItem, "Banned", record.Name));
      }
      else
      {
        var recordsPanelItem = new EmployeesPanelBuilder.RecordsPanelItem(str, null, record);
        second1.Add(new UIPanelBuilder.ListItem<EmployeesPanelBuilder.RecordsPanelItem>(str, recordsPanelItem, "Offline", record.Name));
      }
    }
    first.Sort();
    second1.Sort();
    second2.Sort();
    var list = first.Concat(second1).Concat(second2).ToList();
    builder.AddListDetail<EmployeesPanelBuilder.RecordsPanelItem>((IEnumerable<UIPanelBuilder.ListItem<EmployeesPanelBuilder.RecordsPanelItem>>) list, selectedPlayerId, (Action<UIPanelBuilder, EmployeesPanelBuilder.RecordsPanelItem>) ((builder5, item) =>
    {
      if (item == null)
      {
        builder5.AddLabel("Please select a player.");
      }
      else
      {
        builder5.AddTitle(item.Name, null);
        var accessLevelOptions = new List<AccessLevel>
        {
          AccessLevel.Banned,
          AccessLevel.Passenger,
          AccessLevel.Crew,
          AccessLevel.Dispatcher,
          AccessLevel.Trainmaster,
          AccessLevel.Officer,
          AccessLevel.President
        };
        var accessLevelStrings = accessLevelOptions.Select(al => al.ToString());
        var index = accessLevelOptions.ToList().IndexOf(item.Record.AccessLevel);
        builder5.AddSection("About", builder6 =>
        {
          builder6.AddField("Last Connected", item.Record.LastConnected.ToLocalTime().ToString(CultureInfo.CurrentCulture));
          var removePlayerRecordMessage = new RemovePlayerRecord(item.RecordKey);
          if (!StateManager.CheckAuthorizedToSendMessage(removePlayerRecordMessage) || item.Player != null)
            return;
          builder6.AddField("", builder6.AddButton("Remove Record", () => StateManager.ApplyLocal(removePlayerRecordMessage)).RectTransform);
        });
        builder5.AddSection("Access", builder7 =>
        {
          if (index < 0)
          {
            builder7.AddField("Role", string.Format("Unexpected: {0}", item.Record.AccessLevel));
          }
          else
          {
            var control = builder7.AddDropdown(accessLevelStrings.ToList(), index, newIndex =>
            {
              var accessLevel = accessLevelOptions[newIndex];
              Serilog.Log.Debug<string, AccessLevel>("Request Set Access Level: {recordKey} {newAccessLevel}", item.RecordKey, accessLevel);
              StateManager.ApplyLocal(new RequestSetAccessLevel(item.RecordKey, accessLevel));
              LeanTween.delayedCall(1f, builder7.Rebuild);
            });
            builder7.AddField("Role", control);
          }
          builder7.AddField("Since", item.Record.AccessLevelChanged.ToLocalTime().ToString(CultureInfo.CurrentCulture));
        });
        builder5.AddSection("Steam Data", builder8 =>
        {
          var steamID = item.Record.SteamId.ToString("D");
          
          builder8.AddField("Steam ID", steamID);
          builder8.AddField("Steam Name", EmployeesPanelBuilder.SteamNameForId(item.Record.SteamId));
          
          builder8.AddButtonCompact("Steam Profile", () =>
          {
            SteamFriends.ActivateGameOverlayToWebPage(
              $"https://steamcommunity.com/profiles/{steamID}");
          });
        });
      }
    }));


    return Stuff.SKIP_ORIGINAL;
  }
}


  