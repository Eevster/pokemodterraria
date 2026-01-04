using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class FearowCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 48;
		public override int hitboxHeight => 54;

		public override int totalFrames => 16;
		public override int animationSpeed => 6;
		public override int moveStyle => 2;

		public override int[] idleStartEnd => [0,3];
		public override int[] walkStartEnd => [4,7];
		public override int[] jumpStartEnd => [5,5];
		public override int[] fallStartEnd => [6,6];
		public override int[] attackStartEnd => [12,15];

		public override int[] idleFlyStartEnd => [8,11];
		public override int[] walkFlyStartEnd => [8,11];
		public override int[] attackFlyStartEnd => [12,15];

		public override float catchRate => 90;
		public override int minLevel => 25;

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

	public class FearowCritterNPCShiny : FearowCritterNPC{}
}