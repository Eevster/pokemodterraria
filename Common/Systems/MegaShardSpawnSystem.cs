using Microsoft.Xna.Framework;
using Pokemod.Content.Tiles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Common.Systems
{
    internal class MegaShardSpawnSystem : ModSystem
    {
        public override void Load()
        {
            On_WorldGen.dropMeteor += AfterMeteorDrop;
        }

        private void AfterMeteorDrop(On_WorldGen.orig_dropMeteor orig)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                // Since this happens during gameplay, we need to run this code on another thread. If we do not, the game will experience lag for a brief moment. This is especially necessary for world generation tasks that would take even longer to execute.
                ThreadPool.QueueUserWorkItem(_ =>
                {
                    //Pre Count
                    FindAverageWorldPosition(TileID.Meteorite, out Vector2 oldPosition, out int oldCount, minX: 50, maxX: Main.maxTilesX - 50, minY: 50, maxY: Main.maxTilesY - 50);
                    
                    // Call vanilla meteor spawning
                    orig();

                    // AFTER the meteor has been placed
                    FindAverageWorldPosition(TileID.Meteorite, out Vector2 newPosition, out int newCount, minX: 50, maxX: Main.maxTilesX - 50, minY: 50, maxY: Main.maxTilesY - 50);
                    Vector2 crashPosition = FindCrashLocation(oldPosition, newPosition, oldCount, newCount);
                    SpawnMegaShards(crashPosition);
                });
            }
        }

        //Finds the average position of all tiles of the specified type, within the specified range.
        private static void FindAverageWorldPosition(int tileID, out Vector2 position, out int count, int minX = 0, int maxX = -1, int minY = 0, int maxY = -1)
        {
            count = 0;
            int sumX = 0;
            int sumY = 0;

            if (maxX < 0) maxX = Main.maxTilesX;
            if (maxY < 0) maxY = Main.maxTilesY;
            for (int i = minX; i < maxX; i++)
            {
                for (int j = minY; j < maxY; j++)
                {
                    if (Main.tile[i, j].HasTile)
                    {
                        if (Main.tile[i, j].TileType == tileID)
                        {
                            count++;
                            sumX += i;
                            sumY += j;
                        }
                    }
                }
            }
            if (count > 0)
            {
                position = new(sumX / count, sumY / count);
            }
            else
            {
                position = new(0, 0);
            }
        }

        //uses the average tile positions both before and after impact to determine the centre of the new impact.
        private static Vector2 FindCrashLocation(Vector2 oldPosition, Vector2 newPosition, int oldCount, int newCount)
        {
            int oldX = (int)oldPosition.X;
            int oldY = (int)oldPosition.Y;
            int newX = (int)newPosition.X;
            int newY = (int)newPosition.Y;

            int crashX = (newX * newCount - oldX * oldCount) / Math.Clamp((newCount - oldCount), 1, 20000);
            int crashY = (newY * newCount - oldY * oldCount) / Math.Clamp((newCount - oldCount), 1, 20000);

            Vector2 crashPosition = new(crashX, crashY);

            return crashPosition;
        }

        //spawns mega shard tiles in 5 splotches circling around the bottom of the crash site, only replacing low-value tiles such as dirt or stone.
        private static void SpawnMegaShards(Vector2 crashPosition)
        {
            for (int k = 0; k < 5; k++)
            {
                Vector2 centerOffset = new(0, -6); //adjusts center up slightly to account for the usually lower centre of mass of the crater.
                Vector2 directionalOffset = Vector2.UnitX.RotatedBy(k * MathHelper.TwoPi / 8f) * Main.rand.NextFloat(17f, 20f);
                Vector2 finalTarget = crashPosition + centerOffset + directionalOffset;

                WorldGen.OreRunner((int)finalTarget.X, (int)finalTarget.Y, WorldGen.genRand.Next(6, 9), WorldGen.genRand.Next(5, 7), (ushort)ModContent.TileType<MegaShardTile>());
            }
        }
    }
}
