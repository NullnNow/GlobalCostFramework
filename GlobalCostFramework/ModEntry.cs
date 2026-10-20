using HarmonyLib;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Delegates;
using StardewValley.Menus;
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

            GameStateQuery.Register($"{ModEntry.ModId}_SELLING_TO", SellingToGSQ);
            GameStateQuery.Register($"{ModEntry.ModId}_NPC_HAS_TASTE", NPCHasTasteGSQ);

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
                e.LoadFrom(() => new Dictionary<string, AdjustmentData>(), AssetLoadPriority.Exclusive);
            }
        }

        /// <summary>
        /// Whether the player is currently selling an item to a specific shop.
        /// </summary>
        public static bool SellingToGSQ(string[] query, GameStateQueryContext context)
        {
            if (Game1.activeClickableMenu is ShopMenu menu)
            {
                if (query.Contains(menu.ShopId))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Get whether a specified NPC has a particular gift taste for a provided item.
        /// Has the following arguments:
        /// - NPC (e.g., Pierre)
        /// - Either the qualified or unqualifed ID of an object. Can also be "Input" for
        ///   use in Price Adjustments.
        /// - Taste(s); Can be Love, Like, Neutral, Dislike, Hate. Defaults to Love.
        /// </summary>
        public static bool NPCHasTasteGSQ(string[] query, GameStateQueryContext context)
        {
            if (query.Length >= 3)
            {
                List<string> queryList = [.. query];
                NPC npc = Game1.getCharacterFromName(queryList[1]);
                queryList.RemoveAt(0);
                queryList.RemoveAt(0);
                if (npc != null)
                {
                    Item? item = null;
                    if (queryList[0] == "Input")
                        item = context.InputItem;
                    else
                        item = ItemRegistry.Create(queryList[0]);
                    queryList.RemoveAt(0);

                    var taste = npc.getGiftTasteForThisItem(item);

                    string tasteStr = "Neutral";
                    switch (taste)
                    {
                        case NPC.gift_taste_love:
                            tasteStr = "Love";
                            break;
                        case NPC.gift_taste_like:
                            tasteStr = "Like";
                            break;
                        case NPC.gift_taste_neutral:
                            tasteStr = "Neutral";
                            break;
                        case NPC.gift_taste_dislike:
                            tasteStr = "Dislike";
                            break;
                        case NPC.gift_taste_hate:
                            tasteStr = "Hate";
                            break;
                    }

                    if (queryList.Count > 0)
                        return queryList.Contains(tasteStr);
                    else
                        return tasteStr == "Love";
                }
            }

            return false;
        }

        private static void Object_getPriceAfterMultipliers_postfix(SObject __instance, float startPrice, ref float __result, long specificPlayerID = -1L)
        {
            foreach (Farmer player in Game1.getAllFarmers())
            {
                if (Game1.player.useSeparateWallets)
                {
                    if (specificPlayerID == -1)
                    {
                        if (player.UniqueMultiplayerID != Game1.player.UniqueMultiplayerID || !player.isActive())
                        {
                            continue;
                        }
                    }
                    else if (player.UniqueMultiplayerID != specificPlayerID)
                    {
                        continue;
                    }
                }
                else if (!player.isActive())
                {
                    continue;
                }
            }

            var data = Game1.content.Load<Dictionary<string, AdjustmentData>>("Nullnnow.GCF/AdjustmentData");

            foreach (var entry in data)
            {
                AdjustmentData val = entry.Value;

                if (GameStateQuery.CheckConditions(val.Target, Game1.currentLocation, Game1.player, __instance, __instance))
                {
                    __result = Utility.ApplyQuantityModifiers(__result, val.Modifiers, val.ModifierMode, Game1.currentLocation, Game1.player, __instance, __instance);

                }


            }

        }

    }
}
