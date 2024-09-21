﻿using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class CharmeleonCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 24;
		public override int hitboxHeight => 40;

		public override int totalFrames => 19;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,5];
		public override int[] walkStartEnd => [6,12];
		public override int[] jumpStartEnd => [10,10];
		public override int[] fallStartEnd => [12,12];
		public override int[] attackStartEnd => [13,18];

		public override int minLevel => 16;

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Desert,
				new FlavorTextBestiaryInfoElement("In the rocky mountains where Charmeleon live, their fiery tails shine at night like stars."));
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneDesert) {
                return SpawnCondition.OverworldDay.Chance * 0.5f;
            }

			return 0f;
		}
	}

	public class CharmeleonCritterNPCShiny : CharmeleonCritterNPC{
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneDesert) {
                return SpawnCondition.OverworldDay.Chance * 0.5f * 0.00025f;
            }

			return 0f;
		}
	}
}