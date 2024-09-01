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
using Pokemod.Content.TileEntities;

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
            Main.tileNoFail[Type] = true;
            TileID.Sets.ReplaceTileBreakUp[Type] = true;
            TileID.Sets.IgnoredInHouseScore[Type] = true;
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
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<ApricornTileEntity>().Hook_AfterPlacement, -1, 0, true);

            // This is required so the hook is actually called.
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.addTile(Type);

            HitSound = SoundID.Grass;
            DustType = DustID.Ambient_DarkBrown;
            ApricornTexture = ModContent.Request<Texture2D>(Texture + "_Fruit");
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch) {
			var tile = Main.tile[i, j];

			if (!TileDrawing.IsVisible(tile)) {
				return;
			}

            int topX = i - tile.TileFrameX % 36 / 18;
            int topY = j - tile.TileFrameY % 56 / 18;

            PlantStage stage = GetStage(i, j);

            if(j <= topY){
                if (TileUtils.TryGetTileEntityAs(i, j, out ApricornTileEntity entity))
                {
                    if (stage == PlantStage.Grown && entity.apricornType >= 0) {
                        Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
                        if (Main.drawToScreen) {
                            zero = Vector2.Zero;
                        }

                        Rectangle drawRectangle = new Rectangle(18*(i-topX), 18*entity.apricornType, 16, 16);

                        spriteBatch.Draw(ApricornTexture.Value, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y - 4) + zero, drawRectangle, Lighting.GetColor(i, j), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                    }
                }
            }
		}

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            ModContent.GetInstance<ApricornTileEntity>().Kill(i, j);
            base.KillMultiTile(i, j, frameX, frameY);
        }

        public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY) {
			offsetY = 2; // This is -1 for tiles using StyleAlch, but vanilla sets to -2 for herbs, which causes a slight visual offset between the placement preview and the placed tile. 
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

            int herbSeedItem = ModContent.ItemType<ApricornSeed>();
            int herbSeedStack = 1;

            int herbItemType = chooseSpawnItem(i,j);
            int herbItemStack = 1;

            if (nearestPlayer.active && (nearestPlayer.HeldItem.type == ItemID.StaffofRegrowth || nearestPlayer.HeldItem.type == ItemID.AcornAxe))
            {
                // Increased yields with Staff of Regrowth, even when not fully grown
                 
                herbItemStack = Main.rand.Next(1, 6);
                herbSeedStack = Main.rand.Next(1, 30);
            }
            else if (stage < PlantStage.Grown)
            {
                herbItemStack = 0;
                herbSeedStack = 1;
            }
            else if (stage == PlantStage.Grown)
            {
                // Default yields, only when fully grown
                herbSeedStack = 1;
                herbItemStack = Main.rand.Next(1, 4);
            }

            if (herbItemType > 0 && herbItemStack > 0)
            {
                
                yield return new Item(herbItemType, herbItemStack);
            }

            if (herbSeedItem > 0 && herbSeedStack > 0)
            {
                yield return new Item(herbSeedItem, herbSeedStack);
            }
        }
        public int chooseSpawnItem(int i, int j)
        {
            int itemDrop = 0;
            if (TileUtils.TryGetTileEntityAs(i, j, out ApricornTileEntity entity))
            {
                int type = entity.apricornType;
                if (type == 0) itemDrop = ModContent.ItemType<BlackApricorn>();
                if (type == 1) itemDrop = ModContent.ItemType<BlueApricorn>();
                if (type == 2) itemDrop = ModContent.ItemType<GreenApricorn>();
                if (type == 3) itemDrop = ModContent.ItemType<PinkApricorn>();
                if (type == 4) itemDrop = ModContent.ItemType<RedApricorn>(); 
                if (type == 5) itemDrop = ModContent.ItemType<WhiteApricorn>(); 
                if (type == 6) itemDrop = ModContent.ItemType<YellowApricorn>();
            }

            return itemDrop;
        }

        public override bool IsTileSpelunkable(int i, int j)
        {
            if (TileUtils.TryGetTileEntityAs(i, j, out ApricornTileEntity entity))
            {
                if(entity.apricornType != -1){
                    return true;
                }
            }

            return false;
        }

        public override bool RightClick(int i, int j) {
            PlantStage stage = GetStage(i, j);

            if (TileUtils.TryGetTileEntityAs(i, j, out ApricornTileEntity entity))
            {
                if (stage == PlantStage.Grown && entity.apricornType >= 0)
                {
                    int x = i - Main.tile[i, j].TileFrameX % 36 / 18;
                    int y = j - Main.tile[i, j].TileFrameY % 56 / 18;

                    const int TileWidth = 2;
                    const int TileHeight = 3;

                    float spawnX = (x + TileWidth * 0.5f) * 16;
                    float spawnY = (y + TileHeight * 0.65f) * 16;

                    var entitySource = new EntitySource_TileUpdate(i, j, context: "ApricornTree");
                    
                    int number = Item.NewItem(entitySource, (int)spawnX, (int)spawnY - 20, 0, 0, chooseSpawnItem(i,j), 1, false, -1);

                    if (Main.netMode == NetmodeID.MultiplayerClient){
			            NetMessage.SendData(21, -1, -1, null, number, 1f);
                    }
                    
                    entity.setApricorn(-1);

                    return true;
                }
            }

			return false;
		}

        public override void RandomUpdate(int i, int j)
        {
            if (!WorldGen.genRand.NextBool(20)) {
				return;
			}

            GrowPlant(i, j);
        }

        public void GrowPlant(int i, int j){
            Tile tile = Main.tile[i, j];
            PlantStage stage = GetStage(i, j);

            // Only grow to the next stage if there is a next stage. We don't want our tile turning pink!
            if (stage < PlantStage.Grown)
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
                    NetMessage.SendTileSquare(-1, topX, topY, 2, 3);
                }

            }else{
                if (TileUtils.TryGetTileEntityAs(i, j, out ApricornTileEntity entity))
                {
                    if(entity.apricornType < 0){
                        entity.setApricorn(Main.rand.Next(0, 7));
                    }
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