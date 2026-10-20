using HarmonyLib;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using xTile.Dimensions;
using xTile.Tiles;
using SObject = StardewValley.Object;

namespace GlobalCostFramework
{
    public sealed class ModEntry : Mod
    {
        public const string ModId = "Nullnnow.GCF";

        public static IMonitor StaticMonitor { get; internal set; } = null!;
        public static IModHelper StaticHelper { get; internal set; } = null!;

        public override void Entry(IModHelper helper)
        {
            StaticMonitor = Monitor;
            StaticHelper = Helper;

            Helper.Events.Content.AssetRequested += OnAssetRequested;
            var harmony = new Harmony(ModManifest.UniqueID);
            harmony.Patch(
               original: AccessTools.Method(typeof(SObject),
               "getPriceAfterMultipliers"),
               postfix: new HarmonyMethod(typeof(ModEntry), nameof(Object_getPriceAfterMultipliers_postfix))
               );
        }


        public void OnAssetRequested(object? sender, AssetRequestedEventArgs e)
        {
            if (e.NameWithoutLocale.IsEquivalentTo("Nullnnow.GCF/AdjustmentData"))
            {
                StaticMonitor.Log("----------------", LogLevel.Debug);
                e.LoadFrom(() => new Dictionary<string, AdjustmentData>(), AssetLoadPriority.Exclusive);
            }
        }

        private static void Object_getPriceAfterMultipliers_postfix(SObject __instance, float startPrice, ref float __result, long specificPlayerID = -1L)
        {
            float multiplier = 1f;
            StaticMonitor.Log("Calling get price", LogLevel.Debug);
            //foreach (Farmer player in Game1.getAllFarmers())
            //{
            //    if (Game1.player.useSeparateWallets)
            //    {
            //        if (specificPlayerID == -1)
            //        {
            //            if (player.UniqueMultiplayerID != Game1.player.UniqueMultiplayerID || !player.isActive())
            //            {
            //                continue;
            //            }
            //        }
            //        else if (player.UniqueMultiplayerID != specificPlayerID)
            //        {
            //            continue;
            //        }
            //    }
            //    else if (!player.isActive())
            //    {
            //        continue;
            //    }
            //}

            var data = Game1.content.Load<Dictionary<string, AdjustmentData>>("Nullnnow.GCF/AdjustmentData");
            StaticMonitor.Log($"{data.Count}", LogLevel.Debug);
            foreach (var entry in data)
            {
                StaticMonitor.Log($"Key: {entry.Key} -> {entry.Value}", LogLevel.Debug);
            }

        }

    }
}
