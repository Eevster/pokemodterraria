using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemod.Content.Items.MegaStones;
using Pokemod.Content.Tiles.FossilBlocks;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Threading;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Pokemod.Content.TileEntities;

namespace Pokemod.Content.Tiles
{
	public class MegaShardTile : ModTile
	{
        private Asset<Texture2D> DynamicTexture;
        private Asset<Texture2D> StaticTexture;


        public override void SetStaticDefaults()
		{
            
            TileID.Sets.Ore[Type] = true;
            TileID.Sets.FriendlyFairyCanLureTo[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileShine2[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileSpelunker[Type] = true;
            Main.tileOreFinderPriority[Type] = 450;

            TileUtils.MergeWithCommonBlocks(Type);

            DustType = DustID.MagicMirror;
            HitSound = SoundID.Tink;
            MineResist = 3f;
            MinPick = 105;

            DynamicTexture = ModContent.Request<Texture2D>(Texture);
            StaticTexture = ModContent.Request<Texture2D>(Texture + "Static");

            LocalizedText name = Language.GetText("Mods.Pokemod.Items.MegaShardItem.DisplayName");
            AddMapEntry(new Color(0, 119, 153), name);
        }

        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            return base.TileFrame(i, j, ref resetFrame, ref noBreak);
        }

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}

        private static Color GetRainbowColor(int j)
        {
            float h = (j + (float)(Main.timeForVisualEffects / 20f)) % 20 * 18;
            h = MathHelper.Clamp(h, 0f, 360f);
            
            float min = 0.55f;
            float max = 1f;
            float r = min;
            float g = min;
            float b = min;

            r += (Math.Clamp(Math.Abs(360 - 2 * h) / 120f, 1f, 2f) - 1) * (max - min) - (1 - max);
            g += (2 - Math.Clamp(Math.Abs(240 - 2 * h) / 120f, 1f, 2f)) * (max - min) - (1 - max);
            b += (2 - Math.Clamp(Math.Abs(480 - 2 * h) / 120f, 1f, 2f)) * (max - min) - (1 - max);

            return new Color(r,g,b);
        }

		public override void EmitParticles(int i, int j, Tile tile, short tileFrameX, short tileFrameY, Color tileLight, bool visible)
		{
            if (!visible)
            {
                return;
            }
            if (Main.rand.NextBool(200))
            {
                Color tileColor = GetRainbowColor(j);
                int dust = Dust.NewDust(new Vector2(i * 16, j * 16), 16, 16, DustID.RainbowTorch, SpeedY: 0f, Scale: 1f, newColor: tileColor);
                Main.dust[dust].noLight = false;
                Main.dust[dust].noGravity = true;
            }
		}

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];
            if (!Terraria.GameContent.Drawing.TileDrawing.IsVisible(tile))
            {
                return;
            }
            
            Color staticColor = Lighting.GetColor(i, j);
            Color dynamicColor = GetRainbowColor(j);
            float glowFactor = 0.2f;
            Color glowColor = new(
                (byte)(dynamicColor.R * (staticColor.R * (1 - glowFactor) / 255 + glowFactor)), 
                (byte)(dynamicColor.G * (staticColor.G * (1 - glowFactor) / 255 + glowFactor)), 
                (byte)(dynamicColor.B * (staticColor.B * (1 - glowFactor) / 255 + glowFactor))
                );
            
            Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);

            int width = 16;
            int offsetY = 0;
            int height = 16;
            short frameX = tile.TileFrameX;
            short frameY = tile.TileFrameY;

            TileLoader.SetDrawPositions(i, j, ref width, ref offsetY, ref height, ref frameX, ref frameY); // calculates the draw offsets

            Rectangle drawRectangle = new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16);

            spriteBatch.Draw(DynamicTexture.Value, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y + offsetY) + zero, drawRectangle, glowColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(StaticTexture.Value, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y + offsetY) + zero, drawRectangle, staticColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }

        public override IEnumerable<Item> GetItemDrops(int i, int j)
        {
            yield return new Item(ModContent.ItemType<MegaShardItem>(), 1);
        }
    }

	public class MegaShardBlockItem : ModItem
	{
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;

            Item.value = Item.buyPrice(0);
            Item.maxStack = Item.CommonMaxStack;
            Item.ResearchUnlockCount = 50;

            Item.DefaultToPlaceableTile(ModContent.TileType<MegaShardTile>());
        }
        public override void AddRecipes()
        {
            CreateRecipe(1)
                .AddIngredient<MegaShardItem>(1)
                .AddTile(TileID.Furnaces)
                .Register();
        }
    }
}