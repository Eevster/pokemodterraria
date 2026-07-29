using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class RhyhornCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 44;
		public override int hitboxHeight => 34;

		public override int totalFrames => 18;
        public override int animationSpeed => 5;
        public override int[] idleStartEnd => [0, 4];
        public override int[] walkStartEnd => [5, 8];
        public override int[] jumpStartEnd => [9, 10];
        public override int[] fallStartEnd => [11, 13];
        public override int[] attackStartEnd => [14, 17];
		public override float catchRate => 120;

		public override int minLevel => 24;

		public override int[][] spawnConditions =>
		[
			[(int)SpawnArea.Desert, (int)DayTimeStatus.All, (int)WeatherStatus.All]
        ];

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) { 
			base.SetBestiary(database, bestiaryEntry);
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Desert);
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneDesert) {
				return GetSpawnChance(spawnInfo, SpawnCondition.Overworld.Chance * 0.1f);
			}

			return 0f;
		}
		
	}

	public class RhyhornCritterNPCShiny : RhyhornCritterNPC{}
}