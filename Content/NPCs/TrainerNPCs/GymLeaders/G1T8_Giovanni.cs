using System.Collections.Generic;
using Pokemod.Content.Items.Badges;
using Pokemod.Content.Items.Tools;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace Pokemod.Content.NPCs.TrainerNPCs.GymLeaders
{
	public class G1T8_Giovanni : BattleTrainer
	{
        
        public override bool GymLeader => true;
		public override void LoadTeam()
		{
			pokemonTeam =
            [
                new EnemyPokemonInfo("Rhyhorn", 45, ["Tackle", "RockSlide", "Tackle", "Earthquake"]),
                new EnemyPokemonInfo("Dugtrio", 42, ["Slash", "Dig", "MudSlap", "Earthquake"]),
                new EnemyPokemonInfo("Nidoqueen", 44, ["FocusPunch", "Crunch", "Toxic", "Bite"]),
                new EnemyPokemonInfo("Nidoking", 45, ["FocusPunch", "Crunch", "Toxic", "Bite"]),
                new EnemyPokemonInfo("Rhyhorn", 50, ["Tackle", "RockSlide", "Tackle", "Earthquake"]),
            ];
		}

		public override void GiveRewards(Player opponent)
        {
			opponent.QuickSpawnItem(NPC.GetSource_FromThis(), ModContent.ItemType<EarthBadge>(), 1);
			opponent.QuickSpawnItem(NPC.GetSource_FromThis(), ItemID.GoldCoin, 15);
            base.GiveRewards(opponent);
        }

		private static Profiles.StackedNPCProfile NPCProfile;
		public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
			NPCProfile = new Profiles.StackedNPCProfile(
				new Profiles.DefaultNPCProfile(Texture, -1)
			// new Profiles.DefaultNPCProfile(Texture + "_Shimmer", -1)
			);
        }
		public override ITownNPCProfile TownNPCProfile()
		{
			return NPCProfile;
		}
		
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				new FlavorTextBestiaryInfoElement($"Mods.Pokemod.Bestiary.{GetType().Name}"),
			});
		}

		public override List<string> SetNPCNameList()
		{
			return new List<string> {
				"Giovanni",
			};
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (Main.hardMode && spawnInfo.Player.ZoneForest && !NPC.AnyNPCs(Type))
			{
				return 0.01f;
			}

			return 0f;
		}
		public override string GetChat()
		{
			WeightedRandom<string> chat = new WeightedRandom<string>();

			// These are things that the NPC has a chance of telling you when you talk to it.
			if(!WasDefeatedBy(Main.player[Main.myPlayer])){
				chat.Add(Language.GetTextValue($"Mods.Pokemod.Dialogue.{GetType().Name}.StandardDialogue1"));
			}
			else
			{
				chat.Add(Language.GetTextValue($"Mods.Pokemod.Dialogue.{GetType().Name}.DefeatedDialogue"));
			}

			return chat; // chat is implicitly cast to a string.
		}

		public override void SetChatButtons(ref string button, ref string button2)
		{ // What the chat buttons are when you open up the chat UI
			button = "Gym Battle";
		}

		public override void OnChatButtonClicked(bool firstButton, ref string shop)
		{
			Player opponent = Main.player[Main.myPlayer];

			if (firstButton && !WasDefeatedBy(opponent))
			{
				StartBattle(opponent);
			}
		}
	}
}
