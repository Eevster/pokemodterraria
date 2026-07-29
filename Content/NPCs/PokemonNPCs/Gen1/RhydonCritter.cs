using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class RhydonCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 36;
		public override int hitboxHeight => 70;

		public override int totalFrames => 18;
        public override int animationSpeed => 6;
        public override int[] idleStartEnd => [0, 4];
        public override int[] walkStartEnd => [5, 9];
        public override int[] jumpStartEnd => [10, 11];
        public override int[] fallStartEnd => [12, 12];
        public override int[] attackStartEnd => [13, 17];
		public override float catchRate => 60;
        public override int minLevel => 42;

		public override int[][] spawnConditions =>
		[
			[(int)SpawnArea.Underground, (int)DayTimeStatus.All, (int)WeatherStatus.All]
        ];

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) { 
			base.SetBestiary(database, bestiaryEntry);
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground);
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneNormalUnderground) {
				return GetSpawnChance(spawnInfo, SpawnCondition.Underground.Chance * 0.02f);
			}

			return 0f;
		}
		
	}

	public class RhydonCritterNPCShiny : RhydonCritterNPC{}
}