using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class StaryuCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 22;
		public override int hitboxHeight => 22;

		public override int totalFrames => 13;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,0];
		public override int[] walkStartEnd => [1,4];
		public override int[] jumpStartEnd => [3,3];
		public override int[] fallStartEnd => [1,1];
        public override int[] attackStartEnd => [5,8];

        public override bool canSwim => true;

        public override int[] idleSwimStartEnd => [9,12];
        public override int[] walkSwimStartEnd => [9,12];
		public override int[] attackSwimStartEnd => [5,8];

		public override float catchRate => 225;

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
				return GetSpawnChance(spawnInfo, SpawnCondition.Overworld.Chance * 0.4f);
			}

			return 0f;
		}
		
	}

	public class StaryuCritterNPCShiny : StaryuCritterNPC{}
}