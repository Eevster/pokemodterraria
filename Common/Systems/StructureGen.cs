using Pokemod.Content.Tiles;
using System.Collections.Generic;
using Terraria;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.WorldBuilding;
namespace Pokemod.Common.Systems
{
    // This file is a very roughly put together modified version of Rubble World Gen https://github.com/tModLoader/tModLoader/blob/stable/ExampleMod/Common/Systems/RubbleWorldGen.cs
    // 90% of this stuff could probably be removed, but, it works, its 3:00 am, and I'm tired :(.
    public class AbandonedPokecenterJungleWorldGen : ModSystem
    {
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            // Add a GenPass immediately after the "Piles" pass. ExampleOreSystem explains this approach in more detail.
            int PilesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Piles"));

            if (PilesIndex != -1)
            {
                tasks.Insert(PilesIndex + 1, new AbandonedPokecenterJunglePass("Example Mod Piles", 100f));
            }
        }
    }

    public class AbandonedPokecenterJunglePass : GenPass
    {
        public AbandonedPokecenterJunglePass(string name, float loadWeight) : base(name, loadWeight)
        {
        }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Example Mod Piles";

            int[] tileTypes = [TileID.WoodBlock, TileID.WoodBlock, TileID.WoodBlock];

            // To not be annoying, we'll only spawn 15 Example Rubble near the spawn point.
            // This example uses the Try Until Success approach: https://github.com/tModLoader/tModLoader/wiki/World-Generation#try-until-success
            for (int k = 0; k < 1; k++) // k < 1 = 1 time spawned
            {
                bool success = false;
                int attempts = 0;

                while (!success)
                {
                    attempts++;
                    if (attempts > 1000)
                    {
                        break;
                    }
                    int x = WorldGen.genRand.Next(Main.maxTilesX / Main.maxTilesX, Main.maxTilesX);
                    int y = (int)GenVars.worldSurfaceLow - 100;
                    int tileType = WorldGen.genRand.Next(tileTypes);
                    int placeStyle = WorldGen.genRand.Next(6);
                    if (Main.tile[x, y].TileType == tileType)
                    {
                        continue;
                    }
                    while ((WorldGen.SolidTile(x, y) || !WorldGen.SolidTile(x, y + 1)) && y < Main.maxTilesY - 100)
                    {
                        y++;
                    }

                    if (Main.tile[x, y + 1].TileType != TileID.JungleGrass) {
                        continue;
                    }

                    int heightOfStructure = 20; //The Height of the Structure in Tiles. The Structure point of refrence is the top left.
                    int finalY = y - heightOfStructure + 2; //its offset by 2 upwards for whatever reason, idk why.

                    WorldGen.PlaceTile(x, finalY, tileType, mute: true, style: placeStyle);
                    //StructureHelper.API.Generator.GenerateStructure("Content/Structures/AbandonedPokecenterJungle", new Point16(x, finalY), ModContent.GetInstance<Pokemod>());
                    success = true;

                }
            }
        }
    }
}