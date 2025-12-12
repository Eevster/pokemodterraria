using Microsoft.Xna.Framework;
using Pokemod.Content.TileEntities;
using Pokemod.Content.Items.GeneticSamples;
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
	public class PrimordialFossilBlock : FossilBlock
	{
		
		public override void SetStaticDefaults()
		{
            base.SetStaticDefaults();

            // Block Function
            MineResist = 1.5f;
			MinPick = 0;
            Main.tileOreFinderPriority[Type] = 150;
            TileUtils.MergeWithCommonBlocks(Type);

            // Visuals
            DustType = DustID.Pearlwood;
            dustHighlight = DustID.OrangeTorch;
			dustChance = 200; //Higher values = slower rate
			dustHighlightChance = 20; //Higher values = slower rate 
            HitSound = SoundID.Item127;
			
			// Item Drops
			fossilChance = 11;
				fossilList.Add(ModContent.ItemType<OldAmberItem>());
				fossilList.Add(ModContent.ItemType<HelixFossilItem>());
				fossilList.Add(ModContent.ItemType<DomeFossilItem>());
				fossilList.Add(ModContent.ItemType<SkullFossilItem>());
            fossilBlockChance = 5;
				fossilBlock = ModContent.ItemType<PrimordialFossilItem>();
				defaultBlock = ItemID.DirtBlock;
            commonItemChance = 3;
				commonItems.Add(ItemID.Bone);
				
            LocalizedText name = Language.GetText("Mods.Pokemod.Items.PrimordialFossilItem.DisplayName");
            AddMapEntry(new Color(150, 115, 90), name);
        }

	}

	//Creates 7 clumps of Fossil Blocks in place of dirt/stone/sand each morning. Caps out when there are more than 10000 blocks in the world.
	public class PrimordialFossilSystem : FossilBlockSystem
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();

			fossilBlockType = (ushort)ModContent.TileType<PrimordialFossilBlock>();
			numPerDay = 30;
			maxTiles = 7000;
            priority = 4;
			canSpawn = true;
        }
	}

    public class PrimordialFossilItem : FossilBlockItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<PrimordialFossilBlock>());
		}
	}
}