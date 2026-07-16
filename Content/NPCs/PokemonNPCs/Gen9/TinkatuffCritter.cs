using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class TinkatuffCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 62;
		public override int hitboxHeight => 58;

		public override int totalFrames => 40;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,4];
		public override int[] walkStartEnd => [5,9];
		public override int[] jumpStartEnd => [10,13];
		public override int[] fallStartEnd => [11,12];
        public override int[] attackStartEnd => [14, 39];
        public override float catchRate => 120;
        public override int minLevel => 24;

        public override int[][] spawnConditions =>
		[
			[(int)SpawnArea.Surface, (int)DayTimeStatus.All, (int)WeatherStatus.All]
		];

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) { 
			base.SetBestiary(database, bestiaryEntry);
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Marble);
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
				if (spawnInfo.Player.ZoneMarble) {
					return GetSpawnChance(spawnInfo, (SpawnCondition.Underground.Chance + SpawnCondition.Cavern.Chance) * 0.2f);
				}

			return 0f;
		}
		
	}

	public class TinkatuffCritterNPCShiny : TinkatuffCritterNPC { }
}
