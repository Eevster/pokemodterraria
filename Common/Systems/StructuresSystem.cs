using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Pokemod.Content.Items;
using Pokemod.Content.Items.Consumables;
using Pokemod.Content.Items.EvoStones;
using Pokemod.Content.Items.Pokeballs;
using Pokemod.Content.Items.Accessories;
using Terraria;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.WorldBuilding;

namespace Pokemod.Common.Systems
{
    public class PokemonCenterSystem : ModSystem
	{
		public static LocalizedText PokemonCenterPassMessage { get; private set; }
		public static LocalizedText BlessedWithPokemonCenterMessage { get; private set; }

		public override void SetStaticDefaults() {
			PokemonCenterPassMessage = Mod.GetLocalization($"WorldGen.{nameof(PokemonCenterPassMessage)}");
		}

		// World generation is explained more in https://github.com/tModLoader/tModLoader/wiki/World-Generation
		public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight) {
			int Index = tasks.FindIndex(genpass => genpass.Name.Equals("Surface Chests"));

			if (Index != -1) {
				// PokemonCenterPass is a class seen bellow
				tasks.Insert(Index + 1, new PokemonCenterPass("Pokemon Center", 237.4298f));
			}
		}
	}

	public class PokemonCenterPass : GenPass
	{
        //0=air, 1=red, 2=white, 3=blue, 4=black, 5=platform
        static readonly byte[,] PokemonCenterTiles =
        {
            {0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,1,1,1,2,2,2,1,1,1,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,1,1,2,2,1,2,2,1,1,0,0,0,0,0,0,0,0},
            {0,0,0,1,1,1,1,1,1,1,1,1,2,1,1,1,1,1,1,1,1,1,0,0,0},
            {0,0,1,1,1,1,1,1,1,1,2,2,1,2,2,1,1,1,1,1,1,1,1,0,0},
            {0,0,1,1,1,1,1,1,1,1,1,2,2,2,1,1,1,1,1,1,1,1,1,0,0},
            {0,0,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,0,0},
            {0,1,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,1,0},
            {1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1},
            {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
            {2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
            {1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1},
            {0,3,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,3,0},
            {0,3,0,0,0,0,0,0,0,0,0,0,0,0,5,0,0,0,0,0,0,0,0,3,0},
            {0,3,0,0,0,0,0,0,0,0,0,0,0,5,0,0,0,0,0,0,0,0,0,3,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,5,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,5,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0}
        };

        //0=none, 1=bottom-left, 2=bottom-right, 3=top-left, 4=top-right, 5=half
        static readonly byte[,] PokemonCenterSlopes =
        {
            {0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0},
            {0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0},
            {2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}
        };

        //0=air, 1=red, 2=white, 3=blue, 4=black
        static readonly byte[,] PokemonCenterWalls =
        {
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,3,3,3,4,3,3,3,4,3,3,3,4,3,3,3,4,3,3,3,0,0,0},
            {0,0,0,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,0,0,0},
            {0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0},
            {0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
            {0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,0},
            {0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0},
            {0,0,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,0,0},
            {0,0,3,4,3,3,3,3,3,4,3,3,3,3,3,4,3,3,3,3,3,4,3,0,0},
            {0,0,3,4,3,3,3,3,3,4,3,3,3,3,3,4,3,3,3,3,3,4,3,0,0},
            {0,0,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,0,0},
            {0,0,2,4,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,4,2,0,0},
            {0,0,2,4,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,4,2,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}
        };

		public PokemonCenterPass(string name, float loadWeight) : base(name, loadWeight)
        {
        }

		protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration) {
			// progress.Message is the message shown to the user while the following code is running.
			// Try to make your message clear. You can be a little bit clever, but make sure it is descriptive enough for troubleshooting purposes.
			progress.Message = PokemonCenterSystem.PokemonCenterPassMessage.Value;

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

                    if (TileObject.CanPlace(x, y, TileID.MarbleBlock, 0, 1, out var objectData)) {
                        WorldGen.PlaceTile(x, y, TileID.MarbleBlock, mute: true);
					    success = Main.tile[x, y].TileType == TileID.MarbleBlock;
                    }
                }
            }
		}
	}
}