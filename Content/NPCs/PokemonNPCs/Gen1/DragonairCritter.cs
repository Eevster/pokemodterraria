using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class DragonairCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 40;
		public override int hitboxHeight => 64;

		public override int totalFrames => 23;
		public override int animationSpeed => 6;
		public override int moveStyle => 2;

		public override int[] idleStartEnd => [0,3];
		public override int[] walkStartEnd => [4,10];
		public override int[] jumpStartEnd => [9,9];
		public override int[] fallStartEnd => [10,10];
		public override int[] attackStartEnd => [11, 14];

		public override int[] idleFlyStartEnd => [15,18];
		public override int[] walkFlyStartEnd => [15,18];
		public override int[] attackFlyStartEnd => [19,22];
		
		public override bool canSwim => true;

		public override int[] idleSwimStartEnd => [0,3];
		public override int[] walkSwimStartEnd => [4,10];
		public override int[] attackSwimStartEnd => [11, 14];

        public override float catchRate => 75;
		public override int minLevel => 30;

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
				return GetSpawnChance(spawnInfo, SpawnCondition.Overworld.Chance * 0.01f);
			}

			return 0f;
		}
		
	}

	public class DragonairCritterNPCShiny : DragonairCritterNPC{}
}