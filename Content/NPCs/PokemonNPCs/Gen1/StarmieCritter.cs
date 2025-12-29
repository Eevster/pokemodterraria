using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class StarmieCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 32;
		public override int hitboxHeight => 32;

		public override int totalFrames => 13;
		public override int animationSpeed => 5;
		public override int moveStyle => 2;

		public override int[] idleStartEnd => [0,0];
		public override int[] walkStartEnd => [1,4];
		public override int[] jumpStartEnd => [3,3];
		public override int[] fallStartEnd => [1,1];
        public override int[] attackStartEnd => [5,8];

		public override int[] idleFlyStartEnd => [9,12];
		public override int[] walkFlyStartEnd => [9,12];
		public override int[] attackFlyStartEnd => [5,8];

        public override bool canSwim => true;

        public override int[] idleSwimStartEnd => [9,12];
        public override int[] walkSwimStartEnd => [9,12];
		public override int[] attackSwimStartEnd => [5,8];
		public override float catchRate => 40;

		public override int minLevel => 25;

		public override int[][] spawnConditions =>
		[
            [(int)SpawnArea.Beach, (int)DayTimeStatus.All, (int)WeatherStatus.All]
        ];

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) { 
			base.SetBestiary(database, bestiaryEntry);
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean);
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneBeach) {
				return GetSpawnChance(spawnInfo, SpawnCondition.Overworld.Chance * 0.05f);
			}

			return 0f;
		}
		
	}

	public class StarmieCritterNPCShiny : StarmieCritterNPC{}
}