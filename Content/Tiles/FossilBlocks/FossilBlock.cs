using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Xna.Framework;
using Pokemod.Content.Items.GeneticSamples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Terraria;
using Terraria.GameContent.Biomes;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Tiles.FossilBlocks
{
	public abstract class FossilBlock : ModTile
	{
		public int fossilChance = 30; // 1 in 30 chance to be a Rare Fossil
		public List<int> fossilList = [];
		public int fossilBlockChance = 5; // 1 in 5 chance to be a Fossil Block
        public int fossilBlock;
        public int commonItemChance = 3; // 1 in 3 chance to be a Bone
		public List<int> commonItems = [];
        public int defaultBlock;
		
		public int dustHighlight;
		public int dustChance;
		public int dustHighlightChance;

		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileBlendAll[Type] = false;
			Main.tileBlockLight[Type] = true;
			Main.tileSpelunker[Type] = true;
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}

		public override void EmitParticles(int i, int j, Tile tile, short tileFrameX, short tileFrameY, Color tileLight, bool visible)
		{
			if (Main.rand.NextBool(dustChance))
			{
				if (Main.rand.NextBool(dustHighlightChance))
				{
					int dust = Dust.NewDust(new Vector2(i * 16, j * 16), 16, 16, dustHighlight, Scale: 1f);
					Main.dust[dust].noGravity = true;
                    Main.dust[dust].noLight = true;
				}
				else
				{
					int dust = Dust.NewDust(new Vector2(i * 16, j * 16), 16, 16, DustType, SpeedY: -1f, Scale: 1f);
					Main.dust[dust].noGravity = true;
				}
			}
		}


        public override IEnumerable<Item> GetItemDrops(int i, int j)
        {
            if (Main.rand.NextBool(fossilChance))
            {
                int droppedItem = 0;

                float totalChance = 0;
                foreach (var type in fossilList)
                {
                    GeneticSampleItem item = (GeneticSampleItem)ModContent.GetModItem(type);
                    totalChance += item.dropChance;
                }
                float selection = Main.rand.NextFloat(totalChance);
                foreach (var type in fossilList)
                {
                    GeneticSampleItem item = (GeneticSampleItem)ModContent.GetModItem(type);
                    totalChance -= item.dropChance;
                    if (totalChance <= selection)
                    {
                        droppedItem = type;
                        break;
                    }
                }
                if (droppedItem != 0)
                {
                    yield return new Item(droppedItem);
                }
            }
            if (Main.rand.NextBool(commonItemChance))
            {
                int commonItem = commonItems[Main.rand.Next(commonItems.Count)];
                yield return new Item(commonItem);
            }
            if (Main.rand.NextBool(fossilBlockChance))
            {
                yield return new Item(fossilBlock);
            }
            else
            {
                yield return new Item(defaultBlock);
            }
        }
    }

	//Creates 7 clumps of Fossil Blocks in place of dirt/stone/sand each morning. Caps out when there are more than 10000 blocks in the world.
	public abstract class FossilBlockSystem : ModSystem
	{
        public bool debug = false;

        public bool morning = false;
		public ushort fossilBlockType;
		public int numPerDay;
		public int maxTiles;
		public List<ushort> replaceTiles = [0];
		public int heightTop;
		public int heightBottom;
        public int positionCenterX;
        public int widthX;
        public int priority = 1;
        public bool canSpawn = false;

        public void PlaceTilesInWorld(int numVeins, int maxFossilBlocks)
		{
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				// Since this happens during gameplay, we need to run this code on another thread. If we do not, the game will experience lag for a brief moment. This is especially necessary for world generation tasks that would take even longer to execute.
				ThreadPool.QueueUserWorkItem(_ =>
				{
					int tileCount = CountCurrentTiles();
					if (tileCount < maxFossilBlocks * (Main.maxTilesX / 4200f))
					{
						int splotches = (int)(numVeins * (Main.maxTilesX / 4200f));
						for (int iteration = 0; iteration < splotches; iteration++)
						{
                            FindSplotch();
						}
					}
					if (debug)
					{
                        int newTileCount = CountCurrentTiles();
						Main.NewText("Added " + (newTileCount - tileCount) + " " + Name + " Tile Count: " + newTileCount + " / " + maxFossilBlocks * (Main.maxTilesX / 4200f));
					}
				});
			}
		}

		public void FindSplotch()
		{
            foreach (ushort replaceTile in replaceTiles)
            {
                int j = WorldGen.genRand.Next(heightTop, heightBottom);
                for (int checks = 0; checks < 100; checks++)
                {
                    j = WorldGen.genRand.Next(heightTop, heightBottom);
                    if (j > 0 && j < Main.maxTilesX)
                    {
                        break;
                    }
                    if (checks == 98) return;
                }
                int i = -1;
                bool tileFound = false;

                for (int checks = 0; checks < 1000; checks++)
                {
                    i = WorldGen.genRand.Next(positionCenterX - widthX, positionCenterX + widthX);
                    if (i <= 100 || i >= Main.maxTilesX - 100) continue;
                    if (replaceTile == 0)
                    {
                        tileFound = true;
                        break;
                    }
                    else if (Main.tile[i, j].HasTile)
                    {
                        if (Main.tile[i, j].TileType == replaceTile)
                        {
                            tileFound = true;
                            break;
                        }
                    }
                }
                if (tileFound)
                {
                    bool originalTileValue = false;
                    if (replaceTile > 0){
                        originalTileValue = TileID.Sets.CanBeClearedDuringOreRunner[replaceTile];
                        TileID.Sets.CanBeClearedDuringOreRunner[replaceTile] = true;
                    }
                    WorldGen.OreRunner(i, j, WorldGen.genRand.Next(5, 9), WorldGen.genRand.Next(5, 9), fossilBlockType);
                    if (replaceTile > 0){
                        TileID.Sets.CanBeClearedDuringOreRunner[replaceTile] = originalTileValue;
                    }
                    return;
                }
            }
        }

		public virtual void UpdateWorldData()
		{
            heightTop = (int)Main.rockLayer;
            heightBottom = Main.maxTilesY - 450;
            positionCenterX = Main.maxTilesX / 2;
            widthX = Main.maxTilesX / 2 - 100;
        }

		public int CountCurrentTiles()
		{
			int tileCount = 0;
			for(int i = 0; i< Main.maxTilesX; i++)
			{
				for (int j = 0; j < Main.maxTilesY; j++)
				{
					if (Main.tile[i, j].HasTile)
					{
						if (Main.tile[i, j].TileType == fossilBlockType) tileCount++;
					}
				}
			}
            return tileCount;
		}

        public override void PostUpdateWorld()
        {
            base.PostUpdateWorld();
            if (Main.time == priority && Main.dayTime && morning)
            {
                UpdateWorldData();
                if (canSpawn){
                    PlaceTilesInWorld(numPerDay, maxTiles);
                }
                morning = false;
            }
            else if (Main.time == 1 && !Main.dayTime) morning = true;
        }
    }
}