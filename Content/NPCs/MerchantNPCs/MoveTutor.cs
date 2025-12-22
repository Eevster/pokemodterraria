using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemod.Common.Players;
using Pokemod.Common.Systems;
using Pokemod.Common.UI.MoveTutorUI;
using Pokemod.Content.Items;
using Pokemod.Content.Items.Consumables;
using Pokemod.Content.Items.Consumables.TMs;
using Pokemod.Content.Items.EvoStones;
using Pokemod.Content.Items.GeneticSamples;
using Pokemod.Content.Items.Pokeballs;
using Pokemod.Content.NPCs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Channels;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Personalities;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;
using static Terraria.GameContent.Animations.Actions.NPCs;

namespace Pokemod.Content.NPCs.MerchantNPCs
{
	[AutoloadHead]
	public class MoveTutor : ModNPC
	{
		private static int ShimmerHeadIndex;
		private static Profiles.StackedNPCProfile NPCProfile;

		public int chatIndex = -1;
		public bool isChatting = false;
		public string lastPokemonName = "";

        public override void Load()
		{
			ShimmerHeadIndex = Mod.AddNPCHeadTexture(Type, Texture + "_Shimmer_Head");
		}

		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[Type] = 22;

			NPCID.Sets.ExtraFramesCount[Type] = 8; // Generally for Town NPCs, but this is how the NPC does extra things such as sitting in a chair and talking to other NPCs. This is the remaining frames after the walking frames.
			NPCID.Sets.AttackFrameCount[Type] = 3;
			NPCID.Sets.DangerDetectRange[Type] = 600;
			NPCID.Sets.AttackType[Type] = -1;
			NPCID.Sets.AttackTime[Type] = 30;
			NPCID.Sets.AttackAverageChance[Type] = 2;
			NPCID.Sets.HatOffsetY[Type] = 4;
			NPCID.Sets.ShimmerTownTransform[NPC.type] = true;
			NPCID.Sets.ShimmerTownTransform[Type] = true;


			// Connects this NPC with a custom emote.
			// This makes it when the NPC is in the world, other NPCs will "talk about him".
			// By setting this you don't have to override the PickEmote method for the emote to appear.
			//NPCID.Sets.FaceEmote[Type] = ModContent.EmoteBubbleType<ExamplePersonEmote>();

			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
			{
				Velocity = 0.5f, // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
				Direction = -1 // -1 is left and 1 is right. NPCs are drawn facing the left by default but ExamplePerson will be drawn facing the right
			};

			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

			NPC.Happiness
				.SetBiomeAffection<OceanBiome>(AffectionLevel.Love)
				.SetBiomeAffection<ForestBiome>(AffectionLevel.Like)
				.SetBiomeAffection<DesertBiome>(AffectionLevel.Like)
				.SetBiomeAffection<SnowBiome>(AffectionLevel.Dislike)
				.SetBiomeAffection<UndergroundBiome>(AffectionLevel.Hate)
				.SetNPCAffection(ModContent.NPCType<PokemonProfessor>(), AffectionLevel.Love)
				.SetNPCAffection(NPCID.Demolitionist, AffectionLevel.Like)
				.SetNPCAffection(NPCID.Princess, AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<PokemonScientist>(), AffectionLevel.Dislike)
				.SetNPCAffection(NPCID.Golfer, AffectionLevel.Hate);

			NPCProfile = new Profiles.StackedNPCProfile(
				new Profiles.DefaultNPCProfile(Texture, NPCHeadLoader.GetHeadSlot(HeadTexture)),
				new Profiles.DefaultNPCProfile(Texture + "_Shimmer", ShimmerHeadIndex)
			);


		}

		public override void SetDefaults()
		{
			NPC.townNPC = true; // Sets NPC to be a Town NPC
			NPC.friendly = true; // NPC Will not attack player
			NPC.width = 18;
			NPC.height = 40;
			NPC.aiStyle = NPCAIStyleID.Passive;
			NPC.damage = 10;
			NPC.defense = 15;
			NPC.lifeMax = 250;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0.5f;

			AnimationType = NPCID.Dryad;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			// We can use AddRange instead of calling Add multiple times in order to add multiple items at once
			bestiaryEntry.Info.AddRange([

				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,

				new FlavorTextBestiaryInfoElement("Mods.Pokemod.Bestiary.MoveTutor.BestiaryEntry")
			]);
		}

