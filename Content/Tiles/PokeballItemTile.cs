using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Pokemod.Content.Items;
using Terraria;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.WorldBuilding;

namespace Pokemod.Content.Tiles
{
    public class PokeballItemTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileShine[Type] = 1100;
			Main.tileSolid[Type] = true;
			Main.tileSolidTop[Type] = true;
			Main.tileFrameImportant[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.addTile(Type);

			AddMapEntry(new Color(255, 100, 100), CreateMapEntryName()); // localized text for "Metal Bar"
        }

        public override IEnumerable<Item> GetItemDrops(int i, int j) {
            int[] items = {
                ModContent.ItemType<RareCandyItem>(),
                ModContent.ItemType<FireStoneItem>(),
                ModContent.ItemType<WaterStoneItem>(),
                ModContent.ItemType<ThunderStoneItem>(),
            };

			yield return new Item(items[Main.rand.Next(items.Length)], 1);
		}
    }

    internal class PokeballItemTileItem : ModItem
    {
        public override string Texture => "Pokemod/Content/Tiles/PokeballItemTile";
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 999;
            Item.consumable = true;
            Item.value = Item.buyPrice(silver: 50);

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useTurn = true;
            Item.autoReuse = true;

            Item.createTile = ModContent.TileType<PokeballItemTile>();
        }
    }

    public class PokeballItemTileSystem : ModSystem
	{
		public static LocalizedText PokeballItemTilePassMessage { get; private set; }
		public static LocalizedText BlessedWithPokeballItemTileMessage { get; private set; }

		public override void SetStaticDefaults() {
			PokeballItemTilePassMessage = Mod.GetLocalization($"WorldGen.{nameof(PokeballItemTilePassMessage)}");
		}

		// World generation is explained more in https://github.com/tModLoader/tModLoader/wiki/World-Generation
		public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight) {
			int Index = tasks.FindIndex(genpass => genpass.Name.Equals("Surface Chests"));

			if (Index != -1) {
				// PokeballItemTilePass is a class seen bellow
				tasks.Insert(Index + 1, new PokeballItemTilePass("Pokemon Hidden Items", 237.4298f));
			}
		}
	}

	public class PokeballItemTilePass : GenPass
	{
		public PokeballItemTilePass(string name, float loadWeight) : base(name, loadWeight) {
		}

		protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration) {
			// progress.Message is the message shown to the user while the following code is running.
			// Try to make your message clear. You can be a little bit clever, but make sure it is descriptive enough for troubleshooting purposes.
			progress.Message = PokeballItemTileSystem.PokeballItemTilePassMessage.Value;

            for (int i = 0; i < 30; i++) {
                bool success = false;
                int attempts = 0;
                while (!success) {
                    attempts++;
                    if (attempts > 1000) {
                        break;
                    }
                    int x = WorldGen.genRand.Next(0, Main.maxTilesX);
                    int y = WorldGen.genRand.Next(0, Main.maxTilesY);

                    if (TileObject.CanPlace(x, y, ModContent.TileType<PokeballItemTile>(), 0, 1, out var objectData)) {
                        WorldGen.PlaceTile(x, y, ModContent.TileType<PokeballItemTile>(), mute: true);
					    success = Main.tile[x, y].TileType == ModContent.TileType<PokeballItemTile>();
                        if(success) Console.WriteLine($"Item generated at {x},{y}");
                    }
                }
            }
		}
	}
}