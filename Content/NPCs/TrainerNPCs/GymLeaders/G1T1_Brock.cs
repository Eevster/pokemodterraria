using System.Collections.Generic;
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
	public class G1T1_Brock : BattleTrainer
	{
		public override bool GymLeader => true;
		public override void LoadTeam()
		{
			pokemonTeam = new List<EnemyPokemonInfo>();

			pokemonTeam.Add(new EnemyPokemonInfo("Geodude", 12, ["Tackle", "Harden"]));
			pokemonTeam.Add(new EnemyPokemonInfo("Onix", 14, ["Tackle", "RockThrow", "RockSlide", "Harden"]));
		}
		
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean,
				new FlavorTextBestiaryInfoElement($"Mods.Pokemod.Bestiary.{GetType().Name}"),
			});
		}

		public override List<string> SetNPCNameList()
		{
			return new List<string> {
				"Brock",
			};
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if ((spawnInfo.Player.ZoneRockLayerHeight || spawnInfo.Player.ZoneDesert) && !NPC.AnyNPCs(Type))
			{
				return 0.1f;
			}

			return 0f;
		}
		public override string GetChat()
		{
			WeightedRandom<string> chat = new WeightedRandom<string>();

			// These are things that the NPC has a chance of telling you when you talk to it.
			chat.Add(Language.GetTextValue($"Mods.Pokemod.Dialogue.{GetType().Name}.StandardDialogue1"));
			return chat; // chat is implicitly cast to a string.
		}

		public override void SetChatButtons(ref string button, ref string button2)
		{ // What the chat buttons are when you open up the chat UI
			button = "Gym Battle";
		}

		public override void OnChatButtonClicked(bool firstButton, ref string shop)
		{
			if (firstButton)
			{
				StartBattle(Main.player[Main.myPlayer]);
			}
		}
	}
}
