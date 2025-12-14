using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class CloysterCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 44;
		public override int hitboxHeight => 62;

		public override int totalFrames => 14;
		public override int animationSpeed => 8;
		public override int[] idleStartEnd => [0,5];
		public override int[] walkStartEnd => [0,5];
		public override int[] jumpStartEnd => [1,1];
		public override int[] fallStartEnd => [5, 5];

		public override bool canSwim => true;

		public override int[] idleSwimStartEnd => [0,5];
		public override int[] walkSwimStartEnd => [0,5];
		public override int[] attackSwimStartEnd => [13, 13];
		
		public override float catchRate => 60;
		public override int minLevel => 40;
        public override int[][] spawnConditions =>
        [
            [(int)SpawnArea.Beach, (int)DayTimeStatus.All, (int)WeatherStatus.All],
            [(int)SpawnArea.Snow, (int)DayTimeStatus.All, (int)WeatherStatus.All]
        ];

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) { 
			base.SetBestiary(database, bestiaryEntry);
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface);
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneSnow || spawnInfo.Player.ZoneBeach)
			{
				return GetSpawnChance(spawnInfo, SpawnCondition.OverworldDay.Chance * 0.5f);
			}

			return 0f;
		}
		
	}

	public class CloysterCritterNPCShiny : CloysterCritterNPC{}
}