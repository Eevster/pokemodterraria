using System.Collections.Generic;
using Pokemod.Common.Players;
using Pokemod.Content.Items.Tools;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace Pokemod.Content.NPCs.TrainerNPCs
{
	public class FishermanTrainer : BattleTrainer
	{
		public override int nPokemon => 3;
		public override string[] pokemonOptions => ["Magikarp", "Poliwag", "Shellder"];

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
				"Andrew",
				"Hank",
				"Tylor",
				"Wade"
			};
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if ((spawnInfo.Player.ZoneBeach || spawnInfo.Player.ZoneSnow) && !NPC.AnyNPCs(Type))
			{
				return 0.15f;
			}

			return 0f;
		}

		public override string GetChat()
		{
			WeightedRandom<string> chat = new WeightedRandom<string>();

			// These are things that the NPC has a chance of telling you when you talk to it.
			chat.Add(Language.GetTextValue($"Mods.Pokemod.Dialogue.{GetType().Name}.StandardDialogue1") + "\n"+
				"[c/FFE270:"+Language.GetText($"Mods.Pokemod.PokemonBattle.TrainerLvl").WithFormatArgs(trainerLevel).Value+"]");
			return chat; // chat is implicitly cast to a string.
		}

		public override void SetChatButtons(ref string button, ref string button2)
		{ // What the chat buttons are when you open up the chat UI
			button = "Battle";
			button2 = Language.GetTextValue("LegacyInterface.28"); //This is the key to the word "Shop"
		}

		public override void OnChatButtonClicked(bool firstButton, ref string shop)
		{
			if (firstButton)
			{
				StartBattle(Main.player[Main.myPlayer]);
			}
			else
			{
				shop = "Shop";
			}
		}
		
		public override void AddShops() {
			new NPCShop(Type)
				.Add<OldRod>()
				.Add<GoodRod>()
				.Add<SuperRod>()
				.Register();
		}
	}
}
