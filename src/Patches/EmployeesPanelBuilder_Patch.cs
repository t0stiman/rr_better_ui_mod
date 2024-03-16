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
      return true;
    }
    
    
    List<UIPanelBuilder.ListItem<EmployeesPanelBuilder.RecordsPanelItem>> first = new List<UIPanelBuilder.ListItem<EmployeesPanelBuilder.RecordsPanelItem>>();
    List<UIPanelBuilder.ListItem<EmployeesPanelBuilder.RecordsPanelItem>> second1 = new List<UIPanelBuilder.ListItem<EmployeesPanelBuilder.RecordsPanelItem>>();
    List<UIPanelBuilder.ListItem<EmployeesPanelBuilder.RecordsPanelItem>> second2 = new List<UIPanelBuilder.ListItem<EmployeesPanelBuilder.RecordsPanelItem>>();
    Dictionary<PlayerId, IPlayer> dictionary1 = playersManager.AllPlayers.ToDictionary(p => p.PlayerId, p => p);
    Dictionary<string, PlayerId> dictionary2 = recordsManager.PlayerIdToRecordKey.ToDictionary(kv => kv.Value, kv => kv.Key);
    foreach (KeyValuePair<string, PlayerRecord> playerRecord1 in recordsManager.PlayerRecords)
    {
      string str2 = playerRecord1.Key;
      PlayerRecord record = playerRecord1.Value;
      PlayerId key;
      IPlayer player;
      if (dictionary2.TryGetValue(str2, out key) && dictionary1.TryGetValue(key, out player))
      {
        EmployeesPanelBuilder.RecordsPanelItem recordsPanelItem = new EmployeesPanelBuilder.RecordsPanelItem(str2, player, record);
        first.Add(new UIPanelBuilder.ListItem<EmployeesPanelBuilder.RecordsPanelItem>(str2, recordsPanelItem, "Online", player.Name));
      }
      else if (record.AccessLevel == AccessLevel.Banned)
      {
        EmployeesPanelBuilder.RecordsPanelItem recordsPanelItem = new EmployeesPanelBuilder.RecordsPanelItem(str2, null, record);
        second2.Add(new UIPanelBuilder.ListItem<EmployeesPanelBuilder.RecordsPanelItem>(str2, recordsPanelItem, "Banned", record.Name));
      }
      else
      {
        EmployeesPanelBuilder.RecordsPanelItem recordsPanelItem = new EmployeesPanelBuilder.RecordsPanelItem(str2, null, record);
        second1.Add(new UIPanelBuilder.ListItem<EmployeesPanelBuilder.RecordsPanelItem>(str2, recordsPanelItem, "Offline", record.Name));
      }
    }
    first.Sort();
    second1.Sort();
    second2.Sort();
    List<UIPanelBuilder.ListItem<EmployeesPanelBuilder.RecordsPanelItem>> list = first.Concat(second1).Concat(second2).ToList();
    builder.AddListDetail(list, selectedPlayerId, (builder5, item) =>
    {
      if (item == null)
      {
        builder5.AddLabel("Please select a player.");
      }
      else
      {
        builder5.AddTitle(item.Name, null);
        List<AccessLevel> accessLevelOptions = new List<AccessLevel>
        {
          AccessLevel.Banned,
          AccessLevel.Passenger,
          AccessLevel.Crew,
          AccessLevel.Dispatcher,
          AccessLevel.Trainmaster,
          AccessLevel.Officer,
          AccessLevel.President
        };
        IEnumerable<string> accessLevelStrings = accessLevelOptions.Select(al => al.ToString());
        int index = accessLevelOptions.ToList().IndexOf(item.Record.AccessLevel);
        builder5.AddSection("About", builder6 =>
        {
          builder6.AddField("Last Connected", item.Record.LastConnected.ToLocalTime().ToString(CultureInfo.CurrentCulture));
          RemovePlayerRecord removePlayerRecordMessage = new RemovePlayerRecord(item.RecordKey);
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
            RectTransform control = builder7.AddDropdown(accessLevelStrings.ToList(), index, newIndex =>
            {
              AccessLevel accessLevel = accessLevelOptions[newIndex];
              // Main.Debug<string, AccessLevel>("Request Set Access Level: {recordKey} {newAccessLevel}", item.RecordKey, accessLevel);
              StateManager.ApplyLocal(new RequestSetAccessLevel(item.RecordKey, accessLevel));
              LeanTween.delayedCall(1f, new Action(builder7.Rebuild));
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
    });


    return false;
  }
}


// [HarmonyPatch(typeof(EmployeesPanelBuilder))]
// [HarmonyPatch(nameof(EmployeesPanelBuilder.BuildRecordsPanel))]
// static class aaaaap
// {
//   static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
//   {
//     try
//     {
//       var codeMatcher = new CodeMatcher(instructions)
//         .MatchEndForward(
//           new CodeMatch(OpCodes.Ldloc_S),
//           new CodeMatch(OpCodes.Callvirt, AccessTools.Method(typeof(RenderTexture), nameof(RenderTexture.Release))))
//         .ThrowIfNotMatch("Could not find CanvasDecalRenderer.Render")
//         .SetInstruction(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(UnityEngine.RenderTexture), nameof(UnityEngine.RenderTexture.ReleaseTemporary), new[] { typeof(UnityEngine.RenderTexture) })));
//       return codeMatcher.InstructionEnumeration();
//     } catch (Exception e)
//     {
//       Loader.Log("CanvasDecalRenderer.Render not found");
//       return instructions;
//     }
//   }
// }





