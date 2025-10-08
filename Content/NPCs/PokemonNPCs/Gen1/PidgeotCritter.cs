using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class PidgeotCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 28;
		public override int hitboxHeight => 46;

		public override int totalFrames => 16;
		public override int animationSpeed => 7;
		public override int moveStyle => 2;

		public override int[] idleStartEnd => [0,3];
		public override int[] walkStartEnd => [4,7];
		public override int[] jumpStartEnd => [6,6];
		public override int[] fallStartEnd => [7,7];
		public override int[] attackStartEnd => [12,15];

		public override int[] idleFlyStartEnd => [8,11];
		public override int[] walkFlyStartEnd => [12,15];
		public override int[] attackFlyStartEnd => [12,15];

		public override float moveSpeed => 1.8f;

        public override float catchRate => 90;
        public override int minLevel => 36;

		public override int[][] spawnConditions =>
		[
			[(int)SpawnArea.Surface, (int)DayTimeStatus.Day, (int)WeatherStatus.All]
        ];

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				new FlavorTextBestiaryInfoElement("By flapping its wings with all its might, Pidgeot can make a gust of wind capable of bending tall trees."));
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneForest) {
                return GetSpawnChance(spawnInfo, SpawnCondition.OverworldDay.Chance * 0.5f);
            }

			return 0f;
		}
	}

	public class PidgeotCritterNPCShiny : PidgeotCritterNPC{}
}