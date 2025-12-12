using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemod.Content.Items.GeneticSamples;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Pokemod.Content.Tiles.FossilBlocks
{
	public class SealedFossilBlock : FossilBlock
	{
		public static int animTimer = 0;
        private static Asset<Texture2D> glowTexture;
        private const int animDuration = 400;

        public override void Load()
        {
            glowTexture = ModContent.Request<Texture2D>("Pokemod/Content/Tiles/FossilBlocks/SealedFossilBlockGlow");
        }

        public override void Unload()
        {
			glowTexture = null;
        }

		public override void SetStaticDefaults()
		{
            base.SetStaticDefaults();

            // Block Function
            MineResist = 2f;
			MinPick = 110;
            Main.tileOreFinderPriority[Type] = 300;

			// Visuals
            DustType = DustID.Stone;
            dustHighlight = DustID.BoneTorch;
			dustChance = 200; //Higher values = slower rate
			dustHighlightChance = 15; //Higher values = slower rate 
            HitSound = SoundID.Item127;
			
			// Item Drops
			fossilChance = 30;
				fossilList.Add(ModContent.ItemType<HauntedDomeItem>());
			fossilBlockChance = 5;
				fossilBlock = ModContent.ItemType<SealedFossilItem>();
                defaultBlock = ItemID.StoneSlab;
            commonItemChance = 8;
				commonItems.Add(ItemID.Amethyst);
				commonItems.Add(ItemID.Amethyst);
				commonItems.Add(ItemID.Diamond);

            LocalizedText name = Language.GetText("Mods.Pokemod.Items.SealedFossilItem.DisplayName");
            AddMapEntry(new Color(85, 85, 85), name);
        }

        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            if (animTimer++ > animDuration)
                animTimer = 0;
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            var tile = Main.tile[i, j];

            if (!TileDrawing.IsVisible(tile))
            {
                return;
            }

            int halfTime = (int)(animDuration / 2f);
            float glow = Math.Abs(animTimer - halfTime) / (float)halfTime;

            Color color = Lighting.GetColor(i, j);
            color *= glow;
            

            Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);

            int width = 16;
            int offsetY = 0;
            int height = 16;
            short frameX = tile.TileFrameX;
            short frameY = tile.TileFrameY;
            int addFrX = 0;
            int addFrY = 0;

            TileLoader.SetDrawPositions(i, j, ref width, ref offsetY, ref height, ref frameX, ref frameY); // calculates the draw offsets
            TileLoader.SetAnimationFrame(Type, i, j, ref addFrX, ref addFrY); // calculates the animation offsets

            Rectangle drawRectangle = new Rectangle(tile.TileFrameX, tile.TileFrameY + addFrY, 16, 16);

            // The glow is manually drawn separate from the tile texture so that it can be drawn at full brightness.
            spriteBatch.Draw(glowTexture.Value, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y + offsetY) + zero, drawRectangle, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }

	//Creates 7 clumps of Fossil Blocks in place of dirt/stone/sand each morning. Caps out when there are more than 10000 blocks in the world.
	public class SealedFossilSystem : FossilBlockSystem
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();

			fossilBlockType = (ushort)ModContent.TileType<SealedFossilBlock>();
			numPerDay = 2;
			maxTiles = 500;
            priority = 3;
            replaceTiles = [TileID.GreenDungeonBrick, TileID.BlueDungeonBrick, TileID.PinkDungeonBrick];
        }
        public override void UpdateWorldData()
        {
            base.UpdateWorldData();
            positionCenterX = Main.dungeonX;
            widthX = (int)(((Main.maxTilesX / 2) - Math.Abs(Main.dungeonX - Main.maxTilesX / 2)) * 0.9f);
            canSpawn = Main.hardMode;
        }
	}

    public class SealedFossilItem : FossilBlockItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<SealedFossilBlock>());
		}
	}
}