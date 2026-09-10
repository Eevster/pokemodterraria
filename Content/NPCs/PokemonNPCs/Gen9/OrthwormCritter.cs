using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using System;

namespace Pokemod.Content.NPCs.PokemonNPCs
{	
    public class OrthwormCritterNPC : WormPokemonHead
    {
		public override int hitboxWidth => 16;
		public override int hitboxHeight => 16;

		public override int totalFrames => 4;
		public override float catchRate => 25;
        public override int minLevel => 25;
		
		public override int[][] spawnConditions =>
        [
            [(int)SpawnArea.Underground, (int)DayTimeStatus.All, (int)WeatherStatus.All],
			[(int)SpawnArea.Desert, (int)DayTimeStatus.All, (int)WeatherStatus.All]
        ];

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) { 
			base.SetBestiary(database, bestiaryEntry);
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Desert);
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneNormalUnderground || spawnInfo.Player.ZoneNormalCaverns)
			{
				return GetSpawnChance(spawnInfo, (SpawnCondition.Underground.Chance + SpawnCondition.Cavern.Chance) * 0.1f);
			}
            if (spawnInfo.Player.ZoneDesert)
            {
                return GetSpawnChance(spawnInfo, SpawnCondition.Overworld.Chance * 0.2f);
            }

            return 0f;
		}

        public override bool HasCustomBodySegments => true;


        public override int BodyType => ModContent.NPCType<OrthwormCritterNPC_Body>();

        public override int TailType => ModContent.NPCType<OrthwormCritterNPC_Tail>();

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

			var drawModifier = new NPCID.Sets.NPCBestiaryDrawModifiers() { // Influences how the NPC looks in the Bestiary
				CustomTexturePath = "Pokemod/Assets/Textures/Pokesprites/Pets/"+pokemonName+(shiny?"PetProjectileShiny":"PetProjectile"), // If the NPC is multiple parts like a worm, a custom texture for the Bestiary is encouraged.
				PortraitPositionXOverride = 0f,
				PortraitPositionYOverride = 12f
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, drawModifier);
        }

        public override void SetDefaults()
        {
			NPC.Hitbox = new Rectangle((int)(NPC.position.X + (96 - hitboxWidth) / 2), (int)(NPC.position.Y + (96 - hitboxHeight)/2), hitboxWidth, hitboxHeight);

			NPC.damage = 0;
			NPC.lifeMax = 100;
			NPC.defense = 0;

			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath7;
			NPC.value = 60f;
			NPC.knockBackResist = 0.5f;
			NPC.aiStyle = -1;
			
			NPC.noGravity = true;

			NPC.noTileCollide = true;

			NPC.CanBeReplacedByOtherNPCs = true;

			if (shiny) NPC.rarity = 14;
        }

        public override void Init()
        {
            MinSegmentLength = 6;
			MaxSegmentLength = 6;

			MoveSpeed = moveSpeed;
			Acceleration = 0.2f;
        }

		public override void FindFrame(int frameHeight)
		{
			NPC.frame.Y = 0 * frameHeight;
			NPC.gfxOffY = (40 - NPC.height) / 2; 
		}

        public override int SpawnBodySegments(int segmentCount)
        {
			int latestNPC = NPC.whoAmI;

			for (int i = 0; i < 4; i++) {
				int newWidth = 16;
				int frame = 2;

				switch (i)
				{
					case 0:
						frame = 1;
						newWidth = 20;
						break;
					default:
						break;
				}

				latestNPC = SpawnSegment(NPC.GetSource_FromAI(), BodyType, latestNPC);

				if(Main.npc[latestNPC].ModNPC is WormPokemonNPC currentNPC){
					currentNPC.NPC.Hitbox = new Rectangle((int)(currentNPC.NPC.position.X + (40 - newWidth) / 2), (int)(currentNPC.NPC.position.Y + (40 - newWidth)/2), newWidth, newWidth);
					currentNPC.currentFrame = frame;
				}
			}

			return latestNPC;
        }
    }

    public class OrthwormCritterNPC_Body : WormPokemonBody
    {
		public override int hitboxWidth => 16;
		public override int hitboxHeight => 16;

		public override int totalFrames => 4;
		public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers() {
				Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
			NPCID.Sets.RespawnEnemyID[Type] = ModContent.NPCType<OrthwormCritterNPC>();
        }

		public override void SetDefaults()
        {
			NPC.CloneDefaults(NPCID.DiggerBody);
			NPC.Hitbox = new Rectangle((int)(NPC.position.X + (40 - hitboxWidth) / 2), (int)(NPC.position.Y + (40 - hitboxHeight)/2), hitboxWidth, hitboxHeight);

			NPC.damage = 0;
			NPC.defense = 0;

			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath7;
			NPC.value = 0f;
			NPC.knockBackResist = 0.5f;
			NPC.aiStyle = -1;

			NPC.noGravity = true;

			NPC.noTileCollide = true;

			if (shiny) NPC.rarity = 14;
        }

        public override void Init()
        {
            if(HeadSegment is not null && HeadSegment.ModNPC is PokemonWildNPC head)
			{
				MoveSpeed = head.moveSpeed;
				Acceleration = 0.2f;
			}
        }

		public override void FindFrame(int frameHeight)
		{
			NPC.frame.Y = (int)Math.Clamp(currentFrame,1,100) * frameHeight;
			NPC.gfxOffY = (40 - NPC.height) / 2; 
		}
    }

	public class OrthwormCritterNPC_Tail : WormPokemonTail
    {
		public override int hitboxWidth => 16;
		public override int hitboxHeight => 16;

		public override int totalFrames => 4;
		public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers() {
				Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
			NPCID.Sets.RespawnEnemyID[Type] = ModContent.NPCType<OrthwormCritterNPC>();
        }
		
		public override void SetDefaults()
        {
			NPC.CloneDefaults(NPCID.DiggerTail);
			NPC.Hitbox = new Rectangle((int)(NPC.position.X + (40 - hitboxWidth) / 2), (int)(NPC.position.Y + (40 - hitboxHeight)/2), hitboxWidth, hitboxHeight);

			NPC.damage = 0;
			NPC.defense = 0;

			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath7;
			NPC.value = 0f;
			NPC.knockBackResist = 0.5f;
			NPC.aiStyle = -1;

			NPC.noGravity = true;

			NPC.noTileCollide = true;

			if (shiny) NPC.rarity = 14;
        }

        public override void Init()
        {
            if(HeadSegment is not null && HeadSegment.ModNPC is PokemonWildNPC head)
			{
				MoveSpeed = head.moveSpeed;
				Acceleration = 0.2f;
			}
        }

		public override void FindFrame(int frameHeight)
		{
			NPC.frame.Y = 3 * frameHeight;
			NPC.gfxOffY = (40 - NPC.height) / 2; 
		}
    }

    public class OrthwormCritterNPCShiny : OrthwormCritterNPC
	{
		public override int BodyType => ModContent.NPCType<OrthwormCritterNPCShiny_Body>();

        public override int TailType => ModContent.NPCType<OrthwormCritterNPCShiny_Tail>();
	}

	public class OrthwormCritterNPCShiny_Body : OrthwormCritterNPC_Body{}

	public class OrthwormCritterNPCShiny_Tail : OrthwormCritterNPC_Tail{}
}