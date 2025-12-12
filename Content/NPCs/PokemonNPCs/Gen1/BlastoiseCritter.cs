
using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class BlastoiseCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 42;
		public override int hitboxHeight => 46;

		public override int totalFrames => 24;
		public override int animationSpeed => 6;
		public override int[] idleStartEnd => [0,8];
		public override int[] walkStartEnd => [9,16];
		public override int[] jumpStartEnd => [11,11];
		public override int[] fallStartEnd => [14,14];
		public override int[] attackStartEnd => [17,23];

		public override bool canSwim => true;

		public override int[] idleSwimStartEnd => [0,8];
		public override int[] walkSwimStartEnd => [9,16];
		public override int[] attackSwimStartEnd => [17,23];

		public override int minLevel => 36;
		
		public override int[][] spawnConditions =>
		[
            [(int)SpawnArea.Beach, (int)DayTimeStatus.Day, (int)WeatherStatus.All]
        ];

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean);
            base.SetBestiary(database, bestiaryEntry);
        }

		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneBeach) {
                return GetSpawnChance(spawnInfo, SpawnCondition.OverworldDay.Chance * 0.5f);
            }

			return 0f;
		}
	}

	public class BlastoiseCritterNPCShiny : BlastoiseCritterNPC{}
}