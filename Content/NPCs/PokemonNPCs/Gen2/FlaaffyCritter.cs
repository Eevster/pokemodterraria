using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class FlaaffyCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 24;
		public override int hitboxHeight => 42;

		public override int totalFrames => 36;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,9];
		public override int[] walkStartEnd => [26,35];
		public override int[] jumpStartEnd => [9,14];
		public override int[] fallStartEnd => [11,13];
        public override int[] attackStartEnd => [14, 26];
        public override float catchRate => 120;
        public override int minLevel => 15;

        public override int[][] spawnConditions =>
		[
			[(int)SpawnArea.Surface, (int)DayTimeStatus.All, (int)WeatherStatus.All]
		];

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) { 
			base.SetBestiary(database, bestiaryEntry);
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface);
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
				if (spawnInfo.Player.ZoneForest) {
					return GetSpawnChance(spawnInfo, SpawnCondition.Overworld.Chance * 0.2f);
			}

			return 0f;
		}
		
	}

	public class FlaaffyCritterNPCShiny : FlaaffyCritterNPC { }
}
