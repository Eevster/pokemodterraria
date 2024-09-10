using Pokemod.Content.Items;
using Pokemod.Content.Pets.BulbasaurPet;
using Microsoft.Xna.Framework;
using MonoMod.Cil;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Terraria.DataStructures;
namespace Pokemod.Content.NPCs.Bulbasaur
{
    /// <summary>
    /// This file shows off a critter npc. The unique thing about critters is how you can catch them with a bug net.
    /// The important bits are: Main.npcCatchable, NPC.catchItem, and Item.makeNPC.
    /// We will also show off adding an item to an existing RecipeGroup (see ExampleRecipes.AddRecipeGroups).
    /// Additionally, this example shows an involved IL edit.
    /// </summary>
    public class BulbasaurCritterNPC : Pokemon
    {
        private const int ClonedNPCID = NPCID.Frog; // Easy to change type for your modder convenience

        public override void Load()
        {
            
        }

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = Main.npcFrameCount[ClonedNPCID];
            Main.npcCatchable[Type] = true; // This is for certain release situations

            // These three are typical critter values
            NPCID.Sets.CountsAsCritter[Type] = true;
            NPCID.Sets.TakesDamageFromHostilesWithoutBeingFriendly[Type] = true;
            NPCID.Sets.TownCritter[Type] = true;

            // The frog is immune to confused
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;

            // This is so it appears between the frog and the gold frog
            NPCID.Sets.NormalGoldCritterBestiaryPriority.Insert(NPCID.Sets.NormalGoldCritterBestiaryPriority.IndexOf(ClonedNPCID) + 1, Type);
        }

        public override void SetDefaults()
        {
            name = "Bulbasaur";
            type1 = "Grass";
            type2 = "Poison";
            ID = 001;
            //stat block
            baseHp = 45;
            baseAtk = 49;
            baseDef = 49;
            baseSpatk = 65;
            baseSpdef = 65;
            baseSpeed = 45;

            NPC.width = 26;
            NPC.height = 22;
            NPC.aiStyle = 7;
            NPC.lifeMax = StatFunc(true,baseHp,0,0,5);
            NPC.defDamage = StatFunc(false, baseAtk, 0, 0, 5);
            NPC.defDefense = StatFunc(false, baseDef, 0, 0, 5);
        

            NPC.catchItem = ModContent.ItemType<BulbasaurPetItem>();
            NPC.lavaImmune = true;
            AIType = 7;
            AnimationType = ClonedNPCID;
        }

        public override void OnSpawn(IEntitySource source)
        {
            NPC.GetGlobalNPC<PokemonNPCData>().SetPokemonNPCData("Bulbasaur");
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                new FlavorTextBestiaryInfoElement("For some time after its birth, it grows by taking nourishment from the seed on its back."));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.SurfaceJungle.Chance * 0.1f;
        }

        


        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                for (int i = 0; i < 6; i++)
                {
                    Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Worm, 2 * hit.HitDirection, -2f);
                    if (Main.rand.NextBool(2))
                    {
                        dust.noGravity = true;
                        dust.scale = 1.2f * NPC.scale;
                    }
                    else
                    {
                        dust.scale = 0.7f * NPC.scale;
                    }
                }
                //Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>($"{Name}_Gore_Head").Type, NPC.scale);
                //Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>($"{Name}_Gore_Leg").Type, NPC.scale);
            }
        }

        public override void OnCaughtBy(Player player, Item item, bool failed)
        {
            if (failed)
            {
                return;
            }

            Point npcTile = NPC.Center.ToTileCoordinates();

            if (!WorldGen.SolidTile(npcTile.X, npcTile.Y))
            { // Check if the tile the npc resides the most in is non solid
                Tile tile = Main.tile[npcTile];
                tile.LiquidAmount = tile.LiquidType == LiquidID.Lava ? // Check if the tile has lava in it
                    Math.Max((byte)Main.rand.Next(50, 150), tile.LiquidAmount) // If it does, then top up the amount
                    : (byte)Main.rand.Next(50, 150); // If it doesn't, then overwrite the amount. Technically this distinction should never be needed bc it will burn but to be safe it's here
                WorldGen.SquareTileFrame(npcTile.X, npcTile.Y, true); // Update the surrounding area in the tilemap
            }
        }
    }

    public class BulbasaurCritterItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.IsLavaBait[Type] = true; // While this item is not bait, this will require a lava bug net to catch.
        }

        public override void SetDefaults()
        {
            // useStyle = 1;
            // autoReuse = true;
            // useTurn = true;
            // useAnimation = 15;
            // useTime = 10;
            // maxStack = CommonMaxStack;
            // consumable = true;
            // width = 12;
            // height = 12;
            // makeNPC = 361;
            // noUseGraphic = true;

            // Cloning ItemID.Frog sets the preceding values
            Item.CloneDefaults(ItemID.Frog);
            Item.makeNPC = ModContent.NPCType<BulbasaurCritterNPC>();
            Item.value += Item.buyPrice(0, 0, 30, 0); // Make this critter worth slightly more than the frog
            Item.rare = ItemRarityID.Blue;
        }
    }
}