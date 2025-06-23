using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace Pokemod.Content.NPCs.TrainerNPCs
{
	public class FishermanTrainer : ModNPC
	{
		private static Profiles.StackedNPCProfile NPCProfile;

		public override void SetStaticDefaults() {
			Main.npcFrameCount[Type] = 26; // The amount of frames the NPC has

			NPCID.Sets.ExtraFramesCount[Type] = 10; // Generally for Town NPCs, but this is how the NPC does extra things such as sitting in a chair and talking to other NPCs.
			NPCID.Sets.AttackFrameCount[Type] = 0;
			NPCID.Sets.DangerDetectRange[Type] = 700; // The amount of pixels away from the center of the npc that it tries to attack enemies.
			NPCID.Sets.PrettySafe[Type] = 300;
			NPCID.Sets.AttackType[Type] = -1; // magic attack.
			NPCID.Sets.AttackTime[Type] = 0; // The amount of time it takes for the NPC's attack animation to be over once it starts.
			NPCID.Sets.AttackAverageChance[Type] = 0;
			NPCID.Sets.HatOffsetY[Type] = 0; // For when a party is active, the party hat spawns at a Y offset.
			NPCID.Sets.ShimmerTownTransform[NPC.type] = false; // This set says that the Town NPC has a Shimmered form. Otherwise, the Town NPC will become transparent when touching Shimmer like other enemies.

			NPCID.Sets.ActsLikeTownNPC[Type] = true;
			NPCID.Sets.NoTownNPCHappiness[Type] = true;
			NPCID.Sets.SpawnsWithCustomName[Type] = true;

			NPCID.Sets.AllowDoorInteraction[Type] = true;

			// Influences how the NPC looks in the Bestiary
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers() {
				Velocity = 1f, // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
				Direction = -1 // -1 is left and 1 is right. NPCs are drawn facing the left by default but ExamplePerson will be drawn facing the right
			};

			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

			NPCProfile = new Profiles.StackedNPCProfile(
				new Profiles.DefaultNPCProfile(Texture, -1)
				// new Profiles.DefaultNPCProfile(Texture + "_Shimmer", -1)
			);
		}

		public override void SetDefaults() {
			NPC.friendly = true; // NPC Will not attack player
			NPC.width = 18;
            NPC.height = 40;
			NPC.aiStyle = 7;
			NPC.damage = 0;
			NPC.defense = 50;
			NPC.lifeMax = 500;
			NPC.HitSound = SoundID.PlayerHit;
			NPC.DeathSound = SoundID.PlayerKilled;
			NPC.knockBackResist = 0f;

			AnimationType = NPCID.Demolitionist;
		}

		//Make sure to allow your NPC to chat, since being "like a town NPC" doesn't automatically allow for chatting.
		public override bool CanChat() {
			return true;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean,
				new FlavorTextBestiaryInfoElement($"Mods.Pokemod.Bestiary.{GetType().Name}"),
			});
		}

		public override ITownNPCProfile TownNPCProfile() {
			return NPCProfile;
		}

		public override List<string> SetNPCNameList() {
			return new List<string> {
				"Andrew",
				"Hank",
				"Tylor",
				"Wade"
			};
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if ((spawnInfo.Player.ZoneBeach || spawnInfo.Player.ZoneSnow) && !NPC.AnyNPCs(Type)) {
				return 0.2f;
			}

			return 0f;
		}
		public override string GetChat() {
			WeightedRandom<string> chat = new WeightedRandom<string>();

			// These are things that the NPC has a chance of telling you when you talk to it.
			chat.Add(Language.GetTextValue($"Mods.Pokemod.Dialogue.{GetType().Name}.StandardDialogue1"));
			return chat; // chat is implicitly cast to a string.
		}

		public override void SetChatButtons(ref string button, ref string button2) { // What the chat buttons are when you open up the chat UI
			button = "Battle";
			//button = Language.GetTextValue("LegacyInterface.28"); //This is the key to the word "Shop"
		}

		public override void OnChatButtonClicked(bool firstButton, ref string shop) {
			if (firstButton) {
				//shop = "Shop";
			}
		}
	}
}
