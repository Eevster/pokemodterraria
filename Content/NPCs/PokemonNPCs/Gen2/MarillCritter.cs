using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class MarillCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 20;
		public override int hitboxHeight => 24;

		public override int totalFrames => 26;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,8];
		public override int[] walkStartEnd => [9,17];
		public override int[] jumpStartEnd => [9,11];
		public override int[] fallStartEnd => [13,15];
        public override int[] attackStartEnd => [18,25];

		public override bool canSwim => true;

		public override int[] idleSwimStartEnd => [0,8];
		public override int[] walkSwimStartEnd => [9,17];
		public override int[] attackSwimStartEnd => [18,25];

        public override float catchRate => 180;

		public override int[][] spawnConditions =>
		[
			[(int)SpawnArea.Beach, (int)DayTimeStatus.Day, (int)WeatherStatus.All]
        ];

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) { 
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean);
			base.SetBestiary(database, bestiaryEntry);
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneBeach) {
                return GetSpawnChance(spawnInfo, SpawnCondition.OverworldDay.Chance * 0.3f);
            }

			return 0f;
		}
	}

	public class MarillCritterNPCShiny : MarillCritterNPC { }
}