using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemod.Common.Players;
using Pokemod.Common.Systems;
using Pokemod.Content.Items;
using Pokemod.Content.Items.Consumables;
using Pokemod.Content.Items.Pokeballs;
using Pokemod.Content.Items.EvoStones;
using Pokemod.Content.Items.GeneticSamples;
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
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Pokemod.Content.NPCs.MerchantNPCs
{
	[AutoloadHead]
	public class PokemonScientist : ModNPC
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
			Main.npcFrameCount[Type] = 25;

			NPCID.Sets.ExtraFramesCount[Type] = 9; // Generally for Town NPCs, but this is how the NPC does extra things such as sitting in a chair and talking to other NPCs. This is the remaining frames after the walking frames.
			NPCID.Sets.AttackFrameCount[Type] = 4;
			NPCID.Sets.DangerDetectRange[Type] = 300;
			NPCID.Sets.AttackType[Type] = 0;
			NPCID.Sets.AttackTime[Type] = 90;
			NPCID.Sets.AttackAverageChance[Type] = 60;
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
				.SetBiomeAffection<UndergroundBiome>(AffectionLevel.Love)
				.SetBiomeAffection<OceanBiome>(AffectionLevel.Like)
				.SetBiomeAffection<DesertBiome>(AffectionLevel.Like)
				.SetBiomeAffection<ForestBiome>(AffectionLevel.Dislike)
				.SetBiomeAffection<JungleBiome>(AffectionLevel.Hate)
				.SetNPCAffection(NPCID.Steampunker, AffectionLevel.Love)
				.SetNPCAffection(NPCID.Mechanic, AffectionLevel.Like)
				.SetNPCAffection(NPCID.Princess, AffectionLevel.Like)
				.SetNPCAffection(NPCID.Dryad, AffectionLevel.Dislike)
				.SetNPCAffection(NPCID.Demolitionist, AffectionLevel.Hate);

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
			NPC.aiStyle = 7;
			NPC.damage = 10;
			NPC.defense = 15;
			NPC.lifeMax = 250;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0.5f;

			AnimationType = NPCID.Guide;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			// We can use AddRange instead of calling Add multiple times in order to add multiple items at once
			bestiaryEntry.Info.AddRange([

				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,

				new FlavorTextBestiaryInfoElement("Mods.Pokemod.Bestiary.PokemonScientist.BestiaryEntry")
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
				TownNPCRespawnSystem.unlockedScientistSpawn = true;
                TownNPCRespawnSystem.ScientistArrived = true;
            }
		}

		public override bool CanTownNPCSpawn(int numTownNPCs) // Requirements for the town NPC to spawn.
		{
			var samplesList = ModContent.GetContent<GeneticSampleItem>();
			foreach (var player in Main.ActivePlayers)
			{
				foreach (GeneticSampleItem sampleType in samplesList)
				{
					if (player.HasItem(sampleType.Type))
					{
						TownNPCRespawnSystem.unlockedScientistSpawn = true;
					}
				}
			}
			if (TownNPCRespawnSystem.unlockedScientistSpawn)
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
				"Jed",
				"Connor",
				"Shaun",
				"Ross",
				"Lowell",
				"Ernst",
				"William",
				"Blythe",
				"Reid",
				"Ed",
				"Nicolas"
			};
		}

		public override void AI()
		{
			base.AI();
			if (Main.npcChatRelease) chatIndex = -1;
			if (!TownNPCRespawnSystem.ScientistArrived)
			{
                TownNPCRespawnSystem.ScientistArrived = true;
            }
		}

		public override string GetChat()
		{

			string chosenChat;

			switch (chatIndex)
			{
				case 0:
					chosenChat = Language.GetTextValue("Mods.Pokemod.Dialogue.PokemonScientist.MissingSamplesDialogue");
					break;
				case 1:
					chosenChat = Language.GetTextValue("Mods.Pokemod.Dialogue.PokemonScientist.NoSamplesDialogue");
					break;
				case 2:
					chosenChat = Language.GetTextValue("Mods.Pokemod.Dialogue.PokemonScientist.NoBallDialogue");
					break;
				case 3:
					chosenChat = Language.GetTextValue("Mods.Pokemod.Dialogue.PokemonScientist.LowLevelDialogue");
					break;
				case 4:
                    chosenChat = Language.GetText("Mods.Pokemod.Dialogue.PokemonScientist.SuccessDialogue").WithFormatArgs(lastPokemonName).ToString();
                    break;
				default:
					WeightedRandom<string> chat = new WeightedRandom<string>();
					chat.Add(Language.GetTextValue("Mods.Pokemod.Dialogue.PokemonScientist.StandardDialogue1"));
					chat.Add(Language.GetTextValue("Mods.Pokemod.Dialogue.PokemonScientist.StandardDialogue2"));
					chat.Add(Language.GetTextValue("Mods.Pokemod.Dialogue.PokemonScientist.StandardDialogue3"));
					chosenChat = chat;
					break;
			}
			return chosenChat;
		}

		public override void SetChatButtons(ref string button, ref string button2)
		{ // What the chat buttons are when you open up the chat UI
			button = Language.GetTextValue("Mods.Pokemod.Dialogue.PokemonScientist.Button");
		}

		public override void OnChatButtonClicked(bool firstButton, ref string shop)
		{
			var ballList = ModContent.GetContent<BallItem>();
			int weakestBall = 50000;
			BallItem ball = null;
			Player player = Main.LocalPlayer;

			Color errorColor = new Color(255, 255, 255);

			// Finds the weakest and most abundant pokeball in the player's inventory for the new pokemon to occupy.
			if (ballList.Any())
			{
				foreach (BallItem ballType in ballList)
				{
					if (player.HasItem(ballType.Type))
					{
						if (ballType.Item.value < weakestBall)
						{
							weakestBall = ballType.Item.value;
							int ballIndex = player.FindItem(ballType.Type);
							if (ballIndex < 0) continue;
							ball = (BallItem)player.inventory[ballIndex]?.ModItem;
						}
						else if (ballType.Item.value == weakestBall && player.CountItem(ballType.Type) > player.CountItem(ball.Type))
						{
							int ballIndex = player.FindItem(ballType.Type);
							if (ballIndex < 0) continue;
							ball = (BallItem)player.inventory[ballIndex]?.ModItem;
						}
					}
				}

				// finds the first genetic sample (ordered by fewest samples in excess, eg: between 7/5 Old Ambers and 24/5 Helix Fossils, it will pick the Old Ambers)
				var samplesList = ModContent.GetContent<GeneticSampleItem>();
				if (samplesList.Any())
				{
					int missingSamples = 9999;
					string missingSampleName = "";
					bool lowLevel = false;
					bool pickSuccess = false;
					var samplePick = samplesList.First();

					foreach (GeneticSampleItem sampleType in samplesList)
					{
						if (player.HasItem(sampleType.Type))
						{
							int itemCount = player.CountItem(sampleType.Type);

							if (itemCount >= sampleType.sampleQuantity)
							{
								if (sampleType.minLevel < WorldLevel.MaxWorldLevel)
								{
									if (ball == null)
									{
										CombatText.NewText(player.Hitbox, errorColor, Language.GetTextValue("Mods.Pokemod.Dialogue.PokemonScientist.NoBall"));
										chatIndex = 2;
										Main.npcChatText = GetChat();
										return;
									}
									if (missingSamples > 0) missingSamples = -99999;
									if (missingSamples < sampleType.sampleQuantity - itemCount)
									{
										missingSamples = sampleType.sampleQuantity - itemCount;
										samplePick = sampleType;
										pickSuccess = true;
									}
								}
								else
								{
									lowLevel = true;
								}
							}
							else
							{
								if (missingSamples > sampleType.sampleQuantity - itemCount && !pickSuccess)
									missingSamples = sampleType.sampleQuantity - itemCount;
								missingSampleName = sampleType.DisplayName.Value;
							}
						}
					}
					if (pickSuccess)
					{
						ConstructPokemonFromSample(samplePick, player, ball);
						SoundEngine.PlaySound(SoundID.Item176, NPC.Center);
						for (int i = 0; i < 5; i++)
						{
							Dust.NewDust(NPC.Center, 0, 0, DustID.Dirt);
							Dust.NewDust(NPC.Center, 0, 0, DustID.MagicMirror);
						}
						CombatText.NewText(player.Hitbox, errorColor, Language.GetTextValue("Mods.Pokemod.Dialogue.PokemonScientist.Success"));
						lastPokemonName = samplePick.pokemonName;
						chatIndex = 4;
						Main.npcChatText = GetChat();

						lowLevel = false;
						return;
					}
					if (lowLevel)
					{
						CombatText.NewText(player.Hitbox, errorColor, Language.GetTextValue("Mods.Pokemod.Dialogue.PokemonScientist.LowLevel"));
						chatIndex = 3;
						Main.npcChatText = GetChat();
						return;
					}
					if (missingSamples != 9999 && missingSamples != 0)
					{
						CombatText.NewText(player.Hitbox, errorColor, missingSamples + Language.GetTextValue("Mods.Pokemod.Dialogue.PokemonScientist.MissingSamples"));
						chatIndex = 0;
						Main.npcChatText = GetChat();
						return;
					}
					CombatText.NewText(player.Hitbox, errorColor, Language.GetTextValue("Mods.Pokemod.Dialogue.PokemonScientist.NoSamples"));
					chatIndex = 1;
					Main.npcChatText = GetChat();
					return;
				}
			}
		}

		public void ConstructPokemonFromSample(GeneticSampleItem sampleItem, Player player, BallItem ball)
		{
			int minlvl = sampleItem.minLevel;
			int maxlvl = Math.Min(WorldLevel.MaxWorldLevel, sampleItem.maxLevel);
			if (maxlvl < minlvl) maxlvl = minlvl;
			int lvl = Main.rand.Next(minlvl, maxlvl + 1);

			int[] IVs = PokemonNPCData.GenerateIVs();
			int nature = 10 * Main.rand.Next(5) + Main.rand.Next(5);

			float shinyChance = 0.0024f;
			if (Main.LocalPlayer.GetModPlayer<PokemonPlayer>().HasShinyCharm)
			{
				shinyChance *= 3f;
			}
			bool shiny = Main.rand.NextBool((int)(1f / shinyChance));

			var entitySource = NPC.GetSource_GiftOrReward();

			if (!Main.dedServ && Main.LocalPlayer == player)
			{
				int item = Item.NewItem(entitySource, (int)player.position.X, (int)player.position.Y, player.width, player.height, ModContent.ItemType<CaughtPokemonItem>(), 1, noBroadcast: false, -1);
				CaughtPokemonItem pokeItem = (CaughtPokemonItem)Main.item[item].ModItem;
				pokeItem.SetPokemonData(sampleItem.pokemonName, Shiny: shiny, BallType: ball.Name, lvl, IVs, nature);
				pokeItem.currentHP = pokeItem.GetPokemonStats()[0];
				if (ModContent.TryFind<ModProjectile>("Pokemod", ball.Name.Replace("Item", "Proj"), out ModProjectile proj))
				{
                    if (proj is BallProj ballProj)
                    {
                        ballProj.SetExtraPokemonEffects(ref pokeItem);
                    }
                }

                // Manually Adds the pokemon to the Bestiary when obtained
                string persistentId = "Pokemod/" + sampleItem.pokemonName + "CritterNPC" + (shiny ? "Shiny" : "");
				NPCKillsTracker tracker = Main.BestiaryTracker.Kills;
				int currentCount = tracker.GetKillCount(persistentId);
				tracker.SetKillCountDirectly(persistentId, currentCount + 1);

				if (Main.netMode == NetmodeID.MultiplayerClient)
				{
					NetMessage.SendData(MessageID.SyncItem, -1, -1, null, item, 1f);
				}
			}
			for (int i = 0; i < sampleItem.sampleQuantity; i++)
			{
				int sampleIndex = player.FindItem(sampleItem.Type);
				if (sampleIndex < 0) continue;
				GeneticSampleItem sample = (GeneticSampleItem)player.inventory[sampleIndex]?.ModItem;
				sample.Item.stack--;
			}
			ball.Item.stack--;
			return;
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Potion>()));
		}

		// Make this Town NPC teleport to the King and/or Queen statue when triggered. Return toKingStatue for only King Statues. Return !toKingStatue for only Queen Statues. Return true for both.
		public override bool CanGoToStatue(bool toKingStatue) => toKingStatue;

		public override void TownNPCAttackStrength(ref int damage, ref float knockback)
		{
			damage = 20;
			knockback = 4f;
		}

		public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
		{
			cooldown = 30;
			randExtraCooldown = 30;
		}

		public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
		{
			projType = ProjectileID.Bone;
			attackDelay = 1;
		}

		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			multiplier = 12f;
			randomOffset = 2f;
			gravityCorrection = 2f;
		}
    }
}