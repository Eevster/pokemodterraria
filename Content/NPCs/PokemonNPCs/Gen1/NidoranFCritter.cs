﻿using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class NidoranFCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 32;
		public override int hitboxHeight => 24;

		public override int totalFrames => 16;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0, 4];
		public override int[] walkStartEnd => [5, 9];
		public override int[] jumpStartEnd => [7, 7];
		public override int[] fallStartEnd => [9, 9];
		public override int[] attackStartEnd => [10, 15];
		public override float catchRate => 235;

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				new FlavorTextBestiaryInfoElement("It can freely detach its jaw to swallow large prey whole. It can become too heavy to move, however."));
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (ModContent.GetInstance<BetaMonsConfig>().BetaMonsToggle) {
				if (spawnInfo.Player.ZoneForest) {
					return GetSpawnChance(spawnInfo, SpawnCondition.Overworld.Chance * 0.5f);
			}
			}

			return 0f;
		}
		
	}

	public class NidoranFCritterNPCShiny : NidoranFCritterNPC{}
}