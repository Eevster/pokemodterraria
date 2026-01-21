using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class UnownCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 14;
		public override int hitboxHeight => 22;

        public override int moveStyle => 1;
		
		public override int totalFrames => 1;
		public override int animationSpeed => 5;

		public override int[] idleStartEnd => [0,0];
		public override int[] walkStartEnd => [0,0];

		public override int[] idleFlyStartEnd => [0,0];
		public override int[] walkFlyStartEnd => [0,0];
		public override int[] attackFlyStartEnd => [0,0];

        public override float catchRate => 225;
		public override int minLevel => 10;

		public override string[] variants => ["B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "ExclamationMark", "QuestionMark"];
        public override int variantProbDenominator => 2;

		
		public override int[][] spawnConditions =>
		[
            [(int)SpawnArea.UndergroundDesert, (int)DayTimeStatus.All, (int)WeatherStatus.All]
        ];

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheHallow);
            base.SetBestiary(database, bestiaryEntry);
        }

		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneUndergroundDesert) {
				return GetSpawnChance(spawnInfo, SpawnCondition.DesertCave.Chance * 0.15f);
			}

			return 0f;
		}
	}

	public class UnownCritterNPCShiny : UnownCritterNPC{}
}