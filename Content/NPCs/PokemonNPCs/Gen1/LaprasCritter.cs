using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class LaprasCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 28;
		public override int hitboxHeight => 46;

		public override int totalFrames => 23;
		public override int animationSpeed => 12;
		public override int[] idleStartEnd => [0, 7];
		public override int[] walkStartEnd => [8, 11];
		public override int[] jumpStartEnd => [0, 0];
		public override int[] fallStartEnd => [19, 19];
		
		public override bool canSwim => true;

		public override int[] idleSwimStartEnd => [13,16];
		public override int[] walkSwimStartEnd => [17,21];
		public override float catchRate => 45;
        public override int minLevel => 20;

        public override int[][] spawnConditions =>
        [
            [(int)SpawnArea.Beach, (int)DayTimeStatus.All, (int)WeatherStatus.All]
        ];

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) { 
			base.SetBestiary(database, bestiaryEntry);
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface);
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneBeach)
			{
				return GetSpawnChance(spawnInfo, SpawnCondition.Overworld.Chance * 0.3f);
			}

			return 0f;
		}
		
	}

	public class LaprasCritterNPCShiny : LaprasCritterNPC{}
}