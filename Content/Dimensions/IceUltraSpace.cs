using System;
using System.Collections.Generic;
using SubworldLibrary;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Microsoft.Xna.Framework;
using Pokemod.Backgrounds;
using Terraria.GameContent.Generation;
using Pokemod.Content.NPCs;

public class IceUltraSpaceSubworld : Subworld
{
    public override int Width => 1000;
    public override int Height => 1000;

    public override bool ShouldSave => false;
    public override bool NoPlayerSaving => true;

    public override List<GenPass> Tasks => new List<GenPass>()
    {
        new IceUltraSpaceGenPass()
    };

    // Sets the time to the middle of the day whenever the subworld loads
    public override void OnLoad()
    {
        Main.dayTime = true;
        Main.time = 27000;
        Main.raining = true;
        Main.rainTime = 10000;
        Main.maxRaining = 1f;
    }
}

public class IceUltraSpaceGenPass : GenPass
{
    //TODO: remove this once tML changes generation passes
    public IceUltraSpaceGenPass() : base("Terrain", 1) { }

    protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
    {
        progress.Message = "Generating terrain"; // Sets the text displayed for this pass
        Main.worldSurface = Main.maxTilesY - 42; // Hides the underground layer just out of bounds
        Main.rockLayer = Main.maxTilesY; // Hides the cavern layer way out of bounds

        int surfaceLevel = (int)(Main.worldSurface / 2); // Set the surface level halfway between worldSurface and 0

        for (int i = 0; i < Main.maxTilesX; i++)
        {
            for (int j = 0; j < Main.maxTilesY; j++)
            {
                progress.Set((j + i * Main.maxTilesY) / (float)(Main.maxTilesX * Main.maxTilesY)); // Controls the progress bar, should only be set between 0f and 1f
                Tile tile = Main.tile[i, j];

                // Add surface layer
                if (j == surfaceLevel)
                {
                    tile.HasTile = true;
                    tile.TileType = TileID.SnowBlock; // You can change this to any other tile type for the surface layer
                }
                else if (j > surfaceLevel) // Below the surface level
                {
                    tile.HasTile = true;
                    tile.TileType = TileID.IceBlock; // Set the underground to dirt, change it as needed
                }
            }
        }

        
    }

}