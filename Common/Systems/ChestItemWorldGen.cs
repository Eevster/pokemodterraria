using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Pokemod.Content.Items.Accessories;
using Pokemod.Content.Items.Mounts;
using Pokemod.Content.Items.Consumables.TMs;

namespace Pokemod.Common.Systems
{
    public class ChestItemWorldGen : ModSystem
    {
        public override void PostWorldGen()
        {
            int[] itemsToPlaceInSurfaceChests = {ModContent.ItemType<BikeVoucher>()};
            int itemsToPlaceInSurfaceChestsChoice = 0;
            int itemsPlacedSurface = 0;
            int maxItemsSurface = 2;

            int[] itemsToPlaceInGoldChests = {
                ModContent.ItemType<TMElectric>(), ModContent.ItemType<TMFighting>(), ModContent.ItemType<TMFire>(),
                ModContent.ItemType<TMFlying>(), ModContent.ItemType<TMGhost>(), ModContent.ItemType<TMGrass>(),
                ModContent.ItemType<TMGround>(), ModContent.ItemType<TMIce>(), ModContent.ItemType<TMNormal>(),
                ModContent.ItemType<TMPoison>(), ModContent.ItemType<TMPsychic>(), ModContent.ItemType<TMSteel>(),
                ModContent.ItemType<TMWater>()
            };
            int itemsToPlaceInGoldChestsChoice = 0;
            int itemsPlacedGold = 0;
            int maxItemsGold = 24;

            int[] itemsToPlaceInJungleChests = {ModContent.ItemType<Leftovers>()};
            int itemsToPlaceInJungleChestsChoice = 0;
            int itemsPlacedJungle = 0;
            int maxItemsJungle = 3;

            for (int chestIndex = 0; chestIndex < Main.maxChests; chestIndex++)
            {
                Chest chest = Main.chest[chestIndex];
                if (chest == null)
                {
                    continue;
                }
                Tile chestTile = Main.tile[chest.x, chest.y];

                // Surface Chest
                if (chestTile.TileType == TileID.Containers && chestTile.TileFrameX == 0 * 36 && itemsPlacedSurface < maxItemsSurface)
                {
                    if (WorldGen.genRand.NextBool(3))
                        continue;

                    for (int inventoryIndex = 0; inventoryIndex < Chest.maxItems; inventoryIndex++)
                    {
                        if (chest.item[inventoryIndex].type == ItemID.None)
                        {
                            chest.item[inventoryIndex].SetDefaults(itemsToPlaceInSurfaceChests[itemsToPlaceInSurfaceChestsChoice]);
                            itemsToPlaceInSurfaceChestsChoice = (itemsToPlaceInSurfaceChestsChoice + 1) % itemsToPlaceInSurfaceChests.Length;
                            itemsPlacedSurface++;
                            break;
                        }
                    }
                }

                // Gold Chest
                if (chestTile.TileType == TileID.Containers && chestTile.TileFrameX == 1 * 36 && itemsPlacedGold < maxItemsGold)
                {
                    if (WorldGen.genRand.NextBool(3))
                        continue;

                    for (int inventoryIndex = 0; inventoryIndex < Chest.maxItems; inventoryIndex++)
                    {
                        if (chest.item[inventoryIndex].type == ItemID.None)
                        {
                            chest.item[inventoryIndex].SetDefaults(itemsToPlaceInGoldChests[itemsToPlaceInGoldChestsChoice]);
                            itemsToPlaceInGoldChestsChoice = (itemsToPlaceInGoldChestsChoice + 1) % itemsToPlaceInGoldChests.Length;
                            itemsPlacedGold++;
                            break;
                        }
                    }
                }

                // Jungle Chest
                if (chestTile.TileType == TileID.Containers && chestTile.TileFrameX == 10 * 36 && itemsPlacedJungle < maxItemsJungle)
                {
                    if (WorldGen.genRand.NextBool(3))
                        continue;

                    for (int inventoryIndex = 0; inventoryIndex < Chest.maxItems; inventoryIndex++)
                    {
                        if (chest.item[inventoryIndex].type == ItemID.None)
                        {
                            chest.item[inventoryIndex].SetDefaults(itemsToPlaceInJungleChests[itemsToPlaceInJungleChestsChoice]);
                            itemsToPlaceInJungleChestsChoice = (itemsToPlaceInJungleChestsChoice + 1) % itemsToPlaceInJungleChests.Length;
                            itemsPlacedJungle++;
                            break;
                        }
                    }
                }

                if (itemsPlacedSurface >= maxItemsSurface && itemsPlacedGold >= maxItemsGold && itemsPlacedJungle >= maxItemsJungle)
                {
                    break;
                }
            }
        }
    }
}
