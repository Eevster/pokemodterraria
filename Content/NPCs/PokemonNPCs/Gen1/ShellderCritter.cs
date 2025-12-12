using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class ShellderCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 28;
		public override int hitboxHeight => 46;

		public override int totalFrames => 12;
		public override int animationSpeed => 8;
		public override int[] idleStartEnd => [0,6];
		public override int[] walkStartEnd => [7,10];
		public override int[] jumpStartEnd => [2,2];
		public override int[] fallStartEnd => [4, 4];
		public override float catchRate => 190;

		public override bool canSwim => true;

		public override int[] idleSwimStartEnd => [0,6];
		public override int[] walkSwimStartEnd => [7,10];
		public override int[] attackSwimStartEnd => [22,22];

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
				return GetSpawnChance(spawnInfo, SpawnCondition.Overworld.Chance * 0.4f);
			}

			return 0f;
		}
		
	}

	public class ShellderCritterNPCShiny : ShellderCritterNPC{}
}