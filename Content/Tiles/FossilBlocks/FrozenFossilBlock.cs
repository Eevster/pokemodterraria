using Microsoft.Xna.Framework;
using Pokemod.Content.Items.GeneticSamples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Tiles.FossilBlocks
{
	public class FrozenFossilBlock : FossilBlock
	{
		
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();

			// Block Function
			MineResist = 2f;
			MinPick = 110;
            Main.tileOreFinderPriority[Type] = 300;

			// Visuals
            DustType = DustID.Ice;
            dustHighlight = DustID.BlueTorch;
			dustChance = 200; //Higher values = slower rate
			dustHighlightChance = 15; //Higher values = slower rate 
            HitSound = SoundID.Item127;
			
			// Item Drops
			fossilChance = 45;
				fossilList.Add(ModContent.ItemType<EldritchHelixItem>());
			fossilBlockChance = 5;
				fossilBlock = ModContent.ItemType<FrozenFossilItem>();
			commonItemChance = 3;
				commonItem = ItemID.Bone;
				defaultBlock = ItemID.IceBlock;

            AddMapEntry(new Color(150, 115, 90));
		}

	}

	//Creates 7 clumps of Fossil Blocks in place of dirt/stone/sand each morning. Caps out when there are more than 10000 blocks in the world.
	public class FrozenFossilSystem : FossilBlockSystem
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();

			fossilBlockType = (ushort)ModContent.TileType<FrozenFossilBlock>();
			numPerDay = 8;
			maxTiles = 2500;
			replaceTiles = [TileID.IceBlock, TileID.SnowBlock];
			priority = 2;
        }

        public override void UpdateWorldData()
        {
            base.UpdateWorldData();
            positionCenterX = Main.dungeonX > (Main.maxTilesX / 2)? (3 * Main.maxTilesX / 4): (1 * Main.maxTilesX / 4);
            widthX = (int)((Main.maxTilesX / 4) * 0.9f);
            canSpawn = Main.hardMode;
        }
    }

    public class FrozenFossilItem : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 24;

			Item.value = Item.buyPrice(0);
			Item.maxStack = Item.CommonMaxStack;

			Item.DefaultToPlaceableTile(ModContent.TileType<FrozenFossilBlock>());
		}
	}
}