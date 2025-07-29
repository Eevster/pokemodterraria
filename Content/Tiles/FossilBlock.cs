using Microsoft.Xna.Framework;
using Pokemod.Content.Items.GeneticSamples;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Tiles
{
	public class FossilBlock : ModTile
	{
		public int fossilChance = 30; // 1 in 30 chance to be a Rare Fossil
		public int fossilBlockChance = 5; // 1 in 5 chance to be a Fossil Block
		public int boneChance = 3; // 1 in 3 chance to be a Bone
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileBlendAll[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileSpelunker[Type] = true;
            Main.tileOreFinderPriority[Type] = 150;

            DustType = DustID.Pearlwood;
			HitSound = SoundID.Item127;

			AddMapEntry(new Color(150, 115, 90));
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}

        public override void EmitParticles(int i, int j, Tile tile, short tileFrameX, short tileFrameY, Color tileLight, bool visible)
        {
			if (Main.rand.NextBool(100))
			{
				if (Main.rand.NextBool(20))
				{
					int dust = Dust.NewDust(new Vector2(i * 16, j * 16), 16, 16, DustID.OrangeTorch, Scale: 0.3f);
					Main.dust[dust].noGravity = true;
				}
				else
				{
                    int dust = Dust.NewDust(new Vector2(i * 16, j * 16), 16, 16, DustID.Pearlwood, SpeedY: -1f, Scale: 1f);
                    Main.dust[dust].noGravity = true;
                }
			}
        }   

		public override IEnumerable<Item> GetItemDrops(int i, int j)
		{
			if (Main.rand.NextBool(fossilChance))
			{
				GeneticSampleItem droppedItem = null;

				var samplesList = ModContent.GetContent<GeneticSampleItem>();
				if (samplesList.Any())
				{
					
					float totalChance = 0;
					foreach (var item in samplesList)
					{
						totalChance += item.dropChance;
					}
					float selection = Main.rand.NextFloat(totalChance);
					foreach (var item in samplesList)
					{
                        totalChance -= item.dropChance;
						if (totalChance <= selection)
						{
							droppedItem = item;
							break;
						}
					}
				}
				if (droppedItem != null)
				{
					yield return new Item(droppedItem.Type);
				}
			}
			else if (Main.rand.NextBool(fossilBlockChance))
			{
				var fossilBlock = ModContent.ItemType<FossilBlockItem>();
				yield return new Item(fossilBlock);
			}
			else
			{
				yield return new Item(Main.rand.NextBool(boneChance) ? ItemID.Bone : ItemID.DirtBlock);
			}
		}
	}

	//Creates 7 clumps of Fossil Blocks in place of dirt/stone/sand each morning. Caps out when there are more than 10000 blocks in the world.
	public class FossilBlockSystem : ModSystem
	{
		public bool morning = false;
		public void PlaceTilesInWorld(int numVeins, int maxFossilBlocks)
		{
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				// Since this happens during gameplay, we need to run this code on another thread. If we do not, the game will experience lag for a brief moment. This is especially necessary for world generation tasks that would take even longer to execute.
				ThreadPool.QueueUserWorkItem(_ =>
				{
					if (CountCurrentTiles() < maxFossilBlocks * (Main.maxTilesX / 4200f))
					{
						int splotches = (int)(numVeins * (Main.maxTilesX / 4200f));
						for (int iteration = 0; iteration < splotches; iteration++)
						{
							int i = WorldGen.genRand.Next(100, Main.maxTilesX - 100);
							int j = WorldGen.genRand.Next((int)Main.rockLayer, Main.UnderworldLayer);

							WorldGen.OreRunner(i, j, WorldGen.genRand.Next(5, 9), WorldGen.genRand.Next(5, 9), (ushort)ModContent.TileType<FossilBlock>());
						}
					}
				});
			}
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
						if (Main.tile[i, j].TileType == ModContent.TileType<FossilBlock>()) tileCount++;
					}
				}
			}
            Main.NewText(tileCount);
            return tileCount;
		}

        public override void PostUpdateWorld()
        {
            base.PostUpdateWorld();
			if(Main.time == 0 && Main.dayTime && morning)
			{
				morning = false;
				PlaceTilesInWorld(7, 10000);
			}
			else if(Main.time == 0 && !Main.dayTime) morning = true;
        }
	}

    public class FossilBlockItem : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 24;

			Item.value = Item.buyPrice(0);
			Item.maxStack = Item.CommonMaxStack;

			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.FossilBlock>());
		}
	}
}