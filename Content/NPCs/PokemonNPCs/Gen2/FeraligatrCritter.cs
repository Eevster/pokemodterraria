﻿using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class FeraligatrCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 28;
		public override int hitboxHeight => 46;

		public override int totalFrames => 18;
		public override int animationSpeed => 7;
		public override int[] idleStartEnd => [0,5];
		public override int[] walkStartEnd => [6,11];
		public override int[] jumpStartEnd => [7,7];
		public override int[] fallStartEnd => [10,10];
		public override int[] attackStartEnd => [12,17];

		public override bool canSwim => true;

		public override int[] idleSwimStartEnd => [0,5];
		public override int[] walkSwimStartEnd => [6,11];
		public override int[] attackSwimStartEnd => [12,17];

		public override int minLevel => 36;
		
		public override int[][] spawnConditions =>
		[
			[(int)SpawnArea.Beach, (int)DayTimeStatus.Day, (int)WeatherStatus.All]
        ];

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean,
				new FlavorTextBestiaryInfoElement("It usually moves slowly, but it goes at blinding speed when it attacks and bites prey."));
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneBeach) {
                return GetSpawnChance(spawnInfo, SpawnCondition.OverworldDay.Chance * 0.5f);
            }

			return 0f;
		}
	}

	public class FeraligatrCritterNPCShiny : FeraligatrCritterNPC{}
}