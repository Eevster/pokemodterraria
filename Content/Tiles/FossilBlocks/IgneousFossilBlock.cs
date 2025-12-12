using Microsoft.Xna.Framework;
using Pokemod.Content.TileEntities;
using Pokemod.Content.Items.GeneticSamples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
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
            TileUtils.MergeWithCommonBlocks(Type);

            // Visuals
            DustType = DustID.Obsidian;
            dustHighlight = DustID.RedTorch;
			dustChance = 200; //Higher values = slower rate
			dustHighlightChance = 15; //Higher values = slower rate 
            HitSound = SoundID.Item127;
			
			// Item Drops
			fossilChance = 45;
				fossilList.Add(ModContent.ItemType<InfernalAmberItem>());
			fossilBlockChance = 5;
				fossilBlock = ModContent.ItemType<IgneousFossilItem>();
				defaultBlock = ItemID.Obsidian;
            commonItemChance = 6;
				commonItems.Add(ItemID.Amber);
				commonItems.Add(ItemID.Amber);
				commonItems.Add(ItemID.Ruby);
				commonItems.Add(ItemID.Topaz);
            

            LocalizedText name = Language.GetText("Mods.Pokemod.Items.IgneousFossilItem.DisplayName");
            AddMapEntry(new Color(40, 30, 50), name);
        }

	}

	//Creates 7 clumps of Fossil Blocks in place of dirt/stone/sand each morning. Caps out when there are more than 10000 blocks in the world.
	public class IgneousFossilSystem : FossilBlockSystem
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();

			fossilBlockType = (ushort)ModContent.TileType<IgneousFossilBlock>();
			numPerDay = 10;
			maxTiles = 3000;
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


    public class IgneousFossilItem : FossilBlockItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<IgneousFossilBlock>());
		}
	}
}