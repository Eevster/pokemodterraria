using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Pokemod.Content.Items.Apricorns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.Metadata;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Microsoft.Build.Tasks;
using Terraria.GameContent.Drawing;
using ReLogic.Content;

namespace Pokemod.Content.Tiles
{
    public class ApricornPlant : ModTile
    {
        private Asset<Texture2D> ApricornTexture;

        // An enum for the 4 stages of herb growth
        public enum PlantStage : byte
        {
            Planted,
            Growing,
            GrowingMore,
            Grown
        }


        private const int FrameWidth = 36; // A constant for readability and to kick out those magic numbers
        public override void SetStaticDefaults()
        {


            Main.tileFrameImportant[Type] = true;
            Main.tileObsidianKill[Type] = true;
            Main.tileCut[Type] = true;
            Main.tileNoFail[Type] = true;
            TileID.Sets.ReplaceTileBreakUp[Type] = true;
            TileID.Sets.IgnoredInHouseScore[Type] = true;
            TileID.Sets.IgnoredByGrowingSaplings[Type] = true;
            TileMaterials.SetForTileId(Type, TileMaterials._materialsByName["Plant"]);


            LocalizedText name = CreateMapEntryName();
            AddMapEntry(new Color(128, 128, 128), name);

            TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
            TileObjectData.newTile.AnchorValidTiles = new int[] {
                TileID.Grass
            };
            TileObjectData.newTile.AnchorAlternateTiles = new int[] {
                TileID.PlanterBox
            };
            TileObjectData.addTile(Type);

            HitSound = SoundID.Grass;
            DustType = DustID.Ambient_DarkBrown;
            //ApricornTexture = ModContent.Request<Texture2D>(Texture + "_Flame");
        }

        public override bool CanPlace(int i, int j)
        {
            Tile tile = Framing.GetTileSafely(i, j); // Safe way of getting a tile instance

            if (tile.HasTile)
            {
                int tileType = tile.TileType;
                if (tileType == Type)
                {
                    PlantStage stage = GetStage(i, j); // The current stage of the herb

                    // Can only place on the same herb again if it's grown already
                    return stage == PlantStage.Grown;
                }
                else
                {
                    // Support for vanilla herbs/grasses:
                    if (Main.tileCut[tileType] || TileID.Sets.BreakableWhenPlacing[tileType] || tileType == TileID.WaterDrip || tileType == TileID.LavaDrip || tileType == TileID.HoneyDrip || tileType == TileID.SandDrip)
                    {
                        bool foliageGrass = tileType == TileID.Plants || tileType == TileID.Plants2;
                        bool moddedFoliage = tileType >= TileID.Count && (Main.tileCut[tileType] || TileID.Sets.BreakableWhenPlacing[tileType]);
                        bool harvestableVanillaHerb = Main.tileAlch[tileType] && WorldGen.IsHarvestableHerbWithSeed(tileType, tile.TileFrameX);

                        if (foliageGrass || moddedFoliage || harvestableVanillaHerb)
                        {
                            WorldGen.KillTile(i, j);
                            if (!tile.HasTile && Main.netMode == NetmodeID.MultiplayerClient)
                            {
                                NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, i, j);
                            }

                            return true;
                        }
                    }

                    return false;
                }
            }

            return true;
        }

        public override bool CanDrop(int i, int j)
        {
            PlantStage stage = GetStage(i, j);

            if (stage == PlantStage.Planted)
            {
                // Do not drop anything when just planted
                return false;
            }
            return true;
        }

        public override IEnumerable<Item> GetItemDrops(int i, int j)
        {
            PlantStage stage = GetStage(i, j);

            Vector2 worldPosition = new Vector2(i, j).ToWorldCoordinates();
            Player nearestPlayer = Main.player[Player.FindClosest(worldPosition, 16, 16)];

            int herbItemType = ModContent.ItemType<RedApricorn>();
            int herbItemStack = 1;

            if (nearestPlayer.active && (nearestPlayer.HeldItem.type == ItemID.StaffofRegrowth || nearestPlayer.HeldItem.type == ItemID.AcornAxe))
            {
                // Increased yields with Staff of Regrowth, even when not fully grown
                herbItemStack = Main.rand.Next(1, 4);
            }
            else if (stage < PlantStage.Grown)
            {
                herbItemStack = 1;
            }
            else if (stage == PlantStage.Grown)
            {
                // Default yields, only when fully grown
                herbItemStack = 4;
            }

            if (herbItemType > 0 && herbItemStack > 0)
            {
                yield return new Item(herbItemType, herbItemStack);
            }

        }

        public override bool IsTileSpelunkable(int i, int j)
        {
            PlantStage stage = GetStage(i, j);

            // Only glow if the herb is grown
            return stage == PlantStage.Grown;
        }

        public override void RandomUpdate(int i, int j)
        {
            Tile tile = Main.tile[i, j];
            PlantStage stage = GetStage(i, j);


            // Only grow to the next stage if there is a next stage. We don't want our tile turning pink!
            if (stage != PlantStage.Grown)
            {
                int topX = i - tile.TileFrameX % 36 / 18;
                int topY = j - tile.TileFrameY % 56 / 18;

                for (int x = topX; x < topX + 2; x++)
                {
                    for (int y = topY; y < topY + 3; y++)
                    {
                        Main.tile[x, y].TileFrameX += FrameWidth;
                    }
                }

                // If in multiplayer, sync the frame change
                if (Main.netMode != NetmodeID.SinglePlayer)
                {
                    NetMessage.SendTileSquare(-1, i, topY, 2, 3, TileChangeType.None);
                }

            }
        }

       
        
        // A helper method to quickly get the current stage of the herb (assuming the tile at the coordinates is our herb)
        private static PlantStage GetStage(int i, int j)
        {
            Tile tile = Framing.GetTileSafely(i, j);
            return (PlantStage)(tile.TileFrameX / FrameWidth);
        }


    }
}