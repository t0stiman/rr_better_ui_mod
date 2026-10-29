using Game;
using HarmonyLib;
using Network.Client;
using Steamworks;
using UI.Builder;
using UI.CompanyWindow;
using UI.Map;
using UnityEngine;

namespace better_ui_mod.Patches;

//TODO update for RR 0.6.1

// [HarmonyPatch(typeof(EmployeesPanelBuilder), nameof(EmployeesPanelBuilder.BuildPlayerActions))]
// public class EmployeesPanelBuilder_BuildPlayerActions
// {
//   private static bool Prefix(
//     UIPanelBuilder builder,
//     IPlayer player)
//   {
//     if (!ModEntryPoint.MySettings!.ShowSteamProfileButton)
//     {
//       return Stuff.EXECUTE_ORIGINAL;
//     }
//     
//     builder.AddSection("Actions", builder3 => builder3.ButtonStrip(builder2 =>
//     {
//       builder2.AddButton("Show", () => CameraSelector.shared.JumpToPoint(player.GamePosition, Quaternion.identity, CameraSelector.CameraIdentifier.Strategy));
//       builder2.AddButton("Show on Map", () => MapWindow.Show(player.GamePosition));
//
//       if (player is RemotePlayer)
//       {
//         var steamID = ((RemotePlayer)player).playerId;
//         
//         builder2.AddButton("Steam Profile", () =>
//         {
//           SteamFriends.ActivateGameOverlayToWebPage(
//             $"https://steamcommunity.com/profiles/{steamID}");
//         });
//       }
//     }));
//
//     return Stuff.SKIP_ORIGINAL;
//   }
// }


  