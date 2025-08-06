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
	public class IgneousFossilBlock : FossilBlock
	{
		
		public override void SetStaticDefaults()
		{
            base.SetStaticDefaults();

            // Block Function
            MineResist = 2f;
			MinPick = 110;
            Main.tileOreFinderPriority[Type] = 300;

			// Visuals
            DustType = DustID.Obsidian;
            dustHighlight = DustID.RedTorch;
			dustChance = 200; //Higher values = slower rate
			dustHighlightChance = 15; //Higher values = slower rate 
            HitSound = SoundID.Item127;
			
			// Item Drops
			fossilChance = 90;
				fossilList.Add(ModContent.ItemType<InfernalAmberItem>());
			fossilBlockChance = 5;
				fossilBlock = ModContent.ItemType<IgneousFossilItem>();
			commonItemChance = 3;
				commonItem = ItemID.Bone;
				defaultBlock = ItemID.Obsidian;

            AddMapEntry(new Color(150, 115, 90));
		}

	}

	//Creates 7 clumps of Fossil Blocks in place of dirt/stone/sand each morning. Caps out when there are more than 10000 blocks in the world.
	public class IgneousFossilSystem : FossilBlockSystem
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();

			fossilBlockType = (ushort)ModContent.TileType<IgneousFossilBlock>();
			numPerDay = 15;
			maxTiles = 5000;
            priority = 1;
        }
        public override void UpdateWorldData()
        {
            base.UpdateWorldData();
            heightTop = Main.maxTilesY - 400;
            heightBottom = Main.maxTilesY;
			canSpawn = Main.hardMode;
        }
    }


    public class IgneousFossilItem : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 24;

			Item.value = Item.buyPrice(0);
			Item.maxStack = Item.CommonMaxStack;

			Item.DefaultToPlaceableTile(ModContent.TileType<IgneousFossilBlock>());
		}
	}
}