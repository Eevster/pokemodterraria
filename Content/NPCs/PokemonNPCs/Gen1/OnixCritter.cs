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
    public class OnixCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 92;
		public override int hitboxHeight => 78;

		public override int totalFrames => 5;
		public override int animationSpeed => 15;
		public override int[] idleStartEnd => [0, 1];
		public override int[] walkStartEnd => [0, 3];
		public override int[] jumpStartEnd => [0, 0];
		public override int[] fallStartEnd => [1, 1];
		public override int[] attackStartEnd => [4, 4];
		public override float catchRate => 45;
        public override int minLevel => 8;

        public override int[][] spawnConditions =>
        [
            [(int)SpawnArea.Underground, (int)DayTimeStatus.All, (int)WeatherStatus.All],
			[(int)SpawnArea.Desert, (int)DayTimeStatus.All, (int)WeatherStatus.All]
        ];

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) { 
			base.SetBestiary(database, bestiaryEntry);
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground);
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneNormalUnderground || spawnInfo.Player.ZoneNormalCaverns)
			{
				return GetSpawnChance(spawnInfo, (SpawnCondition.Underground.Chance + SpawnCondition.Cavern.Chance) * 0.2f);
			}
            if (spawnInfo.Player.ZoneDesert)
            {
                return GetSpawnChance(spawnInfo, SpawnCondition.Overworld.Chance * 0.05f);
            }

            return 0f;
		}
	}
	
	public class OnixCritterNPCShiny : OnixCritterNPC {}
	
	/*
    public class OnixCritterNPC : WormPokemonHead
    {
		public override int hitboxWidth => 24;
		public override int hitboxHeight => 24;

		public override int totalFrames => 6;
		public override float catchRate => 45;
        public override int minLevel => 8;
		
		public override int[][] spawnConditions =>
        [
            [(int)SpawnArea.Underground, (int)DayTimeStatus.All, (int)WeatherStatus.All],
			[(int)SpawnArea.Desert, (int)DayTimeStatus.All, (int)WeatherStatus.All]
        ];

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) { 
			base.SetBestiary(database, bestiaryEntry);
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground);
		}

        public override bool HasCustomBodySegments => true;


        public override int BodyType => ModContent.NPCType<OnixCritterNPC_Body>();

        public override int TailType => ModContent.NPCType<OnixCritterNPC_Tail>();

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

			var drawModifier = new NPCID.Sets.NPCBestiaryDrawModifiers() { // Influences how the NPC looks in the Bestiary
				CustomTexturePath = "Pokemod/Assets/Textures/Pokesprites/Pets/"+pokemonName+(shiny?"PetProjectileShiny":"PetProjectile"), // If the NPC is multiple parts like a worm, a custom texture for the Bestiary is encouraged.
				Position = new Vector2(40f, 24f),
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
            MinSegmentLength = 11;
			MaxSegmentLength = 11;

			MoveSpeed = moveSpeed;
			Acceleration = 0.2f;
        }

		public override void FindFrame(int frameHeight)
		{
			NPC.frame.Y = 0 * frameHeight;
			NPC.gfxOffY = (96 - NPC.height) / 2; 
		}

        public override int SpawnBodySegments(int segmentCount)
        {
			int latestNPC = NPC.whoAmI;

			for (int i = 0; i < 9; i++) {
				int newWidth = 20;
				int frame = 1;

				switch (i)
				{
					case 2:
					case 4:
					case 5:
						frame = 2;
						newWidth = 16;
						break;
					case 6:
						frame = 3;
						newWidth = 12;
						break;
					case 7:
					case 8:
						frame = 4;
						newWidth = 8;
						break;
					default:
						break;
				}

				latestNPC = SpawnSegment(NPC.GetSource_FromAI(), BodyType, latestNPC);

				if(Main.npc[latestNPC].ModNPC is WormPokemonNPC currentNPC){
					currentNPC.NPC.Hitbox = new Rectangle((int)(currentNPC.NPC.position.X + (96 - newWidth) / 2), (int)(currentNPC.NPC.position.Y + (96 - newWidth)/2), newWidth, newWidth);
					currentNPC.currentFrame = frame;
				}
			}

			return latestNPC;
        }

    }

    public class OnixCritterNPC_Body : WormPokemonBody
    {
		public override int hitboxWidth => 20;
		public override int hitboxHeight => 20;

		public override int totalFrames => 6;
		public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers() {
				Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
			NPCID.Sets.RespawnEnemyID[Type] = ModContent.NPCType<OnixCritterNPC>();
        }

		public override void SetDefaults()
        {
			NPC.CloneDefaults(NPCID.DiggerBody);
			NPC.Hitbox = new Rectangle((int)(NPC.position.X + (96 - hitboxWidth) / 2), (int)(NPC.position.Y + (96 - hitboxHeight)/2), hitboxWidth, hitboxHeight);

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
			NPC.gfxOffY = (96 - NPC.height) / 2; 
		}
    }

	public class OnixCritterNPC_Tail : WormPokemonTail
    {
		public override int hitboxWidth => 8;
		public override int hitboxHeight => 8;

		public override int totalFrames => 6;
		public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers() {
				Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
			NPCID.Sets.RespawnEnemyID[Type] = ModContent.NPCType<OnixCritterNPC>();
        }
		
		public override void SetDefaults()
        {
			NPC.CloneDefaults(NPCID.DiggerTail);
			NPC.Hitbox = new Rectangle((int)(NPC.position.X + (96 - hitboxWidth) / 2), (int)(NPC.position.Y + (96 - hitboxHeight)/2), hitboxWidth, hitboxHeight);

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
			NPC.frame.Y = 5 * frameHeight;
			NPC.gfxOffY = (96 - NPC.height) / 2; 
		}
    }

    public class OnixCritterNPCShiny : OnixCritterNPC
	{
		public override int BodyType => ModContent.NPCType<OnixCritterNPCShiny_Body>();

        public override int TailType => ModContent.NPCType<OnixCritterNPCShiny_Tail>();
	}

	public class OnixCritterNPCShiny_Body : OnixCritterNPC_Body{}

	public class OnixCritterNPCShiny_Tail : OnixCritterNPC_Tail{}*/
}