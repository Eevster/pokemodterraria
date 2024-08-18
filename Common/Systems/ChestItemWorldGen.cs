using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Pokemod.Content.Items.Apricorns;

namespace Pokemod.Common.Systems
{
    public class ChestItemWorldGen : ModSystem
    {

        public override void PostWorldGen()
        {
            int[] itemsToPlaceInJungleChests = {ModContent.ItemType<RedApricorn>() };
            int itemsToPlaceInJungleChestsChoice = 0;
            int itemsPlaced = 0;
            int maxItems = 0;

            for (int chestIndex = 0; chestIndex < Main.maxChests; chestIndex++)
            {
                Chest chest = Main.chest[chestIndex];
                if (chest == null)
                {
                    continue;
                }
                Tile chestTile = Main.tile[chest.x, chest.y];

                if (chestTile.TileType == TileID.Containers && chestTile.TileFrameX == 8 * 36)
                {

                    if (WorldGen.genRand.NextBool(3))
                        continue;

                    for (int inventoryIndex = 0; inventoryIndex < Chest.maxItems; inventoryIndex++)
                    {
                        if (chest.item[inventoryIndex].type == ItemID.None)
                        {
                            chest.item[inventoryIndex].SetDefaults(itemsToPlaceInJungleChests[itemsToPlaceInJungleChestsChoice]);
                            itemsToPlaceInJungleChestsChoice = (itemsToPlaceInJungleChestsChoice + 1) % itemsToPlaceInJungleChests.Length;
                            itemsPlaced++;
                            break;
                        }
                    }
                }
                if (itemsPlaced >= maxItems)
                {
                    break;
                }



            }



        }

    }

}