		public override void HitEffect(NPC.HitInfo hit)
		{
			int num = NPC.life > 0 ? 1 : 5;

			for (int k = 0; k < num; k++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.DirtSpray);
			}
			/*
			// Create gore when the NPC is killed.
			if (Main.netMode != NetmodeID.Server && NPC.life <= 0) {
				// Retrieve the gore types. This NPC has shimmer and party variants for head, arm, and leg gore. (12 total gores)
				string variant = "";
				if (NPC.IsShimmerVariant) variant += "_Shimmer";
				if (NPC.altTexture == 1) variant += "_Party";
				int hatGore = NPC.GetPartyHatGore();
				int headGore = Mod.Find<ModGore>($"{Name}_Gore{variant}_Head").Type;
				int armGore = Mod.Find<ModGore>($"{Name}_Gore{variant}_Arm").Type;
				int legGore = Mod.Find<ModGore>($"{Name}_Gore{variant}_Leg").Type;

				// Spawn the gores. The positions of the arms and legs are lowered for a more natural look.
				if (hatGore > 0) {
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, hatGore);
				}
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, headGore, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 20), NPC.velocity, armGore);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 20), NPC.velocity, armGore);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 34), NPC.velocity, legGore);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 34), NPC.velocity, legGore);
			}*/
		}

        public override void OnSpawn(IEntitySource source)
		{
			if (source is EntitySource_SpawnNPC)
			{
				// A TownNPC is "unlocked" once it successfully spawns into the world.
				TownNPCRespawnSystem.unlockedMoveTutor = true;
            }
		}

		public override bool CanTownNPCSpawn(int numTownNPCs) // Requirements for the town NPC to spawn.
		{
			if (TownNPCRespawnSystem.unlockedMoveTutor)
			{
				return true;
			}
			return false;
		}

		public override ITownNPCProfile TownNPCProfile()
		{
			return NPCProfile;
		}

		public override List<string> SetNPCNameList()
		{
			return new List<string>() {
				"Sera",
				"Kate",
				"Rinka",
				"Hannah",
				"Mackenzie",
				"Callie",
				"Sally",
				"Anna",
				"Melissa",
				"Margret",
				"Nicole"
			};
		}

		public override string GetChat()
		{
			WeightedRandom<string> chat = new WeightedRandom<string>();
			chat.Add(Language.GetTextValue("Mods.Pokemod.Dialogue.MoveTutor.StandardDialogue1"));
			chat.Add(Language.GetTextValue("Mods.Pokemod.Dialogue.MoveTutor.StandardDialogue2"));
			chat.Add(Language.GetTextValue("Mods.Pokemod.Dialogue.MoveTutor.StandardDialogue3"));

			return chat;
		}

		public override void SetChatButtons(ref string button, ref string button2)
		{ // What the chat buttons are when you open up the chat UI
			button = Language.GetTextValue("Mods.Pokemod.Dialogue.MoveTutor.Button");
		}

		public override void OnChatButtonClicked(bool firstButton, ref string shop)
		{
            MoveTutorUISystem uiSystem = ModContent.GetInstance<MoveTutorUISystem>();
			uiSystem.ShowMyUI(NPC);
            Main.npcChatText = "";
        }

        /*public override void AI()
        {
            base.AI();
			Player player = Main.LocalPlayer;
			int playerDistance = Math.Abs((int)(player.Center - NPC.Center).Length());
			if (playerDistance > 250)
			{
                MoveTutorUISystem uiSystem = ModContent.GetInstance<MoveTutorUISystem>();
                uiSystem.HideMyUI();
            }
        }*/

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BrickBreakTM>()));
		}

		// Make this Town NPC teleport to the King and/or Queen statue when triggered. Return toKingStatue for only King Statues. Return !toKingStatue for only Queen Statues. Return true for both.
		public override bool CanGoToStatue(bool toQueenStatue) => toQueenStatue;

		public override void TownNPCAttackStrength(ref int damage, ref float knockback)
		{
			damage = 60;
			knockback = 8f;
		}

		public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
		{
			cooldown = 10;
			randExtraCooldown = 20;
		}

        public override void TownNPCAttackSwing(ref int itemWidth, ref int itemHeight)
        {
            itemWidth = 16;
            itemHeight = 16;
        }

        public override void DrawTownAttackSwing(ref Texture2D item, ref Rectangle itemFrame, ref int itemSize, ref float scale, ref Vector2 offset)
        {
            item = (Texture2D)ModContent.Request<Texture2D>("Pokemod/Content/NPCs/MerchantNPCs/MoveTutor_Head");
            itemFrame = new Rectangle(0, 0, 1, 1);
            itemSize = 1;
            scale = 0;
            offset = new Vector2(0, 0);
        }
    }
}