using Microsoft.Build.Tasks;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow.ValueContentAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemod.Content.Items.Apricorns;
using Pokemod.Content.Items.Consumables.Mints;
using Pokemod.Content.NPCs;
using Pokemod.Content.TileEntities;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.Drawing;
using Terraria.GameContent.Metadata;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Pokemod.Content.Tiles
{
    public class MintPlant : ModTile
    {
        private Asset<Texture2D> MintTexture;

        // An enum for the 4 stages of herb growth
        public enum PlantStage : byte
        {
            Planted,
            Growing,
            GrowingMore,
            Grown
        }


        private const int FrameWidth = 18; // A constant for readability and to kick out those magic numbers
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileObsidianKill[Type] = true;
            Main.tileNoFail[Type] = true;
            TileID.Sets.ReplaceTileBreakUp[Type] = true;
            TileID.Sets.IgnoredInHouseScore[Type] = true;
            //TileID.Sets.MultiTileSway[Type] = true;
            TileMaterials.SetForTileId(Type, TileMaterials._materialsByName["Plant"]);

            LocalizedText name = CreateMapEntryName();
            AddMapEntry(new Color(128, 128, 128), name);

            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2);
            TileObjectData.newTile.AnchorValidTiles = new int[] {
                TileID.Grass
            };
            TileObjectData.newTile.AnchorAlternateTiles = new int[] {
                TileID.PlanterBox
            };
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<MintTileEntity>().Hook_AfterPlacement, -1, 0, true);

            // This is required so the hook is actually called.
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.addTile(Type);

            HitSound = SoundID.Grass;
            DustType = DustID.Ambient_DarkBrown;
            MintTexture = ModContent.Request<Texture2D>(Texture + "_Fruit");
        }

        /*
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];

            if (TileObjectData.IsTopLeft(tile))
            {
                // Makes this tile sway in the wind and with player interaction when used with TileID.Sets.MultiTileSway
                Main.instance.TilesRenderer.AddSpecialPoint(i, j, TileDrawing.TileCounterType.);
            }
            return false;
        }*/

        /*
        public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
        {
            var tile = Main.tile[i, j];

            if (!TileDrawing.IsVisible(tile))
            {
                return;
            }

            int topY = j - tile.TileFrameY % 38 / 18;

            PlantStage stage = GetStage(i, j);

            if (j <= topY)
            {
                if (TileUtils.TryGetTileEntityAs(i, j, out MintTileEntity entity))
                {
                    if (stage == PlantStage.Grown && entity.MintType >= 0)
                    {
                        Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
                        if (Main.drawToScreen)
                        {
                            zero = Vector2.Zero;
                        }
                        int mintVariety = (int)entity.MintType / 10;
                        if (mintVariety == entity.MintType % 10) mintVariety = 5;
                        Rectangle drawRectangle = new Rectangle(0, 18 * mintVariety, 16, 16);

                        spriteBatch.Draw(MintTexture.Value, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, drawRectangle, Lighting.GetColor(i, j), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                    }
                }
            }
        }*/

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch) {
            var tile = Main.tile[i, j];

            if (!TileDrawing.IsVisible(tile))
            {
                return;
            }

            int topY = j - tile.TileFrameY % 38 / 18;

            PlantStage stage = GetStage(i, j);

            if (j <= topY)
            {
                if (TileUtils.TryGetTileEntityAs(i, j, out MintTileEntity entity))
                {
                    if (stage == PlantStage.Grown && entity.MintType >= 0)
                    {
                        Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
                        if (Main.drawToScreen)
                        {
                            zero = Vector2.Zero;
                        }
                        int mintVariety = (int)entity.MintType / 10;
                        if (mintVariety == entity.MintType % 10) mintVariety = 5;
                        Rectangle drawRectangle = new Rectangle(0, 18 * mintVariety, 16, 16);

                        spriteBatch.Draw(MintTexture.Value, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, drawRectangle, Lighting.GetColor(i, j), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                    }
                }
            }
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            ModContent.GetInstance<MintTileEntity>().Kill(i, j);
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

            int herbSeedItem = ModContent.ItemType<MintSeed>();
            int herbSeedStack = 1;

            int herbItemType = chooseSpawnItem(i, j);
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
                herbSeedStack = 2;
                herbItemStack = Main.rand.Next(1, 3);
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
            if (TileUtils.TryGetTileEntityAs(i, j, out MintTileEntity entity))
            {
                if (entity.MintType > -1) {
                    int primary = entity.MintType / 10;
                    int secondary = entity.MintType % 10;
                    string mintName = PokemonData.PokemonNatures[primary][secondary] + "Mint";
                    if (ModContent.TryFind<ModItem>("Pokemod", mintName, out ModItem value))
                    {
                        itemDrop = value.Type;
                    }
                }
            }
            return itemDrop;
        }

        public override bool IsTileSpelunkable(int i, int j)
        {
            if (TileUtils.TryGetTileEntityAs(i, j, out MintTileEntity entity))
            {
                if (entity.MintType > -1) {
                    return true;
                }
            }

            return false;
        }

        public override void MouseOver(int i, int j)
        {
            if (TileUtils.TryGetTileEntityAs(i, j, out MintTileEntity entity))
            {
                if (entity.MintType > -1) {
                    Player player = Main.LocalPlayer;
                    player.cursorItemIconEnabled = true;
                    player.cursorItemIconID = chooseSpawnItem(i, j);
                }
            }

            base.MouseOver(i, j);
        }

        public override bool RightClick(int i, int j) {
            PlantStage stage = GetStage(i, j);

            if (TileUtils.TryGetTileEntityAs(i, j, out MintTileEntity entity))
            {
                if (stage == PlantStage.Grown && entity.MintType > -1)
                {
                    int x = i - Main.tile[i, j].TileFrameX % 18 / 18;
                    int y = j - Main.tile[i, j].TileFrameY % 38 / 18;

                    const int TileWidth = 1;
                    const int TileHeight = 2;

                    float spawnX = (x + TileWidth * 0.5f) * 16;
                    float spawnY = (y + TileHeight * 0.65f) * 16;

                    var entitySource = new EntitySource_TileUpdate(i, j, context: "MintTree");

                    int number = Item.NewItem(entitySource, (int)spawnX, (int)spawnY - 20, 0, 0, chooseSpawnItem(i, j), 1, false, -1);

                    if (Main.netMode == NetmodeID.MultiplayerClient) {
                        NetMessage.SendData(MessageID.SyncItem, -1, -1, null, number, 1f);
                    }

                    entity.setMint(-1);

                    return true;
                }
            }

            return false;
        }

        public override void RandomUpdate(int i, int j)
        {
            if (!WorldGen.genRand.NextBool(8)) {
                return;
            }
            GrowPlant(i, j);
        }

        public void GrowPlant(int i, int j) {
            Tile tile = Main.tile[i, j];
            PlantStage stage = GetStage(i, j);

            // Only grow to the next stage if there is a next stage. We don't want our tile turning pink!
            if (stage < PlantStage.Grown)
            {
                int topX = i - tile.TileFrameX % 18 / 18;
                int topY = j - tile.TileFrameY % 38 / 18;

                for (int x = topX; x < topX + 1; x++)
                {
                    for (int y = topY; y < topY + 2; y++)
                    {
                        Main.tile[x, y].TileFrameX += FrameWidth;
                    }
                }

                // If in multiplayer, sync the frame change
                if (Main.netMode != NetmodeID.SinglePlayer)
                {
                    NetMessage.SendTileSquare(-1, topX, topY, 1, 2);
                }

            } else {
                if (TileUtils.TryGetTileEntityAs(i, j, out MintTileEntity entity))
                {
                    if (entity.MintType < 0) {
                        int primary = Main.rand.Next(0, 5);
                        int secondary = Main.rand.Next(0, 5);
                        int mintType = primary * 10 + secondary;
                        entity.setMint(mintType);
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


    
